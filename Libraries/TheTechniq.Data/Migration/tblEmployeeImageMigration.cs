using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Masters;
using TheTecniQ.Data.Migrations;

namespace TheTecniQ.Data.Migration
{
    [MigrationInfo("2024-12-12 10:05:00", "tblEmployee")]
    public class tblEmployeeImageMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            #region EmployeeImages
            Create
                    .Table(nameof(tblEmployeeImage)).WithDescription("tblEmployeeImage")
                    .WithColumn(nameof(tblEmployeeImage.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
                    .WithColumn(nameof(tblEmployeeImage.EmployeeID)).AsInt32().Nullable().WithColumnDescription("Employee ID")
                    .WithColumn(nameof(tblEmployeeImage.ImageEmployeePhoto)).AsCustom("image").Nullable().WithColumnDescription("Image Employee Photo")
                    .WithColumn(nameof(tblEmployeeImage.ImageAadharCardFront)).AsCustom("image").Nullable().WithColumnDescription("Image Aadhar Front Photo")
                    .WithColumn(nameof(tblEmployeeImage.ImageAadharCardBack)).AsCustom("image").Nullable().WithColumnDescription("Image Aadhar Back Photo")
                    .WithColumn(nameof(tblEmployeeImage.ImageOtherDoc)).AsCustom("image").Nullable().WithColumnDescription("Image Aadhar Other Photo")                    
                    .WithColumn(nameof(tblEmployeeImage.CreatedBy)).AsInt32().Nullable().WithColumnDescription("Created By")
                    .WithColumn(nameof(tblEmployeeImage.CreatedDate)).AsCustom("datetime").Nullable().WithColumnDescription("Created Date")
                    .WithColumn(nameof(tblEmployeeImage.UpdatedBy)).AsInt32().Nullable().WithColumnDescription("Modified By")
                    .WithColumn(nameof(tblEmployeeImage.UpdatedDate)).AsCustom("datetime").Nullable().WithColumnDescription("Modified Date");
            #endregion
        }

        public override void Down()
        {

        }

    }
}
