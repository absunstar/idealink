using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using iText.StyledXmlParser.Jsoup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.Enum;

namespace Tadrebat.API.Controllers
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
        protected bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        protected bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        protected void ExportToExcel(DataTable dataTable, string fileName)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("List");
                    string tab = string.Empty;
                    int count = 1;
                    foreach (DataColumn datacol in dataTable.Columns)
                    {
                        worksheet.Cell(1, count++).Value = datacol.ColumnName;

                    }
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = Convert.ToString(dataTable.Rows[i][j]);
                        }
                    }
                    workbook.SaveAs(fileName);
                }
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}