<template>
    <div class="grid guter-change">
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">Original FileName</label>
                    <common-input placeholder="Original FileName"
                                  name="OriginalFileName"                                 
                                  v-model="objImage.OriginalFileName"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">FileName</label>
                    <common-input placeholder="FileName"
                                  name="FileName"                                  
                                  v-model="objImage.FileName"></common-input>
                </div>
            </div>            
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">AltTitle</label>
                    <common-input placeholder="AltTitle"
                                  name="AltTitle"                                  
                                  v-model="objImage.AltTitle"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3">                
                <div class="field">
                    <label class="block w-full">AltKeyword</label>
                    <common-input placeholder="AltKeyword"
                                  name="AltKeyword"                                  
                                  v-model="objImage.AltKeyword"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-6">                
                <div class="field">
                    <label class="block w-full">Alt Description</label>
                    <commonArea placeholder="Alt Description"                     
                            v-model="objImage.AltDescription"
                            :autoResize="true"></commonArea>  
                </div>
            </div>   
            <div class="col-12 md:col-6">       
                    <div class="field">
                        <label class="block w-full">Product Image File <span class="info-msg ml-2 mb-0 p-0">Only one file can be uploaded.</span></label>
                        
                        <common-file name="file"  
                            :chooseIcon="'pi pi-plus'"                     
                            v-model="objImage.file"  fileLimit="1" :v$="v$" :accept="'image/*'" ></common-file>
                    </div>  
            </div> 
            <div class="col-12 md:col-2 pb-0">
                <div class="field">
                    <label class="block w-full">IsThumbnail</label>
                    <InputSwitch  v-model="objImage.IsThumbnail"/>
                </div>
            </div>
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Sort Order</label>
                    <common-input fieldlabel="SortOrder"
                                  name="SortOrder"
                                  :IsNumber="true"
                                  inputclass="form-control"
                                  v-model="objImage.SortOrder"></common-input>
                </div>
            </div>
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
    import productModelDTO, { productModel, productImageModel } from "@/models/master/productModel";
    import UrlConstants from "@/utils/urlconstants";
    import commonModule from "@/composables/modules/commonModule";    
    import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
    const { GetData, PostData, PostFormData } = new commonModule();
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const objImage = ref<productImageModel>(new productImageModel());
    const v$ = ref();
    onMounted(async() => {
            if(Id.value > 0){       
                if(Id.value > 0){
                    await GetData(UrlConstants.apiProductImagesMasters +Id.value, (dataImage: any) => {                        
                         objImage.value = dataImage;              
                    },null);
                }
            }
    });
    const CloseModal = async() => {
    dialogRef.value.close();
     }
    const SubmitImageData = async() => {
        //
        PostFormData( UrlConstants.apiProductImagesMasters,objImage.value, (data: any) => {
                 dialogRef.value.close();                 
        },null);
       
    }
</script>
