export class attendanceModel{
    Id!: number | 0;  
    DivisionID!: number | 0;
    DepartmentID!: number | 0;
    DesignationID!: number | 0;
    AttendDays!: number | 0;
    EmployeeID!: number | 0;
    AttendanceDate!: string | null;
    EnrollNo!: number | 0;
    Name!: string |null;
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class attendanceModelDTO {
    static model:attendanceModel=new attendanceModel();
    static rules = { 
        EmployeeID: { required :helpers.withMessage('Employee is required', required)},
        // AttendDays: { required :helpers.withMessage('Attend days is required', required)}, 
        AttendanceDate: { required :helpers.withMessage('Attendance month is required', required)}, 
      }    
}
export default attendanceModelDTO