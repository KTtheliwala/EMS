export class loginModel {
    Username!: string;
    Password!: string;
    Message!: string;
    CaptchaCode!: string;
    CaptchaToken!: string;
}
export class forgotpasswordModel {
    Email!: string;
    CaptchaCode!: string;
    CaptchaToken!: string;
}

import { required, helpers,email } from '@vuelidate/validators'
class loginDTO {
    static model: loginModel = new loginModel();
    static rules = {
        Username: { required: helpers.withMessage('Username is required', required) },
        Password: { required: helpers.withMessage('Password is required', required) },
        CaptchaCode: { required: helpers.withMessage('Captcha code is required', required) }
    }
}
export default loginDTO
export class forgotpasswordModelDTO {
    static model: forgotpasswordModel = new forgotpasswordModel();
    static rules = {
        Email: { required :helpers.withMessage('Username is required', required) }, //,email:helpers.withMessage("Invalid Email",email)
        CaptchaCode: { required: helpers.withMessage('Captcha code is required', required) }
    }
}