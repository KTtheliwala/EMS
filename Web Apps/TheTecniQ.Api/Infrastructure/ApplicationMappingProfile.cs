using AutoMapper;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Domain.Settings;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.API.Models.Settings;
using TheTecniQ.API.Models.Notification;
using TheTecniQ.API.Models.Users;
using TheTecniQ.API.Models.Masters;
using TheTecniQ.API.Models.Orders;
using TheTecniQ.Core.Domain.Employees;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.API.Models.Attendance;
using TheTecniQ.Core.Domain.Reading;
using TheTecniQ.Api.Models.Reading;
using TheTecniQ.API.Models.Expense;

namespace TheTecniQ.API.Infrastructure
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            #region  Settings

            CreateMap<SettingModel, EMS_Settings>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion

            #region  Notification

            CreateMap<EmailTemplateModel, EMS_EmailTemplate>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            

            #endregion

            #region  User

            CreateMap<RoleModel, EMS_Role>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<UserModel, EMS_User>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<EMS_User, UserModel>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? null : src.Password))
            .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<ChangePasswordModel, EMS_User>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();



            #endregion


            #region Application API Model
            CreateMap<tblEmployeeDto, tblEmployee>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EmployeeModel, tblEmployee>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<AttendanceActivationModel, EMS_tblAttendanceActivation>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<tblDGVCLDBUnitsModel, tblDGVCLDBUnits>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<tblDBUnitsModel, tblDBUnits>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EMS_tblEmployeeExpenseModel, EMS_tblEmployeeExpense>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EMS_tblEmployeeAttendanceModel, EMS_tblEmployeeAttendance>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            #endregion
        }
    }
}
