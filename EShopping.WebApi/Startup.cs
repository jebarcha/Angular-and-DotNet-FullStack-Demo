using AutoMapper;
using EShopping.Data;
using EShopping.Data.Contracts;
using EShopping.Data.Repositories;
using EShopping.Models;
using EShopping.WebApi.Extensions;
using EShopping.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;

namespace EShopping.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration) 
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers(config => {
                config.ReturnHttpNotAcceptable = true;          
            })
                .AddXmlDataContractSerializerFormatters();

            services.AddDbContext<EShoppingDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("EShoppingDb")));


            //var coonectionString = "Data Source=local\\MSSQLSERVER01;Initial Catalog=EShoppingDb;Integrated Security=True";
            //services.AddDbContext<EShoppingDbContext>(options => options.UseSqlServer(coonectionString, builder => builder.UseRowNumberForPaging()));




            services.ConfigureDependencies();

            services.ConfigureJwt(Configuration);

            services.ConfigureCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
