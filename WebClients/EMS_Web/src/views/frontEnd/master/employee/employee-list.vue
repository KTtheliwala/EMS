<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :AdvSearchBtnClass="'advance-search-filter-only-for-mobile'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Employee Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Employee No</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
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
                    <label class="block w-full">Employee Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Employee No</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
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
            <Button icon="pi pi-plus-circle" label="Create Employee" v-if="(Page?.IsAdd ?? false )"  @click="OpenAddNewManage"></Button>
        </template>
        <template v-slot:columns>
            <Column selectionMode="multiple" class="check-box-center" frozen v-if="(Page?.IsDelete ?? false )"></Column>
            <Column header="Action" v-if="(Page?.IsEdit ?? false )" class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                    </div>
                </template>
            </Column>
             <!-- Personal Details -->
             <Column header="Active" :sortable="true" field="IsActive" class="active-col">
                <template #body="columns">
                    <InputSwitch  v-model="columns.data.IsActive" :disabled="!(Page?.IsEdit ?? false ) ? true : false" @change="updateIsActive(columns.data.EmployeeID)" />
                </template>
            </Column>
             <Column field="EnrollNo" header="Enroll No" :sortable="true"></Column>
            <Column field="EmployeeName" header="Employee Name" :sortable="true"></Column>
            
            <Column field="DateofBirth" header="Date of Birth" :sortable="true">
                <template #body="slotProps">
                    {{ ConvertToUserTimeZone(slotProps.data.DateofBirth, "DD MMM YYYY") }}
                </template>
            </Column> 
            <Column field="Age" header="Age" :sortable="true"></Column>
            <Column field="Sex" header="Gender" :sortable="true"></Column>
            <Column field="PancardNo" header="Pancard No" :sortable="true"></Column>
            <Column field="AdharcardNo" header="Aadhar Card No" :sortable="true"></Column>
            <Column field="VoterID" header="Voter ID" :sortable="true"></Column>
            <Column field="DrivingLicNo" header="Driving License No" :sortable="true"></Column>

            <!-- Contact Information -->
            <Column field="MobileNo" header="Mobile No" :sortable="true"></Column>
            <Column field="PhoneNo" header="Phone No" :sortable="true"></Column>
            <Column field="Email" header="Email" :sortable="true"></Column>
            <Column field="RelativeName" header="Relative Name" :sortable="true"></Column>
            <Column field="RelativeMobileNo" header="Relative Mobile No" :sortable="true"></Column>
            <Column field="ReferenceBy" header="Reference By" :sortable="true"></Column>

            <!-- Address Information -->
            <Column field="Address" header="Address" :sortable="true"></Column>
            <Column field="Address2" header="Address 2" :sortable="true"></Column>
            <Column field="Pincode" header="Pincode" :sortable="true"></Column>
            <Column field="RoomNo" header="Room No" :sortable="true"></Column>
            <Column field="Resident" header="Resident" :sortable="true"></Column>

            <!-- Banking Details -->
            <Column field="BankName" header="Bank Name" :sortable="true"></Column>
            <Column field="BankAccountNo" header="Bank Account No" :sortable="true"></Column>
            <Column field="PFAccountNo" header="PF Account No" :sortable="true"></Column>
            <Column field="ESIAccountNo" header="ESI Account No" :sortable="true"></Column>
            <Column field="NomineeName" header="Nominee Name" :sortable="true"></Column>
            <Column field="NomineeRelationship" header="Nominee Relationship" :sortable="true"></Column>

            <!-- Employment Details -->
            <Column field="DesignationName" header="Designation Name" :sortable="true"></Column>
            <Column field="DepartmentName" header="Department Name" :sortable="true"></Column>
            <Column field="DivisionName" header="Division Name" :sortable="true"></Column>
            <Column field="BasicSalary" header="Basic Salary" :sortable="true"></Column>
            <Column field="SalaryType" header="Salary Type" :sortable="true"></Column>
            <Column field="ACStatus" header="AC Status" :sortable="true"></Column>
            <Column header="File(s)" class="text-center" :sortable="false">
                <template #body="columns">
                    <Button  class="p-button-aux" icon="pi pi-image" label="Aadhar Card (Front)" v-if="columns.data?.ImageAadharCardFront != null"  @click="imagePreviewFn(columns.data.ImageAadharCardFront,'Aadhar Card (Front)'+'-'+columns.data?.EmployeeName+'(Enroll No.:'+columns.data.EnrollNo+')')"></Button>
                    <Button class="p-button-aux ml-2" icon="pi pi-image" label="Aadhar Card (Back)" v-if="columns.data?.ImageAadharCardBack != null"  @click="imagePreviewFn(columns.data.ImageAadharCardBack, 'Aadhar Card (Back)'+'-'+columns.data?.EmployeeName+'(Enroll No.:'+columns.data.EnrollNo+')')"></Button>
                    <Button class="p-button-aux ml-2" icon="pi pi-image" label="Employee Photo" v-if="columns.data?.ImageEmployeePhoto != null"  @click="imagePreviewFn(columns.data.ImageEmployeePhoto, 'Employee Photo'+'-'+columns.data?.EmployeeName+'(Enroll No.:'+columns.data.EnrollNo+')')"></Button>
                    <Button class="p-button-aux ml-2" icon="pi pi-image" label="Other Docs" v-if="columns.data?.ImageOtherDoc != null"  @click="imagePreviewFn(columns.data.ImageOtherDoc, 'Other Doc'+'-'+columns.data?.EmployeeName+'(Enroll No.:'+columns.data.EnrollNo+')')"></Button>                    
                </template>
            </Column>
        </template>
    </common-grid>
    <common-dialog ref="popupManage" class="ex-large-modal"></common-dialog>
    <common-dialog ref="popupImageManage" class="large-modal"></common-dialog>
</template>
<script setup lang="ts">
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { inject, ref } from "vue";
    import companyManage from "@/views/frontEnd/master/employee/employee-manage.vue";
    import imagePreview from "@/views/frontEnd/master/employee/imagePreview.vue";
    import commonModule from "@/composables/modules/commonModule";
    
    const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');
    import { AuthService } from "@/composables/api/authService";
    const Page = AuthService.GetPagePermission('Masters','Employee')
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const popupImageManage = ref();
    const ddlItem = ref();
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(companyManage, "Create Employee", 0, () => {
            dt.value.reloadGrid();
        });
    };
    const updateIsActive = (data: any) => {
        PatchData(UrlConstants.apiCompanyMasterUpdateStatus, data, "Company Status Updated successfully.", null, null);
    };
    const imagePreviewFn= (data: any, title: any) => {
        popupImageManage.value.OpenModal(imagePreview, title, data, null);        
    };

    const gridConfig = ref({
        api: UrlConstants.apiEmployeeMasterList,
        deleteApi: UrlConstants.apiEmployeeMaster,
        filters: [
            { searchType: "Filter", fieldName: "EmployeeName", fieldValue: null, fieldDisplayName: "Employee Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "", },
            { searchType: "Filter", fieldName: "EnrollNo", fieldValue: null, fieldDisplayName: "Enroll No", opType: SearchOperations.contains },

        ],
        sortcolumn: "EmployeeID",
        sortorder: -1,
        doubleClickHander: null,
    } as GridConfig);

    const gridMenuItems = ref([
        {
            icon: "pi pi-pencil",
            label: "Edit",
            callback: (data: any) => {
              popupManage.value.OpenModal(companyManage, "Edit Employee", data.EmployeeID, dt.value.reloadGrid);
            },
            visible:(Page?.IsEdit ?? false ), 
        },
        // {
        //     icon: "pi pi-trash",
        //     label: "Delete",
        //     callback: (data: any) => {
        //         dt.value.SingleDelete(data.Id);
        //     },
        //     visible:true, 
        // },
    ]);
    const StatusDropOption = ref([
        { Text: "Active", Value: "true" },
        { Text: "Inactive", Value: "false" },
    ]);



    const filename = ref("Employee_list_"+Date.now())
</script>
