export class fieldMappingModel{
    Id!: number | 0;    
    CompanyId!: number | 0;   
    DbFieldId!: number | 0;
    CompanyFieldId!: number | 0;
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers } from '@vuelidate/validators'
class fieldMappingModelDTO {
    static model:fieldMappingModel=new fieldMappingModel();
    static rules = { 
        CompanyFieldId: { required :helpers.withMessage('Company field Name is required', required)},
        DbFieldId: { required :helpers.withMessage('Database field Name is required', required)},
        CompanyId: { required :helpers.withMessage('Company is required', required)}
      }
}
export default fieldMappingModelDTO