<template>
     
    <div class="grid guter-change order-grid">
      <div class="col-12 md:col-12 box3 bottom-seperator" >
        <div class="grid guter-change">
            <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Order No. :</label>
                      <AutoComplete v-model="selectedOrder" optionLabel="Text" optionValue="Value" :suggestions="filteredCountries" @complete="search" />
                  </div>
              </div>  
              <div class="col-12 md:col-2">
                  <div class="field pt-3">
                    <label class="block w-full"></label>
                    <Button label="Search" class="p-button-secondary mr-2"  icon="pi pi-search" @click="SearchOrder" />
                    <Button label="clear" class="p-button-aux"  icon="pi pi-refresh" @click="clearEdit" />
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                   
                  </div>
              </div>
        </div>
      </div>
      <div class="col-12 md:col-6 box3 bottom-seperator">
          <div class="grid guter-change">
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Company Name</label>
                      <common-dropdown placeholder="Please Select" 
                      :v$="v$" name="CompanyId" v-model="obj.CompanyId" 
                      :isWatch="false" 
                      :dataurl="UrlConstants.apidropdown+'?Mode=Company'" />
                  </div>
              </div>  
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Name</label>
                      <common-input placeholder="Contact Name"
                                  name="ContactName"
                                  :v$="v$"
                                  v-model="obj.ContactName"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Moblie</label>
                      <common-input placeholder="Contact Number"
                                  name="ContactNumber"                                  
                                  v-model="obj.ContactNumber"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Email</label>
                      <common-input placeholder="Contact Email"
                                  name="ContactEmail"
                                  :v$="v$"
                                  v-model="obj.ContactEmail"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Date Of Delivery</label>
                      <common-date
                          id="icon"
                          v-model="obj.DateofDelivery"
                          :minDate="minDate"
                          :showIcon="true"
                          class="w-full"
                          placeholder="Select Date"
                          dateFormat="dd M yy"
                          name="DateofDelivery"
                           :v$="v$"
                      ></common-date>
                  </div>
              </div>   
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Date Of Function</label>
                      <common-date
                          id="icon"
                          :minDate="minDate"
                          v-model="obj.DateOfFunction"
                          :showIcon="true"
                          class="w-full"
                          placeholder="Select Date"
                          dateFormat="dd M yy"
                          name="DateOfFunction"
                          :v$="v$"
                      ></common-date>
                  </div>
              </div> 
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Delivery Address Line 1</label>
                      <common-input placeholder="Address Line 1"                     
                      v-model="obj.DeliveryAddress"
                       name="DeliveryAddress"
                      :v$="v$"></common-input>     
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Delivery Address Line 2</label>
                      <common-input placeholder="Address Line 2"                     
                      v-model="obj.DeliveryAddress2"
                       name="DeliveryAddress2"
                      :v$="v$"></common-input>     
                  </div>
              </div>
               <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">City</label>
                      <common-input placeholder="City"
                                  name="City"
                                  :v$="v$"
                                  v-model="obj.City"></common-input>
                  </div>
              </div>
               <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">State</label>
                      <common-input placeholder="State"
                                  name="State"                                  
                                  v-model="obj.State"></common-input>
                  </div>
              </div>
               <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Postal code</label>
                      <common-input placeholder="PostCode"
                                  name="PostCode"
                                  :v$="v$"
                                  v-model="obj.PostCode"></common-input>
                  </div>
              </div>
               <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Country</label>
                      <common-dropdown placeholder="Please Select country" 
                      :v$="v$" name="CountryId" v-model="obj.CountryId" 
                      :isWatch="false" 
                      :dataurl="UrlConstants.apidropdown+'?Mode=Country'" />
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Remark</label>
                      <commonArea placeholder="Remark"                     
                       v-model="obj.Remarks"
                       name="Remark"
                      :v$="v$"
                      :autoResize="true"></commonArea>     
                  </div>
              </div>
          </div>
     </div>      
     <div class="col-12 md:col-6 box3 bottom-seperator">
      <div class="grid guter-change">     
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Product Name</label>
                      <common-dropdown placeholder="Please Select" 
                      :v$="v$" name="ProductId" v-model="ProductId" 
                      :isWatch="false" 
                      :options="ProductListDropDown"
                      optionLabel="Name"
                      optionValue="Id"
                      @update:title="ProductName = $event"
                      :callBack="bindPrice" />
                  </div>
              </div>  
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full">Quantity</label>
                      <common-input placeholder="Quantity"                                  
                                  IsNumber="true"
                                  :v$="v$"
                                  v-model="Quantity"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-4">
                  <div class="field">
                      <label class="block w-full">Remarks</label>
                      <common-input placeholder="Remarks"
                                  name="Remarks"
                                  v-model="Remarks"></common-input>
                  </div>
              </div>
              <div class="col-12 md:col-2">
                  <div class="field">
                      <label class="block w-full mb-3"></label>
                      <Button label="Add" class="p-button-aux" icon="pi pi-plus-circle" @click="AddItemList" />
                  </div>
              </div>
      </div>
      <div class="common__Table custom-datatablewithoutwidth-wrapper mb-3">
      <div class="admin-actions-btn-wrapper mt-0 mb-0">
          <div class="left-part">
              <comnet-button class="p-button-aux w-auto" :label="'Delete'" v-if="ItemselectedProducts.length > 0" @click="Removeconfirm(TIselectedProducts)" />
          </div>
      </div>
      <DataTable ref="dt" v-model:selection="ItemselectedProducts" :value="obj.OrderDetails" :scrollable="true" :paginator="true" :rows="10"
                 paginatorTemplate="RowsPerPageDropdown CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink"
                 :rowsPerPageOptions="[10,15,20]"
                 currentPageReportTemplate="Showing {first} to {last} of {totalRecords}" showGridlines>
          <Column selectionMode="multiple" class="check-box-center" frozen alignFrozen="left"></Column>
          <Column header="Action" class="text-center tb-width-60" frozen alignFrozen="left">
              <template #body="slotProps">

                  <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>

              </template>
          </Column>            
          <Column field="ProductName" header="Product" :sortable="true"></Column>
          <Column field="Quantity" header="Quantity" :sortable="true"></Column>
          <Column field="ProductPrice" header="Price" :sortable="true"></Column>
          <Column field="TotalAmount" header="Total" :sortable="true"></Column>
          <Column field="Remarks" header="Remarks" :sortable="true"></Column>
          <template #empty>
              <div class="no-data text-center">
                  <img src="@/assets/images/no-items.png" alt="No Data Found" />
                  <h4>No Record Found</h4>
              </div>
          </template>
      </DataTable>
      <div class="col-12 md:col-6 box3 bottom-seperator text-left">
        <div>
            <label class="block w-full f-16">SubTotal: {{obj.SubTotalAmount}} </label>
        </div>
    <div>
        <label class="block w-full f-16"><div>
                      
                      <InputText 
                             placeholder="Discount Amount(Flate) i.e 50"
                            :id="'DiscountAmount'" 
                            :name="'DiscountAmount'"     
                             v-model="obj.DiscountAmount" 
                             type="type" 
                             v-on:keypress="calculateTotalAmt"
                            v-on:keyup="calculateTotalAmt" 
                            v-on:paste="calculateTotalAmt"
                            :disabled="obj.Id>0"
                             ref="InputValue" autofocus />
                  </div></label>
    </div>
<div>
    <label class="block w-full f-16"> Total Amount : {{obj.TotalAmount}} </label>
    
      </div>
    </div>
     </div>
     
  </div>
      <div class="w-full p-dialog-footer text-right pt-3">
          <div></div>
          <div>
              <Button label="Cancel" class="p-button-aux mr-3" icon="pi pi-times-circle" @click="$router('OrderList')" />
              <Button :label="(obj.Id>0?'Update':'Create')" icon="pi pi-plus-circle" type="submit" @click="submit" autofocus />
          </div>
      </div>
  
  </div>
  
</template>
<script setup lang="ts">
import { computed, inject, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import ordersModelDTO, { ordersModel, OrderDetailsModel } from "@/models/orders/ordersModel";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
import { useToast } from "primevue/usetoast";
import { useConfirm } from "primevue/useconfirm";
import router from "@/router";
import moment from "moment";
const { GetData, PostData } = new commonModule();
const minDate = ref(new Date());
const Id = ref(0);
const obj = ref<ordersModel>(new ordersModel());
obj.value.SubTotalAmount = 0;
const objDetails = ref<OrderDetailsModel>(new OrderDetailsModel());    
const v$ = ref();
const frm = ref();
const ItemselectedProducts =  ref([]);
obj.value.OrderDetails = [];
const ProductId = ref<number | null>(null);
const ProductName = ref("")
const Quantity = ref<number | null>(null);
const Price = ref<number | null>(null);
const Remarks = ref("")
type ProductListItem = { Id: number; Name: string };
const ProductListDropDown = ref<ProductListItem[]>([]);
const ProductList = ref([]);
const confirm = useConfirm();
const toast = useToast();
const isEdit = ref(false);
const orderPendingDropDown = ref();
const selectedOrder = ref();
const filteredCountries = ref();

onMounted(async() => {
  await GetData(UrlConstants.apiProductsMasterUpdateGetOrder, (data: any) => {
      if(data.length > 0){
          ProductList.value = data;
          data.forEach((element: any) => {
              ProductListDropDown.value.push({
                  Id: parseInt(element.Id),
                  Name: String(element.Name)                    
              })
          });
      } 
      
      //obj.value.RoleId = data.RoleId.toString();        
  },null);
  await GetData(UrlConstants.apidropdown+"?Mode=PedingOrderNo", async (data:any) => {
            if(data.length > 0){
                orderPendingDropDown.value = data;
            }
  },null)
 
});

const search = (event:any) => {
    setTimeout(() => {
        if (!event.query.trim().length) {
            filteredCountries.value = [...orderPendingDropDown.value.map( c => c.Value)];
        } else {
            filteredCountries.value = orderPendingDropDown.value.filter((data:any) => {                
                return data.Text.toLowerCase().startsWith(event.query.toLowerCase());
            });
        }
    }, 250);
}
const bindPrice = async(selectedId : any) => {    
  var selectedItem = ProductList.value.find((item:any) => item.Id === selectedId);
  if (selectedItem) {
       Price.value =  selectedItem.Price
  }
}

const SearchOrder = async() => {    
    var Id = selectedOrder.value.Value;   
    if(Id==undefined) return false;
    GetData(UrlConstants.apiOrderMaster + Id, async (data: ordersModel) => {
    obj.value = data;
  },null);
}
const clearEdit = async() => {    
    obj.value = new ordersModel();
    selectedOrder.value = null;
}

const gridMenuItems = ref([
{
  icon: "pi pi-pencil",
      label: "Edit",
      callback: (data: any) => {
          let objEdit = obj.value.OrderDetails.find(x => x.ProductId == data.ProductId);
              ProductId.value = objEdit.ProductId
              Quantity.value = objEdit.Quantity
              Price.value = objEdit.ProductPrice
              ProductName.value = objEdit.ProductName
              Remarks.value = objEdit.Remarks
              isEdit.value = true;
      },
  },
  {
      icon: "pi pi-trash",
      label: "Delete",
      callback: async (data: any) => {
          confirm.require({
              message: 'Are you sure you want to proceed?',
              header: 'Confirmations',
              icon: 'pi pi-exclamation-triangle',
              accept: async() => {
                  let dataIndex = obj.value.OrderDetails.findIndex(x => x.Id == data.Id);
                  obj.value.OrderDetails.splice(dataIndex, 1);
                  await calculateTotalAmt();  
                  if(obj.value.OrderDetails != undefined){
                      UpdateSrNoToItems();
                  }
                  ItemselectedProducts.value = [];
                  
                  toast.add({ severity: 'success', summary: 'Success', detail: 'Item removed from List.', life: 3000 });
              }
          });
      },
      visible:true,
  }
]);
const AddItemList = async() => {    
   var serilNo = (ItemselectedProducts.value.length + 1) 
   var IscheckOrder = obj.value.OrderDetails.find((x:any) => x.ProductId == ProductId.value);
   if(IscheckOrder != undefined && !isEdit.value){        
      toast.add({ severity: 'error', summary: 'Error', detail: 'Already this product in list', life: 3000 });
   }
   else{
      if(isEdit.value){
          isEdit.value = false;
          var dataIndex = obj.value.OrderDetails.findIndex((x:any) => x.ProductId == ProductId.value);
          obj.value.OrderDetails[dataIndex]['ProductId'] = ProductId.value
          obj.value.OrderDetails[dataIndex]['Quantity'] = Quantity.value
          obj.value.OrderDetails[dataIndex]['ProductName'] = ProductName.value
          obj.value.OrderDetails[dataIndex]['Remarks'] = Remarks.value
      }
      else{
          if(ProductId?.value == null || Quantity?.value == null || ProductId?.value <= 0 || Quantity?.value <= 0)
            return false;
          obj.value.OrderDetails.push({
          SrNo : serilNo,
          Id : (objDetails.value.Id != 0)? objDetails.value.Id : 0,
          OrderId : (obj.value.Id != 0)? obj.value.Id : 0,
          ProductName : ProductName.value,
          ProductId : ProductId.value,
          Quantity : Quantity.value,
          ProductPrice : Price.value,
          TotalAmount : Quantity.value * Price.value,
          Remarks : Remarks.value
        })
      }
      await calculateTotalAmt();      
   }
   
  ProductId.value = null;
  ProductName.value = "";
  Quantity.value = null;
  Price.value = null;
  Remarks.value = "";
}

const UpdateSrNoToItems = () => {
  for (let i = 0; i < obj.value.OrderDetails.length; i++)
      obj.value.OrderDetails[i].SrNo = i + 1;
}

const calculateTotalAmt = async() => {
    obj.value.SubTotalAmount = obj.value.OrderDetails.reduce((sum, item) => sum + item.TotalAmount, 0);
      obj.value.TotalAmount = obj.value.OrderDetails.reduce((sum, item) => sum + item.TotalAmount, 0);
  obj.value.TotalAmount = (obj.value.SubTotalAmount??0) - (obj.value.DiscountAmount??0)
}

const submit = async() => {
  //
  if (obj.value.DateofDelivery) obj.value.DateofDelivery = moment(obj.value.DateofDelivery, "MM/DD/YYYY").utcOffset(0, true).format();
  if (obj.value.DateOfFunction) obj.value.DateOfFunction = moment(obj.value.DateOfFunction, "MM/DD/YYYY").utcOffset(0, true).format();
  const result = ref();
  if(obj.value.DiscountAmount == undefined || obj.value?.DiscountAmount == "")
    {obj.value.DiscountAmount = null;}
  v$.value = useVuelidate(ordersModelDTO.rules, obj.value);
  result.value = await v$.value.value.$validate();
  if(result.value){
    //   obj.value.DateOfFunction = moment(obj.value.DateOfFunction).format("YYYY/MM/DD")
    //   obj.value.DateofDelivery = moment(obj.value.DateofDelivery).format("YYYY/MM/DD")
      PostData( UrlConstants.apiOrderMaster,obj.value, "Order successfully added.", (data: any) => {
        router.push({ name: "OrderList" });
    },null);
  }
}
</script>