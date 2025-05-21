
<template >
    
        <div class="form-custom">
    <div class="grid guter-change p-1 bottom-seperator custom-bg pb-0">
          <div class="col-12 md:col-2">
              <div class="custom-label-container">                  
                <label class="custom">Division:</label> 
                <common-dropdown placeholder="Search Division" 
                  :v$="v$" name="DivisionID" v-model="obj.DivisionID" 
                  :isWatch="false" 
                  :dataurl="UrlConstants.apidropdown+'?Mode=Division'" />
              </div>
          </div>
          <div class="col-12 md:col-2">
            <div class="custom-label-container">
                <label class="custom">Department:</label> 
                  <common-dropdown placeholder="Department" 
                  :v$="v$" name="DepartmentID" v-model="obj.DepartmentID" 
                  :isWatch="false" 
                  :dataurl="UrlConstants.apidropdown+'?Mode=Department'" />
              </div>
          </div>
          
        <div class="col-12 md:col-2">
            <div class="custom-label-container">
                <label class="custom">Designation:</label> 
                  <common-dropdown placeholder="Designation" 
                  :v$="v$" name="DesignationID" v-model="obj.DesignationID" 
                  :isWatch="false" 
                  :dataurl="UrlConstants.apidropdown+'?Mode=Designation'" />
              </div>
          </div>              
          <div class="col-12 md:col-2">                
            <div class="custom-label-container">
                <label class="custom">Name:</label> 
                <common-input placeholder="Name"
                              name="Name"    
                              :v$="v$"
                              v-model="obj.Name"></common-input>
            </div>
        </div>
        <div class="col-12 md:col-2">                
            <div class="custom-label-container">
                <label class="custom">EnrollNo:</label> 
                <common-input placeholder="EnrollNo"
                              name="EnrollNo"    
                              :v$="v$"
                              v-model="obj.EnrollNo"></common-input>
            </div>
        </div>
        <div class="col-12 md:col-2 pb-1 text-right">                
            <Button :label="'Clear'" class="p-button-aux" icon="pi pi-times-circle" type="button" autofocus @click="clearSearchEmployee"/>
            <Button :label="'Find'" class="ml-2" icon="pi pi-plus-circle" type="button" autofocus @click="searchEmployee"/>
        </div>
    
        
        
    </div>     
    <div>
    <common-form ref="frm" :dataId="0" :beforeSubmit="beforeSubmitData" :actionUrl="UrlConstants.apiAttendanceActivation" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change p-1 bottom-seperator custom-bg">
            <div class="col-12 md:col-2">
                <div class="custom-label-container">
                    <label class="custom">Month:</label> 
                  <!-- <label class="block w-full">Attendance Month</label> -->
                  <common-date
                      id="icon"                          
                      v-model="obj.AttendanceDate"
                      :showIcon="true"
                      class="w-full"
                      placeholder="Select Month-Year"
                      :view="'month'" dateFormat="MM yy" 
                      name="AttendanceDate"
                      :v$="v$"
                  ></common-date>
              </div>
          </div> 
            <div class="col-12 md:col-4">
                  <div class="grid guter-change">
                <div class="col-12 md:col-2 text-right p-0 pt-2"><label class="custom">Employee:</label> </div>
                <div class="col-12 md:col-10 text-left">
               
                   <div  :class="{ error: v$ && v$.value && v$.value['EmployeeID'] && v$.value['EmployeeID'].$errors.length }">
                   
                        <Dropdown  name="EmployeeID" class="w-full" v-model="obj.EmployeeID" :options="employeeList"
                             optionLabel="DropdownConcateName" optionValue="EmployeeID" :filter="true" :showToggleAll="false"
                            :filterFields="['EnrollNo','EmployeeName','DesignationName','DepartmentName']"
                            @show="focusSearch" 
                            :virtualScrollerOptions="{ itemSize: 30 }" placeholder="Enroll No/Employee Name/Designation/Department" :showClear="true"/>
                    </div>
                    <div v-if="v$ && v$.value && v$.value['EmployeeID']">
                        <div class="input-errors" v-for="error of v$.value['EmployeeID'].$errors" :key="error.$uid">
                            <div class="error-msg">{{ error.$message }}</div>
                        </div>
                    </div>     
              </div>
            </div>
          </div>    
          <div class="col-12 md:col-2">
            <div class="custom-label-container">
                <label class="custom">AttendDays:</label> 
                  <!-- <label class="block w-full">AttendDays</label> -->
                  <common-input placeholder="AttendDays"                                  
                              IsNumber="true"
                              :v$="v$"
                              name="AttendDays"
                              v-model="obj.AttendDays"></common-input>
              </div>
          </div>
        <!-- <div class="col-12 md:col-1 pb-0">
            <div class="field">
                <label class="block w-full">Active</label>
                <InputSwitch v-model="obj.IsActive" />
            </div>
        </div> -->
        <div class="col-12 md:col-1 text-right">                                
            <Button :label="'Cancel'" class="p-button-aux" icon="pi pi-times-circle" type="button" autofocus @click="cancelEdit" v-if="obj.Id>0"/>            
        </div>
        <div class="col-12 md:col-1 text-left">   
            
            
            <Button :label="(obj.Id>0?'Update':'Create')" class="ml-2" icon="pi pi-plus-circle" v-if="(Page?.IsAdd ?? false )" type="submit"  autofocus/>
            
        </div>
        </div>
</common-form>
</div> 
</div>
<div class="custom-class">
<common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile mt-2'">
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
                <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
            </div>
        </div>
    </template>
    <template v-slot:advancefilter>
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
                <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
            </div>
        </div>
       
    </template>
    <template v-slot:right-buttons>
        
    </template>
    <template v-slot:columns>
        <Column selectionMode="multiple" class="check-box-center" frozen ></Column>
        <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
            <template #body="slotProps">
                <div>
                    <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>&nbsp;
                </div>
            </template>
        </Column>

        <Column field="EmployeeName" header="Employee Name" :sortable="false"></Column> 
        <Column field="EnrollNo" header="EnrollNo" :sortable="true"></Column> 
        <Column field="AttendanceDate" header="Month-Year" :sortable="true">
            <template #body="slotProps">
                {{ ConvertToUserTimeZone(slotProps.data.AttendanceDate, "MMM-YYYY") }}
            </template>
        </Column> 
        <!-- <Column field="AttendDays" header="AttendDays" :sortable="true"></Column>   -->
        <!-- <Column header="Active" :sortable="true" field="IsActive" class="active-col">
            <template #body="columns">
                <InputSwitch  v-model="columns.data.IsActive" @change="updateIsActive(columns.data.Id)" />
            </template>
        </Column> -->
                   
    </template>
    
</common-grid>
</div>
<common-dialog ref="popupManage" class="large-modal"></common-dialog>
<common-dialog ref="popupManageSmall" class="small-modal"></common-dialog>
</template>
<script setup lang="ts">
import { useRoute } from 'vue-router';
import router from '@/router';
import GridConfig from "@/models/controls/Grid/gridConfig";
import SearchOperations from "@/models/controls/Grid/searchOperations";

import { inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import attendanceModelDTO, { attendanceModel } from "@/models/attendance/attendanceModel";
import commonModule from "@/composables/modules/commonModule";
import attendanceManage from "@/views/frontEnd/attendance/attendance-manage.vue";
const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
import { AuthService } from "@/composables/api/authService";
const Page = AuthService.GetPagePermission('Attendance','Activation')
const { PatchData,PostData,GetData } = new commonModule();
import moment from "moment";
const dt = ref(null);
const popupManage = ref();
const popupManageSmall = ref();
const obj = ref<attendanceModel>(new attendanceModel());
const v$ = ref();
const frm = ref();
const employeeList = ref([]);
const ddlItem = ref();

onMounted(() => {  
    searchEmployee();      
    obj.value.AttendanceDate = new Date();
    frm.value.SetValidation((data: attendanceModel) => {
        obj.value = data;
        v$.value = useVuelidate(attendanceModelDTO.rules, obj.value);
    });
});
const gridConfig = ref({
    api: UrlConstants.apiAttendanceActivationList,
    deleteApi: UrlConstants.apiAttendanceActivation,
    filters: [
        { searchType: "Filter", fieldName: "AttendanceDate", fieldValue: null, fieldDisplayName: "Month-Year", opType: SearchOperations.dateRange },
        { searchType: "Filter", fieldName: "OrderStatus", fieldDisplayName: "Status", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
        { searchType: "Filter", fieldName: "EnrollNo", fieldValue: null, fieldDisplayName: "First Name", opType: SearchOperations.contains },
       // { searchType: "Filter", fieldName: "LastName", fieldValue: null, fieldDisplayName: "Last Name", opType: SearchOperations.contains },
    ],
    sortcolumn: "Id",
    sortorder: -1,
    doubleClickHander: null,
} as GridConfig);
gridConfig.value.filters[0].fieldValue = [new Date(),new Date()];
const updateIsActive = (data: any) => {
    PatchData(UrlConstants.apiAttendanceActivationUpdateStatus, data, "Activation Status Updated successfully.", null, null);
};
const OpenAddNewManage = () => {
    popupManage.value.OpenModal(attendanceManage, "Add Activation", 0, () => {
        dt.value.reloadGrid();
    });
};
const gridMenuItems = ref([
{
            icon: "pi pi-pencil",
            label: "Edit",
            callback: (data: any) => {
                editDetail(data)
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

const updateStatus = async (data:any) => {
    popupManageSmall.value.OpenModal(orderStatusManage, "Update Order Status (#"+data.OrderNo+")", data, dt.value.reloadGrid);
}

const editDetail = async (data:any) => {
    clearSearchEmployee();
    await GetData(UrlConstants.apiAttendanceActivation +data.Id, (data: any) => {                        
        obj.value   =data;     
        v$.value = useVuelidate(attendanceModelDTO.rules, obj.value);
                    },null);
}
const successCallback = (data: any) => {
    dt.value.reloadGrid();
    obj.value =new attendanceModel();
    obj.value.AttendanceDate = new Date();
};
const failureCallBack = (data: any) => {
    //
};
const searchEmployee = async() => {

    if(obj.value.EnrollNo == ""){ obj.value.EnrollNo = null}
    PostData( UrlConstants.apiAttendanceActivationSearch,obj.value, "", (data: any) => {
        
        employeeList.value = data;
  },null);
}
  const clearSearchEmployee = async() => {
obj.value =new attendanceModel();
searchEmployee();
    }
  const cancelEdit = async() => {
obj.value =new attendanceModel();

    }
    const beforeSubmitData = (data: any) => {    
        obj.value.IsActive = true;
        if (obj.value.AttendanceDate) {
    obj.value.AttendanceDate = moment(obj.value.AttendanceDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM");
}
};
import $ from "jquery";
const dropdownRef = ref<InstanceType<typeof Dropdown> | null>(null);
const focusSearch = async () => {
    setTimeout(() => {
    $(".p-dropdown-filter").focus();
  }, 100);
};
const filename = ref("Activation_list_"+Date.now())
</script>