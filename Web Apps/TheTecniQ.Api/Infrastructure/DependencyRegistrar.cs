using Microsoft.Extensions.DependencyInjection;
using TheTecniQ.Services.Users;
using TheTecniQ.Services.Logging;
using TheTecniQ.Services.Messaging;
using TheTecniQ.Services.Notification;
using System.Security;
using TheTecniQ.Services.Permission;
using TheTecniQ.Services.Common;
using TheTecniQ.Services.Barcode;
using TheTecniQ.Services.Employees;
using TheTecniQ.Services.Masters;
using TheTecniQ.Services.Attendance;
using TheTecniQ.Services.Reading;
using TheTecniQ.Services.Expense;
using TheTecniQ.Services.Dyeing;
using TheTecniQ.Services.DyeingProcess;
using TheTecniQ.Services.Package;

namespace TheTecniQ.API.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public static class DependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        public static void Register(IServiceCollection services)
        {
            /*
                - Transient operations are always different, a new instance is created with every retrieval of the service.
                - Scoped operations change only with a new scope, but are the same instance within a scope.
                - Singleton operations are always the same, a new instance is only created once.
                - The services are created by the service container and disposed automatically
             */
            /*
                - https://devblogs.microsoft.com/cesardelatorre/comparing-asp-net-core-ioc-service-life-times-and-autofac-ioc-instance-scopes/
                - InstancePerDependency = AddTransient
                - InstancePerLifetimeScope, InstancePerRequest = AddScoped
                - SingleInstance = AddSingleton
             */

            #region Common

            //services.AddScoped<IDataProvider, MsSqlDataProvider>();
            //services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>)); 
            //services.AddScoped<IMessagingService, MessagingService>();

            #endregion

            #region User

            services.AddScoped(typeof(ICommonService<>), typeof(CommonService<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPasswordHistoryService, UserPasswordHistoryService>();
            services.AddScoped<IUserVerificationCodeService, UserVerificationCodeService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            #endregion

            #region Master
           services.AddSingleton<BarcodeService>();
            
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeImagesService, EmployeeImagesService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IActivationService, ActivationService>();
            #endregion

            #region Logging

            services.AddScoped<ILogService, LogService>();

            #endregion

            #region Notification
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IMailQueueService, MailQueueService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<ItblDGVCLDBUnitsService, tblDGVCLDBUnitsService>();
            services.AddScoped<IEMS_tblEmployeeExpenseService, EMS_tblEmployeeExpenseService>();
            services.AddScoped<IPlanningwisePendingOrdersService, PlanningwisePendingOrdersService>();
            services.AddScoped<IDyeingProductionPlanService, DyeingProductionPlanService>();
            services.AddScoped<IPackageServices, PackageServices>();
            #endregion
        }
    }
}

