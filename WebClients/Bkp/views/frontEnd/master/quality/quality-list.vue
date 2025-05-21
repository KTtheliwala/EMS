<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Quality</label>
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
                    <label class="block w-full">Quality Name</label>
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
            <Button icon="pi pi-plus-circle" label="Create Quality"  @click="OpenAddNewManage"></Button>
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
            <Column field="Name" header="Name" :sortable="true"></Column>
            <Column field="ShortName" header="Short Name" :sortable="true"></Column>
            <Column field="GroupName" header="Group Name" :sortable="true"></Column>
            <Column field="SubGroupName" header="Sub Group Name" :sortable="true"></Column>
            <Column field="HSNCode" header="HSN Code" :sortable="true"></Column>
            <Column field="Denier" header="Denier" :sortable="true"></Column>
            <Column field="Filament" header="Filament" :sortable="true"></Column>
            <Column field="Luster" header="Luster" :sortable="true"></Column>
            <Column field="QualityType" header="QualityType" :sortable="true"></Column>
            <Column field="Description" header="Description" :sortable="true"></Column>
            <Column field="SortOrder" header="Sort Order" :sortable="true" class="sort-order-col"></Column>
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
    import qualityManage from "@/views/frontEnd/master/quality/quality-manage.vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
   
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(qualityManage, "Create Quality", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiQualityUpdateStatus, data, "Quality Status Updated successfully.", null, null);
    };

    const gridConfig = ref({
        api: UrlConstants.apiQualityList,
        deleteApi: UrlConstants.apiQuality,
        filters: [
            { searchType: "Filter", fieldName: "Name", fieldValue: null, fieldDisplayName: "Quality", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", }
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
              popupManage.value.OpenModal(qualityManage, "Edit Quality", data.Id, dt.value.reloadGrid);
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

    const filename = ref("quality_list_"+Date.now())
</script>
