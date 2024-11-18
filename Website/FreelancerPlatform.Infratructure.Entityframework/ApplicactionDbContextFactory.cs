using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework
{
    public class ApplicactionDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connecttionString = "Server=.;Database=freelancer_platform_v2;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true;";
            builder.UseSqlServer(connecttionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
