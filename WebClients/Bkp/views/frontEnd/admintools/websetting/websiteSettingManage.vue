<template>
    <common-form ref="frm" :dataId="Id" :actionUrl="UrlConstants.apisystemsetting" :v$="v$" :data="objSystem" :successCallBack="successCallback" :failureCallBack="failureCallback" :successMessage="Msg">
    <div class="grid guter-change">
      <div class="col-12 md:col-4">
        <div class="field">
            <label class="block w-full">Key</label>
            <common-input
                placeholder=""
                name="Key"
                :v$="v$"
                v-model="objSystem.Key"
                disabled
            ></common-input>
        </div>
      </div>    
      <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full">Value</label>
            <common-input
                placeholder=""
                name="Value"
                :v$="v$"
                v-model="objSystem.Value"               
            ></common-input>
          </div>
        </div>
    </div>  
    <div class="p-dialog-footer">
        <div></div>
        <div>
            <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
            <Button :label="buttonText" icon="pi pi-plus-circle" type="submit" autofocus />
        </div>
    </div>
    </common-form>
</template>
<script setup lang="ts">
    import { inject, onMounted, ref } from "vue";
    import { useVuelidate } from '@vuelidate/core'
    import UrlConstants from "@/utils/urlconstants";
    import webSettingsModelDTO, {webSettingsModel} from "@/models/admintools/webSettingsModel";
    import commonModule from "@/composables/modules/commonModule";
    const dialogRef = inject('dialogRef') as any;
    const Id = ref(dialogRef.value.data);
    const objSystem = ref<webSettingsModel>(new webSettingsModel());
    const v$ = ref();
    const frm = ref();
    let buttonText = "Create";
    const Msg = ref(Id.value > 0 ? "Web Settings updated successfully." : "Web Settings added successfully.");
    
    onMounted(() => {
        frm.value.SetValidation((data: webSettingsModel) => {
            objSystem.value = data;
            v$.value = useVuelidate(webSettingsModelDTO.rules, objSystem.value);
        });
    });
    if (Id.value > 0) {
        buttonText = "Update";
    }
    const failureCallback = (data: any) => {
        //console.log(data);
    };
    const successCallback = (data: any) => {
        dialogRef.value.close();
    };
    const CloseModal=()=>{        
        dialogRef.value.close();
    }
</script>