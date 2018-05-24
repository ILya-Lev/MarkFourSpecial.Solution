using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Salary.DataAccess;
using Salary.DataAccess.Implementation;
using Salary.DataAccess.OnDisk;
using Salary.Models;
using Salary.Services;
using Salary.Services.Implementation;
using Salary.Services.Implementation.ChargeStrategies;
using Salary.Services.Implementation.Factories;
using Salary.Services.Implementation.PayrollStrategies;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            RegisterOnDiskStorage(builder);
            //RegisterInMemoryStorage(builder);
            RegisterRepositories(builder);
            RegisterServices(builder);

            return new AutofacServiceProvider(builder.Build());
        }

        private void RegisterOnDiskStorage(ContainerBuilder builder)
        {
            builder.RegisterType<OnDiskStorage<Employee>>().As<IStorage<Employee>>().SingleInstance();
            builder.RegisterType<OnDiskStorage<EntityForEmployee>>().As<IStorage<EntityForEmployee>>()
                .SingleInstance();
        }
        //private void RegisterInMemoryStorage(ContainerBuilder builder)
        //{
        //    builder.RegisterType<InMemoryStorage<Employee>>().As<IStorage<Employee>>().SingleInstance();
        //    builder.RegisterType<InMemoryStorage<EntityForEmployee>>().As<IStorage<EntityForEmployee>>()
        //        .SingleInstance();
        //}

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();

            builder.RegisterType<EntityForEmployeeBaseRepository>().As<IEntityForEmployeeBaseRepository>();
            builder.RegisterType<TimeCardRepository>().As<IEntityForEmployeeRepository<TimeCard>>();
            builder.RegisterType<SalesReceiptRepository>().As<IEntityForEmployeeRepository<SalesReceipt>>();
            builder.RegisterType<ServiceChargeRepository>().As<IEntityForEmployeeRepository<ServiceCharge>>();
            builder.RegisterType<SalaryPaymentRepository>().As<IEntityForEmployeeRepository<SalaryPayment>>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<TradeUnionChargeStrategy>().As<ITradeUnionChargeStrategy>();
            builder.RegisterType<NoneChargeStrategy>().As<INoneChargeStrategy>();
            builder.RegisterType<ChargeStrategyFactory>().As<IChargeStrategyFactory>();

            builder.RegisterType<HourlyPayrollStrategy>().As<IHourlyPayrollStrategy>();
            builder.RegisterType<MonthlyPayrollStrategy>().As<IMonthlyPayrollStrategy>();
            builder.RegisterType<CommissionedPayrollStrategy>().As<ICommissionedPayrollStrategy>();
            builder.RegisterType<PayrollStrategyFactory>().As<IPayrollStrategyFactory>();

            builder.RegisterType<SalaryCalculationService>().As<ISalaryCalculationService>();
        }
    }
}
