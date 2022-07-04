using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;

namespace Tadrebat.Interface
{
    public interface IServiceConfigForm : IServiceRepository<ConfigForm>
    {
        Task<ConfigForm> GetByType(EnumConfigForm type);
        Task<bool> UpdateByType(EnumConfigForm type, List<FieldConfig> form);
    }
}
