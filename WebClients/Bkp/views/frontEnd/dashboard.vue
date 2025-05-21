<template>
  <div class="form-layout dashboard-wrapper">
    <div class="grid guter-change bottom-seperator">
         
         <template  v-if="(dashboardSummaryList).length > 0">
          
   <!-- Check if data is present -->
   
     
     <div class="col-12 lg:col-2 pb-0 text-center status-total" v-for="objBudget of dashboardSummaryList" >
      <div class="status-body" :class="generateClassName(objBudget.Name)">
         <h4 class="p-0 m-0">{{ objBudget.Name }}</h4>
         <p class="p-0 m-0">{{ objBudget.Total }}</p>

        </div>
  
 </div>
</template>
     </div>
    <div class="form-body dashboard-body p-0">
      
      <div class="col-12 md:col-12 box3 bottom-seperator">
        <h4 class="mb-0 mb-2 mt-2">Completed Order</h4>
        <div class="">
          <common-grid :config="gridConfig3" ref="dt3" :IsOnlyGrid="true" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
              <template v-slot:right-buttons>
                  
              </template>
              <template v-slot:columns>
                <Column field="OrderNo" header="#Order" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DateOfFunction" header="DOF" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateOfFunction, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="DateofDelivery" header="DOD" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofDelivery, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="SubTotalAmount" header="Sub Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.SubTotalAmount) }}
                </template>
            </Column> 
            <Column field="DiscountAmount" header="Discount Amt" :sortable="true">
                <template #body="slotProps">
                    {{(slotProps.data?.DiscountAmount > 0)?"$"+(slotProps.data.DiscountAmount):"" }}
                </template>
            </Column> 
            <Column field="TotalAmount" header="Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.TotalAmount) }}
                </template>
            </Column> 
           
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>                  
              </template>      
          </common-grid>
        </div>
      </div>
      <div class="col-12 md:col-12 box3 bottom-seperator">
        <h4 class="mb-0 mb-2">Pending Order</h4>
        <div class="">
          <common-grid :config="gridConfig" ref="dt" :IsOnlyGrid="true" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
              <template v-slot:right-buttons>
                  
              </template>
              <template v-slot:columns>
                <Column field="OrderNo" header="#Order" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DeliveryAddress" header="Address Line 1" :sortable="true"></Column>
            <Column field="DeliveryAddress2" header="Address Line 2" :sortable="true"></Column>  
            <Column field="City" header="City" :sortable="true"></Column>  
            <Column field="State" header="State" :sortable="true"></Column>  
            <Column field="PostCode" header="Postal Code" :sortable="true"></Column> 
            <Column field="Country" header="CountryName" :sortable="true"></Column>  
            <Column field="DateOfFunction" header="DOF" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateOfFunction, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="DateofDelivery" header="DOD" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofDelivery, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="SubTotalAmount" header="Sub Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.SubTotalAmount) }}
                </template>
            </Column> 
            <Column field="DiscountAmount" header="Discount Amt" :sortable="true">
                <template #body="slotProps">
                    {{(slotProps.data?.DiscountAmount > 0)?"$"+(slotProps.data.DiscountAmount):"" }}
                </template>
            </Column> 
            <Column field="TotalAmount" header="Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.TotalAmount) }}
                </template>
            </Column> 
           
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>                  
              </template>      
          </common-grid>
        </div>
      </div>
      <div class="col-12 md:col-12 box3 bottom-seperator">
        <h4 class="mb-0 mb-2">In-Progress Order</h4>
        <div class="">
          <common-grid :config="gridConfig1" ref="dt1" :IsOnlyGrid="true" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
              <template v-slot:right-buttons>
                  
              </template>
              <template v-slot:columns>
                <Column field="OrderNo" header="#Order" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DateOfFunction" header="DOF" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateOfFunction, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="DateofDelivery" header="DOD" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofDelivery, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="SubTotalAmount" header="Sub Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.SubTotalAmount) }}
                </template>
            </Column> 
            <Column field="DiscountAmount" header="Discount Amt" :sortable="true">
                <template #body="slotProps">
                    {{(slotProps.data?.DiscountAmount > 0)?"$"+(slotProps.data.DiscountAmount):"" }}
                </template>
            </Column> 
            <Column field="TotalAmount" header="Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.TotalAmount) }}
                </template>
            </Column> 
           
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>                  
              </template>      
          </common-grid>
        </div>
      </div>      
      <div class="col-12 md:col-12 box3 bottom-seperator">
        <h4 class="mb-0 mb-2">Ready To Dispatch Order</h4>
        <div class="">
          <common-grid :config="gridConfig2" ref="dt2" :IsOnlyGrid="true" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
              <template v-slot:right-buttons>
                  
              </template>
              <template v-slot:columns>
                <Column field="OrderNo" header="#Order" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DateOfFunction" header="DOF" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateOfFunction, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="DateofDelivery" header="DOD" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofDelivery, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="SubTotalAmount" header="Sub Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.SubTotalAmount) }}
                </template>
            </Column> 
            <Column field="DiscountAmount" header="Discount Amt" :sortable="true">
                <template #body="slotProps">
                    {{(slotProps.data?.DiscountAmount > 0)?"$"+(slotProps.data.DiscountAmount):"" }}
                </template>
            </Column> 
            <Column field="TotalAmount" header="Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.TotalAmount) }}
                </template>
            </Column> 
           
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>                  
              </template>      
          </common-grid>
        </div>
      </div>
      <div class="col-12 md:col-12 box3 bottom-seperator">
        <h4 class="mb-0 mb-2">Cancelled Order</h4>
        <div class="">
          <common-grid :config="gridConfig4" ref="dt4" :IsOnlyGrid="true" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        
              <template v-slot:right-buttons>
                  
              </template>
              <template v-slot:columns>
                <Column field="OrderNo" header="#Order" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DateOfFunction" header="DOF" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateOfFunction, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="DateofDelivery" header="DOD" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofDelivery, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="SubTotalAmount" header="Sub Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.SubTotalAmount) }}
                </template>
            </Column> 
            <Column field="DiscountAmount" header="Discount Amt" :sortable="true">
                <template #body="slotProps">
                    {{(slotProps.data?.DiscountAmount > 0)?"$"+(slotProps.data.DiscountAmount):"" }}
                </template>
            </Column> 
            <Column field="TotalAmount" header="Total" :sortable="true">
                <template #body="slotProps">
                    {{ "$"+(slotProps.data.TotalAmount) }}
                </template>
            </Column> 
           
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>                  
              </template>      
          </common-grid>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import ordersModelDTO, { ordersModel, OrderDetailsModel, OrderStatusChange } from "@/models/orders/ordersModel";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import orderStatus from "@/models/controls/Order/orderStatus";
import {dashboardSummary} from "@/models/dashboard/dashboardSummary";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
import GridConfig from "@/models/controls/Grid/gridConfig";
const { GetData, PostData } = new commonModule();
const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');

const obj = ref<OrderStatusChange>(new OrderStatusChange());
  const dashboardSummaryList = ref([]);

const v$ = ref();


// const fetchData = async () => {
//   await GetData(
//     UrlConstants.apiDashboardMaster,
//     async (data: any) => {
//       dashboardSummaryList.value = data; // Assign the API data to the reactive variable
      
//     },
//     ""
//   );
// };
//fetchData();
onMounted(async() => {    
  await GetData(
    UrlConstants.apiDashboardMaster,
    async (data: any) => {
      dashboardSummaryList.value = data; // Assign the API data to the reactive variable
      
    },
    ""
  );
});
const generateClassName = (name: string) => {
  return name.replace(/\s+/g, "");
};
//Pending Order Grid
const gridConfig = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "", opType: SearchOperations.dateRange },
            { searchType: "Param", fieldName: "OrderStatus", fieldValue: orderStatus.Pending, fieldDisplayName: "Order No", opType: SearchOperations.equals },
           ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);
    gridConfig.value.filters[0].fieldValue = [new Date(), new Date()]

//Pending Order Grid End
//In Progress Order Grid
const gridConfig1 = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "", opType: SearchOperations.dateRange },
            { searchType: "Param", fieldName: "OrderStatus", fieldValue: orderStatus.InProgress, fieldDisplayName: "Order No", opType: SearchOperations.equals },
           ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);
    gridConfig1.value.filters[0].fieldValue = [new Date(), new Date()]

//In Progress Order Grid End
//Ready to dispatch Order Grid
const gridConfig2 = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "", opType: SearchOperations.dateRange },
            { searchType: "Param", fieldName: "OrderStatus", fieldValue: orderStatus.ReadyForDelivery, fieldDisplayName: "Order No", opType: SearchOperations.equals },
           ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);
    gridConfig2.value.filters[0].fieldValue = [new Date(), new Date()]
//Ready to dispatch Order Grid End
//Completed Order Grid
const gridConfig3 = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "", opType: SearchOperations.dateRange },
            { searchType: "Param", fieldName: "OrderStatus", fieldValue: orderStatus.Completed, fieldDisplayName: "Order No", opType: SearchOperations.equals },
           ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);
    gridConfig3.value.filters[0].fieldValue = [new Date(), new Date()]
//Completed Order Grid End
//Cancelled Order Grid
const gridConfig4 = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: '',
        filters: [
            { searchType: "Param", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "", opType: SearchOperations.dateRange },
            { searchType: "Param", fieldName: "OrderStatus", fieldValue: orderStatus.Cancelled, fieldDisplayName: "Order No", opType: SearchOperations.equals },
           ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);
    gridConfig4.value.filters[0].fieldValue = [new Date(), new Date()]
//Cancelled Order Grid End
</script>
<style scoped>
.card {
  background-color: #f4f4f4;
  padding: 1rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
.text-center {
  text-align: center;
}
</style>
