using System;
using FluentValidation;
using TheTecniQ.API.Models.Common;

namespace TheTecniQ.API.Models.Users
{
    public class RoleModel : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Role name is required");
        }
    }
}
