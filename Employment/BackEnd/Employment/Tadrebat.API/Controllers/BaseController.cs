using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iText.StyledXmlParser.Jsoup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Employment.Enum;
using Newtonsoft.Json;
using System.Net;

namespace Employment.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public readonly IMapper _mapper;
        EnumUserTypes UserRole;
        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
        //https://medium.com/@matteocontrini/consistent-error-responses-in-asp-net-core-web-apis-bb70b435d1f8
        protected string GetLanguage()
        {
            string currentLang = "en";

            if (!string.IsNullOrEmpty(HttpContext.Request.Headers["currentLang"]))
                currentLang = HttpContext.Request.Headers["currentLang"];

            return currentLang;
        }
        protected async Task<IActionResult> FormatResult(bool IsSuccess)
        {
            if (IsSuccess)
                return Ok();
            else
                return BadRequest();
        }
        protected EnumUserTypes GetUserRole()
        {
            if (this.User != null)
            {
                string role = this.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "role").Value;
                return (EnumUserTypes)System.Enum.Parse(typeof(EnumUserTypes), role);
            }
            return UserRole;
        }
        protected string GetUserId()
        {
            if (this.User != null)
            {
                return this.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "mdid").Value;
            }
            return "-1";
        }
        protected string GetUserName()
        {
            if (this.User != null)
            {
                return this.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name").Value;
            }
            return "";
        }
        protected string GetUserEmail()
        {
            if (this.User != null)
            {
                return this.User.Identity.Name;
            }
            return "";
        }
        //protected static void IncorrectData()
        //{
        //    throw new HttpStatusException("Data doesn't exist in the system", (int)StatusCodes.Status409Conflict, "");
        //}
        //use badRequest Instead
        //protected static void InvalidModel()
        //{
        //    throw new HttpStatusException("Model is not valid", (int)StatusCodes.Status406NotAcceptable, "");
        //}
    }
}
