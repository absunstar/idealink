using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Persistance.Infrastructure;

namespace Tadrebat.Persistance.Data
{
    public class TadrebatDbContextFactory : DesignTimeDbContextFactoryBase<TadrebatDbContext>
    {
        protected override TadrebatDbContext CreateNewInstance(DbContextOptions<TadrebatDbContext> options)
        {
            return new TadrebatDbContext(options);
        }
    }
}
