export class groupModel{
    Id!: number | 0;  
    Name!:string|""; 
    SortOrder!:number|0;
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class groupModelDTO {
    static model:groupModel=new groupModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default groupModelDTO