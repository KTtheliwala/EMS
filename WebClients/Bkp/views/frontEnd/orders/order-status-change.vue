<template>
    <div class="grid guter-change">
        
        <div class="col-12 md:col-12 pb-0">
            <div class="field">
                <p>Current Status: <Tag :value="currentStatus" v-if="CurrentStatusId == 1"></Tag>
                    <Tag severity="info" :value="currentStatus" v-else-if="CurrentStatusId == 2"></Tag>
                    <Tag severity="warning" :value="currentStatus" v-else-if="CurrentStatusId == 3"></Tag>
                    <Tag severity="success" :value="currentStatus" v-else-if="CurrentStatusId == 4"></Tag>
                    <Tag severity="danger" :value="currentStatus" v-else="CurrentStatusId == 5"></Tag>  </p>    
            <label class="block w-full">Change Status To</label>
                <common-dropdown placeholder="Please Select" 
                :v$="v$"
                name="OrderStatus" v-model="obj.OrderStatus" 
                :isWatch="false" 
                optionLabel="Text"
            optionValue="Value"
            :options="StatusOption" />
        </div>
        </div>
        <div class="col-12 md:col-12">
            <div class="field">
                <label class="block w-full">Remarks</label>
                <commonArea placeholder="Remarks"                     
                v-model="obj.Remarks"
                name="DeliveryAddress"                      
                :autoResize="true"></commonArea>     
            </div>
        </div>
        <div class="p-dialog-footer">
        <div></div>
        <div>
            <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
            <Button label="Update" icon="pi pi-plus-circle" type="submit" @click="submit" autofocus />
        </div>
        </div>
    </div>
    
</template>
<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import ordersModelDTO, { ordersModel, OrderDetailsModel, OrderStatusChange } from "@/models/orders/ordersModel";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
const { GetData, PostData } = new commonModule();
const dialogRef = inject("dialogRef") as any;
const dataObj = ref(dialogRef.value.data);
const obj = ref<OrderStatusChange>(new OrderStatusChange());
obj.value.OrderId = dataObj?.value?.Id;
console.log(dataObj?.value)
const v$ = ref();
const StatusOption = ref([]);
const currentStatus = ref(dataObj?.value?.OrderStatusName);
const CurrentStatusId = ref(dataObj?.value?.OrderStatus);
const CloseModal = async() => {
    dialogRef.value.close();
}

const submit = async() => {
    const result = ref();   
    v$.value = useVuelidate(ordersModelDTO.statusRule, obj.value); 
    result.value = await v$.value.value.$validate();
    if(result.value){
        //
        PostData( UrlConstants.apiOrderHistoryUpdateStatus,obj.value, "order status successfully updated.", (data: any) => {
        dialogRef.value.close();
      },null);
    }
}
const Status_Options = async () => {
    await GetData(UrlConstants.apidropdown + "?Mode=OrderStatus", (data: any) => {
        
        StatusOption.value = data.filter((x: { Value: any; }) => (x.Value) > (dataObj?.value.OrderStatus));
        console.log(StatusOption.value)
        //obj.value.OrderStatus = dataObj?.value.OrderStatus;
    },null);
};
//Status_Options();
Promise.all([Status_Options()]).then(async () => {
   // obj.value.OrderStatus = dataObj?.value.OrderStatus;
     });
</script>