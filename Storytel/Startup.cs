using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Extensions.Logging;
using Storytel.Models;
using Storytel.Options;
using Storytel.Repository;
using Storytel.Repository.Interface;
using Storytel.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;

namespace Storytel
{
    public class Startup
    {

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
      
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = Configuration["Jwt:Issuer"],
                      ValidAudience = Configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                  };
              });


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddDbContext<StorytelContext>(options => options.UseInMemoryDatabase(databaseName: "Storytel"));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddSingleton<ILoggerManager, LoggerManager>();

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowMyOrigin"));
            //});
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new Info { Title = "Storytel API", Version = "v1" }); });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => {option.RouteTemplate = swaggerOptions.JsonRoute;});
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description); });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseHttpsRedirection();

            DefaultFilesOptions filesOptions = new DefaultFilesOptions();
            filesOptions.DefaultFileNames.Clear();
            filesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(filesOptions);
            app.UseStaticFiles();
            app.UseMvc();
        }


    }
}
