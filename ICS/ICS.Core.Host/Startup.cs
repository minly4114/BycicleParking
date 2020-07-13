using ICS.Core.Engine.IProviders;
using ICS.Core.Engine.Providers;
using ICS.Core.Host.BackgroundTask;
using ICS.Core.Host.Data;
using ICS.Core.Host.Engine.IProviders;
using ICS.Core.Host.Engine.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace ICS.Core.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddScoped<IClustersProvider, ClustersProvider>()
                .AddScoped<IParkingsProvider, ParkingsProvider>()
                .AddScoped<IParkingPlacesProvider, ParkingPlacesProvider>()
                .AddScoped<IClientsProvider, ClientsProvider>()
                .AddScoped<ISessionsProvider, SessionsProvider>()
                .AddSingleton<ILog4netProvider, Log4netProvider>();
            services.AddHostedService<BackgroundHostedSessionControl>();

            services.AddScoped(typeof(IDbSetProvider<>), typeof(DbSetProvider<>));
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PostgresContext>(options => options.UseNpgsql(connection));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{Configuration.GetValue<string>("ModuleInfo:Name")} API",
                    Description = $"{Configuration.GetValue<string>("ModuleInfo:Description")}",
                    Contact = new OpenApiContact() { Name = "SakuraGamingStudio", Email = "liza.semenova19980@gmail.com", Url = new Uri("https://vk.com/sakuragamingstudio") }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.TagActionsBy(api =>
                {
                    api.TryGetMethodInfo(out var methodInfo);
                    var attribute = methodInfo.DeclaringType.GetCustomAttribute<DisplayNameAttribute>(true);
                    if (attribute == null) return new string[] { api.ActionDescriptor.RouteValues["controller"] };
                    return new[] { attribute.DisplayName };
                });
                c.DescribeAllEnumsAsStrings();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PostgresContext context, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Configuration.GetValue<string>("ModuleInfo:Name")} API V1");
            });
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogWarning("LogWarning ", ex.Message);
            }
        }
    }
}
