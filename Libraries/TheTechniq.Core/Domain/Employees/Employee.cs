using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTecniQ.Core.Domain.Common;
using TheTecniQ.Core.Domain.Logging;

namespace TheTecniQ.Core.Domain.Employees
{
    public class tblEmployee : BaseEntity
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int? DesignationID { get; set; }
        public int? BranchID { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? EnrollNo { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? DateofBirth { get; set; }
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
        public int? DivisionID { get; set; }        
        [NoColumnMap]
        public string CreatedUserName { get; set; }
        [NoColumnMap]
        public string DivisionName { get; set; }
        [NoColumnMap]
        public string DesignationName { get; set; }
        [NoColumnMap]
        public string DepartmentName { get; set; }
        [NoColumnMap]
        public string DropdownConcateName
        {
            get => $"{EnrollNo}/{EmployeeName}/{DesignationName}/{DepartmentName}";
        }
    }


    public class tblEmployeeDto
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
        public DateTime DateofBirth { get; set; } // Not nullable
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
        public string ImageEmployeePhoto { get; set; }
        public string ImageAadharCardFront { get; set; }
        public string ImageAadharCardBack { get; set; }
        public string ImageOtherDoc { get; set; }
       
    }

    public class SearchEmployeeModel
    {
        public int? DivisionID { get; set; }
        public int? DepartmentID { get; set; }
        public int? DesignationID { get; set; }
        public string Name { get; set; }
        public string EnrollNo { get; set; }
        public bool IsActive { get; set; } = false;
    }
    public class SearchActiveEmployeeModel
    {
        public DateTime filterDate { get; set; }
    }

    public class DropdDownEmployeeModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public int? DesignationID { get; set; }
        public int? BranchID { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? EnrollNo { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? DateofBirth { get; set; }
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
        public int? DivisionID { get; set; }
        public string CreatedUserName { get; set; }
        
        public string DivisionName { get; set; }
       
        public string DesignationName { get; set; }
       
        public string DepartmentName { get; set; }
        
        public string DropdownConcateName
        {
            get => $"{EnrollNo}/{EmployeeName}/{DesignationName}/{DepartmentName}";
        }
    }
}
