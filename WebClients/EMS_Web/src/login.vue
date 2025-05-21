<template>
    <div class="auth-wrapper">
        <div class="auth-content">
            <div class="brand-name">
               <img src="@/assets/images/logo.png" />               
            </div>
            <div class="form-wrapper " v-if="!forgotpassword">
                <common-form :actionUrl="UrlConstants.loginUrl" :v$="v$" :data="state" :successCallBack="loginSuccess" :failureCallBack="loginFailure" successMessage="Logged in successfully.">
                    <!-- <div class="btns-wrapper">
                        <Button class="p-button-aux" label="Sign In with Microsoft" @click="handleAdLogin" />
                    </div> -->
                    
                        <h4 class="f-width text-center">Login</h4>
                    
                    <div class="form-group">
                        <common-input propname="Username" :v$="v$" placeholder="Username" v-model="state.Username"></common-input>
                    </div>
                    <div class="form-group">
                        <common-password propname="Password" :feedback="false" :toggleMask="true" placeholder="Password" :v$="v$" v-model="state.Password"></common-password>
                    </div>
                    <div class="captcha-wrapper">
                        <div class="form-group">
                            <common-input name="CaptchaCode" placeholder="Enter captcha code" :v$="v$" v-model="state.CaptchaCode" :length="4" :IsNumber="false"></common-input>
                        </div>
                        <div class="form-group captchaBox">
                            <div class="captcha-img"><img ref="image" :src="captchUrl" /></div>
                            <commonButton btntype="button" icon="pi pi-refresh" @click.prevent="getCaptcha()"></commonButton>
                        </div>
                    </div>
                    <div class="btns-wrapper">
                        <Button class="btn-primary" label="Sign In" type="submit" />
                        <div class="p-error text-right"> {{authStore.getAuthMsg}}</div>
                    </div>
                    <div class="text-center">
                      <!--   <a class="forgotpassword-link" @click="fnforgotpassword(true)">Forgot Password?</a> -->
                    </div>
                </common-form>
            </div>
            <div class="form-wrapper ResetPassword" v-else>
                <common-form :actionUrl="UrlConstants.ForgotUrl" :v$="v$" :data="forgotpasswordState" :successCallBack="forgotpasswordSuccess" :failureCallBack="forgotpasswordFailure" successMessage="Logged in successfully.">
                    <div class="form-group">
                        <common-input propname="Email" name="Email" :v$="v$" placeholder="Username" v-model="forgotpasswordState.Email"></common-input>
                    </div>
                    <div class="captcha-wrapper">
                        <div class="form-group">
                            <common-input name="CaptchaCode" placeholder="Enter captcha code" :v$="v$" v-model="forgotpasswordState.CaptchaCode" :length="4" :IsNumber="true"></common-input>
                        </div>
                        <div class="form-group captchaBox">
                            <div class="captcha-img"><img ref="image" :src="captchUrl" /></div>
                            <commonButton btntype="button" icon="pi pi-refresh" @click.prevent="getCaptcha()"></commonButton>
                        </div>
                    </div>
                    <div class="btns-wrapper">
                        <Button type="submit" class="btn-primary" label="Reset Password" />
                    </div>
                    <div class="text-center">
                        <a class="forgotpassword-link"
                           @click="fnforgotpassword(false)">Back</a>
                        <p class="text-info">
                            Enter your username to reset the password.
                        </p>
                    </div>
                </common-form>
            </div>
            <div class="version-text">
                <!-- <img class="version-logo" src="@/assets/images/EMSPvt-Logo.png" /> -->
                <h4 class="m-0 mb-1">Employee Management System</h4>
                <p class="text-muted m-0">Version : v1.0.0</p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { inject, ref } from "vue";
    import { useVuelidate } from '@vuelidate/core'
    import UrlConstants from "@/utils/urlconstants";
    import { useAdStore } from "@/store/adStore";
    import loginDTO, { forgotpasswordModelDTO } from "@/models/login/loginModel";
    //import { PublicClientApplication } from "@azure/msal-browser";
    import { useAuthDataStore } from "@/store/useAuthDataStore";
    import router from "@/router";
    import commonModule from "@/composables/modules/commonModule";
    import { useRoute } from "vue-router";

    const route = useRoute();
    const AdStore = useAdStore();
    const forgotpassword = ref(false);
    const mobilenumber = ref();
    const state = ref(loginDTO.model);
    const forgotpasswordState = ref(forgotpasswordModelDTO.model);
    const authStore = useAuthDataStore();
    const isDisabled = ref(false);
    const v$ = ref();
   
    v$.value = useVuelidate(loginDTO.rules, state.value);
    if (forgotpassword.value) {
        v$.value = useVuelidate(forgotpasswordModelDTO.rules, forgotpasswordState.value);
    }
    const { GetData, PostData } = new commonModule();
    const loginSuccess = async (data: any) => {
        setAuthData(data);
    };
    const loginFailure = (data: any) => {
        getCaptcha();
        v$.value.value.$reset();
        state.value.CaptchaCode = "";
    }
    const loginpopupdata = ref<any>({});
   
    const captchUrl = ref("");
    const getCaptcha = async () => {
        state.value.CaptchaCode = "";
        GetData(UrlConstants.captchUrl, (data: any) => {
            captchUrl.value = "data:image/png;base64," + data.Img;
            state.value.CaptchaToken = data.Token;
            forgotpasswordState.value.CaptchaToken = data.Token;
        });
    };
    getCaptcha();
    const setAuthData = (authData: any): void => {
        authStore.setAuth({ IsAd: authData.IsAd, Token: authData.Token, UserName: authData.UserName, CountryId: authData.CountryId, Role: authData.Role, Offset: authData.Offset, UserId: authData.UserId, RoleSName: authData.RoleSName,
        BUs:authData.BUs,ForceChangePassword:authData.ForceChangePassword });
        authStore.setAuthPermissions((authData?.Permissions??""),true)
        if (typeof route.query.redirect != "undefined")
        {
           let Prms =  getUrlVars(route.query?.redirect) as any 
           if(Prms?.Id ?? "" != "")
           {
                router.push({ path : String(route.query?.redirect ?? "Dashboard" ) ,query: { Id: Prms?.Id ?? "" }  });
           }
           else
           {
                router.push({ path : String(route.query?.redirect ?? "Dashboard" )  });
           }

        }
        else
        {
            router.push({ name: "Dashboard" });
        }
    };

    const getUrlVars =  (url:any) =>  {
        let vars = [], hash;
        let hashes = url.slice(url.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
           // vars.push(hash[0]); //to get name before =
            let a =hash[0]
            vars[a] = decodeURIComponent(hashes[i].slice(hashes[i].indexOf('=') + 1));
        }
        return vars;
    }
    const fnforgotpassword = async (data: any) => {
        getCaptcha();
        v$.value = useVuelidate(loginDTO.rules, state.value);
        if (data) {
            forgotpasswordState.value.Email = "";
            forgotpasswordState.value.CaptchaCode = "";
            v$.value = useVuelidate(forgotpasswordModelDTO.rules, forgotpasswordState.value);
        }
        forgotpassword.value = data;
    };

    const forgotpasswordSuccess = async (data: any) => {
        fnforgotpassword(false);
        if (data.StatusCode == 400) {
            getCaptcha();
        }
    };
    const forgotpasswordFailure = (data: any) => {
        if (data.StatusCode == 400) {
            getCaptcha();
        }
    }
</script>
