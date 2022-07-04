using iText.IO.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.API.Helpers.AutoMapper
{
    public class HelperTranslate
    {
        public async Task<ResponseCity> MapCity(ResponseCity destination, City source, string lang)
        {
            switch (lang)
            {
                case "ar":
                    destination.Name = !string.IsNullOrEmpty(source.Name2) ? source.Name2 : source.Name;
                    break;
                case "fr":
                    destination.Name = !string.IsNullOrEmpty(source.Name3) ? source.Name3 : source.Name;
                    break;
            }
            destination.Areas = await MapArea(destination.Areas, source.areas, lang);
            return destination;
        }
        public async Task<List<ResponseCity>> MapCity(List<ResponseCity> lstDestination, List<City> lstSource, string lang)
        {
            foreach (var objSource in lstSource)
            {
                var objDestination = lstDestination.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapCity(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponsePaged<ResponseCity>> MapCity(ResponsePaged<ResponseCity> lstDestination, MongoResultPaged<City> lstSource, string lang)
        {
            foreach (var objSource in lstSource.lstResult)
            {
                var objDestination = lstDestination.lstResult.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapCity(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }

        public async Task<ResponseArea> MapArea(ResponseArea destination, Area source, string lang)
        {
            switch (lang)
            {
                case "ar":
                    destination.Name = !string.IsNullOrEmpty(source.Name2) ? source.Name2 : source.Name;;
                    break;
                case "fr":
                    destination.Name = !string.IsNullOrEmpty(source.Name3) ? source.Name3 : source.Name;
                    break;
            }
            return destination;
        }
        public async Task<List<ResponseArea>> MapArea(List<ResponseArea> lstDestination, List<Area> lstSource, string lang)
        {
            foreach (var objSource in lstSource)
            {
                var objDestination = lstDestination.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapArea(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponsePaged<ResponseArea>> MapArea(ResponsePaged<ResponseArea> lstDestination, MongoResultPaged<Area> lstSource, string lang)
        {
            foreach (var objSource in lstSource.lstResult)
            {
                var objDestination = lstDestination.lstResult.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapArea(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }

        public async Task<ResponseTrainingType> MapTrainingType(ResponseTrainingType destination, TrainingType source, string lang)
        {
            switch (lang)
            {
                case "ar":
                    destination.Name = !string.IsNullOrEmpty(source.Name2) ? source.Name2 : source.Name;;
                    break;
                case "fr":
                    destination.Name = !string.IsNullOrEmpty(source.Name3) ? source.Name3 : source.Name;
                    break;
            }
            return destination;
        }
        public async Task<List<ResponseTrainingType>> MapTrainingType(List<ResponseTrainingType> lstDestination, List<TrainingType> lstSource, string lang)
        {
            foreach (var objSource in lstSource)
            {
                var objDestination = lstDestination.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapTrainingType(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponsePaged<ResponseTrainingType>> MapTrainingType(ResponsePaged<ResponseTrainingType> lstDestination, MongoResultPaged<TrainingType> lstSource, string lang)
        {
            foreach (var objSource in lstSource.lstResult)
            {
                var objDestination = lstDestination.lstResult.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapTrainingType(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponseTrainingCategory> MapTrainingCategory(ResponseTrainingCategory destination, TrainingCategory source, string lang)
        {
            switch (lang)
            {
                case "ar":
                    destination.Name = !string.IsNullOrEmpty(source.Name2) ? source.Name2 : source.Name;;
                    break;
                case "fr":
                    destination.Name = !string.IsNullOrEmpty(source.Name3) ? source.Name3 : source.Name;
                    break;
            }
            destination.Course = await MapCourse(destination.Course, source.Course, lang);
            return destination;
        }
        public async Task<List<ResponseTrainingCategory>> MapTrainingCategory(List<ResponseTrainingCategory> lstDestination, List<TrainingCategory> lstSource, string lang)
        {
            foreach (var objSource in lstSource)
            {
                var objDestination = lstDestination.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapTrainingCategory(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponsePaged<ResponseTrainingCategory>> MapTrainingCategory(ResponsePaged<ResponseTrainingCategory> lstDestination, MongoResultPaged<TrainingCategory> lstSource, string lang)
        {
            foreach (var objSource in lstSource.lstResult)
            {
                var objDestination = lstDestination.lstResult.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapTrainingCategory(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponseCourse> MapCourse(ResponseCourse destination, Course source, string lang)
        {
            switch (lang)
            {
                case "ar":
                    destination.Name = !string.IsNullOrEmpty(source.Name2) ? source.Name2 : source.Name;;
                    break;
                case "fr":
                    destination.Name = !string.IsNullOrEmpty(source.Name3) ? source.Name3 : source.Name;
                    break;
            }
            return destination;
        }
        public async Task<List<ResponseCourse>> MapCourse(List<ResponseCourse> lstDestination, List<Course> lstSource, string lang)
        {
            foreach (var objSource in lstSource)
            {
                var objDestination = lstDestination.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapCourse(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
        public async Task<ResponsePaged<ResponseCourse>> MapCourse(ResponsePaged<ResponseCourse> lstDestination, MongoResultPaged<Course> lstSource, string lang)
        {
            foreach (var objSource in lstSource.lstResult)
            {
                var objDestination = lstDestination.lstResult.Where(x => x.Id == objSource._id).FirstOrDefault();
                if (objDestination != null)
                {
                    objDestination = await MapCourse(objDestination, objSource, lang);
                }
            }
            return lstDestination;
        }
    }
}
