<template>
    <common-form ref="frm" :dataId="Id" :actionUrl="UrlConstants.apiQuality" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
        <div class="grid guter-change">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Name</label>
                    <common-input placeholder="Name"
                                  name="Name"
                                  :v$="v$"
                                  v-model="obj.Name"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Short Name</label>
                    <common-input fieldlabel="ShortName"                                 
                                  name="ShortName"
                                  inputclass="form-control"
                                  v-model="obj.ShortName"></common-input>
                </div>
            </div>          
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Group Name</label>
                    <common-dropdown placeholder="Please Select" 
                    :v$="v$" name="GroupId" v-model="obj.GroupId" 
                    :isWatch="false" 
                    :dataurl="UrlConstants.apidropdown+'?Mode=Group'" />
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Sub Group Name</label>
                    <common-dropdown placeholder="Please Select" 
                    :v$="v$" name="GroupId" v-model="obj.SubGroupId" 
                    :isWatch="false" 
                    :dataurl="UrlConstants.apidropdown+'?Mode=SubGroup'" />
                </div>
            </div> 
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">HSN Code</label>
                    <common-input fieldlabel="HSNCode"                                  
                                  name="HSNCode"                               
                                  inputclass="form-control"
                                  v-model="obj.HSNCode"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Denier</label>
                    <common-input fieldlabel="Denier"                                 
                                  name="Denier"
                                  :IsNumber="true"
                                  inputclass="form-control"
                                  v-model="obj.Denier"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Filament</label>
                    <common-input fieldlabel="Filament"  
                    :IsNumber="true"                               
                                  name="Filament"
                                  inputclass="form-control"
                                  v-model="obj.Filament"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Luster</label>
                    <common-input fieldlabel="Luster"                                 
                                  name="Luster"
                                  inputclass="form-control"
                                  v-model="obj.Luster"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Quality Type</label>
                    <common-input fieldlabel="QualityType"                                 
                                  name="QualityType"
                                  inputclass="form-control"
                                  v-model="obj.QualityType"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Description</label>
                    <commonArea placeholder="Description"                     
                    v-model="obj.Description"
                    :autoResize="true"></commonArea>                   
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Sort Order</label>
                    <common-input fieldlabel="SortOrder"
                                  name="SortOrder"
                                  :IsNumber="true"
                                  inputclass="form-control"
                                  v-model="obj.SortOrder"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <InputSwitch v-model="obj.IsActive" />
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
    import { useVuelidate } from "@vuelidate/core";
    import UrlConstants from "@/utils/urlconstants";
    import qualityModelDTO, { qualityModel } from "@/models/master/qualityModel";
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const obj = ref<qualityModel>(new qualityModel());
    if (Id.value == 0)
        obj.value.IsActive = true;
    const v$ = ref();
    const frm = ref();
    const Msg = ref(Id.value > 0 ? "Company updated successfully." : "Company added successfully.");
    onMounted(() => {
        frm.value.SetValidation((data: qualityModel) => {
            obj.value = data;
            v$.value = useVuelidate(qualityModelDTO.rules, obj.value);
        });
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
</script>