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

    getModulePagePath(ModuleName: any,DefaultPagecode:any):any {
        
        this.setMenuList()
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return e.ModuleName === ModuleName && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        let PagePath = ''
        let  result1 = [] as  any 
        (router?.getRoutes() ?? []).filter((e: any) => {
            (result ||[]).filter(function(item:any) {
                if((e?.meta?.ModuleName ?? [] as any).includes((item?.ModuleName ?? '')) &&  
                (e?.meta?.PageCode ?? [] as any).includes((item?.PageCode ?? '')))
                {
                    result1.push(e);
                } 
             });
        }) as any;
        if((result1 || []).length == 0)
        {
            const result2 = (router?.getRoutes() ?? []).filter((e: any) => {
                return (e?.meta?.ModuleName ?? [] as any).includes(ModuleName  ?? '')
            }) as any;
            PagePath=(result2[0]?.path ?? '')
        }
        else
        {
            PagePath=(result1[0]?.path ?? '')
        }
        
        return (result || []).length == 0 ? '' : PagePath;
    }
    CheckPermissionsPageWise(ModuleName: any,PageCode: any): boolean {        
        
        this.setMenuList()
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return e.ModuleName === ModuleName && e.PageCode === PageCode && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        return (result || []).length == 0 ? false : result[0]?.IsView ?? false;

    }
    CheckMultipleModuleAndPage(Modules: any,PageCode: any): boolean {
        this.setMenuList()
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return Modules.includes(e.ModuleName) && PageCode.includes(e.PageCode) && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        return (result || []).length == 0 ? false : result[0]?.IsView ?? false;
    }
    

    

     CheckPermissionsModuleWise(ModuleName: any):boolean {
        this.setMenuList();        
        
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return e.ModuleName === ModuleName && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        
        return (result || []).length == 0 ? false : result[0]?.IsView ?? false;
    }
  getMultipleModulePagePath(ModuleName: any,PageCode:any,DefaultPagecode:any,TabName:any):any 
    {
        this.setMenuList()
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return ModuleName.includes(e.ModuleName) && PageCode.includes(e.PageCode) && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        let  result1 = [] as  any 
        //let r = router?.getRoutes() as any
         (router?.getRoutes() ?? []).filter((e: any) => {

            (result ||[]).filter(function(item:any) {

                if((e?.meta?.ModuleName ?? [] as any).includes((item?.ModuleName ?? '')) &&  
                (e?.meta?.PageCode ?? [] as any).includes((item?.PageCode ?? '')))
                {
                    //debugger
                    result1.push(e);
                } 
             });
        }) as any;
        let path = (result1[0]?.path ?? '')
        if((result1 || []).length > 1)
        {
            const CheckResult1 = (result1 ?? []).filter((e: any) => {
                return (e?.meta?.TabName ?? '' as any) === TabName 
            }) as any;
            path= (CheckResult1[0]?.path ?? '')
        }
        return (result || []).length == 0 ? '' : (path ?? '');
    }
    GetPagePermission(ModuleName: any,PageCode: any): any {
        this.setMenuList()
        const result = (this.CurrentMenulist.value ?? []).filter((e: any) => {
            return e.ModuleName.toLowerCase() === ModuleName.toLowerCase() && e.PageCode.toLowerCase() === PageCode.toLowerCase() && e.PageCode !== 'NoPage' && e.IsView;
        }) as any;
        return result[0];
    }
    
}
export default authService;
const AuthService = new authService()
export {
    AuthService,
}