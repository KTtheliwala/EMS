export class categoryModel{
    Id!: number | 0;  
    Name!:string|""; 
    Description!: string | "";
    ShortDescription!: string | "";
    Address3!: string | ""
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}
import { required, helpers } from '@vuelidate/validators'
class categoryModelDTO {
    static model:categoryModel=new categoryModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default categoryModelDTO