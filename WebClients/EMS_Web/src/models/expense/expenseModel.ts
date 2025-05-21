export class expenseModel{
    Id!: number | 0;      
    Amount!: number | 0;
    EmployeeID!: number | 0;
    EnrollNo!: number | 0;
    ExpenseDate!: string | null;    
}
export class employeeActivationCheck{    
    EmployeeID!: number | 0;
    ExpenseDate!: string | null;    
}
import { required, helpers } from '@vuelidate/validators'
class expenseModelDTO {
    static model:expenseModel=new expenseModel();
    static rules = { 
        EmployeeID: { required :helpers.withMessage('Employee is required', required)},        
        ExpenseDate: { required :helpers.withMessage('Date is required', required)}, 
        Amount: { required :helpers.withMessage('Amount is required', required)}, 
      }    
}
export default expenseModelDTO