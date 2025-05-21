<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
        <div class="col-12 md:col-3 pb-0">
            <div class="field">
                <label class="block w-full">Month-Year</label>      
                <Calendar v-model="gridConfig.filters[0].fieldValue" :view="'month'" dateFormat="MM yy" :showButtonBar="true"
              selectionMode="range" :selectOtherMonths="true" :showIcon="true" class="w-full" inputId="dateRange2"></Calendar>
            </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
            <div class="field">
                <label class="block w-full">Enroll No</label>
                <common-input inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
            </div>
        </div>
    </template>
    <template v-slot:advancefilter>
        <div class="col-12 md:col-3 pb-0">
            <div class="field">
                <label class="block w-full">Month-Year</label>      
                <Calendar v-model="gridConfig.filters[0].fieldValue" :view="'month'" dateFormat="MM yy" :showButtonBar="true"
              selectionMode="range" :selectOtherMonths="true" :showIcon="true" class="w-full" inputId="dateRange2"
              
              @show="focusSearch"></Calendar>
            </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
            <div class="field">
                <label class="block w-full">Enroll No</label>
                <common-input inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
            </div>
        </div>
       
    </template>
        <template v-slot:right-buttons>
            <Button icon="pi pi-plus-circle" label="Add Attendance" v-if="(Page?.IsAdd ?? false )" @click="OpenAddNewManage"></Button>
        </template>
        <template v-slot:columns>
            <!-- <Column selectionMode="multiple" class="check-box-center" frozen ></Column> -->
        <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
            <template #body="slotProps">
                <div>
                    <Badge v-tooltip="'Record Lock'" size="small" class="custom-badge" severity="danger" v-if="!isCurrentMonth(slotProps.data.AttendanceDate)"><i class="pi pi-lock" style="font-size: 0.9rem" /></Badge>
                    
                    <common-action-button :menuItems="gridMenuItems" :data="slotProps.data" v-else></common-action-button>&nbsp;
                </div>
            </template>
        </Column>

        <Column field="AttendanceDate" header="Month-Year" :sortable="true">
            <template #body="slotProps">
                {{ ConvertToUserTimeZone(slotProps.data.AttendanceDate, "MMM-YYYY") }}
            </template>
        </Column> 
        <Column field="EnrollNo" header="EnrollNo" :sortable="true"></Column> 
        <Column field="EmployeeName" header="Employee Name" :sortable="false"></Column> 
        <Column field="DivisionName" header="Division" :sortable="false"></Column> 
        <Column field="DepartmentName" header="Department" :sortable="false"></Column> 
        <Column field="DesignationName" header="Designation" :sortable="false"></Column> 
        <Column field="SalaryType" class="text-center" header="Salary Type" :sortable="false"></Column>
        <Column field="AttendDays" class="text-center" header="Attend Days" :sortable="true"></Column>
        <Column field="BasicSalary" class="text-center" header="Basic Salary" :sortable="true"></Column>   
        <Column field="TotalAmount" class="text-center" header="Total AMT" :sortable="true"></Column>
        <Column field="TotalExpenseAmount" header="Total Kharchi" :sortable="true" class="text-center">
            <template #body="slotProps">
                <td :class="getCellClass(slotProps.data)">
                    {{ slotProps.data.TotalExpenseAmount || '&nbsp;' }}
                </td>
            </template>            
        </Column>
        <Column field="TotalExtraAmount" class="text-center" header="Total Extra AMT" :sortable="true"></Column> 
        <Column field="PayableAmount" class="text-center" header="Payable AMT" :sortable="true"></Column>
        <Column field="Remarks" class="text-center" header="Remarks" :sortable="true"></Column> 
         
        <Column field="CreatedDate" header="Entry Date" :sortable="true">        
            <template #body="slotProps">
                {{ ConvertToUserTimeZone(slotProps.data.CreatedDate, "DD-MMM-YYYY hh:mm A",false,330) }}
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
    import userManage from "@/views/frontEnd/attendance/attendance-manage.vue";
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    const Page = AuthService.GetPagePermission('Attendance','Attendance')
    const { ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(userManage, "Add Attendance", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiuserupdatestatus, data, "User Status Updated successfully.", null, null);
    };
    
function isCurrentMonth(dateStr: string): boolean {
    
  const expenseDate = new Date(dateStr);
  const today = new Date();

  return (
    expenseDate.getFullYear() === today.getFullYear() &&
    expenseDate.getMonth() === today.getMonth()
  );
}
    const gridConfig = ref({
        api: UrlConstants.apiEmployeeAttendanceList,
        deleteApi: UrlConstants.apiEmployeeAttendance,
        filters: [
        { searchType: "Filter", fieldName: "AttendanceDate", fieldValue: null, fieldDisplayName: "Month-Year", opType: SearchOperations.dateRange },
        { searchType: "Filter", fieldName: "EnrollNo", fieldValue: null, fieldDisplayName: "First Name", opType: SearchOperations.contains },
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
              popupManage.value.OpenModal(userManage, "Edit Attendance", data.Id, dt.value.reloadGrid);
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
    import $ from "jquery";
const dropdownRef = ref<InstanceType<typeof Dropdown> | null>(null);
const focusSearch = async () => {
    setTimeout(() => {
    $(".p-dropdown-filter").focus();
  }, 100);
};
    const filename = ref("Attendance_list_"+Date.now())

    
const getCellClass = (rowData) => {
    return rowData.rowClass;
};
</script>