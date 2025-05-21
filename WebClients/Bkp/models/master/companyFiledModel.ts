export class companyFiledModel{
    Id!: number | 0;  
    Name!:string|""; 
    FieldFormat!:string|""; 
    SortOrder!:number|0;
    FieldTypeId!:number|0;
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class companyFiledModelDTO {
    static model:companyFiledModel=new companyFiledModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default companyFiledModelDTO