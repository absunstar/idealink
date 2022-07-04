using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    public class LanguagesController : GenericController<Languages>
    {
        private readonly IServiceLanguages _BLService;
        public LanguagesController(IServiceLanguages BLService, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
        }
    }
}