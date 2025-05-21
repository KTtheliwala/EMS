<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Party Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div> 
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Party Code</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <common-dropdown fieldlabel="Active" placeholder="Please Select" v-model="gridConfig.filters[1].fieldValue" :options="StatusDropOption" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div>
        </template>
        <template v-slot:advancefilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Party Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div> 
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Party Code</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <common-dropdown placeholder="Please Select" v-model="gridConfig.filters[1].fieldValue" :options="StatusDropOption" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div>
        </template>
        <template v-slot:right-buttons>
            <Button icon="pi pi-plus-circle" label="Add Customer"  @click="OpenAddNewManage"></Button>
        </template>
        <template v-slot:columns>
            <Column selectionMode="multiple" class="check-box-center" frozen></Column>
            <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                    </div>
                </template>
            </Column>
            <Column field="PartyCode" header="Party Code" :sortable="true"></Column>
            <Column field="PartyName" header="Party Name" :sortable="true"></Column> 
            <Column field="Email" header="Email" :sortable="true"></Column>
            <Column field="BrokerCode" header="Broker Code" :sortable="true"></Column>
            <Column field="BrokerName" header="Broker Name" :sortable="true"></Column>
            <Column field="Address1" header="Address1" :sortable="true"></Column>    
            <Column field="Address2" header="Address2" :sortable="true"></Column>   
            <Column field="Address3" header="Address3" :sortable="true"></Column>               
            <Column field="City" header="City" :sortable="true"></Column>               
            <Column field="MobileNo" header="Mobile No" :sortable="true"></Column>               
            <Column field="BankName" header="Bank" :sortable="true"></Column>                  
            <Column field="ContactName" header="Contact Name" :sortable="true"></Column>                 
            <Column field="PanNo" header="Pan No" :sortable="true"></Column>                   
            <Column field="Area1" header="Area1" :sortable="true"></Column>                   
            <Column field="AccountGroupCode" header="AC Group Code" :sortable="true"></Column>                
            <Column field="AreaCode" header="Area Code" :sortable="true"></Column>               
            <Column field="SmsNo" header="SMS No" :sortable="true"></Column>                 
            <Column field="GSTNo" header="GST No" :sortable="true"></Column>                  
            <Column field="CollectionMen" header="Collection Men" :sortable="true"></Column>                
            <Column field="PropritorName" header="Propritor Name" :sortable="true"></Column>         
            <Column header="Active" :sortable="true" field="IsActive" class="active-col">
                <template #body="columns">
                    <InputSwitch v-model="columns.data.IsActive" @change="updateIsActive(columns.data.Id)" />
                </template>
            </Column>
        </template>
    </common-grid>
    <common-dialog ref="popupManage" class="large-modal"></common-dialog>
</template>
<script setup lang="ts">
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { ref } from "vue";
    import customerManage from "@/views/frontEnd/master/customer/customer-manage.vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
   
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(customerManage, "Create Company Filed", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apicustomerUpdateStatus, data, "Company Filed Status Updated successfully.", null, null);
    };

    const gridConfig = ref({
        api: UrlConstants.apicustomerList,
        deleteApi: UrlConstants.apicustomer,
        filters: [
            { searchType: "Filter", fieldName: "PartyName", fieldValue: null, fieldDisplayName: "Party Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
            { searchType: "Filter", fieldName: "PartyCode", fieldValue: null, fieldDisplayName: "Party Code", opType: SearchOperations.contains }
        ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);

    const gridMenuItems = ref([
        {
            icon: "pi pi-pencil",
            label: "Edit",
            callback: (data: any) => {
                popupManage.value.OpenModal(customerManage, "Edit Company Filed", data.Id, dt.value.reloadGrid);
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

    const filename = ref("customer_list_"+Date.now())
</script>
