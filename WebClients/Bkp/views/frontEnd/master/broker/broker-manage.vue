<template>
     <div class="grid guter-change">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Code <small>*</small></label>
                    <common-input fieldlabel="Code"                                 
                                  name="Code"
                                  :IsNumber="true"
                                  :v$="v$"
                                  :onblur="onChangeCode"
                                  @input="updateValue($event.target.value)"
                                  inputclass="form-control"
                                  v-model="obj.Code"></common-input>
                </div>
            </div>        
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Name</label>
                    <common-input placeholder="Name"
                                  name="Name"  
                                  Id="Name"                                  
                                  v-model="obj.Name" 
                                  inputclass="form-control"
                                  :disabled="isNameEdit"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Email</label>
                    <common-input fieldlabel="Email"                                 
                                  name="Email"
                                  inputclass="form-control"
                                  v-model="obj.Email" :disabled="isEdit"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Mobile</label>
                    <common-input fieldlabel="Mobile"                                  
                                  name="Mobile"
                                  inputclass="form-control"
                                  v-model="obj.MobileNo" :disabled="isEdit"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Address</label>
                    <commonArea placeholder="Address"                     
                    v-model="obj.Address"
                    :autoResize="true" :disabled="isEdit"></commonArea>     
                </div>
            </div>               
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Sort Order</label>
                    <common-input fieldlabel="SortOrder"
                                  name="SortOrder"
                                  :IsNumber="true"
                                  inputclass="form-control"
                                  v-model="obj.SortOrder" :disabled="isEdit"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <InputSwitch v-model="obj.IsActive" :disabled="isEdit" />
                </div>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                <Button :label="('Edit')" icon="pi pi-plus-circle" v-if="isEdit" @click="UpdateRecord" />
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" v-if="!isEdit" @click="submitData" type="submit" autofocus />
            </div>
        </div>    
</template>
<script setup lang="ts">
    import { inject, onMounted, ref } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import UrlConstants from "@/utils/urlconstants";
    import brokerModelDTO, { brokerModel } from "@/models/master/brokerModel";
    import commonModule from "@/composables/modules/commonModule";
    
    const { GetData, PostData } = new commonModule();
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const isAllow = ref((dialogRef.value.data > 0) ? false : true);
    const obj = ref<brokerModel>(new brokerModel());
    const isEdit = ref(false);
    const isNameEdit = ref(false);
    if (Id.value == 0)
        obj.value.IsActive = true;
    const v$ = ref();
    const frm = ref();
    const Msg = ref(Id.value > 0 ? "Broker updated successfully." : "Broker added successfully.");
    onMounted(() => {
        if (Id.value > 0) {
            GetData(UrlConstants.apiBrokerMaster + Id.value,(data: brokerModel) => { 
                obj.value = data;
            },
            null)
        }
    });
    const successCallback = (data: any) => {
        dialogRef.value.close();
    };
    const failureCallBack = (data: any) => {
        //
    };
    const CloseModal = () => {
        dialogRef.value.close();
    };

    const onChangeCode = () => {           
        if(isAllow.value){     
            if(obj.value.Code == null){
                Id.value = 0;
                 obj.value.Id = 0;
                isEdit.value=false;
               setTimeout(function(){   
                        isNameEdit.value=false;},100) 
                obj.value.Code = 0;
                obj.value.Name = "";
                obj.value.Email = "";
                obj.value.MobileNo = "";
                obj.value.Address = "";
                return false
            }
            if (obj.value.Code !== null|| Id.value > 0){
                GetData(UrlConstants.apiBrokerMasterGetByCode + obj.value.Code,(data: brokerModel) => { 
                  //  setTimeout(function(){      
                             
                    if(data['Id'] != undefined){
                        obj.value = data;
                        Id.value = obj.value.Id;
                        isEdit.value=true;
                        setTimeout(function(){   
                        isNameEdit.value=true;},100)    
                    }else{
                        Id.value = 0;
                        isEdit.value=false;
                            setTimeout(function(){   
                        isNameEdit.value=false;},100) 
                        obj.value.Name = "";
                        obj.value.Email = "";
                        obj.value.MobileNo = "";
                        obj.value.Address = "";
                        obj.value.Id = 0;
                    }
                        //},2000)    
                    },
                null)
            }
        }
    }

    const UpdateRecord = () => {        
        isEdit.value = false;
        isNameEdit.value=false;
    }

    const submitData = async () => {  
  
    v$.value = useVuelidate(brokerModelDTO.rules, obj.value);  
    const result = await v$.value.value.$validate();
    if (result) {
        PostData(UrlConstants.apiBrokerMaster, obj.value,"", (data: any) => {
            dialogRef.value.close();
        },
        null
        );
    }
};
</script>