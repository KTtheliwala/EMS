<template>
    <div class="auth-wrapper">
        <div class="auth-content">
            <div class="brand-name">
                <img src="@/assets/images/logo.svg" />
            </div>
            <div class="form-wrapper ResetPassword">
                <form v-if="enableForm">
                    <div class="form-group">                        
                        <common-password propname="NewPassword" :feedback="false" :toggleMask="true" placeholder="New Password" :v$="v$" v-model="objResetPassword.NewPassword"></common-password>
                    </div>
                    <div class="form-group">                        
                        <common-password propname="ConfirmPassword" :feedback="false" :toggleMask="true" placeholder="Confirm Password" :v$="v$" v-model="objResetPassword.ConfirmPassword"></common-password>
                    </div>
                    <div class="btns-wrapper">
                        <Button type="button" @click="FormSubmit" autofocus class="btn-primary" label="Reset Password" />
                        <Button type="button" @click="fnBackToLogin" class="p-button-aux mt-2" label="Back to Login" />
                    </div>
                </form>
                <div v-else>
                    <div class="text-center m-5">{{ responseMsg }}</div>
                    <div class="btns-wrapper">
                        <Button type="button" @click="fnBackToLogin" autofocus class="btn-primary" label="Back to Login" />                        
                    </div>

                </div>
            </div>
            <div class="version-text">
                <!-- <img class="version-logo" src="@/assets/images/EMSPvt-Logo.png" /> -->
                <h4 class="m-0 mb-1">Employee Management System</h4>
                <p class="text-muted m-0">Version : v1.1.2</p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { inject, onMounted, ref ,defineProps, computed} from "vue";
    import {
  required,
  helpers,
  minLength,
  sameAs,
  requiredIf,
} from "@vuelidate/validators";
    import { useVuelidate } from '@vuelidate/core'    
    import UrlConstants from "@/utils/urlconstants";
    import resetPasswordDTO, {resetPasswordModel} from "@/models/login/resetPassword";    
    import { useRoute } from "vue-router";
    import router from "@/router";
    import commonModule from "@/composables/modules/commonModule";    
    const objResetPassword = ref(new resetPasswordModel());
    const v$ = ref();    
    //v$.value = useVuelidate(resetPasswordDTO.rules, objResetPassword.value);
    
    const { GetData, PostData } = new commonModule();
    const frm = ref();
    const enableForm = ref(false);
    const responseMsg = ref();
    const route = useRoute()       
let Props = defineProps({
    Id: {
    type: String,
    default: null,
  }
    });
    
    onMounted(async () => {             
        if(Props.Id == null || Props.Id== '')
        {
            router.push('/');
        }
        GetData(UrlConstants.ResetPasswordUrl+"?Id="+Props.Id, (data : any) => {  
            
            const customRules = resetPasswordDTO.rules;            
            delete customRules["NewPassword"];
            customRules["NewPassword"] = {
                required: helpers.withMessage("Password is required.", required),       
                passwordPolicy:helpers.withMessage("Password must be of at least 8 characters or longer (max 15 characters) with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.", passwordPolicy)
            };
            
            v$.value = useVuelidate(customRules, objResetPassword.value);
            if(data.StatusCode === '200'){
                if (data?.Data){
          if(data.Data.Id > 0)
          {
            enableForm.value = true
            objResetPassword.value.UserId = data.Id;
            objResetPassword.value.NewPassword = "";
            objResetPassword.value.ConfirmPassword = "";
            objResetPassword.value.EncryptedId = Props.Id;
          }else {
                router.push('/');
            }
        }else{
            enableForm.value = false
            responseMsg.value = data.Message;
        }
        }else{
            enableForm.value = false
            responseMsg.value = data.Message;
        }
            })
    });
    const passwordPolicy = (value:string) => {return /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[.!@#$&*~]).{8,15}$/g.test(value)}
    const fnBackToLogin = async () => {  
        router.push('/');
      };
      

    const FormSubmit = async () => {        
        const customRules = resetPasswordDTO.rules as any;        
        const newpassword = computed(() => objResetPassword.value.NewPassword);
    const cnfrimpassword = computed(() => objResetPassword.value.ConfirmPassword);
    if(newpassword.value.length !== 0  && cnfrimpassword.value.length !== 0){
        delete customRules["ConfirmPassword"];
      customRules["ConfirmPassword"] = {
        sameAsPassword: helpers.withMessage(
          "Password and confirmation password does not match",
          sameAs(newpassword.value)
        )
      };
    }
    v$.value = useVuelidate(customRules, objResetPassword.value);
    const result = await v$.value.value.$validate()
    if (result) {
        PostData(UrlConstants.ResetPasswordUrl, objResetPassword.value, "Password reset successfully.", (data: any) => {            
            if(data.StatusCode == 200)
            {
                router.push('/');
            }
        },null);
    }
};
</script>
