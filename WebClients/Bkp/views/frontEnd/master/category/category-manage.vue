<template>
    <common-form ref="frm" :dataId="Id" :actionUrl="UrlConstants.apiCategoryMasters" :v$="v$" :data="obj" :successCallBack="successCallback" :failureCallBack="failureCallBack" :successMessage="Msg">
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
                    <label class="block w-full">Short Description</label>
                    <commonArea placeholder="Short Description"                     
                    v-model="obj.ShortDescription"
                    :autoResize="true"></commonArea>     
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
    import categoryModelDTO, { categoryModel } from "@/models/master/categoryModel";
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const obj = ref<categoryModel>(new categoryModel());
    if (Id.value == 0)
        obj.value.IsActive = true;
    const v$ = ref();
    const frm = ref();
    const Msg = ref(Id.value > 0 ? "Company updated successfully." : "Company added successfully.");
    onMounted(() => {
        frm.value.SetValidation((data: categoryModel) => {
            obj.value = data;
            v$.value = useVuelidate(categoryModelDTO.rules, obj.value);
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