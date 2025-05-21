export class companyModel{
    Id!: number | 0;  
    Name!:string|""; 
    Email!:string|"";     
    MobileNo!:string|""; 
    Address!: string | "";
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers } from '@vuelidate/validators'
class companyModelDTO {
    static model:companyModel=new companyModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default companyModelDTO