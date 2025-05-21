<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Role Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
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
                    <label class="block w-full">Role Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
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
            <Button icon="pi pi-plus-circle" label="Create Role"  v-if="(Page?.IsAdd ?? false )" @click="OpenAddNewManage"></Button>
        </template>
        <template v-slot:columns>
            <Column selectionMode="multiple" class="check-box-center" frozen v-if="(Page?.IsDelete ?? false )"></Column>
            <Column header="Action" v-if="(Page?.IsEdit ?? false ) || (Page?.IsDelete ?? false) || ((PagePermission?.IsEdit ?? false)||(PagePermission?.IsAdd ?? false))" class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                    </div>
                </template>
            </Column>
            <Column field="Name" header="Name" :sortable="true"></Column>                        
           <!--  <Column field="SortOrder" header="Sort Order" :sortable="true" class="sort-order-col"></Column> -->
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
    import { ref } from "vue";
    import rolesManage from "@/views/frontEnd/admintools/roles/roles-manage.vue";
    import managePermission from '@/views/frontEnd/admintools/roles/permission-manage.vue';
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    const Page = AuthService.GetPagePermission('AdminTools','Role')
    const PagePermission = AuthService.GetPagePermission('AdminTools','Permission')
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(rolesManage, "Create Role", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiroleupdatestatus, data, "Role Status Updated successfully.", null, null);
    };

    const gridConfig = ref({
        api: UrlConstants.apirolelist,
        deleteApi: UrlConstants.apirole,
        filters: [
            { searchType: "Filter", fieldName: "Name", fieldValue: null, fieldDisplayName: "Role Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", }
        ],
        sortcolumn: "Id",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);

    const gridMenuItems = ref([
        {
            icon: "pi pi-user-edit",
            label: "Permission",
            callback: (data: any) => {
              popupManage.value.OpenModal(managePermission, 'Manage Permission', data, dt.value.reloadGrid);
            },
            visible:((PagePermission?.IsEdit ?? false)||(PagePermission?.IsAdd ?? false)), 
        },
        {
            icon: "pi pi-pencil",
            label: "Edit",
            callback: (data: any) => {
              popupManage.value.OpenModal(rolesManage, "Edit Role", data.Id, dt.value.reloadGrid);
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

    const filename = ref("role_list_"+Date.now())
</script>