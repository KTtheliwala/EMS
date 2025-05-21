export class usersModel{
    Id!: number | 0; 
    RoleId!: number | 0;
    UserName!: string | "";
    Password!: string | ""; 
    ConfirmPassword!: string | "";
    FirstName!: string | "";
    LastName!:string|"";    
    Email!: string | "";
    Mobile!: string | "";    
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers, maxLength, minLength, email } from '@vuelidate/validators'
class usersModelDTO {
    static model:usersModel=new usersModel();
    static rules = { 
        RoleId: { required: helpers.withMessage('Role name is required', required) },       
        UserName: { required :helpers.withMessage('User name is required', required)},
        FirstName: { required :helpers.withMessage('Name is required', required)},
        //Mobile: { required: helpers.withMessage('Mobile number is required', required), minLength: helpers.withMessage("Please enter valid mobile number", minLength(7)), maxLength: helpers.withMessage("Please enter valid mobile number", maxLength(11)) },
        //Email: { required: helpers.withMessage('Email is required', required), email: helpers.withMessage("Invalid Email", email) },
      }
}
export default usersModelDTO