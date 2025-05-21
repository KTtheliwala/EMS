export class emailTemplateModel{
    Id!: number | 0;  
    FromEmail!:string|"";
    FromName!:string|"";
    TemplateCode!:string|"";
    TemplateSubject!:string|"";
    TemplateBody!:string|"";
    MailCCRoleIds!:string|"";
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class emailTemplateModelDTO {
    static model:emailTemplateModel=new emailTemplateModel();
    static rules = { 
        FromEmail: { required :helpers.withMessage('From email is required', required)},
        FromName: { required :helpers.withMessage('From name is required', required)},
        TemplateCode: { required :helpers.withMessage('Template Code is required', required)},
        TemplateSubject: { required :helpers.withMessage('Template Subject is required', required)},
        TemplateBody: { required :helpers.withMessage('Template Body is required', required)},
      }
}
export default emailTemplateModelDTO