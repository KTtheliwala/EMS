export class resetPasswordModel {
    UserId!: number | 0; 
    NewPassword!: string|"";
    ConfirmPassword!: string|"";
    EncryptedId!: string|"";
}

import { required, helpers,minLength,sameAs } from '@vuelidate/validators'
class resetPasswordDTO {
    static model: resetPasswordModel = new resetPasswordModel();
    static rules = {
          ConfirmPassword: {
            required: helpers.withMessage("Confirm Password is required.", required),
            minLength: helpers.withMessage(
              "Confirm Password minimum 8 character is required.",
              minLength(8)
            ),
          }
    }
}
export default resetPasswordDTO