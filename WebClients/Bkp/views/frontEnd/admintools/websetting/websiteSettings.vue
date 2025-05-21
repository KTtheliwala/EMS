<template>
    <div class="content-wrap">
        <common-grid :config="gridConfig" ref="dt" :filters="filters" :hideExportBtn="false" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
            <template v-slot:basicfilter>
                <div class="col-12 md:col-4 pb-0">
                    <div class="field">
                        <label class="block w-full">Key</label>
                        <common-input :placeholder="''" inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-4 pb-0">
                    <div class="field">
                        <label class="block w-full">Value</label>
                        <common-input :placeholder="''" inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                    </div>
                </div>                
            </template>
            <template v-slot:advancefilter>
                <div class="col-12 md:col-4 pb-0">
                    <div class="field">
                        <label class="block w-full">Key</label>
                        <common-input :placeholder="''" :IsNumber="true" inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-4 pb-0">
                    <div class="field">
                        <label class="block w-full">Value</label>
                        <common-input :placeholder="''" :IsNumber="true" inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                    </div>
                </div>                
            </template>
            <template v-slot:columns>
                <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
                    <template #body="columns">
                        <div>
                            <Button class="p-button-aux smallIconBtn" label="Edit" @click="OpenAddNewManage(columns.data)" />
                        </div>
                    </template>
                </Column>
                <Column field="Key" header="Key" :sortable="true">
                </Column>
                <Column field="Value" header="Value" :sortable="true"></Column>                
            </template>
        </common-grid>
        <common-dialog ref="popupManage" class="large-modal"></common-dialog>
    </div>
</template>
<script setup lang="ts">
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { inject, ref } from "vue";
    import websiteSettingManage from '@/views/frontEnd/admintools/websetting/websiteSettingManage.vue';
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    //const Page = AuthService.GetPagePermission('Setting','SystemSetting')
    const { GetData, PostData, PatchData } = new commonModule();
    const dialogRef = inject('dialogRef') as any;
    const IsPopup = dialogRef ? ref(dialogRef.value.data) : 0;
    const dt = ref(null);
    const popupManage = ref();
    const OpenAddNewManage = (data: any) => {
        popupManage.value.OpenModal(websiteSettingManage, "Edit Website Setting", data.Id, dt.value.reloadGrid);
    }
    const gridConfig = ref({
        api: UrlConstants.apiesystemsettinglist,
        deleteApi: UrlConstants.apisystemsetting,
        filters: [
            { searchType: "Filter", fieldName: "Key", fieldValue: "", fieldDisplayName: "Key", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "Value", fieldValue: null, fieldDisplayName: "Value", fieldDisplayValue: "", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "CountryId", fieldDisplayName: "Country", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "" }
        ],
        sortcolumn: "Id",
        sortorder: 1,
        doubleClickHander: IsPopup.value == 1 ? (data: any) => { dialogRef.value.close(data); } : null
    } as GridConfig);
</script>
