import { inject, reactive, ref } from "vue";
import { ExportType } from "@/models/controls/Grid/gridRequest";
import requestMethod from "@/composables/api/requestMethod";
import GridColumn from "@/models/controls/Grid/gridColumn";
import GridConfig from "@/models/controls/Grid/gridConfig";
import CommonHelper from "@/utils/commonHelper";
import { useRoute } from "vue-router";
// import permissionModel from "@/models/controls/permissionModel";
import router from "@/router";
import UrlConstants from "@/utils/urlconstants";
import GridRequest from "@/models/controls/Grid/gridRequest";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import moment from "moment";
import { useToast } from "primevue/usetoast";
class GridModule {
    readonly authStore = inject("authentication") as any;
    readonly selectedRecords = ref<any[]>([]);
    readonly selectAll = ref(false);
    readonly dt = ref();
    readonly loading = ref(false);
    readonly hideOnlyGrid = ref(false);
    readonly hideActiveBtn = ref(false);
    readonly hideDeactiveBtn = ref(false);
    isFilter = ref(false);
    readonly totalRows = ref(0);
    // readonly activeFilters = ref<SearchGrid[]>([]);
    lazyParams = {} as GridRequest;
    readonly resultData = ref([]);
    sidebarVisible = ref(false);
    isShowPopup = ref(false);
    readonly props?: any;
    objGrid = reactive({} as GridConfig);
    selectedColumns = ref();
    columns = ref<any[]>([]);
    // readonly useLang = useLanguageStrore();
    // curPagePermission = ref({} as permissionModel);
    readonly favourite = ref(1);
    readonly route = useRoute();
    readonly first = ref();
    readonly last = ref();
    readonly IsTop = ref(false);
    readonly toast = useToast();
    searchProdList = ref<any[]>([]);
    constructor(props?: unknown) {
        this.props = props;
        this.hideActiveBtn.value = this.props.hidesActivate;
        this.hideDeactiveBtn.value = this.props.hideDeactivate;
        this.objGrid = this.props.config;
        this.initData();
    }
    readonly selectedvaluefilter = ref(false);
    readonly AdvanceSearchModel = ref();
    readonly selectedvaluefilterTitle = ref(true);
    OpenAdvanceSearchModel = () => {
        this.AdvanceSearchModel.value = true;
    };
    CloseAdvanceSearchModel = () => {
        this.AdvanceSearchModel.value = false;
    };
    GetTimeZone = () => {
        const timezone = JSON.parse(localStorage.getItem('useAuthData')?.toString() ?? "null");
        if (timezone && (timezone?.userModel?.Offset ?? "") != "")
            return timezone.userModel.Offset;

        else {
            return moment().format('Z');
        }
    }
    initData = () => {        
        this.lazyParams = {
            first: 0,
            rows: this.dt.value?.rows ?? this.props.NumnerOfRow,
            sortField: this.objGrid.sortcolumn,
            sortOrder: this.objGrid.sortorder,
            filters: this.objGrid.filters.filter(x => x.fieldValue != ""),
            columns: [],
            Timezone: this.GetTimeZone()
        };
        if(this.props.IsInItData)
        {
            this.ApplyFilter();
        }
        
    };
    reloadGrid = (isReset: boolean) => {
        if (isReset) {
            this.lazyParams.first = 0;
            this.dt.value.d_first = 0
            this.lazyParams.sortField = this.objGrid.sortcolumn;
            this.lazyParams.sortOrder = this.objGrid.sortorder;
            this.lazyParams.filters = [];
        }
        this.loadLazyData();
    };
    setFilterPermeter = () => {
        this.lazyParams.filters = this.objGrid.filters.filter((newitems: { fieldValue: any, fieldName: any, opfieldValue?: any, opType?: SearchOperations }) => {
            debugger
            if (newitems.opType == SearchOperations.dateRange) {
                const fromDate = newitems.fieldValue[0];
                const toDate = newitems.fieldValue[1];
                if (typeof newitems.fieldValue != "string")
                    newitems.fieldValue = fromDate.getDate() + '/' + (fromDate.getMonth() + 1) + '/' + fromDate.getFullYear() + '-' + toDate.getDate() + '/' + (toDate.getMonth() + 1) + '/' + toDate.getFullYear();
            }
            if (typeof newitems.opfieldValue == 'object') {
                if (typeof newitems.opfieldValue == 'object') {
                    if (Number(newitems.opfieldValue.length ?? 0) > 1) {
                        newitems.opfieldValue?.forEach((els: any) => {
                            newitems.fieldValue = newitems.fieldValue + "," + els.Key
                        })
                    }
                    else {
                        if (Number(newitems.opfieldValue.length ?? 0) > 1) {
                            newitems.fieldValue = newitems.opfieldValue[0].Key
                        }
                    }
                }
            }
            return newitems.fieldValue != ""
        }, {})
        this.lazyParams.filters.forEach(function (item: any) {
            item.fieldValue = typeof item.fieldValue !== "undefined" ? item.fieldValue : "";
            item.fieldValue = item.fieldValue.toString();
        });
        this.lazyParams.first = 0;
    };
    loadLazyData = () => {        
        this.selectedRecords.value = [];
        this.columns = ref<any[]>([]);
        this.loading.value = true;
        this.resultData.value = []
        this.totalRows.value = 0;
        if (this.objGrid?.api ?? "" != "") {
            const apiData=JSON.parse(JSON.stringify(this.lazyParams));
            apiData.filters=apiData.filters.map((obj: { fieldName: any; fieldValue: any; isTimeZone: any; opType: any; }) => ({fieldName: obj.fieldName, fieldValue: obj.fieldValue,isTimeZone:obj.isTimeZone,opType:obj.opType}));
            requestMethod.Post(this.objGrid.api, apiData).then((data: any) => {
                if (!data.isAxiosError) {
                    if ((data?.data?.StatusCode ?? 0) == 200) {
                        if (this.IsTop.value) {
                            this.scrollTop();
                            this.IsTop.value = false
                        }
                        
                       
                            if (typeof this.props.callBack != "undefined") {
                              //  if(this.objGrid.api == UrlConstants.apiCashMeterMatchingList || this.objGrid.api == UrlConstants.apiSaleMeterReadingList || UrlConstants.apiMeterCashMatchingList){
                                    this.props.callBack(data?.data);
                              /* }else {
                                    this.props.callBack(data?.data?.Data?.Data);
                                }*/
                               
                            }  
                        
                        if (typeof this.props.ExtraDatacallBack != "undefined" && typeof data?.data.ExtraData != "undefined")
                        {
                            this.props.ExtraDatacallBack(data?.data.ExtraData);
                        }
                        this.resultData.value = data.data.Data;
                        this.totalRows.value = data.data.RecordsTotal;
                        this.loading.value = false;
                        this.columns.value = []
                        for (let i = 0; i < (this.dt?.value?.columns?.length ?? 0); i++) {
                            if (this.dt.value.columns[i].props.header != undefined)
                                this.columns.value.push({
                                    name: this.dt.value.columns[i].props.header,
                                    data: this.dt.value.columns[i].props.field,
                                });
                        }
                        // setTimeout(() => {
                            
                        
                        // document.querySelectorAll('.p-datatable-tbody tr').forEach(row => {
                            
                        //     const hiddenInput = row.querySelector('input.hide-checkbox') as HTMLInputElement;

                        //     if (hiddenInput && hiddenInput.value === 'true') {
                        //         const checkboxWrapper = row.querySelector('.p-selection-column .p-checkbox') as HTMLElement;
                        //         if (checkboxWrapper) {
                        //         checkboxWrapper.remove();
                        //         }
                        //     }
                        //     });
                        // }, 100);
                        //AuthService.setMenuPermission(this.Menulist.value);
                        // this.selectedColumns.value = this.columns.value;
                        //this.curPagePermission.value = AuthService.getPermissionByPageName(String(this?.route?.name))
                    }
                    else  if ((data?.response?.status ?? 0) == 403){
                        this.toast.add({ severity: 'error', summary: 'Permission denied', detail: data?.response?.data?.Message ?? "Something went wrong. Please try again after sometime.", life: 3000 });                
                        this.loading.value = false;
                    }
                }
                else  if ((data?.response?.status ??0) == 403){
                    this.toast.add({ severity: 'error', summary: 'Permission denied', detail: data?.response?.data?.Message ?? "Something went wrong. Please try again after sometime.", life: 3000 });                
                    this.loading.value = false;
                }
            });
        }
    };
    onChange = (event: any) => {
        this.selectedColumns.value = event;
        for (let i = 0; i < this.dt.value.columns.length; i++) {
            const IsCol = this.selectedColumns.value.indexOf(
                this.dt.value.columns[i].props.header
            );
            if (IsCol >= 0 && this.dt.value.columns[i].props.header != undefined) {
                this.dt.value.columns[i].props.hidden = true;
            }
        }
    };
    ExportExcel = (type: number) => {
        if((this.resultData.value || []).length  == 0){ return false; }
        this.loading.value = true;
        this.lazyParams.exportType = type == 2 ? ExportType.Pdf : ExportType.Excel;
        this.lazyParams.ResponseType = type == 2 ? ExportType.Pdf : ExportType.Excel;
        this.lazyParams.Timezone = this.GetTimeZone();
        this.lazyParams.columns = this.columns.value;
        this.lazyParams.first = 0;
        requestMethod.ExportData(this.objGrid.api, this.lazyParams).then((data) => {
            this.loading.value = false;
            CommonHelper.ExportToGrid(this.props.filename, data.data, this.lazyParams.exportType ?? 2);
            this.lazyParams.exportType = ExportType.None;
            this.lazyParams.ResponseType = ExportType.None;
        });
    };
    SelectedRecordExport = (type: number) => {
        const ids = this.selectedRecords.value.map(function (item) {
            return parseInt(item.Id);
        }) as [];
        // adding id filter for selected record to export 
        // this.lazyParams.exportFilters.push({ fieldName: 'Id', fieldValue: ids, opType: SearchOperations.inClause });
        this.loading.value = true;
        // this.lazyParams.exportType = type == 2 ? ExportType.Pdf : ExportType.Excel;
        // this.lazyParams.columns = this.columns.value;
        // requestMethod.ExportData(this.objGrid.api, this.lazyParams).then((data) => {
        //     this.loading.value = false;
        //     CommonHelper.ExportToGrid(this.props.filename, data.data, this.lazyParams.exportType ?? 2);
        //     this.lazyParams.exportType = ExportType.None;
        //     this.lazyParams.exportFilters = []; // clear selected export filter so export all work okay
        // });
    };
    ApplyFilter = () => {        
        const filtersList = JSON.parse(JSON.stringify(this.objGrid.filters));
        this.lazyParams.filters = filtersList.filter((newitems: { fieldValue: any, fieldName: any, opfieldValue?: any, opType?: SearchOperations, searchType: string }) => {            
            
            if (newitems.opType == SearchOperations.dateRange) {                
                if (newitems.fieldValue != null && newitems.fieldValue != "") {                    
                    const fromDate = moment(newitems.fieldValue[0]).format(((this.objGrid.api === "AttendanceActivation/List" || this.objGrid.api === "Reports/List" || this.objGrid.api === "EmployeeAttendance/List" || this.objGrid.api === "Expense/List")?'MMM YYYY':'DD MMM YYYY'))
                    let toDate = undefined;
                    if (newitems.fieldValue[1] !== null) {
                        //if((this.objGrid.api !== "AttendanceActivation/List"))
                        {
                            toDate = moment(newitems.fieldValue[1]).format(((this.objGrid.api === "AttendanceActivation/List" || this.objGrid.api === "Reports/List" || this.objGrid.api === "EmployeeAttendance/List" || this.objGrid.api === "Expense/List")?'MMM YYYY':'DD MMM YYYY'))
                        }
                    }
                    newitems.fieldValue = toDate !== undefined ? String(`${fromDate}-${toDate}`) : String(`${fromDate}`)
                }
            }
            else if (typeof newitems.opfieldValue == 'number') {
                if (Number(newitems.opfieldValue ?? 0) > 0) {
                    newitems.fieldValue = Number(newitems.opfieldValue ?? 0)
                }
            }
            else if (typeof newitems.opfieldValue == 'object') {
                if (typeof newitems.opfieldValue == 'object') {
                    if (Number(newitems.opfieldValue.length ?? 0) > 1) {
                        newitems.opfieldValue?.forEach((els: any) => {
                            newitems.fieldValue = newitems.fieldValue + "," + els.Key
                        })
                    }
                    else {
                        if (Number(newitems.opfieldValue.length ?? 0) == 1) {
                            newitems.fieldValue = newitems.opfieldValue[0].Key
                        }

                    }
                    if (Number(newitems.opfieldValue.length ?? 0) == 0) {
                        newitems.fieldValue = ""
                    }

                }
            }
            return newitems.fieldValue != "" && newitems.fieldValue != "ALL" && newitems.fieldValue != "All" && newitems.fieldValue != null

        }, {});
        this.lazyParams.filters.forEach(function (item: any) {
            item.fieldValue = typeof item.fieldValue !== "undefined" ? item.fieldValue : "";
            item.fieldValue = item.fieldValue.toString();
        });
        this.lazyParams.first= 0;
        //this.lazyParams.rows= this.dt.value?.rows ?? this.props.NumnerOfRow;
        this.loadLazyData();
        this.sidebarVisible.value = false;
        this.isFilter.value = true;
        this.hideOnlyGrid.value = false;
        this.CloseAdvanceSearchModel();
    };
    onDelete = (parm: any) => {
        const ids: number[] = [];
        this.selectedRecords.value.forEach((e: any) => ids.push(e.Id));
        this.props.config.deletePopup.OpenPopUp(true, ids, "Remove");
    };
    onDeactivate = (IsActive: boolean) => {
        const ids = [] as any;
        this.selectedRecords.value.forEach((e: any) => ids.push({ Id: e.Id, IsActive: IsActive, LastStatus: e.IsActive }));
        this.props.config.updatePopup.OpenPopUpForActiveStatus(ids, IsActive ? "Activate" : "Deactivate");
    };
    onPage = (event: any) => {
        if (this.lazyParams.first == event.first && this.lazyParams.rows == event.rows) {
            return
        }
        //this.lazyParams.sortField = this.lazyParams.sortField == null ?  this.objGrid.sortcolumn : this.lazyParams.sortField;
        //if (event.sortOrder == null) this.lazyParams.sortOrder = 1;
        if (event.sortField == null)
            this.lazyParams.sortField = this.lazyParams.sortField != "" ? this.lazyParams.sortField : this.objGrid.sortcolumn;
        this.lazyParams.sortOrder = event.sortOrder == null ? this.lazyParams.sortOrder == null ? 1 : this.lazyParams.sortOrder : event.sortOrder;

        this.lazyParams.first = event.first;
        this.lazyParams.rows = event.rows;
        this.lazyParams.Length = event.rows;
        this.IsTop.value = true
        this.loadLazyData();
        this.first.value = (event.originalEvent.first + 1)
        this.last.value = (event.originalEvent.pageCount == (event.originalEvent.page + 1) ? this.totalRows.value : (event.rows * (event.originalEvent.page + 1)));

    };
    onSort = (event: any) => {
        if((this.resultData.value || []).length  == 0){ return false; }
        this.lazyParams.sortField = event.sortField == null ? this.objGrid.sortcolumn : event.sortField;
        this.lazyParams.sortOrder = event.sortOrder == null ? 1 : event.sortOrder;
        this.lazyParams.first = 0;
        this.loadLazyData();        
    };
    onDropDownSort = (sortField: any, sortOrder: any) => {
        this.lazyParams.sortField = sortField == null ? this.objGrid.sortcolumn : sortField;
        this.lazyParams.sortOrder = sortOrder == null ? 1 : sortOrder;

        this.lazyParams.first = 0;
        this.first.value = 0;

        this.loadLazyData();
    };
    removeFilterItems = (name: string): void => {
        this.lazyParams.filters = this.lazyParams.filters.filter(obj => { return obj.fieldName !== name });
        const objFilter = this.objGrid.filters.find(obj => obj.fieldName == name);
        if (objFilter)
            objFilter.fieldValue = null as any;
        this.loadLazyData();
    };
    clearFilterItems = (): void => {
        this.lazyParams.filters = this.lazyParams.filters.filter(obj => { return obj.searchType != "Filter" });
        this.objGrid.filters.forEach(element => {
            if (element.searchType == "Filter")
                element.fieldValue = null as any;
        });
        this.loadLazyData();
        this.CloseAdvanceSearchModel();
    };
    // onGlobleSearch = (): void => {
    //     //if (IsPageStateClear) {
    //         this.dt.value.d_first = 0;
    //         this.lazyParams.first = 0;
    //         this.first.value = 0;
    //     //}

    //     this.last.value = this?.dt?.value?.processedData?.length ?? 10;
    //     const foundInfo = this.objGrid.filters.
    //         findIndex((x: { fieldName: any; }) => x.fieldName == this.objGrid.globleFilter.fieldName);
    //     if (foundInfo === -1) {
    //         this.objGrid.filters.push(this.objGrid.globleFilter);
    //     }
    //     else {
    //         this.objGrid.filters[foundInfo].fieldValue = this.objGrid.globleFilter.fieldValue
    //     }
    //     this.ApplyFilter('');
    // };
    onRowSelect = (): void => {
        this.hideActiveBtn.value = false;
        this.hideDeactiveBtn.value = false;
        if(Array.isArray(this.selectedRecords)) {       
            this.selectedRecords.value.forEach((e: any) => {
                if (e.IsActive == true)
                    this.hideDeactiveBtn.value = true;
                if (e.IsActive == false)
                    this.hideActiveBtn.value = true;
            });
        }
    }
    scrollTop = (): void => {
        window.scroll(0, 0);
    }
    onRowDubleClick = (data: any) => {
        if (this.objGrid.doubleClickHander) {
            this.objGrid.doubleClickHander(data);
        }
    }
    reinitGrid = (): void => {
        this.lazyParams.filters = this.lazyParams.filters.filter(obj => { return obj.searchType != "Filter" });
        this.objGrid.filters.forEach(element => {
            if (element.searchType == "Filter")
                element.fieldValue = null as any;
        });
        this.lazyParams = {
            first: 0,
            rows: this.dt.value?.rows ?? this.props.NumnerOfRow,
            sortField: this.objGrid.sortcolumn,
            sortOrder: this.objGrid.sortorder,
            filters: this.objGrid.filters.filter(x => x.fieldValue != ""),
            columns: [],
            Timezone: this.GetTimeZone()
        };
        this.resultData.value = []
        this.dt.value.d_first = 0
        this.totalRows.value = 0
    }
}
export default GridModule;