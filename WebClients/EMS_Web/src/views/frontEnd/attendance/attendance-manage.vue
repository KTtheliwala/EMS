<template>
    <common-form ref="frm" :beforeSubmit="beforeSubmitData"  :dataId="Id" :actionUrl="UrlConstants.apiEmployeeAttendance" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change">
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Month-Year</label>
                      <common-date
                      id="icon"                          
                      v-model="obj.AttendanceDate"
                      :showIcon="true"
                      class="w-full"
                      placeholder="Select Month-Year"
                      :view="'month'" dateFormat="MM yy" 
                      name="AttendanceDate"
                      :v$="v$"
                      :disabled="obj.Id > 0"
                      @update:modelValue="onDateChange"
                  ></common-date>
                  </div>
              </div>
              <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Employee  <i title ="Fetch employee list" style="cursor: pointer;background: #4f46e5;color: #fff;border-color: #4f46e5;border-radius: var(--radius-4) !important;padding: 2px 5px;" class="pi pi-refresh" @click="searchEmployee"></i></label>
                      <div  :class="{ error: v$ && v$.value && v$.value['EmployeeID'] && v$.value['EmployeeID'].$errors.length }">
                   
                        <Dropdown  name="EmployeeID" class="w-full" v-model="obj.EmployeeID" :options="employeeList"
                             optionLabel="DropdownConcateName" optionValue="EmployeeID" :filter="true" :showToggleAll="false"
                             ref="dropdownRef"
                      :disabled="obj.Id > 0"
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
              
            <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Attend Days</label>
                      <common-input placeholder="Attend Days"                                  
                              IsNumber="true"
                              :v$="v$"
                              name="AttendDays"
                              v-model="obj.AttendDays"></common-input>                             
    
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Total Kharchi</label>
                      <common-input placeholder="Total Kharchi"                                  
                              IsNumber="true"
                              
                              name="TotalExpenseAmount"
                              v-model="obj.TotalExpenseAmount"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-1">
                  <div class="field">
                      <label class="block w-full">Total AMT</label>
                      <common-input placeholder="Total AMT"                                  
                              IsNumber="true"
                              :v$="v$"
                              name="TotalAmount"
                              v-model="obj.TotalAmount"></common-input>
                  </div>
              </div>              
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Extra Payable</label>
                      <common-input placeholder="Total Extra AMT"                                  
                              IsNumber="true"
                              name="TotalExtraAmount"
                              v-model="obj.TotalExtraAmount"></common-input>
                  </div>
              </div>        
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Salary Type <Checkbox v-model="salaryTypeEnable" binary /></label>
                      <common-dropdown placeholder="Salary Type" 
                  :v$="v$" name="SalaryType" v-model="obj.SalaryType" :options="SalaryTypeDropOption"
                  :disabled="!salaryTypeEnable" :showClear="false"
                  :callBack="onHHT_MCOlOptionChange"
                   />
                  
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Division Name</label>
                      <common-input placeholder="Division Name"
                              
                              name="DivisionName"
                              :disabled="true"
                              v-model="obj.DivisionName"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Department Name</label>
                      <common-input placeholder="Department Name"                              
                              name="DepartmentName"
                              :disabled="true"
                              v-model="obj.DepartmentName"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Designation Name</label>
                      <common-input placeholder="Designation Name"                              
                              name="DesignationName"
                              :disabled="true"
                              v-model="obj.DesignationName"></common-input>
                  </div>
              </div> 
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Basic Salary</label>
                      <common-input placeholder="Basic Salary"                                  
                              IsNumber="true"                              
                              name="BasicSalary"
                              :disabled="!(obj.Id > 0)"
                              v-model="obj.BasicSalary"></common-input>
                  </div>
              </div>     
                <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Payable AMT</label>
                      <common-input placeholder="Payable AMT"                                  
                              IsNumber="true"
                              :v$="v$"
                              name="PayableAmount"
                              v-model="obj.PayableAmount"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Remarks</label>
                      <common-area placeholder="Remarks"                              
                              name="Remarks"
                              v-model="obj.Remarks"></common-area>
                  </div>
              </div>
              <div class="mb-2" v-if="obj.EmployeeID > 0">
              <Tag severity="danger" value="Please update first employee 'Salary Type'" v-if="obj.EmployeeID == null || obj.SalaryType == undefined || obj.SalaryType == null || obj.SalaryType == ''"></Tag>
              <Tag severity="danger" value="Please update first employee 'Basic Salary'" v-if="obj.EmployeeID == null || obj.BasicSalary == null || obj.BasicSalary <= 0"></Tag>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Recalculate" severity="danger" icon="pi pi-refresh" @click="recalculate(obj.EmployeeID)" v-if="obj.rowClass === 'alert-mismatch'" />
                <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" type="submit" autofocus />
            </div>
        </div>
    </common-form>
</template>
<script setup lang="ts">
    import { inject, onMounted, ref, watch } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import UrlConstants from "@/utils/urlconstants";
    import employeeAttendanceModelDTO, { employeeAttendanceModel } from "@/models/attendance/employeeAttendanceModel";
    import attendanceModelDTO, { attendanceModel } from "@/models/attendance/attendanceModel";
    import expenseModelDTO, { employeeActivationCheck,  expenseModel} from "@/models/expense/expenseModel";
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    import moment from "moment";
    const obj = ref<employeeAttendanceModel>(new employeeAttendanceModel());
    import commonModule from "@/composables/modules/commonModule";
    const { PatchData,PostData,GetData } = new commonModule();
    
    const objEmp = ref<attendanceModel>(new attendanceModel());
    const objEmpVerifyModel = ref<employeeActivationCheck>(new employeeActivationCheck());
    const v$ = ref();
    const frm = ref();
    const employeeList = ref([]);
    const employeeActive = ref(false);
    const expenseLoaded = ref(false);
    const salaryTypeEnable = ref(false);
    const SalaryType = ref('');
    const BasicSalary = ref(0);
    const Msg = ref(Id.value > 0 ? "Atctivation updated successfully." : "Atctivation added successfully.");
    onMounted(() => {
        obj.value.AttendanceDate = new Date();
        //searchEmployee();    
       
       
        frm.value.SetValidation((data: employeeAttendanceModel) => {            
            obj.value = data;   
            BasicSalary.value =  obj.value.BasicSalary;
            SalaryType.value = obj.value.SalaryType;
            searchEmployee();
      //onEmployeeSelect();         
            v$.value = useVuelidate(employeeAttendanceModelDTO.rules, obj.value);
        });
        obj.value.TotalExpenseAmount =0;
    });
    const successCallback = (data: any) => {
        dialogRef.value.close();            
    };
    const failureCallBack = (data: any) => {
        //
    };
    const CloseModal = () => {
        dialogRef.value.close();
    };
    const lastAttendanceDate = ref<string | null>(null); 
    const lastAttendEmployee = ref<string | null>(null); 
    const searchEmployee = async() => {
        
        const empListobj = {
            filterDate: (obj.value.Id>0)?(moment(obj.value.AttendanceDate).utcOffset(0, true).format("YYYY-MM-DD"))
            :(moment(obj.value.AttendanceDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD"))
        };
        if(empListobj.filterDate==="Invalid date")
        {
            return ;
        }
        const formattedDate = obj.value.AttendanceDate
      ? moment(obj.value.AttendanceDate, "MM yy").format("YYYY-MM")
      : null;
      if (formattedDate && formattedDate !== lastAttendanceDate.value) {
PostData( UrlConstants.apiGetActiveEmployee,empListobj, "", (data: any) => {
    lastAttendanceDate.value = moment(obj.value.AttendanceDate, "MM yy").format("YYYY-MM");
    employeeList.value = data;
    if(obj.value.Id <= 0){
    obj.value.PayableAmount = null;
    obj.value.TotalAmount = null;
    obj.value.TotalExpenseAmount = null;
    obj.value.TotalExtraAmount = null;
    obj.value.Remarks = null;
    obj.value.AttendDays = null;
    }
    
    expenseLoaded.value = false;

    if(employeeList.value.length <= 0)
{
    obj.value.EmployeeID= null;
    
}
},null);
      }
}

const onEmployeeSelect = async(data:any) => {
    if(data == undefined)
{
    return ;
}
    SalaryType.value = '';
    obj.value.SalaryType = '';
    obj.value.BasicSalary = null;
    
    
    obj.value.DivisionName = null;
    obj.value.DesignationName = null;
    obj.value.DepartmentName = null;
    //if(data != undefined && data != null)
    {
        if(obj.value.EmployeeID != undefined || obj.value.EmployeeID != null && obj.value.EmployeeID !== 0)
        {
        var findEmp = employeeList.value.filter(x=>x.EmployeeID === obj.value.EmployeeID);
       if(findEmp != null){
        SalaryType.value = (findEmp[0]?.SalaryType)
        obj.value.SalaryType = SalaryType.value;
        BasicSalary.value = (findEmp[0]?.BasicSalary)
        obj.value.BasicSalary =BasicSalary.value ;
        obj.value.DivisionName = (findEmp[0]?.DivisionName)
        obj.value.DesignationName = (findEmp[0]?.DesignationName)
        obj.value.DepartmentName = (findEmp[0]?.DepartmentName)
       }
    }else{
        obj.value.PayableAmount = null;
    obj.value.TotalAmount = null;
    obj.value.TotalExpenseAmount = null;
    obj.value.TotalExtraAmount = null;
    obj.value.Remarks = null;
    obj.value.AttendDays = null;
    expenseLoaded.value = false;
    }
    if (obj.value.AttendanceDate) {
        objEmpVerifyModel.value.ExpenseDate = moment(obj.value.AttendanceDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD");
}
   objEmpVerifyModel.value.EmployeeID = obj.value.EmployeeID;

   const formattedEmployeeId = obj.value.EmployeeID
      ? obj.value.EmployeeID
      : null;
if((objEmpVerifyModel.value.ExpenseDate != undefined || objEmpVerifyModel.value.ExpenseDate != null)&&
(objEmpVerifyModel.value.EmployeeID != undefined || objEmpVerifyModel.value.EmployeeID != null) && objEmpVerifyModel.value.EmployeeID !== 0 && !expenseLoaded.value && lastAttendEmployee.value !== formattedEmployeeId){
    
    await PostData(UrlConstants.apiGetEmployeeExpense, objEmpVerifyModel.value, "", (data: any) => {    
        expenseLoaded.value = true;
        lastAttendEmployee.value = obj.value.EmployeeID;
            if(data.Status > 0)
            {
                obj.value.TotalExpenseAmount = data.Status; 
            }else{
                obj.value.TotalExpenseAmount =0;
            }
        },null);
    }   
}
};




const recalculate = async(data:any) => {
    if(data == undefined)
{
    return ;
}
    
    //if(data != undefined && data != null)
    {
       
    if (obj.value.AttendanceDate) {
        objEmpVerifyModel.value.ExpenseDate = moment(obj.value.AttendanceDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD");
}
   objEmpVerifyModel.value.EmployeeID = obj.value.EmployeeID;

   const formattedEmployeeId = obj.value.EmployeeID
      ? obj.value.EmployeeID
      : null;
if((objEmpVerifyModel.value.ExpenseDate != undefined || objEmpVerifyModel.value.ExpenseDate != null)&&
(objEmpVerifyModel.value.EmployeeID != undefined || objEmpVerifyModel.value.EmployeeID != null) && objEmpVerifyModel.value.EmployeeID !== 0 && !expenseLoaded.value && lastAttendEmployee.value !== formattedEmployeeId){
    
    await PostData(UrlConstants.apiGetEmployeeExpense, objEmpVerifyModel.value, "", (data: any) => {    
        
        
            if(data.Status > 0)
            {
                obj.value.TotalExpenseAmount = data.Status; 
            }else{
                obj.value.TotalExpenseAmount =0;
            }
        },null);
    }   
}
};

const onDateChange = async() => {
  await onEmployeeSelect();
};
const calculationAmt = async (data: any) => {
        const attendDays = obj.value.AttendDays ?? 0; // Default to 0 if null/undefined
      const totalExpense = obj.value.TotalExpenseAmount ?? 0; // Default to 0 if null/undefined
      
      if (attendDays > 0) {
        if(salaryTypeEnable.value && SalaryType.value !== obj.value.SalaryType)
      {
        if (obj.value.SalaryType === 'Daily'){
            //obj.value.BasicSalary = parseInt(BasicSalary.value/30);
            obj.value.TotalAmount = parseInt((attendDays * obj.value.BasicSalary)+parseInt(obj.value?.TotalExtraAmount??0));
            obj.value.PayableAmount = parseInt(obj.value.TotalAmount - totalExpense);
        }

      }else{
        if (obj.value.SalaryType === 'Daily') {
          obj.value.TotalAmount = parseInt((attendDays * obj.value.BasicSalary)+parseInt(obj.value?.TotalExtraAmount??0));
          obj.value.PayableAmount = parseInt(obj.value.TotalAmount - totalExpense); // Deduct expense amount
        } else if (obj.value.SalaryType === 'Monthly') {
          obj.value.TotalAmount = parseInt(obj.value.BasicSalary+parseInt(obj.value?.TotalExtraAmount??0));
          obj.value.PayableAmount = parseInt(obj.value.TotalAmount - totalExpense); // Deduct expense amount
        }
    }
      }
    };

import $ from "jquery";
const dropdownRef = ref<InstanceType<typeof Dropdown> | null>(null);
const focusSearch = async () => {
    setTimeout(() => {
    $(".p-dropdown-filter").focus();
  }, 100);
};
watch(
  [
    () => obj.value?.AttendDays, 
    () => obj.value?.TotalExpenseAmount, 
    () => obj.value?.TotalExtraAmount, 
    () => obj.value?.BasicSalary, 
    () => obj.value?.AttendanceDate,
    ()=> salaryTypeEnable.value  // ✅ Watching for date change
  ],
  async () => {
    obj.value.TotalAmount = null;
    
    if (obj.value) {
        calculationAmt();
    }

    
    if (obj.value.AttendanceDate) {
        if(obj.value.Id == 0){
      await searchEmployee();
      await onEmployeeSelect();
        }
    }
  }
);

const onHHT_MCOlOptionChange = async (data: any) => {
    const attendDays = obj.value.AttendDays ?? 0; // Default to 0 if null/undefined
    const totalExpense = obj.value.TotalExpenseAmount ?? 0; // Default to 0 if null/undefined

    if(salaryTypeEnable.value && SalaryType.value !== obj.value.SalaryType)
      {
        if (obj.value.SalaryType === 'Daily'){            
            obj.value.BasicSalary = parseInt(BasicSalary.value/30);
            obj.value.TotalAmount = parseInt((attendDays * obj.value.BasicSalary)+parseInt(obj.value?.TotalExtraAmount??0));
            obj.value.PayableAmount = parseInt(obj.value.TotalAmount - totalExpense);
        }
      }else{
if(salaryTypeEnable.value){

    var findEmp = employeeList.value.filter(x=>x.EmployeeID === obj.value.EmployeeID);
       if(findEmp != null){
        obj.value.BasicSalary =BasicSalary.value ;
    }
}

      }

    };

    

const beforeSubmitData = (data: any) => {    
    
    if (obj.value.AttendanceDate) {
obj.value.AttendanceDate = moment(obj.value.AttendanceDate, "MM/DD/YYYY").utcOffset(0, true).format("YYYY-MM-DD");
}
};

const SalaryTypeDropOption = ref([
        { Text: "Daily", Value: "Daily" },
        { Text: "Monthly", Value: "Monthly" },
    ]);
    
    const updateIsActive = () => {
        salaryTypeEnable.value = !salaryTypeEnable.value;
        
    };
</script>