<template>
    <div>
        <div class="grid guter-change">
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">{{ AduserLabel }} Username</label>
                    <common-input :placeholder="''"
                                  name="UserName"
                                  inputclass="form-control"
                                  v-model="objUser.UserName"
                                  :disabled="disabled"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">First Name</label>
                    <common-input inputclass="form-control"
                                  name="FirstName"
                                  v-model="objUser.FirstName"
                                  :disabled="disabled"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Last Name</label>
                    <common-input :placeholder="''"
                                  name="LastName"
                                  inputclass="form-control"
                                  v-model="objUser.LastName"
                                  :disabled="disabled"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Mobile No</label>
                    <common-input inputclass="form-control"
                                  name="Mobile"
                                  v-model="objUser.Mobile"
                                  :disabled="disabled"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Email</label>
                    <common-input :placeholder="''"
                                  name="Email"
                                  inputclass="form-control"
                                  v-model="objUser.Email"
                                  :disabled="disabled"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">User Role</label>
                    <common-input :placeholder="''"
                                  name="RoleName"
                                  inputclass="form-control"
                                  v-model="objUser.RoleName"
                                  :disabled="disabled"></common-input>                    
                </div>
            </div>            
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Last Password Changed On</label>
                    <common-input :placeholder="''"
                                  name="RoleName"
                                  inputclass="form-control"
                                  v-model="PasswordChangedOnDate"
                                  :disabled="disabled"></common-input>                    
                </div>
            </div>            
        </div>
    </div>
    <div class="p-dialog-footer">
        <div></div>
        <div>
            <Button label="Close"
                    class="p-button-aux"
                    icon="pi pi-times-circle"
                    @click="CloseModal" />            
        </div>
    </div>
</template>
<script setup lang="ts">
import { inject, onMounted, ref, nextTick } from "vue";
import UrlConstants from "@/utils/urlconstants";
import userModelDTO, { usersModel } from "@/models/admintools/usersModel";
import commonModule from "@/composables/modules/commonModule";
const { ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
const { GetData, PostData } = new commonModule();
const objUser = ref<usersModel>(new usersModel());

    const dialogRef = inject("dialogRef") as any;
    const AduserLabel = ref();
    const CloseModal = async () => {
        dialogRef.value.close();
    };
    const BusinessUnit_Options = ref();

    const disabled = ref(true);
    const PasswordChangedOnDate = ref("");
    const buttonText = ref(false);
    const submitUserData = () => {
        buttonText.value = true;
    }

    onMounted(() => {    
        GetData(UrlConstants.apiGetLoginUserProfile, (data : usersModel) => {
            objUser.value = data;             
            if(data?.PasswordChangedOn != null && data?.PasswordChangedOn != undefined) {
                PasswordChangedOnDate.value = ConvertToUserTimeZone(data?.PasswordChangedOn, "DD MMM YYYY HH:ss")
            }
            
        }); 
    });
</script>
