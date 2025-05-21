import { defineStore } from "pinia";
import { useCookies } from 'vue3-cookies'
const { cookies } = useCookies();
export const useAuthDataStore = defineStore("useAuthData", {
    state: () => {
        return {
            userModel: null,
            productAuthModel: null,
            AuthMessage: "",
            Permissions:"",
            IsResetPermissions:false,
        };
    },
    persist: true,
    getters: {
        getAuth(state) {
            return state.userModel;
        },
        getAccessToken(state) {
            return state.productAuthModel;
        },
        getAuthMsg(state) {
            return state.AuthMessage;
        },
        getAuthPermissions(state) {
            return state.Permissions;
        },
    },
    actions: {
        setAuth(model: any) {            
            this.userModel = model;
            this.productAuthModel = null;  
            this.AuthMessage = "";
            return model;
        },
        setAuthForProduct(authData: any) {
            this.productAuthModel = authData;
            if (authData != null) {
                if (!cookies.isKey("download_token_base"))
                    cookies.remove("download_token_base");
                cookies.set("download_token_base", authData.download_token);
            }
        },
        setAuthMsg(Msg: any) {            
            this.AuthMessage = Msg;
        },
        setAuthPermissions(model: any,IsReset?:any) {            
            this.Permissions = model;
            this.IsResetPermissions = IsReset;
        },
        setAuthPermissionsReset(IsReset:any) {            
            this.IsResetPermissions = IsReset;
        },
    },
});


