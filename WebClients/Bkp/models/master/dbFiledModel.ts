export class dbFiledModel{
    Id!: number | 0;  
    Name!:string|""; 
    SortOrder!:number|0;
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class dbFiledModelDTO {
    static model:dbFiledModel=new dbFiledModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default dbFiledModelDTO