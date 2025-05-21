<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">User Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
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
                    <common-dropdown fieldlabel="Active" placeholder="Please Select" v-model="gridConfig.filters[1].fieldValue" :options="StatusDropOption" @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
                </div>
            </div>
        </template>
        <template v-slot:advancefilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">User Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
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
            </div>
        </template>
        <template v-slot:right-buttons>
            <Button icon="pi pi-plus-circle" label="Create User" v-if="(Page?.IsAdd ?? false )" @click="OpenAddNewManage"></Button>
        </template>
        <template v-slot:columns>
            <Column selectionMode="multiple" class="check-box-center" frozen v-if="(Page?.IsDelete ?? false )"></Column>
            <Column header="Action" v-if="(Page?.IsEdit ?? false ) || (Page?.IsDelete ?? false )" class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                    </div>
                </template>
            </Column>
            <Column field="UserName" header="User Name" :sortable="true"></Column> 
            <Column field="FirstName" header="First Name" :sortable="true"></Column>   
            <Column field="LastName" header="Last Name" :sortable="true"></Column>    
            <Column field="Email" header="Email" :sortable="true"></Column>  
            <Column field="Mobile" header="Mobile" :sortable="true"></Column> 
            <Column field="RoleName" header="Role Name" :sortable="true"></Column> 
            <Column field="PasswordChangedOn" header="Last Pwd Change On" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.PasswordChangedOn, "DD MMM YYYY HH:ss") }}
                </template>
            </Column> 
            <Column field="LastLoggedInOn" header="Last LoggedIn On" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.LastLoggedInOn, "DD MMM YYYY HH:ss") }}
                </template>
            </Column> 
            <Column field="SortOrder" header="Sort Order" :sortable="true" class="sort-order-col"></Column>
            <Column header="Active" :sortable="true" field="IsActive" class="active-col">
                <template #body="columns">
                    <InputSwitch  v-model="columns.data.IsActive" :disabled="!(Page?.IsEdit ?? false ) ? true : false" @change="updateIsActive(columns.data.Id)" />
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
    import { inject, ref } from "vue";
    import userManage from "@/views/frontEnd/admintools/users/user-manage.vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    const Page = AuthService.GetPagePermission('AdminTools','User')
    const { ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(userManage, "Create User", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiuserupdatestatus, data, "User Status Updated successfully.", null, null);
    };
    const gridConfig = ref({
        api: UrlConstants.apiuserlist,
        deleteApi: UrlConstants.apiuser,
        filters: [
            { searchType: "Filter", fieldName: "UserName", fieldValue: null, fieldDisplayName: "User Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
            { searchType: "Filter", fieldName: "FirstName", fieldValue: null, fieldDisplayName: "First Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "LastName", fieldValue: null, fieldDisplayName: "Last Name", opType: SearchOperations.contains },
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
              popupManage.value.OpenModal(userManage, "Edit User", data.Id, dt.value.reloadGrid);
            },
            visible:(Page?.IsEdit ?? false ), 
        },
        {
            icon: "pi pi-trash",
            label: "Delete",
            callback: (data: any) => {
                dt.value.SingleDelete(data.Id);
            },
            visible:(Page?.IsDelete ?? false ), 
        },
    ]);
    const StatusDropOption = ref([
        { Text: "Active", Value: "true" },
        { Text: "Inactive", Value: "false" },
    ]);

    const filename = ref("user_list_"+Date.now())
</script>