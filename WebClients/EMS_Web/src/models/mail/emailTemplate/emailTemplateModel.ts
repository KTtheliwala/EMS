export class EmailTemplateModel {
    Id: number = 0;
    FromEmail: string = "";
    FromName: string = "";
    TemplateCode: string = "";
    TemplateSubject: string = "";
    TemplateBody: string = "";
    IsActive: boolean = false;
}
import { required, helpers, email } from '@vuelidate/validators'
class EmailTemplateModelDTO {
    static readonly  model: EmailTemplateModel = new EmailTemplateModel();
    static readonly rules = {
        FromEmail: {
            required: helpers.withMessage('From email is required', required),
            email: helpers.withMessage('Invalid email format', email)
        },
        FromName: { required :helpers.withMessage('From name is required', required)},
        TemplateCode: { required :helpers.withMessage('Template Code is required', required)},
        TemplateSubject: { required :helpers.withMessage('Template Subject is required', required)},
        TemplateBody: { required :helpers.withMessage('Template Body is required', required)}
    }
}
export default EmailTemplateModelDTO