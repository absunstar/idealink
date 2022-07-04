using Employment.Entity.Mongo;
using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceConfigForm : IServiceRepository<ConfigForm>
    {
        Task<ConfigForm> GetByType(EnumConfigForm type);
        Task<bool> UpdateByType(EnumConfigForm type, List<FieldConfig> form);
    }
}
