<template>
    <TabView :lazy="true">
    <TabPanel header="Product Details">
        <div class="grid guter-change">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Category</label>
                    <common-dropdown placeholder="Please Select" 
                    :v$="v$" name="CategoryId" v-model="obj.CategoryId" 
                    :isWatch="false" 
                    :dataurl="UrlConstants.apidropdown+'?Mode=Category'" />
                </div>
            </div>  
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
                    <label class="block w-full">Product Code</label>
                    <common-input placeholder="Product Code"
                                  name="ProductCode"
                                  :v$="v$"
                                  v-model="obj.ProductCode"></common-input>
                </div>
            </div>

            <div class="col-12 md:col-6">
                <div class="field">
                    <label class="block w-full">Short Description</label>
                    <commonArea placeholder="Short Description"                     
                    v-model="obj.ShortDescription"
                    :autoResize="true"></commonArea>     
                </div>
            </div>    
           
            <div class="col-12 md:col-6">
                <div class="field">
                    <label class="block w-full">Description</label>
                    <commonArea placeholder="Description"                     
                    v-model="obj.Description"
                    :autoResize="true"></commonArea>     
                </div>
            </div>    
            
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Meta Title</label>
                    <commonArea placeholder="Meta Title"                     
                    v-model="obj.MetaTitle  "
                    :autoResize="true"></commonArea>     
                </div>
            </div>    
           
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Meta Keyword </label>
                    <commonArea placeholder="Meta Keyword"                     
                    v-model="obj.MetaKeyword"
                    :autoResize="true"></commonArea>     
                </div>
            </div>   
            
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Meta Description</label>
                    <commonArea placeholder="Meta Description"                     
                    v-model="obj.MetaDescription"
                    :autoResize="true"></commonArea>     
                </div>
            </div> 
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="block w-full">Price</label>
                    <common-input fieldlabel="Price"
                                  name="Price"
                                  :IsNumber="true"
                                  :IsDecimal="true"
                                  inputclass="form-control"
                                  v-model="obj.Price"></common-input>
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
                <Button :label="(Id>0?'Update':'Create')" icon="pi pi-plus-circle" @click="SubmitData" type="submit" autofocus />
            </div>
        </div>
    </TabPanel>
    <TabPanel header="Image Upload" :disabled="obj.Id == 0" >
        <common-grid :config="gridConfig" ref="dt" :filters="filters" :filename="filename" :hideFilter="true" :hideExportBtn="false">
            <template v-slot:right-buttons>
                <Button icon="pi pi-plus-circle" label="Add Product Image"  @click="OpenNewManage"></Button>
            </template>
            <template v-slot:columns>
                <!-- <Column selectionMode="multiple" class="check-box-center" frozen></Column> -->
                <Column header="Action" class="action-dot-button" frozen alignFrozen="left">
                    <template #body="slotProps">
                        <div>
                            <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                        </div>
                    </template>
                </Column>
                <Column field="OriginalFileName" header="Original FileName" :sortable="true"></Column> 
                <Column field="FileName" header="File Name" :sortable="true"></Column>   
                <Column field="AltTitle" header="Alt Title" :sortable="true"></Column>    
                <Column field="AltKeyword" header="Alt Keyword" :sortable="true"></Column>  
                <Column field="AltDescription" header="AltDescription" :sortable="true"></Column>                                  
                <Column field="SortOrder" header="Sort Order" :sortable="true" class="sort-order-col"></Column>
                <Column header="Thumbnail" :sortable="true" field="IsThumbnail" class="active-col">
                    <template #body="columns">
                        <InputSwitch v-model="columns.data.IsThumbnail" @change="updateIsActive(columns.data.Id)" />
                    </template>
                </Column>
            </template>
        </common-grid>
        <common-dialog ref="popupManage" class="large-modal"></common-dialog>
    </TabPanel>
    <!-- <TabPanel header="Header III" :disabled="false">
        <p class="m-0">
            At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in
            culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus.
        </p>
    </TabPanel> -->
   
</TabView>

</template>
<script setup lang="ts">
    import { inject, onMounted, ref } from "vue";
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import { useVuelidate } from "@vuelidate/core";
    import productModelDTO, { productModel, productImageModel } from "@/models/master/productModel";
    import UrlConstants from "@/utils/urlconstants";
    import commonModule from "@/composables/modules/commonModule";    
    import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
    import productsImageManage from "@/views/frontEnd/master/products/productsImage-manage.vue";
    const { GetData, PostData, PostFormData, PatchData } = new commonModule();
    const dialogRef = inject("dialogRef") as any;
    const Id = ref(dialogRef.value.data);
    const obj = ref<productModel>(new productModel());   
    if (Id.value == 0)
        obj.value.IsActive = true;
    const v$ = ref();
    const dt = ref(null);
    const popupManage = ref();
    const frm = ref();
    const Msg = ref(Id.value > 0 ? "Product updated successfully." : "Product added successfully.");
    onMounted(async() => {
        if(Id.value > 0){
            await GetData(UrlConstants.apiProductsMaster +Id.value, (data: any) => {
                obj.value = data;                            
            },null);
        }
    });
    const updateIsActive = (data: any) => {
        GetData(UrlConstants.apiProductImagesMastersUpdateStatus+"/"+data.Id+"/"+data.ProductId, null, null);
    };

    const gridConfig = ref({
        api: UrlConstants.apiProductImagesMastersList,
        deleteApi: UrlConstants.apiProductImagesMasters,
        filters: [
             { searchType: "Filter", fieldName: "OrginalFileName", fieldValue: null, fieldDisplayName: "Orginal FileName", opType: SearchOperations.contains },
             { searchType: "Filter", fieldName: "FileName", fieldValue: null, fieldDisplayName: "FileName", opType: SearchOperations.contains },
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
              popupManage.value.OpenModal(productsImageManage, "Edit Product Image", data.Id, dt.value.reloadGrid);
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

    const filename = ref("user_list_"+Date.now())
    const CloseModal = async() => {
       dialogRef.value.close();
    }
    const SubmitData = async() => {
        //
        const result = ref();
        v$.value = useVuelidate(productModelDTO.rules, obj.value);
        result.value = await v$.value.value.$validate();
        if(result.value){
            PostFormData( UrlConstants.apiProductsMaster,obj.value, (data: any) => {
                 //dialogRef.value.close();
                 //objImage.value.ProductId = data.Id;
        },null);
        }
    }

    const OpenNewManage = async() =>{
        popupManage.value.OpenModal(productsImageManage, "Add Product Image",0, dt.value.reloadGrid);
    }

   
</script>