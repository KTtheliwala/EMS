<template>
    <div class="grid guter-change">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Role</label>
                    <common-dropdown placeholder="Please Select" 
                    :v$="v$" name="RoleId" v-model="obj.RoleId" 
                    :isWatch="false" 
                    :dataurl="UrlConstants.apidropdown+'?Mode=Role'" />
                </div>
            </div>   
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">UserName</label>
                    <common-input placeholder="UserName"
                                  name="UserName"
                                  :v$="v$"
                                  v-model="obj.UserName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                        <label class="block w-full">Password</label>
                        <common-password
                            v-model="obj.Password"
                            :v$="v$"
                            name="Password"
                            type="password"
                            :toggleMask="true"
                        ></common-password>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                        <label class="block w-full">Confirm Password</label>
                        <common-password
                            v-model="obj.ConfirmPassword"
                            name="ConfirmPassword"
                            :toggleMask="true"
                            :v$="v$"
                        ></common-password>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">First Name</label>
                    <common-input placeholder="FirstName"
                                  name="FirstName"
                                  :v$="v$"
                                  v-model="obj.FirstName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Last Name</label>
                    <common-input placeholder="Last Name"
                                  name="LastName"
                                  v-model="obj.LastName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Email</label>
                    <common-input placeholder="Email"
                                  name="Email"
                                  v-model="obj.Email"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Mobile</label>
                    <common-input placeholder="Mobile"
                                  name="Mobile"                                  
                                  v-model="obj.Mobile"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Sort Order</label>
                    <common-input fieldlabel="SortOrder"
                                  name="SortOrder"
                                  :IsNumber="true"
                                  inputclass="form-control"
                                  v-model="obj.SortOrder"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <InputSwitch v-model="obj.IsActive" />
                </div>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" type="submit" @click="submit" autofocus />
            </div>
        </div>
</template>
<script setup lang="ts">
import { computed, inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import usersModelDTO, { usersModel } from "@/models/admintools/usersModel";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
const { GetData, PostData } = new commonModule();
const dialogRef = inject("dialogRef") as any;
const Id = ref(dialogRef.value.data);
const obj = ref<usersModel>(new usersModel());
if (Id.value == 0)
    obj.value.IsActive = true;
const v$ = ref();
const frm = ref();
const Msg = ref(Id.value > 0 ? "Role updated successfully." : "Role added successfully.");
onMounted(() => {
   if(Id.value > 0){
    GetData(UrlConstants.apiuser + "/"+Id.value, (data: any) => {
        obj.value = data;
        //obj.value.RoleId = data.RoleId.toString();        
    },null);
   }
});
const CloseModal = () => {
        dialogRef.value.close();
};

const checkPasswordValidation = () => {
    const customRules = usersModelDTO.rules as any;
    delete customRules["Password"];
    delete customRules["ConfirmPassword"];  
    const newpassword = computed(() => (obj.value.Password ?? ""));
    const cnfrimpassword = computed(() => (obj.value.ConfirmPassword ?? ""));
    if (obj.value.Id == 0) {
      customRules["Password"] = {
        required: helpers.withMessage("Password is required", required),       
        passwordPolicy:helpers.withMessage("Password must be of at least 8 characters or longer (max 15 characters) with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.", passwordPolicy)
      };
       if(newpassword.value.length === 0  && cnfrimpassword.value.length === 0){
      customRules["ConfirmPassword"] = {
        required: helpers.withMessage("Confirm Password is required",required)
      };
        }else if(newpassword.value.length !== 0  && cnfrimpassword.value.length === 0){
      customRules["ConfirmPassword"] = {
        required: helpers.withMessage(
          "Confirm Password is required",
          requiredIf(() => {            
            return true;
          })
        )
      };
    }
    else if(newpassword.value.length !== 0  || cnfrimpassword.value.length !== 0){
      customRules["ConfirmPassword"] = {
        sameAsPassword: helpers.withMessage(
          "Password and confirmation password does not match",
          sameAs(newpassword.value)
        )
      };
    }  
    }
    else if(obj.value.Id > 0 && (newpassword.value.length !== 0  || cnfrimpassword.value.length !== 0))
    {
        if(newpassword.value.length !== 0  && cnfrimpassword.value.length === 0){
            customRules["ConfirmPassword"] = {
                required: helpers.withMessage(
                "Confirm Password is required",
                requiredIf(() => {            
                    return true;
                })
                )
            };      
        }else if(newpassword.value.length === 0  && cnfrimpassword.value.length !== 0){      
            customRules["Password"] = {
                required: helpers.withMessage("Password is required", required),
                minLength: helpers.withMessage(
                "Password minimum 5 character is required",
                minLength(5)
                ),
            };   
        }else if(newpassword.value.length !== 0  && cnfrimpassword.value.length !== 0){
            customRules["ConfirmPassword"] = {
                sameAsPassword: helpers.withMessage(
                "Password and confirmation password does not match",
                sameAs(newpassword.value)
                )
            };
        }
    }
    v$.value = useVuelidate(customRules, obj.value);
}

const submit = async() => {
    //
    const result = ref();
    await checkPasswordValidation();
    result.value = await v$.value.value.$validate();
    if(result.value){
        //
        PostData( UrlConstants.apiuser,obj.value, "user successfully added.", (data: any) => {
        dialogRef.value.close();
      },null);
    }
}
</script>