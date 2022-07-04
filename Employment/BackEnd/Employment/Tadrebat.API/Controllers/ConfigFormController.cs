using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model.Model;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    public class ConfigFormController : BaseController
    {
        private readonly IServiceConfigForm _BLService;
        public ConfigFormController(IServiceConfigForm BLService,
                            IMapper mapper) : base(mapper)
        {
            _BLService = BLService;
        }
        public async Task<IActionResult> GetByType(ModelConfigFormGet model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

          //  model = Enum.EnumConfigForm.JobFairCreate;

            if (model.type == 0)
                return BadRequest();

            var result = await _BLService.GetByType(model.type);

            return Ok(result != null ? result.Form : null) ;
        }
        public async Task<IActionResult> Update(ModelConfigForm model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (model.FormType == 0)
                return BadRequest();

            var lst = _mapper.Map<List<ModelFieldConfig>, List<FieldConfig>>(model.Form);
            var result = await _BLService.UpdateByType(model.FormType, lst);

            return await FormatResult(result);
        }
    }
}