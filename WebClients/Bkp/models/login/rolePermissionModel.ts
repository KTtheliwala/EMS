export class rolePermissionModel{
    Id!: number | 0;  
    Name!:string|"";     
    IsActive!:boolean|false;
    CommAmt!:number|null;
}
import { required, helpers } from '@vuelidate/validators'
class rolePermissionModelDTO {
    static model:rolePermissionModel=new rolePermissionModel();
    static rules = { 
        Name: { required :helpers.withMessage('Role name is required', required)}
      }
}
export default rolePermissionModelDTO