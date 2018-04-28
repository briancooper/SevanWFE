using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Workflow.Abstractions.Database;
using Workflow.Abstractions.Models;
using Workflow.Abstractions.Services;
using Workflow.Application;
using Workflow.Application.Controllers;
using Workflow.Application.Filters;
using Workflow.Application.Utils;
using Workflow.Converter.Services;
using Workflow.Core.ServiceLocator;
using Workflow.Database;
using Workflow.Emailing.Configuration;
using Workflow.Emailing.Services;
using Workflow.Engine.Configuration;
using Workflow.Engine.Services;
using Workflow.Security.Configuration;
using Workflow.Security.Services;
using Workflow.Storage.Configuration;
using Workflow.Storage.Models;
using Workflow.Storage.Services;

namespace Workflow.Web
{
    public class Startup
    {
        private const string LOCALHOST_CORS_POLICY = "localhost";

        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key123"))
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy(LOCALHOST_CORS_POLICY, builder =>
                {
                    builder.WithOrigins(Configuration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddScoped<IBag, Bag>();

            services.AddDbContext<WorkflowContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            Mappers.Initialize();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<ISecurityConfiguration>(new SecurityConfiguration { Issuer = "Issuer", Audience = "Audience", SecretKey = "super_secret_key123" });

            services.AddTransient<ISecurityService, SecurityService>();

            services.AddSingleton<IEngineConfiguration>(new EngineConfiguration(Configuration.GetConnectionString("Default")));

            services.AddTransient<IActionService, ActionService>();

            services.AddTransient<IEngineService, EngineService>();

            services.AddSingleton<IEmailingConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailingConfiguration>());

            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton<IStorageConfiguration>(new StorageConfiguration { Amazon = new AmazonSettings() { Region = RegionEndpoint.USEast1, Directory = "workflow", AccessKey = "AKIAIGBDKSC5BFGCFH5A", SecretKey = "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", BucketName = "xamarin-sevan" } });

            services.AddTransient<IStorageService, StorageService>();

            services.AddTransient<IConverterService, ConverterService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ResultWrapperFilter));
            }).AddApplicationPart(Assembly.Load(new AssemblyName("Workflow.Application")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddResponseCompression();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Workflow API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseCors(LOCALHOST_CORS_POLICY);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ServiceLocator.Init(serviceScope.ServiceProvider);
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<WorkflowContext>();

                context.Database.Migrate();
            }

            app.UseResponseCompression();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow API V1");
            });

            app.UseMvc();
        }
    }
}
