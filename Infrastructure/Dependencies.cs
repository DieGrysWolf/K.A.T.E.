using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void ConfigureApiServices(IConfiguration configuration, IServiceCollection services)
        {
            var connectionString = configuration.GetConnectionString("MariaDbConnectionString");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<MariaDBContext>(options =>
                options.UseMySql(
                connectionString,
                serverVersion,
                mysqlOptions =>
                {
                    mysqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                }));
            }
    }
}
