import { useAuthDataStore } from "@/store/useAuthDataStore";
import { inject, ref } from "vue";
import router from "@/router";
import * as CryptoJS from 'crypto-js';
class authService {

    readonly CurrentMenulist = ref([]);
    async checkViewPermission(PagePath: any): Promise<boolean> {
        const Menulist = (useAuthDataStore().getAuth || []) as any;
        return (Menulist?.Token ?? "").length == 0 ? false : true;
    }
    async setMenuList(): Promise<any> {
        const authStore = useAuthDataStore() as any 
        if((this.CurrentMenulist.value || []).length == 0 || ( authStore?.IsResetPermissions ?? false))
        {
            this.CurrentMenulist.value =  JSON.parse(this.DecryptMenuData((authStore?.Permissions ?? "")))  as any 
            authStore.setAuthPermissionsReset(false)
        }
    }
    
    DecryptMenuData (decString: any) : any {
    if (decString && decString.trim().length > 0) {
        decString = decString.replace(/ /g,"+")
        const key = CryptoJS.enc.Utf8.parse(process.env.VUE_APP_ENCRYPT_KEY);
        const iv = CryptoJS.enc.Utf8.parse(process.env.VUE_APP_ENCRYPT_IV);
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

    
    
}
export default authService;
const AuthService = new authService()
export {
    AuthService,
}