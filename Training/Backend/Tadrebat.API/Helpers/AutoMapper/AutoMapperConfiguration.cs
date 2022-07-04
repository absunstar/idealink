using AutoMapper;
using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<TrainingType, ResponseTrainingType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<TrainingCategory, ResponseTrainingCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<Course, ResponseCourse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap(typeof(MongoResultPaged<>), typeof(ResponsePaged<>));
            CreateMap<NGOType, ResponseNGOType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<City, ResponseCity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<Area, ResponseArea>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<EntityPartner, ResponseEntityPartner>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<EntityPartner, ResponseEntityPartnerReport>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<EntitySubPartner, ResponseEntitySubPartner>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<EntityTrainingCenter, ResponseEntityTrainingCenter>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<UserProfile, ResponseUserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id))
                .ForMember(dest => dest.MyPartnerListIds, opt => opt.Ignore())
                .ForMember(dest => dest.MySubPartnerListIds, opt => opt.Ignore());
            CreateMap<Trainee, ResponseTrainee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<Training, ResponseTraining>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id))
                .ForMember(dest => dest.TrainingCategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.TrainingTypeId, opt => opt.Ignore());
            CreateMap<ItemDetails, ResponseItemDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<Sessions, ResponseSessions>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<TraineeInfo, ResponseTraineeInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._Id));
            CreateMap<Attendance, ResponseAttendance>();
            CreateMap<AttendanceTrainee, ResponseAttendanceTrainee>();
            CreateMap<Question, ResponseQuestion>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<Answer, ResponseAnswer>();
            CreateMap<TraineeTraining, ResponseMyTrainingItems>();
            CreateMap<ContentData, ResponseContentData>();
            CreateMap<Certificate, ResponseCertificate>();



            CreateMap<ModelEntityPartner, EntityPartner>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelEntityTrainingCenter, EntityTrainingCenter>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelEntitySubPartner, EntitySubPartner>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelUserProfile, UserProfile>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelTrainee, Trainee>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelTraining, Training>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id))
                .ForPath(dest => dest.PartnerId._id, opt => opt.MapFrom(y => y.PartnerId))
                .ForPath(dest => dest.SubPartnerId._id, opt => opt.MapFrom(y => y.SubPartnerId))
                .ForPath(dest => dest.TrainingCenterId._id, opt => opt.MapFrom(y => y.TrainingCenterId));
            CreateMap<ModelAttendance, Attendance>();
            CreateMap<ModelAttendanceTrainee, AttendanceTrainee>();
            CreateMap<ModelQuestion, Question>()
            .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelAnswer, Answer>();
            CreateMap<ModelFieldConfig,FieldConfig>();
            CreateMap<ModelOptions, Options>();
            CreateMap<ModelConfigForm, ConfigForm>();
            CreateMap<Trainee, ConfigForm>();
            CreateMap<ModelContentData, ContentData>();
            CreateMap<ModelExamTemplate, ExamTemplate>()
            .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));


            CreateMap<Trainee, TraineeInfo>();

            CreateMap<ModelTranslateData, TranslateData>();
            CreateMap<City, ResponseTranslateData>();
            CreateMap<Area, ResponseTranslateData>();
            CreateMap<TrainingCategory, ResponseTranslateData>();
            CreateMap<TrainingType, ResponseTranslateData>();
            CreateMap<Course, ResponseTranslateData>();
            CreateMap<Trainee, TraineeError>();
        }
    }
}