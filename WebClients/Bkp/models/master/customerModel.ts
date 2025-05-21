export class customerModel{
    Id!: number | 0;  
    PartyCode!:string|""; 
    PartyName!:string|"";
    Email!:string|"";
    Address1!:string|"";
    Address2!:string|"";
    City!:string|"";
    MobileNo!:string|"";
    PinCode!:number|null;
    StateCode!:number|null;
    BankName!:string|"";
    ContactName!:string|"";
    STNo!:number|null;
    PanNo!:string|"";
    Address3!:string|"";
    Amount!:number|null;
    BrokerCode!:number|null;
    Area1!:string|"";
    AccountGroupCode!:string|"";
    AreaCode!:string|"";
    SmsNo!:string|"";
    GSTNo!:string|"";
    CollectionMen!:string|"";
    PropritorName!:string|"";
    IsActive!:boolean|false;
}
import { required, helpers } from '@vuelidate/validators'
class customerModelDTO {
    static model:customerModel=new customerModel();
    static rules = { 
        PartyCode: { required :helpers.withMessage('Party code is required', required)},
        PartyName: { required :helpers.withMessage('Party name is required', required)}
      }
}
export default customerModelDTO