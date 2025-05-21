export class employeeModel{
    EmployeeID!: number | 0;
    EmployeeName!: string | "";
    MobileNo!: string | "";
    Address!: string | "";
    StrDateofBirth!: string | "";
    DesignationID!: number | 0;
    BranchID!: number | 0;
    IsActive!: boolean | false;    
    EnrollNo!: number | 0;
    DepartmentID!: number | 0;
    DateofBirth!:  Date ;
    Age!: number | 0;
    RelativeName!: string | "";
    RelativeMobileNo!: string | "";
    ReferenceBy!: string | "";
    Address2!: string | "";
    Pincode!: string | "";
    RoomNo!: string | "";
    PhoneNo!: string | "";
    Email!: string | "";
    BankName!: string | "";
    BankAccountNo!: string | "";
    PFAccountNo!: string | "";
    NomineeName!: string | "";
    NomineeRelationship!: string | "";
    ESIAccountNo!: string | "";
    PancardNo!: string | "";
    AdharcardNo!: string | "";
    DrivingLicNo!: string | "";
    Resident!: string | "";
    BasicSalary!: number | 0;
    SalaryType!: string | "";
    Sex!: string | "";
    ACStatus!: string | "";
    VoterID!: string | "";
    DivisionID!: number | 0;
    ImageEmployeePhoto!: File | null;
    ImageAadharCardFront!: File | null;
    ImageAadharCardBack!: File | null;
    ImageOtherDoc!: File | null;
}
import { required, helpers } from '@vuelidate/validators'
class employeeModelDTO {
    static model:employeeModel=new employeeModel();
    static rules = { 
        EmployeeName: { required :helpers.withMessage('Employee  name is required', required)},
        StrDateofBirth: { required :helpers.withMessage('DOB is required', required)},
        Address: { required :helpers.withMessage('Address is required', required)},
        DesignationID: { required :helpers.withMessage('Designation is required', required)},
        DepartmentID: { required :helpers.withMessage('Department is required', required)},
        DivisionID: { required :helpers.withMessage('Division is required', required)},
        EnrollNo: { required :helpers.withMessage('EnrollNo. is required', required)},
        SalaryType: { required :helpers.withMessage('Salary Type is required', required)},
        BasicSalary: { required :helpers.withMessage('Basic Salary is required', required)},
      }
}
export default employeeModelDTO