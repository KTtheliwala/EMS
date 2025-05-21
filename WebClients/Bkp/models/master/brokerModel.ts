export class brokerModel{
    Id!: number | 0;  
    Name!:string|""; 
    Email!:string|"";     
    MobileNo!:string|"";     
    Address!: string | "";   
    Code!: number | 0;
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers } from '@vuelidate/validators'
class brokerModelDTO {
    static model:brokerModel=new brokerModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default brokerModelDTO