using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.Enum;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    public class ExamController : BaseController
    {
        private readonly IExam BLExam;
        private readonly HelperMapperData HLMapperData;
        public string currentLang = "en";
        private readonly ITrainee BLServiceTrainee;
        private readonly IUserProfile BLServiceTrainer;
        public ExamController(IExam _BLExam, IMapper mapper,
              ITrainee _BLServiceTrainee,
              IUserProfile _BLServiceTrainer,
            HelperMapperData _HLMapperData) : base(mapper)
        {
            BLExam = _BLExam;
            HLMapperData = _HLMapperData;
            BLServiceTrainee = _BLServiceTrainee;
            BLServiceTrainer = _BLServiceTrainer;
        }

        public async Task<IActionResult> TakeExam(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            //var TraineeId = GetUserId();
            //SS -Commented because not getting proper id
            string TraineeId = string.Empty;
            var role = GetUserRole();

            if (role == EnumUserTypes.Trainee)
            {
                var userDetails = await BLServiceTrainee.GetByEmail(this.User.Identity.Name);
                TraineeId = userDetails._id;
            }
            else
            {
                var userDetails = await BLServiceTrainer.UserProfileGetByEmail(this.User.Identity.Name);
                TraineeId = userDetails == null ? "" : userDetails._id;
            }

            var result = await BLExam.TakeExam(model.Id, TraineeId);


            currentLang = GetLanguage();
            switch (result.result)
            {
                case ExamResult.TrainingNotOver: return BadRequest(currentLang == "ar" ? "لا يزال التدريب قيد التشغيل ، وسيتم إجراء الاختبار بعد انتهاء التدريب." : "Training is still running, exam will be taken after ithe training ends.");
                case ExamResult.ExamPassed: return BadRequest(currentLang == "ar" ? "لقد اجتزت الاختبار بالفعل" : "You had already passed the exam.");
                case ExamResult.TrailExceeded: return BadRequest(currentLang == "ar" ? "لقد تجاوزت الحد الأقصى لعدد الممرات" : "You exceeded max number of trails.");
                case ExamResult.AttendanceFailed: return BadRequest(currentLang == "ar" ? "لم تجتز فحص الحضور" : "You didn't pass attendance check.");
                case ExamResult.TrainingNotFound: return BadRequest(currentLang == "ar" ? "التدريب غير موجود" : "Training not found.");
                case ExamResult.NeedAddQuestions: return BadRequest(currentLang == "ar" ? "يرجى طلب  من المشرف لإضافة أسئلة" : "Please request admin to add questions.");
                case ExamResult.Generic: return BadRequest(currentLang == "ar" ? "الامتحان ليس مفتوحا بعد" : "Exam is not open yet.");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(ExamResponse model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLExam.SubmitExam(model.ExamId, model.questions);


            return Ok(result);
        }
    }
}