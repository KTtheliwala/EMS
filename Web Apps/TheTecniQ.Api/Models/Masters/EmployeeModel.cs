using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using TheTecniQ.API.Models.Common;
using TheTecniQ.Core.Domain.Common;

namespace TheTecniQ.API.Models.Masters
{
    public class EmployeeModel : BaseModel
    {
        public int EmployeeID { get; set; } // Not nullable
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int DesignationID { get; set; } // Not nullable
        public int? BranchID { get; set; }
        public bool IsActive { get; set; } // Not nullable
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public int? EnrollNo { get; set; }
        public int DepartmentID { get; set; } // Not nullable
        public DateTime? DateofBirth { get; set; } // Not nullable
        public string StrDateofBirth { get; set; } // Not nullable        
        public int? Age { get; set; }
        public string RelativeName { get; set; }
        public string RelativeMobileNo { get; set; }
        public string ReferenceBy { get; set; }
        public string Address2 { get; set; }
        public string Pincode { get; set; }
        public string RoomNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string PFAccountNo { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string ESIAccountNo { get; set; }
        public string PancardNo { get; set; }
        public string AdharcardNo { get; set; }
        public string DrivingLicNo { get; set; }
        public string Resident { get; set; }
        public float? BasicSalary { get; set; }
        public string SalaryType { get; set; }
        public string Sex { get; set; }
        public string ACStatus { get; set; }
        public string VoterID { get; set; }
        public string DivisionName { get; set; }
        public int? DivisionID { get; set; }
        public IFormFile ImageEmployeePhoto { get; set; }
        public IFormFile ImageAadharCardFront { get; set; }
        public IFormFile ImageAadharCardBack { get; set; }
        public IFormFile ImageOtherDoc { get; set; }

        public byte[] ImageEmployeePhoto_byte { get; set; }
        public byte[] ImageAadharCardFront_byte { get; set; }
        public byte[] ImageAadharCardBack_byte { get; set; }
        public byte[] ImageOtherDoc_byte { get; set; }
    }
    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(x => x.EmployeeName).NotNull().NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.EnrollNo).NotNull().NotEmpty().WithMessage("EnrollNo is required.");
            RuleFor(x => x.StrDateofBirth).NotNull().NotEmpty().WithMessage("DOB is required.");
            RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.DesignationID).NotNull().NotEmpty().WithMessage("Designation is required.");
            RuleFor(x => x.DepartmentID).NotNull().NotEmpty().WithMessage("Department is required.");
            RuleFor(x => x.DivisionID).NotNull().NotEmpty().WithMessage("Division is required.");
        }
    }
}
