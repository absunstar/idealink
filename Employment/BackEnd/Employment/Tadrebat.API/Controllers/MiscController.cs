using AutoMapper;
using Employment.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    public class MiscController : BaseController
    {
        protected readonly ICacheConfig _BLCacheConfig;
        public MiscController(IMapper mapper,
                                ICacheConfig cacheConfig) : base(mapper)
        {
            _BLCacheConfig = cacheConfig;
        }
        public ActionResult MCTURL()
        {
            return Ok(_BLCacheConfig.URLMCT);
        }
    }
}
