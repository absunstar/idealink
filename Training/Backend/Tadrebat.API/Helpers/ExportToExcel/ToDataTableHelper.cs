using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Helpers.ExportToExcel
{
    public static class ToDataTableHelper
    {
        public static DataTable ToDataTable<T>(IList<T> data)
        {

            var table = new DataTable();
            using (var reader = ObjectReader.Create(data))
            {
                table.Load(reader);
            }

            return table;
        }

        public static DataTable ToDataTable<T>(IList<T> data, params string[] properties)
        {


            var table = new DataTable();
            using (var reader = ObjectReader.Create(data, properties))
            {
                table.Load(reader);
            }

            return table;
        }
    }
}
