<template>
    <div class="employee-manage-body" >
        <Fieldset class="p-0" legend="Personal Details">
            <div class="grid guter-change">
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Employee Name</label>
                    <common-input placeholder="Employee Name"
                                  name="EmployeeName"    
                                  :v$="v$"                             
                                  v-model="objImage.EmployeeName"></common-input>
                </div>
            </div>
            
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Employee EnrollNo</label>
                    <common-input placeholder="Employee EnrollNo"
                                  name="EnrollNo"    
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="objImage.EnrollNo"></common-input>
                </div>
            </div>
            
            <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Date Of Birth</label>
                      <common-date
                          id="icon"
                          v-model="objImage.DateofBirth"                          
                          :showIcon="true"
                          dateFormat="dd/mm/yy" 
                          class="w-full"
                          placeholder="Select Date"                          
                          name="DateofBirth"
                           :v$="v$"
                      ></common-date>
                  </div>
              </div> 
              <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Age</label>
                    <common-input placeholder="Age"
                                  name="Age"    
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="objImage.Age"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Gender(Male Or Female)</label>
                    <common-input placeholder="Gender(Male Or Female)"
                                  name="Sex"    
                                  :v$="v$"
                                  v-model="objImage.Sex"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Pancard No.</label>
                    <common-input placeholder="PancardNo"
                                  name="PancardNo"    
                                  :v$="v$"
                                  v-model="objImage.PancardNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Adharcard No.</label>
                    <common-input placeholder="AdharcardNo"
                                  name="AdharcardNo"    
                                  :v$="v$"
                                  v-model="objImage.AdharcardNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Voter ID</label>
                    <common-input placeholder="VoterID"
                                  name="VoterID"    
                                  :v$="v$"
                                  v-model="objImage.VoterID"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Driving License No</label>
                    <common-input placeholder="DrivingLicNo"
                                  name="DrivingLicNo"    
                                  :v$="v$"
                                  v-model="objImage.DrivingLicNo"></common-input>
                </div>
            </div>
            </div>
</Fieldset>
<Fieldset class="p-0" legend="Contact Information">
            <div class="grid guter-change">
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Mobile No</label>
                    <common-input placeholder="Mobile No"
                                  name="MobileNo"    
                                  :v$="v$"                             
                                  v-model="objImage.MobileNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">PhoneNo</label>
                    <common-input placeholder="Phone No"
                                  name="PhoneNo"    
                                  :v$="v$"
                                  v-model="objImage.PhoneNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Email</label>
                    <common-input placeholder="Email"
                                  name="Email"    
                                  :v$="v$"
                                  v-model="objImage.Email"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Relative Name</label>
                    <common-input placeholder="Relative Name"
                                  name="RelativeName"   
                                  :v$="v$"
                                  v-model="objImage.RelativeName"></common-input>
                </div>
            </div>
            
              <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Relative Mobile No</label>
                    <common-input placeholder="Relative Mobile No"
                                  name="RelativeMobileNo"    
                                  :v$="v$"
                                  v-model="objImage.RelativeMobileNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Reference By</label>
                    <common-input placeholder="Reference By"
                                  name="ReferenceBy"    
                                  :v$="v$"
                                  v-model="objImage.ReferenceBy"></common-input>
                </div>
            </div>
            
            </div>
</Fieldset>
<Fieldset class="p-0" legend="Contact Information">
            <div class="grid guter-change">
                <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Address</label>
                    <common-input placeholder="Address"
                                  name="Address"    
                                  :v$="v$"
                                  v-model="objImage.Address"></common-input>
                </div>
            </div>
                <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Address 2</label>
                    <common-input placeholder="Address 2"
                                  name="Address2"
                                  :v$="v$"
                                  v-model="objImage.Address2"></common-input>
              
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Pin code</label>
                    <common-input placeholder="Pin code"
                                  name="Pincode"    
                                  :v$="v$"
                                  v-model="objImage.Pincode"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Room No.</label>
                    <common-input placeholder="Room No."
                                  name="RoomNo"    
                                  :v$="v$"
                                  v-model="objImage.RoomNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Resident</label>
                    <common-input placeholder="Resident"
                                  name="Resident"    
                                  :v$="v$"
                                  v-model="objImage.Resident"></common-input>
                </div>
            </div>
            </div>
</Fieldset>
<Fieldset class="p-0" legend="Banking Details">
            <div class="grid guter-change">
                <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Bank Name</label>
                    <common-input placeholder="Bank Name"
                                  name="BankName"    
                                  :v$="v$"
                                  v-model="objImage.BankName"></common-input>
                </div>
            </div>
                <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Bank Account No.</label>
                    <common-input placeholder="Bank Account No."
                                  name="BankAccountNo"
                                   :IsNumber="true"
                                  :v$="v$"
                                  v-model="objImage.BankAccountNo"></common-input>
              
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">PF Account No.</label>
                    <common-input placeholder="PF Account No."
                                  name="PFAccountNo"    
                                  :IsNumber="true"
                                  :v$="v$"
                                  v-model="objImage.PFAccountNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">ESI Account No.</label>
                    <common-input placeholder="ESI Account No."
                                  name="ESIAccountNo"   
                                  :IsNumber="true" 
                                  :v$="v$"
                                  v-model="objImage.ESIAccountNo"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Nominee Name</label>
                    <common-input placeholder="Nominee Name"
                                  name="NomineeName"    
                                  :v$="v$"
                                  v-model="objImage.NomineeName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Nominee Relationship</label>
                    <common-input placeholder="Nominee Relationship"
                                  name="NomineeRelationship"    
                                  :v$="v$"
                                  v-model="objImage.NomineeRelationship"></common-input>
                </div>
            </div>
            </div>
</Fieldset>
<Fieldset class="p-0" legend="Other Details">
    <div class="grid guter-change">
               <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Designation</label>
                      <common-dropdown placeholder="Please Select Designation" 
                      :v$="v$" name="DesignationID" v-model="objImage.DesignationID" 
                      :isWatch="false" 
                      :dataurl="UrlConstants.apidropdown+'?Mode=Designation'" />
                  </div>
              </div>
              <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Department</label>
                      <common-dropdown placeholder="Please Select Department" 
                      :v$="v$" name="DepartmentID" v-model="objImage.DepartmentID" 
                      :isWatch="false" 
                      :dataurl="UrlConstants.apidropdown+'?Mode=Department'" />
                  </div>
              </div>
              
              <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Division</label>
                      <common-dropdown placeholder="Please Select Division" 
                      :v$="v$" name="DivisionID" v-model="objImage.DivisionID" 
                      :isWatch="false" 
                      :dataurl="UrlConstants.apidropdown+'?Mode=Division'" />
                  </div>
              </div>
              <div class="col-12 md:col-3">
                  <div class="field">
                      <label class="block w-full">Salary Type</label>
                      <common-dropdown placeholder="Please Salary Type" 
                      :v$="v$" name="SalaryType" v-model="objImage.SalaryType" 
                      :isWatch="false" :options="SalaryTypeDropOption"
                       />
                  </div>
              </div>
              <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Basic Salary</label>
                    <common-input placeholder="Basic Salary"
                                  name="BasicSalary"   
                                  :IsNumber="true" 
                                  :v$="v$"
                                  v-model="objImage.BasicSalary"></common-input>
                </div>
            </div>
              <div class="col-12 md:col-3 pb-0">
                <div class="field">
                <label class="block w-full" for="Status">Status</label>
                <InputSwitch inputId="Status" v-model="objImage.IsActive" />
                </div>
            </div>
            
        </div>
    </Fieldset>
    <Fieldset class="p-0" legend="Document Details(Only one file can be accept.)">
    <div class="grid guter-change">
        <div class="col-12 md:col-3">       
                    <div class="field">
                        <label class="block w-full">Aadhar Card Front Image</label>
                        
                        <common-file name="file"  
                            :chooseIcon="'pi pi-plus'"                     
                            v-model="objImage.ImageAadharCardFront"  fileLimit="1" :v$="v$" :accept="'image/*'" ></common-file>
                    </div>  
            </div> 
            <div class="col-12 md:col-3">       
                    <div class="field">
                        <label class="block w-full">Aadhar Card Back Image</label>
                        
                        <common-file name="file"  
                            :chooseIcon="'pi pi-plus'"                     
                            v-model="objImage.ImageAadharCardBack"  fileLimit="1" :v$="v$" :accept="'image/*'" ></common-file>
                    </div>  
            </div> 
            <div class="col-12 md:col-3">       
                    <div class="field">
                        <label class="block w-full">Employee Photo </label>
                        
                        <common-file name="file"  
                            :chooseIcon="'pi pi-plus'"                     
                            v-model="objImage.ImageEmployeePhoto"  fileLimit="1" :v$="v$" :accept="'image/*'" ></common-file>
                    </div>  
            </div> 
            <div class="col-12 md:col-3">       
                    <div class="field">
                        <label class="block w-full">Other Document </label>
                        
                        <common-file name="file"  
                            :chooseIcon="'pi pi-plus'"                     
                            v-model="objImage.ImageOtherDoc"  fileLimit="1" :v$="v$" ></common-file>
                    </div>  
            </div> 
        </div>
    </Fieldset>
        </div>  
            <div class="p-dialog-footer">
                <div></div>
                <div>
                    <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                    <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" @click="SubmitImageData" type="submit" autofocus />
                </div>
        </div>
</template>
<script setup lang="ts">
 import { inject, onMounted, ref } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import employeeModelDTO, { employeeModel } from "@/models/master/employeeModel";
    import moment from "moment";
    import UrlConstants from "@/utils/urlconstants";
    import commonModule from "@/composables/modules/commonModule";    
    import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
    const { GetData, PostData, PostFormData } = new commonModule();
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const objImage = ref<employeeModel>(new employeeModel());
    const v$ = ref();
    onMounted(async() => {
            if(Id.value > 0){       
                
                    await GetData(UrlConstants.apiEmployeeMaster +Id.value, (dataImage: any) => {                        
                         objImage.value = dataImage;              
                    },null);
                
            }
    });
    const CloseModal = async() => {
    dialogRef.value.close();
     }
    const SubmitImageData = async() => {
        const result = ref(false);
       

        if (objImage.value.DateofBirth) {
    // Format DateofBirth using moment.js to the desired format
    objImage.value.StrDateofBirth = moment(objImage.value.DateofBirth).utcOffset(0, true).format("YYYY-MM-DD");
    objImage.value.DateofBirth = null;
}
        v$.value = useVuelidate(employeeModelDTO.rules, objImage.value);
        result.value = await v$.value.value.$validate();
            if(result.value){
        PostFormData( UrlConstants.apiEmployeeMaster,objImage.value, (data: any) => {
                 dialogRef.value.close();                 
        },null);
    }
       
    }

    const SalaryTypeDropOption = ref([
        { Text: "Daily", Value: "Daily" },
        { Text: "Monthly", Value: "Monthly" },
    ]);
    
</script>
