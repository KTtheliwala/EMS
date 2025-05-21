export class changePasswordModel{    
    Oldpassword!:string|"";
    Newpassword!:string|"";
    Confirmpassword!:string|"";
}
import { required, helpers, minLength } from '@vuelidate/validators'
class changePasswordModelDTO {
    static model:changePasswordModel=new changePasswordModel();
    static rules = {            
        Oldpassword: { required :helpers.withMessage('Old Password is required', required)},
        //Newpassword : { required :helpers.withMessage('New Password is required', required), minLength:helpers.withMessage("New Password minimum 5 character is required",minLength(5))},
        Confirmpassword: { required :helpers.withMessage('Confirm Password is required', required)},
      }
}
export default changePasswordModelDTO