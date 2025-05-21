export class employeeAttendanceModel{
    Id!: number | 0;      
    TotalAmount!: number | 0;
    PayableAmount!: number | 0;
    AttendDays!: number | 0;
    EmployeeID!: number | 0;
    AttendanceDate!: string | null;
    SalaryType!: string | null;
    TotalExpenseAmount!: number | 0;    
    BasicSalary!: number | 0;    
    TotalExtraAmount!: number | 0;    
    DivisionName!: string | null;
    Remarks!: string | null;
    DesignationName!: string | null;
    DepartmentName!: string | null;
    rowClass!: string | null;
}
import { required, helpers } from '@vuelidate/validators'
class employeeAttendanceModelDTO {
    static model:employeeAttendanceModel=new employeeAttendanceModel();
    static rules = { 
        EmployeeID: { required :helpers.withMessage('Employee is required', required)},
        // AttendDays: { required :helpers.withMessage('Attend days is required', required)}, 
        AttendanceDate: { required :helpers.withMessage('Attendance month-year is required', required)}, 
        AttendDays: { required :helpers.withMessage('Attend Days is required', required)}, 
        TotalAmount: { required :helpers.withMessage('Total Amount is required', required)}, 
        PayableAmount: { required :helpers.withMessage('Payable Amount is required', required)}, 
      }    
}
export default employeeAttendanceModelDTO