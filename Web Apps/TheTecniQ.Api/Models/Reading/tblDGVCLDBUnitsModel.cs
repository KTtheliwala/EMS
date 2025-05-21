using FluentValidation;
using System;

namespace TheTecniQ.Api.Models.Reading
{
    public class tblDGVCLDBUnitsModel
    {
        public int DBDGVCLUnitID { get; set; }        
        public DateTime DBDGVCLUnitDate { get; set; }
        public int? DBPanelID { get; set; }
        public float KWH { get; set; }
        public float KVAH { get; set; }
        public float KVARH { get; set; }
        public float? PF { get; set; }
        public float? Units { get; set; }        
    }
    public class tblDGVCLDBUnitsModelValidator : AbstractValidator<tblDGVCLDBUnitsModel>
    {
        public tblDGVCLDBUnitsModelValidator()
        {
            RuleFor(x => x.DBPanelID).NotNull().NotEmpty().WithMessage("Panel is required.");
            RuleFor(x => x.DBDGVCLUnitDate).NotNull().NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.KWH).NotNull().NotEmpty().GreaterThan(0).WithMessage("KWH is required.");
            RuleFor(x => x.KVAH).NotNull().NotEmpty().GreaterThan(0).WithMessage("KVAH is required.");
            RuleFor(x => x.KVARH).NotNull().NotEmpty().GreaterThan(0).WithMessage("KVARH is required.");
            //RuleFor(x => x.KWHDiff).NotNull().NotEmpty().GreaterThan(0).WithMessage("KWHDiff is required.");
            //RuleFor(x => x.KWHDiff).NotNull().NotEmpty().GreaterThan(0).WithMessage("KWHDiff is required.");
        }
    }
}
