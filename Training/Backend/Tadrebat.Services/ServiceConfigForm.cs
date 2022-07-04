using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceConfigForm : ServiceRepository<ConfigForm>, IServiceConfigForm
    {
        private readonly IDBConfigForm _dBConfigForm;
        public ServiceConfigForm(IDBConfigForm dBConfigForm) : base(dBConfigForm)
        {
            _dBConfigForm = dBConfigForm;
        }
        public async Task<ConfigForm> GetByType(EnumConfigForm type)
        {
            var filter = Builders<ConfigForm>.Filter.Where(x => x.FormType == type);

            var obj = await _dBConfigForm.GetOne(filter);

            return obj;
        }
        protected async Task<bool> CheckByType(EnumConfigForm type)
        {
            var obj = GetByType(type);

            return obj != null;
        }
        protected async Task<bool> InitType(EnumConfigForm type)
        {
            var obj = new ConfigForm();
            obj.FormType = type;
            obj.Name = System.Enum.GetName(typeof(EnumConfigForm), type);

            await _dBConfigForm.AddAsync(obj);

            return true;
        }
        public async Task<bool> UpdateByType(EnumConfigForm type, List<FieldConfig> form)
        {
            var obj = await GetByType(type);

            //if (!await CheckByType(type))
            if (obj == null)
            {
                await InitType(type);
                obj = await GetByType(type);
            }

            obj.Form = form;

            await _dBConfigForm.UpdateObj(obj._id, obj);

            return true;
        }
    }
}
