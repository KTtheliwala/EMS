<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :IsOnlyGrid="true" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
        <template v-slot:right-buttons>
            
        </template>
        <template v-slot:columns>
           <Column field="OrderStatusName" header="Status" :sortable="true"></Column> 
            <Column field="Remarks" header="Remarks" :sortable="true"></Column>               
            <Column field="CreatedDate" header="Date" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.CreatedDate, "DD MMM YYYY HH:ss") }}
                </template>
            </Column> 
            <Column field="CreatedUserName" header="Created by" :sortable="true"></Column>                     
        </template>
        
    </common-grid>
    
</template>
<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import ordersModelDTO, { ordersModel, OrderDetailsModel, OrderStatusChange } from "@/models/orders/ordersModel";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
import GridConfig from "@/models/controls/Grid/gridConfig";
const { GetData, PostData } = new commonModule();
const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
const dialogRef = inject("dialogRef") as any;
const dataObj = ref(dialogRef.value.data);
const obj = ref<OrderStatusChange>(new OrderStatusChange());
obj.value.OrderId = dataObj?.value?.Id;
const v$ = ref();


const gridConfig = ref({
        api: UrlConstants.apiOrderHistoryList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "OrderId", fieldValue: obj.value.OrderId, fieldDisplayName: "Order No", opType: SearchOperations.equals },
            //{ searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
           // { searchType: "Filter", fieldName: "FirstName", fieldValue: null, fieldDisplayName: "First Name", opType: SearchOperations.contains },
           // { searchType: "Filter", fieldName: "LastName", fieldValue: null, fieldDisplayName: "Last Name", opType: SearchOperations.contains },
        ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);

</script>