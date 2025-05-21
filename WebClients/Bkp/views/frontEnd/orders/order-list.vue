<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Order No</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full" for="status">Status</label>
                    <common-dropdown name="status" placeholder="All" v-model="gridConfig.filters[1].fieldValue"
                    :dataurl="UrlConstants.apidropdown + '?Mode=OrderStatus'" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div>
            <!-- <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Last Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <common-dropdown fieldlabel="Active" placeholder="Please Select" v-model="gridConfig.filters[1].fieldValue" :options="StatusDropOption" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div> -->
        </template>
        <template v-slot:advancefilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Order No</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full" for="status">Status</label>
                    <comnet-dropdown name="status" placeholder="All" v-model="gridConfig.filters[1].fieldValue"
                    :dataurl="UrlConstants.apidropdown + '?Mode=OrderStatus'" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div>
            <!-- <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">First Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Last Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <common-dropdown placeholder="Please Select" v-model="gridConfig.filters[1].fieldValue" :options="StatusDropOption" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div> -->
        </template>
        <template v-slot:right-buttons>
            
        </template>
        <template v-slot:columns>
            <Column selectionMode="multiple" class="check-box-center" frozen></Column>
            <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>&nbsp;
                        <Button :label="'Update'" title="Update Status" class="p-button-aux mr-3" @click="updateStatus(slotProps.data)" v-if="(slotProps.data.OrderStatus != 4 && slotProps.data.OrderStatus != 5)" /> 
                    </div>
                </template>
            </Column>

            <Column field="OrderNo" header="Order No" :sortable="true"></Column> 
            <Column field="ContactName" header="Name" :sortable="true"></Column>   
            <Column field="ContactNumber" header="Moblie" :sortable="true"></Column>    
            <Column field="ContactEmail" header="Email" :sortable="true"></Column> 
            <Column field="DeliveryAddress" header="Address" :sortable="true"></Column> 
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
            <Column field="OrderStatusName" header="Status" :sortable="true" class="text-center">
                <template #body="slotProps">                    
                    <Tag :value="slotProps.data.OrderStatusName" v-if="slotProps.data.OrderStatus == 1"></Tag>
                    <Tag severity="info" :value="slotProps.data.OrderStatusName" v-else-if="slotProps.data.OrderStatus == 2"></Tag>
                    <Tag severity="warning" :value="slotProps.data.OrderStatusName" v-else-if="slotProps.data.OrderStatus == 3"></Tag>
                    <Tag severity="success" :value="slotProps.data.OrderStatusName" v-else-if="slotProps.data.OrderStatus == 4"></Tag>
                    <Tag severity="danger" :value="slotProps.data.OrderStatusName" v-else="slotProps.data.OrderStatus == 5"></Tag>  
                </template>
            </Column>  
            <Column field="TotalItems" header="Item(s)" :sortable="true"></Column>  
            <Column field="CreatedUserName" header="Created by" :sortable="true"></Column>
            <Column field="ModifyUserName" header="Modify by" :sortable="true"></Column>             
        </template>
        
    </common-grid>
    <common-dialog ref="popupManage" class="medium-modal"></common-dialog>
    <common-dialog ref="popupManageSmall" class="small-modal"></common-dialog>
</template>
<script setup lang="ts">
    import { useRoute } from 'vue-router';
    import router from '@/router';
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { inject, ref } from "vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    import orderStatusManage from "@/views/frontEnd/orders/order-status-change.vue";
    import orderHistory from "@/views/frontEnd/orders/order-history.vue";
    const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
    const dt = ref(null);
    const popupManage = ref();
    const popupManageSmall = ref();
    const ddlItem = ref();
    const gridConfig = ref({
        api: UrlConstants.apiOrderMasterList,
        deleteApi: UrlConstants.apiOrderMaster,
        filters: [
            { searchType: "Filter", fieldName: "OrderNo", fieldValue: null, fieldDisplayName: "Order No", opType: SearchOperations.equals },
            { searchType: "Filter", fieldName: "OrderStatus", fieldDisplayName: "Status", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
           // { searchType: "Filter", fieldName: "FirstName", fieldValue: null, fieldDisplayName: "First Name", opType: SearchOperations.contains },
           // { searchType: "Filter", fieldName: "LastName", fieldValue: null, fieldDisplayName: "Last Name", opType: SearchOperations.contains },
        ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);

    const gridMenuItems = ref([        
        {
            icon: "pi pi-eye",
            label: "Details",
            callback: (data: any) => {
              router.push({ path: "/orders/view-order", query: { Id: EncryptData(data.Id.toString()) } });
            },
            visible:true, 
        },
        {
            icon: "pi pi-history",
            label: "History",
            callback: (data: any) => {
              popupManage.value.OpenModal(orderHistory, "Order History (#"+data.OrderNo+")", data, dt.value.reloadGrid);
            },
            visible:true, 
        },
        {
            icon: "pi pi-trash",
            label: "Delete",
            callback: (data: any) => {
                dt.value.SingleDelete(data.Id);
            },
            visible:true, 
        },
    ]);
    const StatusDropOption = ref([
        { Text: "Active", Value: "true" },
        { Text: "Inactive", Value: "false" },
    ]);

    const updateStatus = async (data:any) => {
        popupManageSmall.value.OpenModal(orderStatusManage, "Update Order Status (#"+data.OrderNo+")", data, dt.value.reloadGrid);
    }
   

    const filename = ref("order_list_"+Date.now())
</script>