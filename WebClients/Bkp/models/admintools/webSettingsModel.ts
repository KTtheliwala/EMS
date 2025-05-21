export class webSettingsModel{
    Id!: number | 0;  
    Key!:string|"";     
    Value!:string|""; 
    CountryId!: number | 0;  
    CountryName!: string|"";
}
import { required, helpers } from '@vuelidate/validators'
class webSettingsModelDTO {
    static model:webSettingsModel=new webSettingsModel();
    static rules = { 
        Key: { required :helpers.withMessage('Key is required', required)},
      }
}
export default webSettingsModelDTO