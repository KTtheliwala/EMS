
<template >
    
        
    <common-form ref="frm" :dataId="0" :beforeSubmit="beforeSubmitData" :actionUrl="UrlConstants.apiExpense" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change p-1 bottom-seperator custom-bg">
            <div class="col-12 md:col-2">
                <div class="custom-label-container">
                    <label class="custom">Date:</label>                   
                  <common-date
                      id="icon"                          
                      v-model="obj.ExpenseDate"
                      :showIcon="true"
                      class="w-full"
                      placeholder="Select Month-Year"
                      dateFormat="d MM yy" 
                      name="ExpenseDate"    
                      :maxDate="new Date()"                  
                      :v$="v$"
                      @update:modelValue="onDateChange"
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
                             ref="dropdownRef"
                            :filterFields="['EnrollNo','EmployeeName','DesignationName','DepartmentName']" 
                            :virtualScrollerOptions="{ itemSize: 30 }" placeholder="Enroll No/Employee Name/Designation/Department" :showClear="true"
                            @change="onEmployeeSelect"
                            @show="focusSearch"/>
                            
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
                <label class="custom">Kharchi Amount:</label> 
                  
                  <common-input placeholder="Amount"                                  
                              IsNumber="true"
                              :v$="v$"
                              name="Amount"
                              :disabled="!employeeActive"
                              v-model="obj.Amount"></common-input>
                              
              </div>
          </div>
          <div class="col-12 md:col-2" v-if="(obj.EmployeeID > 0 && obj.ExpenseDate != null)">
            <div class="custom-label-container">
                <label class="custom">
                    
                    
<Button aria-label="Verified" severity="success" icon="pi pi-check"  v-if="employeeActive"/>
<Button aria-label="Danger" label="Activation Click Here" severity="danger" icon="pi pi-times" @click="employeeActivationConfirmation" v-else/>

                </label>                   
                
              </div>
          </div>
       
        <div class="col-12 md:col-1 text-right">                                
            <Button :label="(obj.Id>0?'Update':'Add')" class="" icon="pi pi-plus-circle" v-if="(Page?.IsAdd ?? false )" type="submit"  autofocus/>

        </div>
        <div class="col-12 md:col-1 text-left">   
                <Button :label="'Cancel'" class="p-button-aux" icon="pi pi-times-circle" type="button" autofocus @click="cancelEdit" />            
            
        </div>

        <div class="col-12 md:col-12">
            <div class="common__Table">
                <div class="custom-datatablewithoutwidth-wrapper table-responsive">
                
            
    <table class="expense-table" v-if="existingExpenseList?.length > 0">
        <thead>
            <tr>
                <th>Date</th>
                <th>Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="expense in existingExpenseList.filter(x=>x.Id !== obj.Id)" :key="expense.Id">
                <td>{{ ConvertToUserTimeZone(expense.ExpenseDate, "DD-MMM-YYYY") }}</td>
                <td>₹{{ expense.Amount.toFixed(2) }}</td>
            </tr>
        </tbody>
    </table>
</div>    
</div>
</div>
        </div>
</common-form>
<Dialog header="Activation Confirmation" v-model:visible="RemoveConfirmation" :modal="true" @hide="CancelEvent">
        <div class="msgPopup">
            <p class="m-0 mb-3" style="font-size: 16px;">Do you want Active ?</p>
            
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="No" severity="danger"  @click="CancelEvent" class="mr-2" />
                <Button position='bottom-right' label="Yes" severity="success" @click="employeeActivation" class="p-button-primary" autofocus />
            </div>
        </div>
    </Dialog>
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
                <label class="block w-full">EnrollNo</label>
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
                <label class="block w-full">EnrollNo</label>
                <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
            </div>
        </div>
       
    </template>
    <template v-slot:right-buttons>
        
    </template>
    <template v-slot:columns>
        <!-- <Column selectionMode="" class="check-box-center" frozen></Column> -->
        <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
            <template #body="slotProps">
                <div>
                    <!-- <input type="hidden" :id="'HideChkBox_' + slotProps.data.Id" class="hide-checkbox" :value="!isCurrentMonth(slotProps.data.ExpenseDate)" /> -->
                    <Badge v-tooltip="'Record Lock'" size="small" class="custom-badge" severity="danger" v-if="!isCurrentMonth(slotProps.data.ExpenseDate)"><i class="pi pi-lock" style="font-size: 0.9rem" /></Badge>
                    <common-action-button :menuItems="gridMenuItems" :data="slotProps.data" v-else></common-action-button>&nbsp;
                </div>
            </template>
        </Column>

        <Column field="ExpenseNo" header="Expense No" :sortable="true"></Column> 
        <Column field="EnrollNo" header="EnrollNo" :sortable="true"></Column> 
        <Column field="EmployeeName" header="Employee" :sortable="false"></Column>         
        <Column field="Amount" header="Amount" :sortable="true"></Column>
        <Column field="ExpenseDate" header="Expense Date" :sortable="true">        
            <template #body="slotProps">
                {{ ConvertToUserTimeZone(slotProps.data.ExpenseDate, "DD-MMM-YYYY") }}
            </template>
        </Column> 
        <Column field="CreatedDate" header="Entry Date" :sortable="true">        
            <template #body="slotProps">
                {{ ConvertToUserTimeZone(slotProps.data.CreatedDate, "DD-MMM-YYYY hh:mm A",false,330) }}
            </template>
        </Column>
        
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

import Dropdown from 'primevue/dropdown';
import { inject, onMounted, ref, nextTick } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import expenseModelDTO, { employeeActivationCheck,  expenseModel} from "@/models/expense/expenseModel";
import attendanceModelDTO, { attendanceModel } from "@/models/attendance/attendanceModel";
import commonModule from "@/composables/modules/commonModule";

const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
import { AuthService } from "@/composables/api/authService";
const Page = AuthService.GetPagePermission('Attendance','Activation')
const { PatchData,PostData,GetData } = new commonModule();
import moment from "moment";
const dt = ref(null);
const popupManage = ref();
const employeeActive = ref(false);
const RemoveConfirmation = ref(false);
const popupManageSmall = ref();
const obj = ref<expenseModel>(new expenseModel());
const objActivationModel = ref<attendanceModel>(new attendanceModel());
const objEmpVerifyModel = ref<employeeActivationCheck>(new employeeActivationCheck());
const v$ = ref();
const frm = ref();
const employeeList = ref([]);
const existingExpenseList = ref([]);
const ddlItem = ref();
import $ from "jquery";

onMounted(() => {  
    searchEmployee();    
    obj.value.ExpenseDate = new Date();
    frm.value.SetValidation((data: expenseModel) => {
        obj.value = data;
        v$.value = useVuelidate(expenseModelDTO.rules, obj.value);
        
    });
});
const gridConfig = ref({
    api: UrlConstants.apiExpenseList,
    deleteApi: UrlConstants.apiExpense,
    filters: [
        { searchType: "Filter", fieldName: "ExpenseDate", fieldValue: null, fieldDisplayName: "Month-Year", opType: SearchOperations.dateRange },
        { searchType: "Filter", fieldName: "OrderStatus", fieldDisplayName: "Status", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
         { searchType: "Filter", fieldName: "EnrollNo", fieldValue: null, fieldDisplayName: "EnrollNo", opType: SearchOperations.contains },
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


const editDetail = async (data:any) => {
    clearSearchEmployee();
    await GetData(UrlConstants.apiExpense +data.Id, (data: any) => {                        
        obj.value   =data;
        employeeActive.value =true;     
        v$.value = useVuelidate(expenseModelDTO.rules, obj.value);
        setTimeout(() => {
            onEmployeeSelectEdittime();    
        }, 100);
        
        
                    },null);
}
const successCallback = (data: any) => {
    dt.value.reloadGrid();
    RemoveConfirmation.value = false;
    employeeActive.value=false;
    obj.value =new expenseModel();
    obj.value.ExpenseDate = new Date();
    frm.value.SetValidation((data: expenseModel) => {
        v$.value = useVuelidate(expenseModelDTO.rules, obj.value);
    });
};
const failureCallBack = (data: any) => {
    //
};
const searchEmployee = async() => {    
    if(obj.value.EnrollNo == ""){ obj.value.EnrollNo = null}
    PostData(UrlConstants.apiAttendanceActivationSearch,obj.value, "", (data: any) => {        
        employeeList.value = data;
  },null);
}
  const clearSearchEmployee = async() => {
obj.value =new expenseModel();
searchEmployee();
    }
  const cancelEdit = async() => {
    RemoveConfirmation.value = false;
    obj.value =new expenseModel();
    obj.value.ExpenseDate = new Date();

    }
    const beforeSubmitData = (data: any) => {    
        if ( obj.value.EmployeeID != null && obj.value.EmployeeID != undefined) {
        var findEmpEnrol = employeeList.value.filter(x=>x.EmployeeID === obj.value.EmployeeID );
        obj.value.EnrollNo = findEmpEnrol[0].EnrollNo;

        }
        if (obj.value.ExpenseDate) {
    obj.value.ExpenseDate = moment(obj.value.ExpenseDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD");
}
};

const onEmployeeSelect = async (data: any) => {
    
    
    if (!data || obj.value.EmployeeID == null) {
        existingExpenseList.value = [];
        employeeActive.value = false;
        return;
    }

    if (obj.value.ExpenseDate) {
        objEmpVerifyModel.value.ExpenseDate = moment(obj.value.ExpenseDate, "MM/DD/YYYY")
            .utcOffset(0, true)
            .format("YYYY-MM-DD");
    }

    if(objEmpVerifyModel.value.EmployeeID !== obj.value.EmployeeID)
    {employeeActive.value = false;}
    objEmpVerifyModel.value.EmployeeID = obj.value.EmployeeID;

    if (
        objEmpVerifyModel.value.ExpenseDate &&
        objEmpVerifyModel.value.EmployeeID &&
        !employeeActive.value
    ) {
        existingExpenseList.value = [];
        employeeActive.value = false;
        await PostData(
            UrlConstants.apiEmployeeCheckActivation,
            objEmpVerifyModel.value,
            "",
            (data: any) => {
                if (data.Status) {
                    employeeActive.value = true;
                }
if(data?.ExistingExpense != undefined && data?.ExistingExpense != null && data?.ExistingExpense.length > 0){
                existingExpenseList.value = data.ExistingExpense
}
            },
            null
        );
    }
};



const onEmployeeSelectEdittime = async (data: any) => {
    
    

    if (obj.value.ExpenseDate) {
        objEmpVerifyModel.value.ExpenseDate = moment(obj.value.ExpenseDate, "MM/DD/YYYY")
            .utcOffset(0, true)
            .format("YYYY-MM-DD");
    }

    if(objEmpVerifyModel.value.EmployeeID !== obj.value.EmployeeID)
    {employeeActive.value = false;}
    objEmpVerifyModel.value.EmployeeID = obj.value.EmployeeID;
    if (
        objEmpVerifyModel.value.ExpenseDate &&
        objEmpVerifyModel.value.EmployeeID 
        
    ) {
    
        existingExpenseList.value = [];
        employeeActive.value = false;
        await PostData(
            UrlConstants.apiEmployeeCheckActivation,
            objEmpVerifyModel.value,
            "",
            (data: any) => {
                if (data.Status) {
                    employeeActive.value = true;
                }
if(data?.ExistingExpense != undefined && data?.ExistingExpense != null && data?.ExistingExpense.length > 0){
                existingExpenseList.value = data.ExistingExpense
}
            },
            null
        );
        }
};

//const dropdownRef = ref(null);
const onDateChange = async () => {
    // if (RemoveConfirmation.value) {
    //     console.log("Popup is still open, skipping onEmployeeSelect");
    //     return;
    // }
    await onEmployeeSelect();
    setTimeout(() => {
        onEmployeeSelectEdittime();    
        }, 200);
};
const employeeActivation = async() => {
  objActivationModel.value.EmployeeID = obj.value.EmployeeID;
  if (obj.value.ExpenseDate) {
    objActivationModel.value.AttendanceDate = moment(obj.value.ExpenseDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD");
  }
  objActivationModel.value.AttendDays =null;
  objActivationModel.value.IsActive =true;

  await PostData(UrlConstants.apiAttendanceActivation, objActivationModel.value, "", (data: any) => {    

if(data?.Id > 0)           
{
    RemoveConfirmation.value = false;
    employeeActive.value = true;
}

            },null);
        
};
const CancelEvent = () => {
    RemoveConfirmation.value = false;
    // Remove these lines to prevent resetting obj.value after popup closes
    // obj.value = new expenseModel();
    // obj.value.ExpenseDate = new Date();
};

const employeeActivationConfirmation = () => {
        
        RemoveConfirmation.value = true;
        
    }

  


const dropdownRef = ref<InstanceType<typeof Dropdown> | null>(null);
const focusSearch = async () => {
    setTimeout(() => {
    $(".p-dropdown-filter").focus();
    
  }, 100);
};

setTimeout(() => {
  
      const checkboxes = $('input[type="checkbox"].p-checkbox-input');
      console.log('Checkbox count:', checkboxes.length);
    
  }, 500);
const filename = ref("Kharchi_list_"+Date.now())

function isCurrentMonth(dateStr: string): boolean {
    
  const expenseDate = new Date(dateStr);
  const today = new Date();

  return (
    expenseDate.getFullYear() === today.getFullYear() &&
    expenseDate.getMonth() === today.getMonth()
  );
}
</script>