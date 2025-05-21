using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Notification;
using TheTecniQ.Data.Migrations;

namespace TheTecniQ.Data.Migration
{
    [MigrationInfo("2025-01-01 12:55:01", "Result")]
    public class NotificationMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            #region Notification

            //Create
            //    .Table(nameof(EMS_EmailTemplate)).WithDescription("Email Templates")
            //    .WithColumn(nameof(EMS_EmailTemplate.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
            //    .WithColumn(nameof(EMS_EmailTemplate.FromEmail)).AsString(400).Nullable().WithColumnDescription("From email id.")
            //    .WithColumn(nameof(EMS_EmailTemplate.FromName)).AsString(100).Nullable().WithColumnDescription("From name.")
            //    .WithColumn(nameof(EMS_EmailTemplate.TemplateCode)).AsString(200).NotNullable().WithColumnDescription("Template code")
            //    .WithColumn(nameof(EMS_EmailTemplate.TemplateSubject)).AsString(400).Nullable().WithColumnDescription("Template subject with token.")
            //    .WithColumn(nameof(EMS_EmailTemplate.TemplateBody)).AsCustom("varchar(max)").Nullable().WithColumnDescription("Template body with token.")
            //    .WithColumn(nameof(EMS_EmailTemplate.IsActive)).AsBoolean().NotNullable().WithDefaultValue(1).WithColumnDescription("Template active or not.");

            //Create
            //    .Table(nameof(EMS_MailQueue)).WithDescription("Mail Queue")
            //    .WithColumn(nameof(EMS_MailQueue.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
            //    .WithColumn(nameof(EMS_MailQueue.FromEmail)).AsString(400).NotNullable().WithColumnDescription("From email id.")
            //    .WithColumn(nameof(EMS_MailQueue.FromName)).AsString(100).Nullable().WithColumnDescription("From name.")
            //    .WithColumn(nameof(EMS_MailQueue.ToEmail)).AsString(400).NotNullable().WithColumnDescription("To email id.")
            //    .WithColumn(nameof(EMS_MailQueue.Cc)).AsString().Nullable().WithColumnDescription("CC of emails")
            //    .WithColumn(nameof(EMS_MailQueue.Bcc)).AsString().Nullable().WithColumnDescription("BCC of emails")
            //    .WithColumn(nameof(EMS_MailQueue.MailSubject)).AsCustom("varchar(max)").NotNullable().WithColumnDescription("Email subject.")
            //    .WithColumn(nameof(EMS_MailQueue.MailBody)).AsCustom("varchar(max)").NotNullable().WithColumnDescription("Email body content.")
            //    .WithColumn(nameof(EMS_MailQueue.AttachmentName)).AsCustom("varchar(max)").Nullable().WithColumnDescription("Attachment Name")
            //    .WithColumn(nameof(EMS_MailQueue.AttachmentLink)).AsCustom("varchar(max)").Nullable().WithColumnDescription("Attachment Link")
            //    .WithColumn(nameof(EMS_MailQueue.CreatedDate)).AsDateTime().NotNullable().WithColumnDescription("Record created date.")
            //    .WithColumn(nameof(EMS_MailQueue.Status)).AsInt16().Nullable().WithColumnDescription("Status of mail.")
            //    .WithColumn(nameof(EMS_MailQueue.Retry)).AsInt16().Nullable().WithColumnDescription("Sending try count after failure.")
            //    .WithColumn(nameof(EMS_MailQueue.LastSendingTry)).AsDateTime().Nullable().WithColumnDescription("Last sending try date.")
            //    .WithColumn(nameof(EMS_MailQueue.LastResponse)).AsCustom("varchar(max)").Nullable().WithColumnDescription("Last try time any message.");

            #endregion
        }

        public override void Down()
        {

        }

    }
}
