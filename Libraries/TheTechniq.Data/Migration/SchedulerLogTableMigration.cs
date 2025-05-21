using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Masters;
using TheTecniQ.Data.Migrations;

namespace TheTecniQ.Data.Migration
{
    [MigrationInfo("2024-08-28 13:37:48", "Bidding Details")]
    public class SchedulerLogTableMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            #region Bidding Details
            Create
                    .Table(nameof(SchedulerLog)).WithDescription("SchedulerLog")
                    .WithColumn(nameof(SchedulerLog.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
                    .WithColumn(nameof(SchedulerLog.Date)).AsCustom("datetime").NotNullable().WithColumnDescription("Date and time of the bid")
                    .WithColumn(nameof(SchedulerLog.LogDetail)).AsCustom("nvarchar(max)").Nullable().WithColumnDescription("Date and time of the bid")
                    .WithColumn(nameof(SchedulerLog.Type)).AsInt32().NotNullable().WithColumnDescription("Bid Quantity")
                    .WithColumn(nameof(SchedulerLog.CreatedBy)).AsInt32().NotNullable().WithColumnDescription("Created By")
                     .WithColumn(nameof(SchedulerLog.CreatedDate)).AsCustom("datetime").NotNullable().WithColumnDescription("Created Date")
                .WithColumn(nameof(SchedulerLog.ModifyBy)).AsInt32().Nullable().WithColumnDescription("Modified By")
                .WithColumn(nameof(SchedulerLog.ModifyDate)).AsCustom("datetime").Nullable().WithColumnDescription("Modified Date");
            #endregion
        }

        public override void Down()
        {

        }

    }
}
