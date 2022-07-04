using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Employment.Persistance.Infrastructure;

namespace Employment.Persistance.Data
{
    public class EmploymentDbContextFactory : DesignTimeDbContextFactoryBase<EmploymentDbContext>
    {
        protected override EmploymentDbContext CreateNewInstance(DbContextOptions<EmploymentDbContext> options)
        {
            return new EmploymentDbContext(options);
        }
    }
}
