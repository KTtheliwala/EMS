using FluentValidation;
using System;

namespace TheTecniQ.Api.Models.Reading
{
    public class tblDBUnitsModel
    {
        public int DBUnitID { get; set; }
        public DateTime? DBUnitDate { get; set; }
        public int? DBID { get; set; }
        public float? Units { get; set; }
    }
    public class tblDBUnitsModelValidator : AbstractValidator<tblDBUnitsModel>
    {
        public tblDBUnitsModelValidator()
        {
            RuleFor(x => x.DBID).NotNull().NotEmpty().WithMessage("DB is required.");
            RuleFor(x => x.DBUnitDate).NotNull().NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.Units).NotNull().NotEmpty().GreaterThan(0).WithMessage("Unit is required.");
        }
    }
}
