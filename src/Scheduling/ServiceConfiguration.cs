using Application.Common.Interfaces;
using Hangfire;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scheduling.Jobs;
using Scheduling.Services;

namespace Scheduling
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection secvices, IConfiguration configuration)
        {
            secvices.AddDbContextFactory<HangfireDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HangfireConnection")));

            var serviceProvider = secvices.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider
                    .GetRequiredService<IDbContextFactory<HangfireDbContext>>();

                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    dbContext.EnsureDatabaseCreated();
                }
            }

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));

            secvices.AddHangfire(cfg => cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            secvices.AddDbContext<AcademyDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b=>b.MigrationsAssembly(typeof(AcademyDbContext).Assembly.FullName)));

            secvices.AddScoped<IAcademyDbContext>(provider => provider.GetService<AcademyDbContext>());

            secvices.AddScoped<ISchedulingService, SchedulingService>();
            //secvices.AddScoped<IScheduleJob, ScheduleJob>();
            secvices.AddSingleton<ScheduleJob, ScheduleJob>();

            secvices.AddHangfireServer();

        }
    }
}
