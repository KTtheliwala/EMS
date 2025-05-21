export class transpoterModel{
    Id!: number | 0;  
    ContactName!:string|""; 
    Email!:string|"";     
    Mobile!:string|""; 
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class transpoterModelDTO {
    static model:transpoterModel=new transpoterModel();
    static rules = { 
        ContactName: { required :helpers.withMessage('Name is required', required)}
      }
}
export default transpoterModelDTO