using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Salary.DataAccess;
using Salary.DataAccess.InMemory;
using Salary.Models;
using Salary.WebApi.Middleware;
using System;

namespace Salary.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterType<InMemoryEmployeeRepository>().As<IEmployeeRepository>().SingleInstance();
            builder.RegisterType<InMemoryTimeCardRepository>().As<IEntityForEmployeeRepository<TimeCard>>().SingleInstance();
            builder.RegisterType<InMemorySalesReceiptRepository>().As<IEntityForEmployeeRepository<SalesReceipt>>().SingleInstance();

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMvc();
        }
    }
}
