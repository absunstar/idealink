using AutoMapper;
using Microsoft.AspNetCore.Razor.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Model;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;
using Tadrebat.Persistance.Interfaces;

namespace Tadrebat.Services
{
    public class ServiceExam : IExam
    {
        private IAsyncRepository<Report> _repositoryReports;
        private IAsyncRepository<ReportTrainingTrainee> _repositoryReportsTrainingTrainee;

        private readonly ITrainee _BLTrainee;
        private readonly IDBExam _dBExam;
        private readonly IDBTraining _dBTraining;
        private readonly IDBTrainingType _dBTrainingType;
        private readonly IDBUserProfile _dBUserProfile;
        private readonly IDBTrainingCategory _dBTrainingCategory;
        private readonly IDBCity _dBCity;
        private readonly ICacheConfig _cacheConfig;
        private readonly IQuestion _question;
        public readonly IMapper _mapper;
        private readonly ICertificate _BLCertificate;
        private readonly IUserProfile _BLUserProfile;
        private readonly IExamTemplate _BLExamTemplate;
        public ServiceExam(IAsyncRepository<Report> repositoryReports, IAsyncRepository<ReportTrainingTrainee> repositoryTrainingTrainee, IExamTemplate BLExamTemplate, IUserProfile BLUserProfile, ICertificate BLCertificate, ITrainee BLTrainee, IDBExam dBExam, IDBCity dBCity, IDBTraining dBTraining, IDBTrainingCategory dBTrainingCategory, IDBTrainingType dBTrainingType, IDBUserProfile dBUserProfile, ICacheConfig cacheConfig, IQuestion question, IMapper mapper)
        {
            _repositoryReports = repositoryReports;
            _repositoryReportsTrainingTrainee = repositoryTrainingTrainee;

            _BLTrainee = BLTrainee;
            _dBExam = dBExam;
            _dBTraining = dBTraining;
            _dBTrainingCategory = dBTrainingCategory;
            _dBTrainingType = dBTrainingType;
            _dBUserProfile = dBUserProfile;
            _dBCity = dBCity;
            _cacheConfig = cacheConfig;
            _question = question;
            _mapper = mapper;
            _BLCertificate = BLCertificate;
            _BLUserProfile = BLUserProfile;
            _BLExamTemplate = BLExamTemplate;
        }
        public async Task<Exam> GetById(string Id)
        {
            return await _dBExam.GetById(Id);
        }
        public async Task<ExamResponse> TakeExam(string TrainingId, string TraineeId)
        {
            var response = new ExamResponse();
            var training = await _dBTraining.GetById(TrainingId);

            if (training == null)
            {
                response.result = ExamResult.TrainingNotFound;
            }
            else if (training.EndDate > DateTime.UtcNow)
            {
                response.result = ExamResult.TrainingNotOver;
            }
            else if (!training.IsAdminApproved || !training.IsConfirm1 || !training.IsConfirm2 || !training.IsActive.GetValueOrDefault() || !training.Trainees.Any(x => x.IsApproved == true && x._Id == TraineeId))
            {
                response.result = ExamResult.Generic;
            }
            else if (!isExceedAttendanceRatio(training, TraineeId))
            {
                response.result = ExamResult.AttendanceFailed;
            }
            else
            {
                var traineeExams = await _dBExam.ListByTrainingTraineeId(training._id, TraineeId);
                bool isAnyExamPassed = traineeExams.Any(te => te.IsPass);
                if (isAnyExamPassed)
                {
                    response.result = ExamResult.ExamPassed;
                }
                else if (traineeExams.Count >= _cacheConfig.ExamTrailCount)
                {
                    response.result = ExamResult.TrailExceeded;
                }
                else
                {
                    int examQuestionCount = _cacheConfig.ExamQuestionCount;
                    var questions = await _question.QuestionListActiveByTrainingCategoryId(training.TrainingCategoryId);
                    if (questions == null || questions.Count == 0)
                    {
                        response.result = ExamResult.NeedAddQuestions;
                    }
                    else
                    {
                        var easy = 0.4;
                        var medium = 0.3;
                        var hard = 0.3;

                        if (!string.IsNullOrEmpty(training.ExamTemplateId))
                        {
                            var template = await _BLExamTemplate.ExamTemplateGetById(training.ExamTemplateId);
                            if (template != null)
                            {
                                easy = (double)template.Easy / 100;
                                medium = (double)template.Medium / 100;
                                hard = (double)template.Hard / 100;
                            }
                        }


                        var random = new Random();
                        var examQuestinos = new List<Question>();
                        //TODO replace 1 with enum
                        //TODO check available count of questions with difficulty before fetching them 
                        examQuestinos.AddRange(questions.Where(q => q.Difficulty == 3).OrderBy(x => random.Next()).Take((int)(examQuestionCount * hard)).ToList());
                        examQuestinos.AddRange(questions.Where(q => q.Difficulty == 2).OrderBy(x => random.Next()).Take((int)(examQuestionCount * medium)).ToList());
                        var qcount = examQuestinos.Count();
                        if (qcount < (int)(examQuestionCount * (hard + medium)))
                        {
                            qcount = examQuestionCount - qcount;
                        }
                        else
                        {
                            qcount = (int)(examQuestionCount * easy);
                        }

                        examQuestinos.AddRange(questions.Where(q => q.Difficulty == 1).OrderBy(x => random.Next()).Take(qcount).ToList());


                        examQuestinos = examQuestinos.OrderBy(a => Guid.NewGuid()).ToList(); //shuffle

                        //cateogrize exam questions
                        var exam = new Exam();
                        exam.CreatedAt = DateTime.UtcNow;
                        exam.IsActive = true;
                        exam.TraineeId = TraineeId;
                        exam.TrainingId = training._id;
                        exam.ExamTemplate = examQuestinos.Select(ex => new QuestionTemplate()
                        {
                            Answer = ex.Answer.OrderBy(a => Guid.NewGuid()).ToList(), //shuffle
                            CreatedAt = ex.CreatedAt,
                            Difficulty = ex.Difficulty,
                            IsActive = ex.IsActive,
                            Name = ex.Name,
                            TrainingCategoryId = ex.TrainingCategoryId,
                            TrainingTypeId = ex.TrainingTypeId,
                            SelectedAnswer = null,
                            _id = ex._id
                        }).ToList();

                        response.result = ExamResult.Success;

                        response.questions = exam.ExamTemplate;

                        await _dBExam.AddAsync(exam);

                        HideCorrectAnswers(response.questions);
                        response.ExamId = exam._id;

                    }
                }
            }



            return response;
        }
        private void HideCorrectAnswers(List<QuestionTemplate> questions)
        {
            foreach (QuestionTemplate question in questions)
            {
                foreach (Answer answer in question.Answer)
                {
                    answer.IsCorrectAnswer = false;
                }
            }
        }
        public async Task<bool> SubmitExam(string ExamId, List<QuestionTemplate> questions)
        {
            var examObject = await GetById(ExamId);
            if (examObject == null)
                return false;

            //examObject.ExamTemplate = questions;
            for (int i = 0; i < questions.Count; i++)
            {
                var q = examObject.ExamTemplate.Where(x => x._id == questions[i]._id).FirstOrDefault();

                if (q != null)
                {
                    q.SelectedAnswer = questions[i].SelectedAnswer;
                }
            }

            int score = CalculateExamScore(questions, examObject);
            examObject.Score = score;
            examObject.IsPass = score >= _cacheConfig.ExamPassingPercent;
            await _dBExam.UpdateObj(examObject._id, examObject);

            Report examReport = await GenerateExamReport(examObject);
            //await _dBReport.AddAsync(examReport);
            try
            {
                await _repositoryReports.AddAsync(examReport);
            }
            catch (Exception ex)
            {

                ServiceHelper.Log(ex.Message + ex.InnerException);
            }
            var trainee = await _BLTrainee.GetById(examObject.TraineeId);
            var traineeTraining = trainee.myTrainings.Where(x => x.TrainingId == examObject.TrainingId).FirstOrDefault();
            traineeTraining.ExamCount++;
            if (examObject.IsPass)
            {
                traineeTraining.HasCertificate = true;
            }
            await _BLTrainee.UpdateObj(trainee);

            if (examObject.IsPass)
            {
                var training = await _dBTraining.GetById(examObject.TrainingId);
                //add to trainer certificate count
                await _BLUserProfile.AddTrainerExamPass(training.TrainerId, training.PartnerId._id, training.TrainingCategoryId);

                //generate certificate
                try
                {
                    await _BLCertificate.GenerateTraineePassedExamCertificate(trainee._id, examObject.TrainingId);
                }
                catch (Exception ex)
                {

                    ServiceHelper.Log(ex.Message + ex.InnerException);
                }
            }
            return examObject.IsPass;
        }

        private async Task<Report> GenerateExamReport(Exam examObject)
        {
            Report examReport = new Report();
            var training = await _dBTraining.GetById(examObject.TrainingId);
            examReport.PartnerId = training.PartnerId._id;
            examReport.PartnerName = training.PartnerId.Name;
            examReport.SubPartnerId = training.SubPartnerId._id;
            examReport.SubPartnerName = training.SubPartnerId.Name;
            examReport.TrainingCenterId = training.TrainingCenterId._id;
            examReport.TrainingCenterName = training.TrainingCenterId.Name;
            examReport.TrainerId = training.TrainerId;
            var trainer = await _dBUserProfile.GetById(training.TrainerId);
            examReport.TrainerName = trainer.Name;
            examReport.TrainingTypeId = training.TrainingTypeId;
            var trainingType = await _dBTrainingType.GetById(training.TrainingTypeId);
            examReport.TrainingTypeName = trainingType.Name;
            var trainingCategory = await _dBTrainingCategory.GetById(training.TrainingCategoryId);
            examReport.TrainingCategoryName = trainingCategory.Name;
            examReport.CityId = training.CityId;
            var city = await _dBCity.GetById(training.CityId);
            examReport.CityName = city.Name;
            examReport.AreaId = training.AreaId;
            var area = city.areas.Where(a => a._id == training.AreaId).FirstOrDefault();
            examReport.AreaName = area != null ? area.Name : null;
            examReport.StartDate = training.StartDate;
            examReport.EndDate = training.EndDate;
            examReport.IsOnline = training.IsOnline;
            examReport.TrainerCount = training.TrainerCount;
            examReport.TraineeId = examObject.TraineeId;
            var trainee = await _BLTrainee.GetById(examObject.TraineeId);
            examReport.TraineeName = trainee.Name;
            examReport.NationalId = trainee.NationalId;
            examReport.Email = trainee.Email;
            examReport.Gender = trainee.Gender == 1 ? "Male" : "Female";
            examReport.DOB = trainee.DOB;
            examReport.TraineeRegisterDate = trainee.CreatedAt;
            examReport.ExamDate = examObject.CreatedAt;
            examReport.ExamId = examObject._id;
            examReport.IsPassExam = examObject.IsPass ? "True" : "False";
            examReport.Score = examObject.Score;
            examReport.TrainingId = examObject.TrainingId;
            examReport.TrainerCityId = trainer.CityId;
            examReport.IdType = trainee.IdType;

            city = await _dBCity.GetById(trainer.CityId);
            if (city != null)
                examReport.TrainerCityName = city.Name;
            return examReport;
        }
        private async Task<List<Training>> GetTrainingToBeCopiedToReportDB()
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true
                                                    && x.EndDate < DateTime.Now
                                                    && x.IsCopiedToReportDB == true);
            var sort = Builders<Training>.Sort.Descending(x => x.StartDate);
            var result = await _dBTraining.GetPaged(filter, sort, 1, int.MaxValue);

            return result.lstResult;
        }
        private async Task<bool> GenerateReportTrainingTrainee()
        {

            var lstTraining = await GetTrainingToBeCopiedToReportDB();
            foreach(var training in lstTraining)
            { 
                var lstTrainee = training.Trainees.Where(x => x.IsApproved == true);
                foreach(var trainee in lstTrainee)
                {
                    var obj = await GenerateobjTrainingTrainee(training, trainee._Id);
                    try
                    {
                        await _repositoryReportsTrainingTrainee.AddAsync(obj);
                    }
                    catch (Exception ex)
                    {

                        ServiceHelper.Log(ex.Message + ex.InnerException);
                    }
                }
            }
            return true;
        }
        private async Task<ReportTrainingTrainee> GenerateobjTrainingTrainee(Training training, string TraineeId)
        {
            ReportTrainingTrainee objReport = new ReportTrainingTrainee();
            //var training = await _dBTraining.GetById(TrainingId);
            objReport.PartnerId = training.PartnerId._id;
            objReport.PartnerName = training.PartnerId.Name;
            objReport.SubPartnerId = training.SubPartnerId._id;
            objReport.SubPartnerName = training.SubPartnerId.Name;
            objReport.TrainingCenterId = training.TrainingCenterId._id;
            objReport.TrainingCenterName = training.TrainingCenterId.Name;
            objReport.TrainerId = training.TrainerId;

            var trainer = await _dBUserProfile.GetById(training.TrainerId);
            objReport.TrainerName = trainer.Name;
            objReport.TrainingTypeId = training.TrainingTypeId;

            var trainingType = await _dBTrainingType.GetById(training.TrainingTypeId);
            objReport.TrainingTypeName = trainingType.Name;
            
            var trainingCategory = await _dBTrainingCategory.GetById(training.TrainingCategoryId);
            objReport.TrainingCategoryName = trainingCategory.Name;
            objReport.CityId = training.CityId;
            
            var city = await _dBCity.GetById(training.CityId);
            objReport.CityName = city.Name;
            objReport.AreaId = training.AreaId;
            
            var area = city.areas.Where(a => a._id == training.AreaId).FirstOrDefault();
            objReport.AreaName = area != null ? area.Name : null;
            
            objReport.StartDate = training.StartDate;
            objReport.EndDate = training.EndDate;
            objReport.IsOnline = training.IsOnline;
            objReport.TrainerCount = training.TrainerCount;
            objReport.TraineeId = TraineeId;

            var trainee = await _BLTrainee.GetById(TraineeId);
            objReport.TraineeName = trainee.Name;
            objReport.NationalId = trainee.NationalId;
            objReport.Email = trainee.Email;
            objReport.Gender = trainee.Gender == 1 ? "Male" : "Female";
            objReport.DOB = trainee.DOB;
            objReport.TraineeRegisterDate = trainee.CreatedAt;
            objReport.TrainingId = training._id;
            objReport.TrainerCityId = trainer.CityId;
            objReport.IdType = trainee.IdType;

            city = await _dBCity.GetById(trainer.CityId);
            if (city != null)
                objReport.TrainerCityName = city.Name;
            return objReport;
        }
        private int CalculateExamScore(List<QuestionTemplate> questions, Exam exam)
        {
            int correctQuestionsCount = 0;
            for (int i = 0; i < exam.ExamTemplate.Count; i++)
            {
                var correctAnswer = exam.ExamTemplate[i].Answer.Where(a => a.IsCorrectAnswer).FirstOrDefault();

                if (correctAnswer != null && questions[i].SelectedAnswer != null && questions[i].SelectedAnswer.Equals(correctAnswer._id))
                {
                    correctQuestionsCount++;
                }
            }
            return (int)((100 * (float)correctQuestionsCount) / (float)questions.Count);
        }

        private bool isExceedAttendanceRatio(Training training, string traineeId)
        {
            //TODO check on isTakenAttendance sessions only or all sessions as done
            int sessionsCount = training.Sessions.Count;
            int traineeAttendanceCount = training.Attendances.SelectMany(a => a.Attendances).Where(a => a.TraineeId == traineeId && a.IsAttendant).Count();
            float attendaceRatio = ((float)traineeAttendanceCount / (float)sessionsCount) * 100;
            int configuredRatio = _cacheConfig.AttendancePercent;
            if (attendaceRatio >= configuredRatio)
                return true;
            return false;
        }
    }
}
