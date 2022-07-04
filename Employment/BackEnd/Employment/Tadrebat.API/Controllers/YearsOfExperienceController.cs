using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    public class YearsOfExperienceController : GenericController<YearsOfExperience>
    {
        private readonly IServiceYearsOfExperience _BLService;
        public YearsOfExperienceController(IServiceYearsOfExperience BLService, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
        }
    }
}