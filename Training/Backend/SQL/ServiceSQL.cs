using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace SQL
{
    public class ServiceSQL
    {

        public IConfiguration _Configuration { get; }
        public ServiceSQL(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public async Task<bool> GeneratePartnerAccountView(string PartnerId, string PartnerPassword)
        {
            var param = new ArrayList();
            param.Add(new SqlParameter("@PartnerId", PartnerId));
            param.Add(new SqlParameter("@Password", PartnerPassword));

            ExecCommand("GeneratePartnerAccountView", param);
            return true;
            //return await Task.Run(() => ConvertDataTable<ReportBilling>(dt));
        }
        private DataTable ExecCommand(string SPName, ArrayList param)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection sqlConn = new SqlConnection(_Configuration.GetConnectionString("ProjectDbContext")))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SPName, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        foreach (var obj in param)
                        {
                            sqlCmd.Parameters.Add(obj);
                        }

                        sqlConn.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }

        }
        private DataTable GetDataTable(string SPName, ArrayList param)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand sqlCmd = new SqlCommand(SPName, sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    foreach (var obj in param)
                    {
                        sqlCmd.Parameters.Add(obj);
                    }

                    sqlConn.Open();
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
