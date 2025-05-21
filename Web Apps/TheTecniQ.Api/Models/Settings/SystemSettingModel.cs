using FluentValidation;
using System.Collections.Generic;
using TheTecniQ.API.Models.Common;

namespace TheTecniQ.API.Models.Settings
{
    public class SettingModel : BaseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }    
    public class SettingModelValidator : AbstractValidator<SettingModel>
    {
        public SettingModelValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty().WithMessage("Key is required.");
        }
    }
}
