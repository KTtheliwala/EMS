<template>
    <!-- <common-form ref="frm" :dataId="Id" :actionUrl="UrlConstants.apiLoginUserChangePassword" :v$="v$" :data="objUser" :successCallBack="successCallback" :failureCallBack="failureCallback" :successMessage="Msg"> -->
        <div class="grid guter-change">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Enter Current Password</label>
                    <common-password :toggleMask="true"
                                     placeholder="Enter Current Password"
                                     :v$="v$"
                                     propname="Oldpassword"                                     
                                     v-model="objUser.Oldpassword"
                                     ></common-password>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Enter New Password</label>
                    <common-password :feedback="false"
                                     :toggleMask="true"
                                     placeholder="Enter New Password"
                                     :v$="v$"
                                     propname="Newpassword"                                     
                                     v-model="objUser.Newpassword"
                                     ></common-password>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Re-enter New Password</label>
                    <common-password :feedback="false"
                                     :toggleMask="true"
                                     placeholder="Re-enter New Password"
                                     :v$="v$"
                                     propname="Confirmpassword"                                     
                                     v-model="objUser.Confirmpassword"
                                     ></common-password>
                </div>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Cancel" v-if="!ForceChangePassword" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                <Button label="Save" icon="pi pi-plus-circle" type="button" @click="FormSubmit" autofocus />
            </div>
        </div>
    <!-- </common-form> -->
</template>
<script setup lang="ts">     
import { inject, onMounted, ref ,defineProps, computed} from "vue";
import {required, helpers, minLength, sameAs, requiredIf} from "@vuelidate/validators";
import { useVuelidate } from '@vuelidate/core'    
import UrlConstants from "@/utils/urlconstants";
import changePasswordModelDTO, { changePasswordModel } from "@/models/master/changePasswordModel";
import commonModule from "@/composables/modules/commonModule";
import { useAuthDataStore } from "@/store/useAuthDataStore";
const { PatchData } = new commonModule();
const dialogRef = inject("dialogRef") as any;
const Id = ref(0);
const objUser = ref<changePasswordModel>(new changePasswordModel());
objUser.value.Newpassword="";
objUser.value.Oldpassword="";
objUser.value.Confirmpassword="";
const v$ = ref();
v$.value = useVuelidate(changePasswordModelDTO.rules, objUser.value);
const frm = ref();
let buttonText = "Create";
const Msg = ref(
    Id.value > 0
    ? "Password updated successfully."
    : "Password added successfully."
);
const authStore = useAuthDataStore();
const ForceChangePassword = ref(false)
if (JSON.parse(localStorage.getItem('useAuthData'))) {
    ForceChangePassword.value = (JSON.parse(localStorage.getItem('useAuthData'))?.userModel?.ForceChangePassword ?? false);
}

onMounted(async () => { 

const customRules = changePasswordModelDTO.rules;            
          delete customRules["Newpassword"];
          customRules["Newpassword"] = {
              required: helpers.withMessage("Password is required.", required),       
              passwordPolicy:helpers.withMessage("Password must be of at least 8 characters or longer (max 15 characters) with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.", passwordPolicy)
          };
          
          v$.value = useVuelidate(customRules, objUser.value);
});
const passwordPolicy = (value:string) => {return /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[.!@#$&*~]).{8,15}$/g.test(value)}
const successCallback = (data: any) => {
        dialogRef.value.close();
    };
    const failureCallback = (data: any) => {
        console.log(data)
    };
const StatusItem = ref();
const CloseModal = () => {
  dialogRef.value.close();
};
const FormSubmit = async () => {     
    const customRules = changePasswordModelDTO.rules as any;        
    const newpassword = computed(() => objUser.value.Newpassword);
    const cnfrimpassword = computed(() => objUser.value.Confirmpassword);
    if(newpassword.value.length !== 0  && cnfrimpassword.value.length !== 0){
        delete customRules["Confirmpassword"];
      customRules["Confirmpassword"] = {
        sameAsPassword: helpers.withMessage(
          "New Password and confirmation password does not match",
          sameAs(newpassword.value)
        )
      };
    }
    v$.value = useVuelidate(customRules, objUser.value);
    const result = await v$.value.value.$validate()
    if (result) {
        PatchData(UrlConstants.apiLoginUserChangePassword, objUser.value, "Password change successfully.", (data: any) => {            
            if(data.StatusCode == 200)
            {
                if (JSON.parse(localStorage.getItem('useAuthData'))) 
                {
                    let authData =  JSON.parse(localStorage.getItem('useAuthData'))?.userModel as any 
                    authStore.setAuth({ IsAd: authData.IsAd, Token: authData.Token, UserName: authData.UserName, CountryId: authData.CountryId, Role: authData.Role, Offset: authData.Offset, UserId: authData.UserId, RoleSName: authData.RoleSName,
                    BUs:authData.BUs,ForceChangePassword:false });
                }
                CloseModal();
            }
        },null);
    }
};
</script>
