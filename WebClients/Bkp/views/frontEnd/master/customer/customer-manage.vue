<template>
    <common-form ref="frm" :dataId="Id" :actionUrl="UrlConstants.apicustomer" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change">
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Party Code</label>
                    <common-input placeholder="PartyCode"
                                  name="PartyCode"
                                  :v$="v$"
                                  v-model="obj.PartyCode"></common-input>
                </div>
            </div>           
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Party Name</label>
                    <common-input placeholder="PartyName"
                                  name="PartyName"
                                  :v$="v$"
                                  v-model="obj.PartyName"></common-input>
                </div>
            </div>           
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Email</label>
                    <common-input placeholder="Email"
                                  name="Email"
                                  :v$="v$"
                                  v-model="obj.Email"></common-input>
                </div>
            </div>           
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Mobile No</label>
                    <common-input placeholder="MobileNo"
                                  name="MobileNo"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.MobileNo"></common-input>
                </div>
            </div>            
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Address1</label>
                    <common-area placeholder="Address1"
                                  name="Address1"
                                  :v$="v$"
                                  v-model="obj.Address1"></common-area>
                </div>
            </div>     
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Address2</label>
                    <common-area placeholder="Address2"
                                  name="Address2"
                                  :v$="v$"
                                  v-model="obj.Address2"></common-area>
                </div>
            </div>   
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Address3</label>
                    <common-area placeholder="Address3"
                                  name="Address3"
                                  :v$="v$"
                                  v-model="obj.Address3"></common-area>
                </div>
            </div>             
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">City</label>
                    <common-input placeholder="City"
                                  name="City"
                                  :v$="v$"
                                  v-model="obj.City"></common-input>
                </div>
            </div>              
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">State Code</label>
                    <common-input placeholder="StateCode"
                                  name="StateCode"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.StateCode"></common-input>
                </div>
            </div>                
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Pin Code</label>
                    <common-input placeholder="PinCode"
                                  name="PinCode"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.PinCode"></common-input>
                </div>
            </div>             
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Bank Name</label>
                    <common-input placeholder="BankName"
                                  name="BankName"
                                  :v$="v$"
                                  v-model="obj.BankName"></common-input>
                </div>
            </div>            
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Contact Name</label>
                    <common-input placeholder="ContactName"
                                  name="ContactName"
                                  :v$="v$"
                                  v-model="obj.ContactName"></common-input>
                </div>
            </div>            
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">ST No</label>
                    <common-input placeholder="STNo"
                                  name="STNo"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.STNo"></common-input>
                </div>
            </div>          
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">PAN No</label>
                    <common-input placeholder="PanNo"
                                  name="PanNo"
                                  :v$="v$"
                                  v-model="obj.PanNo"></common-input>
                </div>
            </div>            
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Amount</label>
                    <common-input placeholder="Amount"
                                  name="Amount"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.Amount"></common-input>
                </div>
            </div>             
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Broker Code</label>
                    <common-input placeholder="BrokerCode"
                                  name="BrokerCode"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.BrokerCode"></common-input>
                </div>
            </div>                
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Area1</label>
                    <common-input placeholder="Area1"
                                  name="Area1"
                                  :v$="v$"
                                  v-model="obj.Area1"></common-input>
                </div>
            </div>              
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Account Group Code</label>
                    <common-input placeholder="AccountGroupCode"
                                  name="AccountGroupCode"
                                  :v$="v$"
                                  v-model="obj.AccountGroupCode"></common-input>
                </div>
            </div>               
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Area Code</label>
                    <common-input placeholder="AreaCode"
                                  name="AreaCode"
                                  :v$="v$"
                                  v-model="obj.AreaCode"></common-input>
                </div>
            </div>                
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">SMS No</label>
                    <common-input placeholder="SmsNo"
                                  name="SmsNo"
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="obj.SmsNo"></common-input>
                </div>
            </div>                
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">GST No</label>
                    <common-input placeholder="GSTNo"
                                  name="GSTNo"
                                  :v$="v$"
                                  v-model="obj.GSTNo"></common-input>
                </div>
            </div>                
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Collection Men</label>
                    <common-input placeholder="CollectionMen"
                                  name="CollectionMen"
                                  :v$="v$"
                                  v-model="obj.CollectionMen"></common-input>
                </div>
            </div>                 
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Propritor Name</label>
                    <common-input placeholder="PropritorName"
                                  name="PropritorName"
                                  :v$="v$"
                                  v-model="obj.PropritorName"></common-input>
                </div>
            </div>  
            <div class="col-12 md:col-3 pb-0">
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
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" type="submit" autofocus />
            </div>
        </div>
    </common-form>
</template>
<script setup lang="ts">
    import { inject, onMounted, ref } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import UrlConstants from "@/utils/urlconstants";
    import customerModelDTO, { customerModel } from "@/models/master/customerModel";
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const obj = ref<customerModel>(new customerModel());
    if (Id.value == 0)
        obj.value.IsActive = true;
    const v$ = ref();
    const frm = ref();
    const Msg = ref(Id.value > 0 ? "Company Filed updated successfully." : "Company Filed added successfully.");
    onMounted(() => {
        frm.value.SetValidation((data: customerModel) => {
            obj.value = data;
            v$.value = useVuelidate(customerModelDTO.rules, obj.value);
        });
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
</script> 