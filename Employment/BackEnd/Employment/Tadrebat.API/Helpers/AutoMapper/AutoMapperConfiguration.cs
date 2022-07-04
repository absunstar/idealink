using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;

namespace Employment.API.Helpers.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap(typeof(MongoResultPaged<>), typeof(ResponsePaged<>));
            CreateMap<NGOType, ResponseNGOType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<UserProfile, ResponseUserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id))
                .ForPath(dest => dest.MyCompanies, opt => opt.Ignore());
            CreateMap<UserProfile, ResponseCompanyEmployers>();
            CreateMap<Company, ResponseCompanyEmployers>();
            CreateMap<Company, ResponseCompany>();
            CreateMap<SubItem, ResponseSubItem>();
            CreateMap<SubItemActive, ResponseSubItemActive>();
            CreateMap<SubItemURL, ResponseSubItemURL>();
            CreateMap<Job, ResponseJob>();
            CreateMap<JobSeeker, ResponseJobSeeker>();
            CreateMap<ResumeItem, ResponseResumeItem>();
            CreateMap<ResumeCertification, ResponseResumeCertification>();
            CreateMap<Favourite, ResponseFavourite>();
            CreateMap<Apply, ResponseApply>();
            CreateMap<ApplySubItem, ResponseApplySubItem>();
            CreateMap<JobFair, ResponseJobFair>();

            CreateMap<ModelUserProfile, UserProfile>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(y => y.Id));
            CreateMap<ModelCompany, Company>()
                .ForPath(dest => dest.Country._id, opt => opt.MapFrom(y => y.CountryId))
                .ForPath(dest => dest.City._id, opt => opt.MapFrom(y => y.CityId))
                .ForPath(dest => dest.Industry._id, opt => opt.MapFrom(y => y.IndustryId));
            CreateMap<ModelJob, Job>()
                .ForPath(dest => dest.Company._id, opt => opt.MapFrom(y => y.CompanyId))
                .ForPath(dest => dest.Country._id, opt => opt.MapFrom(y => y.CountryId))
                .ForPath(dest => dest.City._id, opt => opt.MapFrom(y => y.CityId))
                .ForPath(dest => dest.Industry._id, opt => opt.MapFrom(y => y.IndustryId))
                .ForPath(dest => dest.JobField._id, opt => opt.MapFrom(y => y.JobFieldId))
                .ForPath(dest => dest.JobSubField._id, opt => opt.MapFrom(y => y.JobSubFieldId))
                .ForPath(dest => dest.Qualification._id, opt => opt.MapFrom(y => y.QualificationId))
                .ForPath(dest => dest.Experience._id, opt => opt.MapFrom(y => y.ExperienceId));
            CreateMap<ModelJobSeeker, JobSeeker>()
                .ForPath(dest => dest.Country._id, opt => opt.MapFrom(y => y.CountryId))
                .ForPath(dest => dest.City._id, opt => opt.MapFrom(y => y.CityId))
                .ForPath(dest => dest.Qualification._id, opt => opt.MapFrom(y => y.QualificationId))
                .ForPath(dest => dest.Experience._id, opt => opt.MapFrom(y => y.ExperienceId))
                .ForPath(dest => dest.Languages, opt => opt.Ignore());
            CreateMap<ModelResumeItem, ResumeItem>();
            CreateMap<ModelResumeCertification, ResumeCertification>();
            CreateMap<ModelJobFair, JobFair>();
            CreateMap<ModelJobFairRegisteration, JobFairRegisteration>();
            CreateMap<ModelFieldConfig, FieldConfig>();
            CreateMap<ModelOptions, Options>();

            CreateMap<Country, ResponseTranslateData>();
            CreateMap<City, ResponseTranslateData>();
            CreateMap<JobSubFields, ResponseTranslateData>();
            CreateMap<JobFields, ResponseTranslateData>();
            CreateMap<Industry, ResponseTranslateData>();
            CreateMap<Languages, ResponseTranslateData>();
            CreateMap<Qualification, ResponseTranslateData>();
            CreateMap<YearsOfExperience, ResponseTranslateData>();
            CreateMap<ModelTranslateData, TranslateData>();
            CreateMap<CanAccessContactInformation, ResponseContactInformationRequest>(); 
        }
    }
}