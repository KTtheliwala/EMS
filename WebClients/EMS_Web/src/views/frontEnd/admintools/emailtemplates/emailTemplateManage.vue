<template>
    <common-form ref="frm" :beforeSubmit="beforeSubmitData" :dataId="Id" :actionUrl="UrlConstants.apiemailtemplate" :v$="v$" :data="objEmailTemplate" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change">
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">From Email</label>
                    <common-input placeholder="" name="FromEmail" inputclass="form-control" :v$="v$" v-model="objEmailTemplate.FromEmail"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">From Name</label>
                    <common-input placeholder="" name="FromName" inputclass="form-control" :v$="v$" v-model="objEmailTemplate.FromName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">                    
                    <label class="block w-full">Template Code</label>
                    <label class="">{{objEmailTemplate.TemplateCode}}</label>
                </div>
            </div>
            <div class="col-12 md:col-3">
                <div class="field">
                    <label class="block w-full">Subject</label>
                    <common-input placeholder="" name="TemplateSubject" inputclass="form-control" :v$="v$" v-model="objEmailTemplate.TemplateSubject"></common-input> 
                </div>
            </div>
            <div class="col-12 md:col-12">
                <div class="field">
                    <label class="block w-full">Template Body</label>                 
                    <commonEditor 
                     name="TemplateBody" 
                     :v$="v$" 
                      v-model="objEmailTemplate.TemplateBody" />
                     
                    <!--  <newcommonEditor name="TemplateBody" :v$="v$" :Id="'TemplateBody'" v-model="objEmailTemplate.TemplateBody"  ></newcommonEditor>  -->
                </div>
            </div>                    
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <InputSwitch v-model="objEmailTemplate.IsActive" />
                </div>
            </div>
        </div>
        <div class="p-dialog-footer">
            <div></div>
            <div>
                <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" type="submit" autofocus />
            </div>
        </div>
    </common-form>
</template>
<script setup lang="ts">
    import { inject, onMounted, ref } from "vue";
    import { useVuelidate } from '@vuelidate/core'        
    import UrlConstants from "@/utils/urlconstants";
    import commonModule from "@/composables/modules/commonModule";
    import objemailTemplateModelDTO, { emailTemplateModel } from "@/models/admintools/emailTemplateModel";
    const { GetData, PostData } = new commonModule();
    const dialogRef = inject('dialogRef') as any;
    const Id = ref(dialogRef.value.data);
    const objEmailTemplate = ref<emailTemplateModel>(new emailTemplateModel());
    if (Id.value == 0)
    objEmailTemplate.value.IsActive = true;
    const v$ = ref();
    const templateBody =  ref('Always bet on Prime');
    const editorConfig = ref();    
    const editorData= '<p>Content of the editor.</p>';
    const frm = ref();    
    const refMailCCRoleId = ref([]);
    const refNotiCCRoleId = ref([]);
    const EmailTemplateCCRoleId_Options = ref([]);    
    var FilterMailCCRoleIdList = ref([]);
    var FilterNotiCCRoleIdList = ref([]);
     objEmailTemplate.value.TemplateBody = "<p>Initial content</p>"
    const Msg = ref(Id.value > 0 ? "Email template update successfully." : "Email template added successfully.");
    onMounted(() => {    
        
        frm.value.SetValidation((data: emailTemplateModel) => {
            Promise.all([BindEmailTemplate_Options()]).then(async () => { 
            objEmailTemplate.value = data;            
            if(objEmailTemplate.value.MailCCRoleIds != null && objEmailTemplate.value.MailCCRoleIds !='')
            {
            for (let i = 0; i < objEmailTemplate.value.MailCCRoleIds.split(',').length; i++) {
        var GetSelectedRaw = EmailTemplateCCRoleId_Options.value.find(
          (x) => x.Value == objEmailTemplate.value.MailCCRoleIds.split(',')[i]
        );
        refMailCCRoleId.value.push(GetSelectedRaw.Value);
        FilterMailCCRoleIdList.value.push(GetSelectedRaw.Value);
      }}
      if(objEmailTemplate.value.NotiCCRoleIds != null && objEmailTemplate.value.NotiCCRoleIds !='')
            {
            for (let i = 0; i < objEmailTemplate.value.NotiCCRoleIds.split(',').length; i++) {
        var GetSelectedRawNotiCC = EmailTemplateCCRoleId_Options.value.find(
          (x) => x.Value == objEmailTemplate.value.NotiCCRoleIds.split(',')[i]
        );
        refNotiCCRoleId.value.push(GetSelectedRawNotiCC.Value);
        FilterNotiCCRoleIdList.value.push(GetSelectedRawNotiCC.Value);
      }}
       v$.value = useVuelidate(objemailTemplateModelDTO.rules, objEmailTemplate.value);
       });
    });   
    });
    
    const successCallback = (data: any) => {
        dialogRef.value.close();
    };
    const getContent = (val: any): void => {        
        objEmailTemplate.value.TemplateBody = val;
        }
    const failureCallBack = (data: any) => {
        //
    };
    const CloseModal = () => {
        dialogRef.value.close();
    }
    const BindEmailTemplate_Options = async () => {
  await GetData(UrlConstants.apidropdown + "?Mode=AllRole",async (data: any) => {
    for (let i = 0; i < data.length; i++) {
      EmailTemplateCCRoleId_Options.value.push({
        Text: data[i]["Text"],
        Value: data[i]["Value"],
      });
    }
  });
  
};
BindEmailTemplate_Options();
const onEmailTemplateCCRoleIdOptionChange = async (e: number) => {
    FilterMailCCRoleIdList.value = [];
  for (let i = 0; i < e.length; i++) {
    var GetSelectedRaw = EmailTemplateCCRoleId_Options.value.find((x) => x.Value == e[i]);
    FilterMailCCRoleIdList.value.push(            
      GetSelectedRaw.Value
    );
  }
};

const beforeSubmitData = (data: any) => {    
    //
};
</script>