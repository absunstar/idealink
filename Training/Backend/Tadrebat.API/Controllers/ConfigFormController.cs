using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.API.Model.Model;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin")]
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

            return Ok(result != null ? result.Form : new List<FieldConfig>());
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