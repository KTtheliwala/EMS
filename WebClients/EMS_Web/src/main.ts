import Application from "@/composables/configuration";
import moment from "moment";
import * as CryptoJS from 'crypto-js';
import { ref } from "vue";
const app = new Application();
app.mount();
//-------------------------------------------Global functions --------------------------------
const ConvertToUserTimeZone = (date: Date, format = "DD MMM YYYY",isSkipTimeZone=false,Offset=0) => {
    if(!date)
        return "";
    if(isSkipTimeZone)
        return moment(date).format(format);
    else {        
        const timezone = JSON.parse(localStorage.getItem('useAuthData')?.toString() ?? "");
        if (timezone && (timezone?.userModel?.Offset ?? "") != "")
            return moment.utc(date).utcOffset(timezone.userModel.Offset).format(format);
        else if(Offset > 0)
            return moment.utc(date).utcOffset(Offset).format(format);
        else
            return moment.utc(date).local().format(format);
    }
}
const key = CryptoJS.enc.Utf8.parse(process.env.VUE_APP_ENCRYPT_KEY);
const iv = CryptoJS.enc.Utf8.parse(process.env.VUE_APP_ENCRYPT_IV);
const NewEncryptData = (data: string) => {
    if (data && data.trim().length > 0) {
        const Id = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(data), key.toString());
        return encodeURI(Id.toString());
    }
    else
        return "";
}
const NewDecryptData = (decString: string) => {
    if (decString && decString.trim().length > 0) {
        decString = decString.replace(/ /g,"+")
        const Id = CryptoJS.AES.decrypt(decodeURI(decString), key.toString());
        return Id.toString(CryptoJS.enc.Utf8);
    }
    else
        return "";
}
const EncryptData = (data: string) => {
    if (data && data.trim().length > 0) {
        const encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(data), key, {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }) 
        return encodeURIComponent(encrypted.toString());
    }
    else
        return "";
}



const DecryptData = (decString: string) => {
    if (decString && decString.trim().length > 0) {
        decString = decString.replace(/ /g,"+")
        const decrypted = CryptoJS.AES.decrypt(decodeURIComponent(decString), key, {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        return decrypted.toString(CryptoJS.enc.Utf8);
    }
    else
        return "";
}

const GetClassByStatus = (status: any) => {
    if (!status)
        return "";
    if (status.toString() == "5")
        return "na";
    else if (status.toString() == "1")
        return "pending";
    else if (status.toString() == "2" || status.toString() == "3")
        return "completed";
    else if (status.toString() == "4")
        return "rejected";
    else return "";
}
const GetMainRoute = (currRoute: any) => {
    if (currRoute.name.startsWith("Technical") || (currRoute.meta.parentNode?.startsWith("Technical") ?? false))
        return "technical";
    else if (currRoute.name.startsWith("Operation") || (currRoute.meta.parentNode?.startsWith("Operation") ?? false))
        return "operations";
    else
        return "sales";
}


const checkJosnIsValid = (jsn: any): boolean => {
    try {
        JSON.parse(jsn);
    } catch (e) {
        return false;
    }
    return true;
}

const checkXMLIsValid = (xmlString: any): boolean => {
    if (typeof xmlString !== 'string' || xmlString.trim() === '') {
        return false;
    }

    try {
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlString, 'text/xml');

        // Check for parsing errors
        const parserError = xmlDoc.getElementsByTagName('parsererror');
        return parserError.length === 0;
    } catch (e) {
        return false;
    }
};
const GetDecimalTwoPlace=(obj:any)=>{
    try{
       // return Number(obj.toFixed(2));
        return parseFloat(obj ?? 0).toFixed(2) ?? 0.00;
    } catch(e){
        return 0.00;
    }
}
const GetDecimalUptoFourPlace=(obj:any, noOfDecimal:number)=>{
    try{
       // return Number(obj.toFixed(2));
        return parseFloat(obj ?? 0).toFixed(noOfDecimal);
    } catch(e){
        return parseFloat('0').toFixed(noOfDecimal);
    }
}
app.application.provide("GlobalFunctions", {
    ConvertToUserTimeZone, EncryptData, DecryptData, GetClassByStatus, GetMainRoute,checkJosnIsValid,checkXMLIsValid,
    NewEncryptData,NewDecryptData,GetDecimalTwoPlace,GetDecimalUptoFourPlace
});