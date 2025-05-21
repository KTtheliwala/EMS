using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Attendance;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Core.Domain.Settings;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Data.Migrations;

namespace TheTecniQ.Data.Migration
{
    [MigrationInfo("2025-03-09 11:50:01", "Alter EMS_tblEmployeeAttendance Master")]
    public class CompanyOrderMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            //#region ImageType




            //Create
            //    .Table(nameof(EMS_tblEmployeeAttendance)).WithDescription("EMS_tblEmployee Expense")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.EmployeeID)).AsInt32().Nullable().WithColumnDescription("Employee Id")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.AttendanceDate)).AsDateTime().Nullable().WithColumnDescription("")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.AttendDays)).AsCustom("decimal(12,2)").Nullable().WithColumnDescription("")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.TotalExpenseAmount)).AsCustom("decimal(12,2)").Nullable().WithColumnDescription("")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.TotalAmount)).AsCustom("decimal(12,2)").Nullable().WithColumnDescription("")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.SalaryType)).AsCustom("nvarchar(max)").Nullable().WithColumnDescription("")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.CreatedBy)).AsInt32().Nullable().WithColumnDescription("Created by")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.CreatedDate)).AsDateTime().Nullable().WithColumnDescription("Created date time")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.ModifyBy)).AsInt32().Nullable().WithColumnDescription("Modify By")
            //    .WithColumn(nameof(EMS_tblEmployeeAttendance.ModifyDate)).AsDateTime().Nullable().WithColumnDescription("Modify Date");


            if (!Schema.Table(nameof(EMS_tblEmployeeExpense)).Column(nameof(EMS_tblEmployeeExpense.EnrollNo)).Exists())
            {
                  Alter.Table(nameof(EMS_tblEmployeeExpense)).AddColumn(nameof(EMS_tblEmployeeExpense.EnrollNo)).AsInt32().Nullable().WithColumnDescription("");
                  //Alter.Table(nameof(EMS_tblEmployeeAttendance)).AddColumn(nameof(EMS_tblEmployeeAttendance.Remarks)).AsCustom("nvarchar(1000)").Nullable().WithColumnDescription("");
                //Alter.Table(nameof(EMS_tblEmployeeAttendance)).AddColumn(nameof(EMS_tblEmployeeAttendance.PayableAmount)).AsCustom("decimal(12,2)").Nullable().WithColumnDescription("");
                //Alter.Table(nameof(EMS_tblEmployeeAttendance)).AddColumn(nameof(EMS_tblEmployeeAttendance.DivisionName)).AsCustom("nvarchar(500)").Nullable().WithColumnDescription("");
                //Alter.Table(nameof(EMS_tblEmployeeAttendance)).AddColumn(nameof(EMS_tblEmployeeAttendance.DesignationName)).AsCustom("nvarchar(500)").Nullable().WithColumnDescription("");
                //Alter.Table(nameof(EMS_tblEmployeeAttendance)).AddColumn(nameof(EMS_tblEmployeeAttendance.DepartmentName)).AsCustom("nvarchar(500)").Nullable().WithColumnDescription("");
            }
            //if (!Schema.Table(nameof(OrderMaster)).Column(nameof(OrderMaster.City)).Exists())
            //{
            //    Alter.Table(nameof(OrderMaster)).AddColumn(nameof(OrderMaster.City)).AsCustom("nvarchar(250)").Nullable().WithColumnDescription("");
            //}
            //if (!Schema.Table(nameof(OrderMaster)).Column(nameof(OrderMaster.State)).Exists())
            //{
            //    Alter.Table(nameof(OrderMaster)).AddColumn(nameof(OrderMaster.State)).AsCustom("nvarchar(250)").Nullable().WithColumnDescription("");
            //}
            //if (!Schema.Table(nameof(OrderMaster)).Column(nameof(OrderMaster.PostCode)).Exists())
            //{
            //    Alter.Table(nameof(OrderMaster)).AddColumn(nameof(OrderMaster.PostCode)).AsCustom("nvarchar(50)").Nullable().WithColumnDescription("");
            //}
            //if (!Schema.Table(nameof(OrderMaster)).Column(nameof(OrderMaster.CountryId)).Exists())
            //{                
            //    Alter.Table(nameof(OrderMaster)).AddColumn(nameof(OrderMaster.CountryId)).AsInt32().Nullable().ForeignKey(nameof(CountryMaster), nameof(CountryMaster.Id)).WithColumnDescription("Role Id");
            //}
            //#endregion
        }


        public override void Down()
        {

        }

    }
}
