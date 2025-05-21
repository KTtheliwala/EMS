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
                        <div class="col-12 md:col-3 pb-0">
                            <div class="field">
                                <label class="block w-full">Enroll No</label>
                                <common-input inputclass="form-control" v-model="enrollFilter"></common-input>
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
            </div>
            <div class="right-part">
                <common-button class="btn p-button-aux mr-2" :label="'Export to Excel'"
                v-show="gridDataList.length > 0"   @click="exportData(ExportType.Excel)"  />
                <common-button class="btn p-button-aux mr-2" :label="'Export to PDF'"
                v-show="gridDataList.length > 0"   @click="exportData(ExportType.Pdf)"  />
            </div>
        </div>
        <div class="custom-datatablewithoutwidth-wrapper table-responsive">
    <DataTable  ref="dt" :class="gridDataList?.length > 0 ? '' : ''" 
                :value="gridDataList" :scrollable="true" :paginator="false" :rows="10"
                 showGridlines>
                 <Column field="SrNo" class="text-center" header="Sr. No" :sortable="false"></Column> 
                 <Column field="EnrollNo" header="Enroll No" :sortable="true">
                    <template #footer> <strong>Total</strong> </template>
                </Column> 
                
                 <Column field="EmployeeName" header="Employee Name" :sortable="false"></Column> 
        <Column field="DepartmentName" header="Department" :sortable="true"></Column> 
        <Column field="DesignationName" header="Designation" :sortable="true"></Column> 
        <Column field="BasicSalary" class="text-center" header="Basic Salary" :sortable="true"></Column>   
        <Column field="SalaryType" class="text-center" header="Salary Type" :sortable="true"></Column>        
        <Column field="Amount" header="Amount" :sortable="true">
            <template #footer> {{ totalAmount }} </template>
        </Column>
        <Column field="ExpenseDate" header="Expense Date" :sortable="true">        
            <template #body="slotProps">
                <div v-if="slotProps.data.EmployeeName !== 'Total'">
                {{ ConvertToUserTimeZone(slotProps.data.ExpenseDate, "DD-MMM-YYYY") }}</div>
            </template>
        </Column> 
        <Column field="CreatedDate" header="Entry Date" :sortable="true">        
            <template #body="slotProps">
                <div v-if="slotProps.data.EmployeeName !== 'Total'">
                {{ ConvertToUserTimeZone(slotProps.data.CreatedDate, "DD-MMM-YYYY hh:mm A",false,330) }}</div>
            </template>
        </Column>
        <Column field="AdharcardNo" class="text-center" header="Adharcard No" :sortable="true"></Column> 
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
    const { PatchData,PostData ,ExportData} = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const dateFilter = ref(new Date());
    const enrollFilter = ref(null);
    const gridDataList = ref([]);
    
   
    const filename = ref("KharchiRpt_"+(moment(Date.now()).utcOffset(0, true).format("YYYY_MMM_DD_HH_MM_SS")))

    const totalAmount = computed(() => gridDataList.value.reduce((sum, item) => sum + (item.Amount || 0), 0));
    const getData =async (data: any) => {
        if(dateFilter.value == null){
            dateFilter.value = new Date();}
        gridDataList.value = [];
        // await GetData(UrlConstants.apiReportsList+'?date='+(moment(dateFilter.value, "MM/DD/YYYY").utcOffset(0, true).format("YYYY MMM DD")), (data: any) => {                        
        //     gridDataList.value = data;
        //             },null);

        let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        DateFilter: (moment(dateFilter.value, "MM/DD/YYYY").utcOffset(0, true).format("YYYY MMM DD")),        
        EnrollNo: enrollFilter.value,        
        Columns: []
    }));
                    PostData(UrlConstants.apiExpenseReportsList,Postdata, "", (data: any) => {        
                        gridDataList.value = data;
  },null);

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
    enrollFilter.value = null;

};

const exportCSV = () => {

exportData(ExportType.Excel);
};
const exportData = (exportType: any) => {

if (dateFilter?.value != null) {

    let Postdata = {} as any
    Postdata = JSON.parse(JSON.stringify({
        DateFilter: (moment(dateFilter.value, "MM/DD/YYYY").utcOffset(0, true).format("YYYY MMM DD")),
        EnrollNo: enrollFilter.value,        
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
    
    ExportData(UrlConstants.apiExpenseReportsList,Postdata, ("KharchiRpt_"+(moment(Date.now()).utcOffset(0, true).format("YY_MMM_DD_hhmmss A"))),"",(exportType == 1)?"xlsx":"pdf", (data: any) => {        
                       // gridDataList.value = data;
  });
}

};
</script>