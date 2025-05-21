export class roleModel{
    Id!: number | 0;  
    Name!:string|"";    
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers } from '@vuelidate/validators'
class roleModelDTO {
    static model:roleModel=new roleModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default roleModelDTO