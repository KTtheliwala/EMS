<template>
    <div class="advance-search-filter-only-for-mobile">
        <div class="filter-wrapper">
            <div class="default-filter-wrap">
                <div class="filter-fields">
                    <div class="grid guter-change">
                        <div class="col-12 md:col-3 pb-0">
                            <div class="field">
                                <label class="block w-full">Month-Year</label>
                                <Calendar v-model="dateFilter" :view="'month'" dateFormat="MM yy" :showButtonBar="true"
               :selectOtherMonths="true" :showIcon="true" class="w-full" inputId="dateRange2"></Calendar>
                            </div>
                        </div>
                        <div class="col-12 md:col-2 pb-0">
                            <div class="field">
                                <label class="block w-full">Enroll No</label>
                                <common-input inputclass="form-control" v-model="enrollFilter"></common-input>
                            </div>
                        </div>
                        
                        <div class="col-12 md:col-2">                            
                            <div class="field">
                                <label class="custom">Division:</label> 
                                <common-dropdown placeholder="Division"
                                name="DivisionID" v-model="DivisionID" 
                                :dataurl="UrlConstants.apidropdown+'?Mode=Division'"
                                :isWatch="false" 
                                 />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="AdvanceSearchAction">
                    <common-button class="p-button-secondary mr-2" btnicon="pi pi-search" :btnlabel="'Search'"
                        :btntype="'text'" @click="getData()"></common-button>
                    <common-button class="p-button-aux" btnicon="pi pi-refresh" @click="ClearAll" :btnlabel="'Clear All'"
                        :btntype="'text'"></common-button>
                </div>
            </div>
            <Button class="p-button-aux popup-filter-btn" @click.prevent="OpenAdvanceSearchModel">
                <img src="@/assets/images/filter.svg" />
            </Button>
        </div>
    </div>
    <div class="common__Table">
        <div class="admin-actions-btn-wrapper mt-0 mb-0">
            <div class="left-part">
                <common-button class="mr-2" icon="pi pi-file-pdf" severity="danger" :label="'Bulk Export Voucher'"
                v-show="gridDataList.length > 0"   @click="exportBulkDataVoucher"  />
                <common-button class=" mr-2" severity="success"  :label="'Export to Excel'"
                v-show="gridDataList.length > 0"   @click="exportData(ExportType.Excel)"  />
                <common-button class="btn p-button-aux mr-2" :label="'Export to PDF'"
                v-show="gridDataList.length > 0"   @click="exportData(ExportType.Pdf)"  />
            </div>
            <div class="right-part">
                
                
            </div>
        </div>
        <div class="custom-datatablewithoutwidth-wrapper table-responsive">
    <DataTable  ref="dt" :class="gridDataList?.length > 0 ? '' : ''" 
                :value="gridDataList" :scrollable="true" :paginator="false" :rows="10" 
                 showGridlines>
                 
                 
    <Column field="" header="" :sortable="false"> 
        <template #body="slotProps">
            <Button icon="pi pi-file-pdf" severity="danger" title="Generate voucher" @click="exportDataVoucher(slotProps.data)" style="height: 24px; font-size: 12px; padding: 2px 6px;" />


        <!-- <common-button class="btn p-button-aux mr-2" :label="'Export to PDF'"
        v-show="gridDataList.length > 0"   @click="exportDataVoucher(slotProps.data.Id)"  /> -->
    </template>
    </Column>
    <Column field="SrNo" header="Sr. No" class="text-center" :sortable="false"></Column> 
    <Column field="VoucherNo" header="Voucher No" class="text-center" :sortable="false"></Column> 
    <Column field="EnrollNo" header="Enroll No" class="text-center" :sortable="true">
        <template #footer> <strong>Total</strong> </template>
    </Column>
    <Column field="EmployeeName" header="Employee Name" :sortable="false"> 
        <template #footer> </template>
    </Column>
    <Column field="DivisionName" header="Division" :sortable="true">
        <template #footer> </template>
    </Column>
    <Column field="DepartmentName" header="Department" :sortable="true">
        <template #footer> </template>
    </Column>

    <Column field="DesignationName" header="Designation" :sortable="true">
        <template #footer> </template>
    </Column>

    <Column field="BasicSalary" class="text-center" header="Basic Salary" :sortable="true">
        <template #footer> </template>
    </Column>   

    <Column field="SalaryType" class="text-center" header="Salary Type" :sortable="true">
        <template #footer> </template>
    </Column>        

    <Column field="AttendDays" class="text-center" header="Attend Days" :sortable="true">
        
    </Column>        

    <Column field="TotalAmount" class="text-center" header="Total AMT" :sortable="true">
        <template #footer> {{ totalAmount }} </template>
    </Column>

    <Column field="TotalExtraAmount" class="text-center" header="Total Extra AMT" :sortable="true">
        <template #footer> {{ totalExtraAmount }} </template>
    </Column>

    <Column field="TotalExpenseAmount" header="Total Kharchi" :sortable="true" class="text-center">
    <template #body="slotProps">
        <td class="text-center" :class="getCellClass(slotProps.data)">
            {{ slotProps.data.TotalExpenseAmount || '&nbsp;' }}
        </td>
    </template>
    <template #footer> {{ totalExpenseAmount }} </template>
</Column>

    <Column field="PayableAmount" class="text-center" header="Payable AMT" :sortable="true">
        <template #footer> {{ totalPayableAmount }} </template>
    </Column> 

    <Column field="AdharcardNo" class="text-center" header="Adharcard No" :sortable="true">
        <template #footer> </template>
    </Column> 
                <template #empty>
                    <div class="no-data text-center">
                        <img src="@/assets/images/no-items.png" alt="No Data Found" />
                        <h4>No Record Found</h4>
                    </div>
                </template>
            </DataTable>
        </div>
    </div>
</template>
<script setup lang="ts">
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { inject, ref, computed } from "vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    import moment from "moment";
    const Page = AuthService.GetPagePermission('Attendance','Attendance')
    const { ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
    import requestMethod from "@/composables/api/requestMethod";
    import CommonHelper from "@/utils/commonHelper";
    import { ExportType } from "@/models/controls/Grid/gridRequest";
    const { PatchData,PostData ,ExportData,GetData} = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const dateFilter = ref(new Date());
    const enrollFilter = ref(null);
    const DivisionID = ref(null);
    const DepartmentID = ref(null);
    const DepartmentList = ref(null);
    const DesignationID = ref(null);
    const DesignationList = ref(null);
    const gridDataList = ref([]);
    const filterData = ref([]);
    
const totalAmount = computed(() => gridDataList.value.reduce((sum, item) => sum + (item.TotalAmount || 0), 0));
const totalExtraAmount = computed(() => gridDataList.value.reduce((sum, item) => sum + (item.TotalExtraAmount || 0), 0));
const totalExpenseAmount = computed(() => gridDataList.value.reduce((sum, item) => sum + (item.TotalExpenseAmount || 0), 0));
const totalPayableAmount = computed(() => gridDataList.value.reduce((sum, item) => sum + (item.PayableAmount || 0), 0));
    const filename = ref("MonthwiseRpt_"+(moment(Date.now()).utcOffset(0, true).format("YYYY_MMM_DD_HH_MM_SS")))
    const getDepartmentListData =async () => {
    GetData(UrlConstants.apidropdown+'?Mode=Department', (data: any) => {
        DepartmentList.value = data;
        
    },null);
    };
    getDepartmentListData();

    const getDesignationListData =async () => {
        DesignationList.value = [];
        var url = UrlConstants.apidropdown+'?Mode=Designation';
        if(DepartmentID.value != undefined || DepartmentID.value != null || DepartmentID.value > 0)
    {
        url = UrlConstants.apidropdown+'?Mode=Designation&&Id='+DepartmentID.value;

    }
    GetData(url, (data: any) => {
        DesignationList.value = data;
        
    },null);
    };
    getDesignationListData();



    const AddFilter = () => {
        
        if(dateFilter.value == null){
        dateFilter.value = new Date();}
    filterData.value = [];
        filterData.value.push({
  fieldName: "AttendanceDate",
  fieldValue: moment(dateFilter.value, "MM/DD/YYYY").utcOffset(0, true).format("YYYY MMM DD")
});
if(DesignationID.value != null){
filterData.value.push({
  fieldName: "DesignationID",
  fieldValue: DesignationID.value
});
}

if(DepartmentID.value != null){
filterData.value.push({
  fieldName: "DepartmentID",
  fieldValue: DepartmentID.value
});
}


if(enrollFilter.value != null && enrollFilter.value != ''){
filterData.value.push({
  fieldName: "EnrollNo",
  fieldValue: enrollFilter.value
});
}
};


    const getData =async (data: any) => {

        AddFilter();

        gridDataList.value = [];

        let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        Filters: filterData.value,        
        Columns: []
    }));
    PostData(UrlConstants.apiReportsDivisionWiseList, Postdata, "", (data: any) => {        
    if (data && data.length > 0) {
        // Calculate totals for required fields
        gridDataList.value =data;
        // Append "Total" row to the data
        
    } else {
        gridDataList.value = [];
    }
}, null);

    };
    getData();

    const AdvanceSearchModel = ref(false);
const OpenAdvanceSearchModel = () => {
    AdvanceSearchModel.value = true;
};
const CloseAdvanceSearchModel = () => {
    AdvanceSearchModel.value = false;
};
const ClearAll = () => {
    dateFilter.value = new Date();
    gridDataList.value = [];
};



const exportCSV = () => {

exportData(ExportType.Excel);
};
const exportData = (exportType: any) => {

if (dateFilter?.value != null) {
    AddFilter();

    let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        Filters: filterData.value,        
        ResponseType: exportType,
        Columns: []
    }));
    if ((dt?.value?.columns?.length ?? 0) > 0) {
        for (let i = 0; i < (dt?.value?.columns?.length ?? 0); i++) {
            if (dt?.value?.columns[i]?.props.header != undefined)
                Postdata.Columns.push({
                    name: dt.value?.columns[i]?.props.header,
                    data: dt.value?.columns[i]?.props.field,
                })
        }
    }
    ExportData(UrlConstants.apiReportsDivisionWiseList,Postdata, ("DivisionRpt_"+(moment(Date.now()).utcOffset(0, true).format("YY_MMM_DD_hhmmss A"))),"",(exportType == 1)?"xlsx":"pdf", (data: any) => {        
                       // gridDataList.value = data;
  });
}

};

const exportDataVoucher = (exportType: any) => {
var ids = [];
ids.push(exportType.Id);
if (dateFilter?.value != null) {
    let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        ids: ids
    }));
    
    ExportData(UrlConstants.apiGetVoucherList,Postdata, (exportType.EnrollNo+"_"+(moment(exportType.AttendanceDate).utcOffset(0, true).format("YY_MMM"))),"",(exportType == 1)?"xlsx":"pdf", (data: any) => {        
                       // gridDataList.value = data;
  });
}

};

const exportBulkDataVoucher = () => {
var ids = gridDataList.value.map(item => item?.Id)


if (dateFilter?.value != null) {
    let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        ids: ids
    }));
    
    ExportData(UrlConstants.apiGetVoucherList,Postdata, ((moment(dateFilter.value).utcOffset(0, true).format("MMM_YYYY"))),"","pdf", (data: any) => {        
                       // gridDataList.value = data;
  });
}

};

const customSort = (event: any) => {
    alert(1)
    gridDataList.value.sort((a: any, b: any) => {
        // Ensure "Total" row stays at the bottom
        if (a.EmployeeName === "Total") return 1;
        if (b.EmployeeName === "Total") return -1;

        // Default sorting logic
        const field = event.sortField;
        const order = event.sortOrder;

        const valueA = a[field] ?? "";
        const valueB = b[field] ?? "";

        if (typeof valueA === "number" && typeof valueB === "number") {
            return order * (valueA - valueB);
        } else {
            return order * valueA.toString().localeCompare(valueB.toString());
        }
    });
};

const getCellClass = (rowData) => {
    return rowData.rowClass;
};
</script>