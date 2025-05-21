<template>
    <div v-if="(!Props.IsOnlyGrid && !Props.hideNotificationReadBtn && !Props.hideFilter)" :class="AdvSearchBtnClass">
        <div class="filter-wrapper" :class="{ active: lazyParams.filters.length>0 }" v-if="Props.IsShowFilterBox">
            <div class="default-filter-wrap" >
                <div class="filter-fields">
                    <div class="grid guter-change">
                        <slot name="basicfilter"></slot>
                    </div>
                </div>
                <div class="AdvanceSearchAction">
                    <Button class="p-button-secondary mr-2"  icon="pi pi-search" label="Search" @click="ApplyFilter"></Button>
                    <Button class="p-button-aux"  icon="pi pi-refresh" @click="clearFilterItems" label="Clear All"></Button>
                </div>
            </div>
            <Button class="p-button-aux popup-filter-btn" v-if="Props.IsAdvFilter" @click.prevent="OpenAdvanceSearchModel">
                <img src="@/assets/images/filter.svg" />
            </Button>
            <div class="selected-filter-wrap" v-if="lazyParams.filters.filter(x=>x.searchType=='Filter').length>0">
                <h5>Filters</h5>
                <div class="selectedfilter">
                    <div class="selectedfilterItems" v-for="item in lazyParams.filters.filter(x=>x.searchType=='Filter')" :key="item.fieldName">
                        <span>{{IsNullEmptyOrUndefined(item.fieldDisplayName)?item.fieldName:item.fieldDisplayName}}</span>
                        <strong>{{IsNullEmptyOrUndefined(item.fieldDisplayValue)?item.fieldValue:item.fieldDisplayValue}}</strong>
                        <span class="Remove"><i class="pi pi-times" @click="removeFilterItems(item.fieldName)"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="admin-actions-btn-wrapper mt-0 mb-0" v-if="!Props.IsOnlyGrid">
        <div class="left-part">
            <Button class="btn p-button-aux" label="Export All" v-if="hideExportBtn" v-show="resultData.length>0" @click="ExportExcel" />
            <Button class="btn p-button-aux ml-1" :label='"Bulk Dispatch"+"("+selectedRecords.length+")"' v-if="hideMultiDispatch" v-show="selectedRecords.length>1" @click="multiDispacth" />
            <Button class="btn p-button-aux ml-1" label="Delete" v-if="selectedRecords.length && Props.hideDeletBtn && hideMultiDispatch != true" @click="Removeconfirm" />
            <slot name="left-part-button" v-if="selectedRecords.length"></slot>
            <slot name="extraDOM"></slot>
        </div>
        <div class="right-part">
            <slot name="right-buttons"></slot>
        </div>
    </div>
    <div class="common__Table">
        <div class="scroll_height" :class="{ active: lazyParams.filters.length>0 && !Props.IsOnlyGrid}">
            <DataTable ref="dt" :value="resultData" v-model:selection="selectedRecords" :lazy="true" :paginator="true" :rows="NumnerOfRow"
                       :totalRecords="totalRows" :loading="loading" @page="onPage($event)" @sort="onSort($event)"
                       class="p-datatable-customers" filterDisplay="menu"
                       :paginatorTemplate="paginatorTemplate"
                       
                       currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
                       :rowsPerPageOptions="[15, 30, 50, 100]" :responsiveLayout="responsiveLayoutValue"
                       @rowSelect="onRowSelect" @rowUnselect="onRowSelect" @rowSelectAll="onRowSelectAll" @rowUnSelectAll="onRowSelectAll" :scrollHeight="IsscrollHeight" showGridlines
                       @rowDblclick="onRowDubleClick" :resizableColumns="resizableColumns" :columnResizeMode="columnResizeMode"
                       :selectionMode="selectionMode">
                <slot name="columns"></slot>
                <template #empty>
                    <div class="no-data">
                        <img src="@/assets/images/no-items.png" alt="No Data Found" />
                        <h4>No Record Found</h4>
                    </div>
                </template>
                <template #footer v-if="showFooter">
                    <div class="table_footer">
                        <div class="tf_right">
                        </div>
                    </div>
                </template>
            </DataTable>
        </div>
    </div>

    <Dialog class="large-modal AdvanceFiltersDialog" header="Advance Search" v-model:visible="AdvanceSearchModel" :modal="true">
        <div class="grid guter-change">
            <slot name="advancefilter"></slot>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button icon="pi pi-refresh" label="Clear All" class="p-button-aux" @click="clearFilterItems()" />
                <Button icon="pi pi-plus-circle" label="Add Filters" @click="ApplyFilter($event, true)" autofocus />
            </div>
        </div>
    </Dialog>

    <Dialog header="Remove Confirmation" v-model:visible="RemoveConfirmation" :modal="true" @hide="CancelEvent">
        <div class="msgPopup">
            <p class="m-0 mb-3">Do you want to <strong>Remove {{selectedRecords.length}} Records?</strong></p>
            <div class="field-checkbox mb-0">
                <Checkbox inputId="Checkbox" name="Checkbox" value="Checkbox" v-model="RemoveRecords" />
                <label for="Checkbox">Confirm that I want to Remove.</label>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Cancel" @click="CancelEvent" class="p-button-aux mr-2" />
                <Button position='bottom-right' label="Remove" @click="Remove" class="p-button-primary" autofocus :disabled='!RemoveRecords || !RemoveRecords.length' />
            </div>
        </div>
    </Dialog>
</template>
<script setup lang="ts">
    import { ref, defineProps, defineEmits, PropType, defineExpose } from "vue";
    import DataTable from "primevue/datatable";
    import GridModule from "@/composables/modules/controls/gridModule";
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import DeleteModel from "@/models/common/deleteModel";
    import commonModule from "@/composables/modules/commonModule"
    import UrlConstants from "@/utils/urlconstants";
    import requestMethod from "@/composables/api/requestMethod";

    import importModel from "@/models/complaint/importModel";
import { useAuthDataStore } from "@/store/useAuthDataStore";

    const { DeleteData, PostData,ImportData } = new commonModule();
    const IsNullEmptyOrUndefined = (obj: any) => {
        return !obj || obj == "";
    }
    const emit = defineEmits(["update:modelValue", "update:title"]);
    const Props = defineProps({
        config: { type: Object as PropType<GridConfig>, required: true },
        filename: { type: String, required: true },
        hideExportBtn: { type: Boolean, default: true },
        hideMultiDispatch: { type: Boolean, default: false },
        hideNotificationReadBtn: { type: Boolean, default: false },
        hideFilter: { type: Boolean, default: false },
        hideAssignedToBtn: { type: Boolean, default: false },
        IsPageStateClear: { type: Boolean, default: false },
        Isscrollable: { type: Boolean, default: false },
        Ispaginator: { type: Boolean, default: false },
        IsscrollHeight: { type: String, default: "" },
        showFooter: { type: Boolean, default: false },
        NumnerOfRow: { type: Number, default: 15 },
        AdvSearchBtnClass: { type: String, default: "" },
        IsAdvFilter: { type: Boolean, default: true },
        IsOnlyGrid: { type: Boolean, default: false },
        hidecomplaintImportBtn: { type: Boolean, default: false },
        callBack: Function,
        IsInItData: { type: Boolean, default: true },
        IsShowFilterBox: { type: Boolean, default: true },
        ExtraDatacallBack:Function,
        resizableColumns: { type: Boolean, default: false },
        
        columnResizeMode:{ type: String, default: "fit" },
        selectionMode:{ type: String, default: "" },
        paginatorTemplate:{type:String,default:"RowsPerPageDropdown CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink"},
        hideDeletBtn: { type: Boolean, default: true },
    });
    const {
        loadLazyData,
        onDropDownSort,
        favourite,
        selectedRecords,
        dt,
        loading,
        resultData,
        totalRows,
        activeFilters,
        onPage,
        onSort,
        initData,
        //sidebarVisible,
        lazyParams,
        selectedColumns,
        columns,
        onChange,
        onDelete,
        reloadGrid,
        isFilter,
        //useLang,
        filtersCount,
        curPagePermission,
        ExportExcel,
        hideOnlyGrid,
        hideActiveBtn,
        hideDeactiveBtn,
        onRowSelect,
        onRowDubleClick,
        route, first, last, SelectedRecordExport, ApplyFilter, selectedvaluefilter, AdvanceSearchModel, selectedvaluefilterTitle, OpenAdvanceSearchModel,
        removeFilterItems, clearFilterItems,reinitGrid
    } = new GridModule(Props);
    const responsiveLayoutValue = ref("scroll");
    const MenuViewItem = ref(true);

    const Remove = () => {
        TobeDeleteRecords.value.Ids = [];
        if (selectedRecords.value.length > 0) {
            for (let i = 0; i < selectedRecords.value.length; i++)
                TobeDeleteRecords.value.Ids.push(selectedRecords.value[i]["Id"]);
            DeleteData(Props.config.deleteApi, TobeDeleteRecords.value, (data: any) => {
                loadLazyData();
                RemoveConfirmation.value = false;
                RemoveRecords.value = false;
            }, (data: any) => {
                RemoveConfirmation.value = false;
                RemoveRecords.value = false;
            });
        }
    }
    const CancelEvent = () => {
        selectedRecords.value = [];
        RemoveConfirmation.value = false;
        RemoveRecords.value = false;
    }
    const RemoveConfirmation = ref(false);
    const RemoveRecords = ref(false);
    const TobeDeleteRecords = ref<DeleteModel>({ Ids: [], IsDelete: false });
    const Removeconfirm = () => {
        RemoveConfirmation.value = true;
    }
    const SingleDelete = (id: any) => {
        selectedRecords.value = [];
        selectedRecords.value.push({ Id: id });
        Removeconfirm();
    }

    const getSlectedItems = () => {
        // emit("update:modelValue", value);
        if (typeof Props.callBack != "undefined") {
            Props.callBack(selectedRecords.value);
        }
    }

    const multiDispacth = () => {
        // emit("update:modelValue", value);
        if (typeof Props.ExtraDatacallBack != "undefined") {
            Props.ExtraDatacallBack(selectedRecords.value);
        }
    }

    const getSlectedItemsForNotification = (ReloadGrid:any) => {
        // emit("update:modelValue", value);
        if (typeof Props.callBack != "undefined") {
            Props.callBack(selectedRecords.value,ReloadGrid);
        }
    }

    //
     const importRecords = ref<importModel>({ Ids: [], BUId:0, CountryId:0 });
    const authLogin = useAuthDataStore().getAuth as any;
    const ImportComplaint = async () => {
        TobeDeleteRecords.value.Ids = [];
        importRecords.value.Ids = []
        importRecords.value.BUId = 0
        importRecords.value.CountryId = 0
        if (selectedRecords.value.length > 0) {
            for (let i = 0; i < selectedRecords.value.length; i++)
                importRecords.value.Ids.push(selectedRecords.value[i]["Id"]);
              
              if((authLogin.BUs || []).length > 0)
              {
                importRecords.value.BUId = authLogin.BUs[0]
              }
              importRecords.value.CountryId = authLogin.CountryId
             ImportData(UrlConstants.apiSaveComplaint, importRecords.value, (data: any) => {

                loadLazyData();
            }, (data: any) => {
                RemoveConfirmation.value = false;
                RemoveRecords.value = false;
            },true);
        }
    }

    defineExpose({
        selectedRecords,
        reloadGrid,
        onDropDownSort,
        curPagePermission,
        MenuViewItem,
        hideOnlyGrid,
        SingleDelete,
        initData,
        ApplyFilter,
        ExportExcel,
        reinitGrid,
        clearFilterItems,
        lazyParams
    });

</script>
