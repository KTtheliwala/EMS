export class subGroupModel{
    Id!: number | 0;  
    GroupId!: number | 0;
    Name!:string|""; 
    SortOrder!:number|0;
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class subGroupModelDTO {
    static model:subGroupModel=new subGroupModel();
    static rules = { 
        GroupId: { required :helpers.withMessage('Group is required', required)},
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default subGroupModelDTO