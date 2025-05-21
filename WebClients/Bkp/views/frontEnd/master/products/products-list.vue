<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Product Name</label>
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
                    <label class="block w-full">Product Name</label>
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
            <common-button btntype="button" label="Import Data" @click="OpenImportData()"></common-button>
            &nbsp;
            <Button icon="pi pi-plus-circle" label="Create Product"  @click="OpenAddNewManage"></Button>
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
            <Column field="CategoryName" header="Category Name" :sortable="true"></Column>
            <Column field="Name" header="Name" :sortable="true">
                <template #body="columns">
                    <div v-html="columns.data.Name"></div>
                </template>
            </Column>
            <Column field="ProductCode" header="ProductCode" :sortable="true"></Column>                                   
            <Column field="ShortDescription" header="Short Descr" :sortable="true"></Column>                                   
            <Column field="Description" header="Description" :sortable="true"></Column>                                   
            <Column field="Price" header="Price" :sortable="true"></Column>                                   
            <Column field="MetaTitle" header="Meta Title" :sortable="true"></Column>                                   
            <Column field="MetaKeyword" header="Meta Keyword" :sortable="true"></Column>                                   
            <Column field="MetaDescription" header="Meta Descr" :sortable="true"></Column>                                   
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
    import ProductManage from "@/views/frontEnd/master/products/products-manage.vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    import commonImportFile from "@/components/shared/commonControls/commonImportFile.vue";
   
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(ProductManage, "Create Product", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiProductsMasterUpdateStatus, data, "Product Status Updated successfully.", null, null);
    };

    const gridConfig = ref({
        api: UrlConstants.apiProductsMasterList,
        deleteApi: UrlConstants.apiProductsMaster,
        filters: [
            { searchType: "Filter", fieldName: "Name", fieldValue: null, fieldDisplayName: "Company", opType: SearchOperations.contains },
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
              popupManage.value.OpenModal(ProductManage, "Edit Product", data.Id, dt.value.reloadGrid);
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
const OpenImportData = () => {
  let cols: any[] = [{ Label: 'Name',ColName:'Name' }, { Label: 'ProductCode',ColName:'ProductCode' },{ Label: 'CategoryName',ColName:'Category Name' }, { Label: 'ShortDescription',ColName:'Short Descr' }, { Label: 'Description',ColName:'Description' }, { Label: 'Price',ColName:'Price' }, { Label: 'MetaTitle',ColName:'Meta Title' }, { Label: 'MetaKeyword',ColName:'Meta Keyword' }, { Label: 'MetaDescription',ColName:'Meta Descr' },{ Label: 'SortOrder',ColName:'Sort Order' },{ Label: 'IsActive',ColName:'Active' }];
  let data: any = { Mappings: cols, ValidateUrl: UrlConstants.apiProductsMasterValidateData, ImportUrl: UrlConstants.apiProductsMasterImportData, importFrom:'ImportProduct' }
  popupManage.value.OpenModal(commonImportFile, 'Import Product Data', data, dt.value.reloadGrid);
}
    const filename = ref("Product_list_"+Date.now())
</script>