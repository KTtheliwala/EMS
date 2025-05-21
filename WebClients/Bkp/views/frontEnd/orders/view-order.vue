<template>
  <div class="content-wrap">
    <div class="detail-wrap">
      <div class="grid guter-change mt-5">
        
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="OrderNo">Order No</label>
            <common-input placeholder="" name="OrderNo" v-model="Data.OrderNo" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="pageName3">Company Name</label>
            <common-input placeholder="" name="pageName3" v-model="Data.CompanyName"
              :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="ContactName">Contact Name</label>
            <common-input placeholder="" name="ContactName" v-model="Data.ContactName" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="action3">Contact Number</label>            
            <common-input placeholder="" name="ContactNumber" v-model="Data.ContactNumber" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="ContactEmail">Contact Email</label>
            <common-input placeholder="" name="ContactEmail" v-model="Data.ContactEmail" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="DeliveryAddress">Address Line 1</label>
            <common-input placeholder="" name="DeliveryAddress" v-model="Data.DeliveryAddress" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="DeliveryAddress">Address Line 2</label>
            <common-input placeholder="" name="DeliveryAddress2" v-model="Data.DeliveryAddress2" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="City">City</label>
            <common-input placeholder="" name="City" v-model="Data.City" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="State">State</label>
            <common-input placeholder="" name="State" v-model="Data.State" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="PostalCode">Postal Code</label>
            <common-input placeholder="" name="PostCode" v-model="Data.PostCode" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="CountryName">Country</label>
            <common-input placeholder="" name="CountryName" v-model="Data.CountryName" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="DateofDelivery">Date of delivery</label>
            <common-input placeholder="" name="DateofDelivery" v-model="Data.DateofDelivery" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="DateOfFunction">Date of function</label>
            <common-input placeholder="" name="DateOfFunction" v-model="Data.DateOfFunction" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="OrderStatusName">Status</label>
            <common-input placeholder="" name="OrderStatusName" v-model="Data.OrderStatusName" :disabled="true"></common-input>
          </div>
        </div>       
        <div class="col-12 md:col-12">
          
    <div class="orderdetail-table-wrapper mb-0">
                    <table class="Role-table ">
                        <thead>
                            <tr>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th>Product Per Price</th>
                                <th>Total</th>
                                <th>Remarks</th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="item of Data?.OrderDetails" :key="item">
                                <tr role="row" class="b-white">
                                    <td class="">{{ item.ProductName }}({{item.ProductCode}})</td>
                                    <td class="">{{ item.Quantity }}</td>
                                    <td class="">{{ item.ProductPrice }}</td>
                                    <td class="">{{ item.TotalAmount }}</td>
                                    <td class="">{{ item.Remarks }}</td>
                                </tr>
                            </template>
                        </tbody>
                    </table>
                </div>

        </div>
        <div class="col-12 md:col-11 pb-0 text-right">
          <div class="field">
            <label class="block w-full mt-3" for="OrderStatusName"><h4>Subtotal: {{Data.SubTotalAmount}}</h4></label>           
          </div>
          <div class="m-0 p-0" v-if="Data.DiscountAmount > 0">
            <label class="block w-full" for="OrderStatusName"><h4>Discount(Flate): {{Data.DiscountAmount}}</h4></label>           
          </div>
          <div class="">
            <label class="block w-full" for="OrderStatusName"><h4>Total: {{Data.TotalAmount}}</h4></label>           
          </div>
        </div>          
      </div>
    </div>
  </div>
  <div class="p-dialog-footer text-right">
    <div></div>
    <div>
      <common-button btntype="button" :class="'p-button-aux'" label="Cancel"
        @click="CloseModal"></common-button>
    </div>
  </div>
</template>
<script setup lang="ts">
import { inject, ref } from "vue";
import { useRoute } from 'vue-router';
import router from "@/router";
const route = useRoute();
import VueJsonPretty from "vue-json-pretty";
import "vue-json-pretty/lib/styles.css";
import commonModule from "@/composables/modules/commonModule";
import ordersModelDTO, { ordersModel, OrderDetailsModel } from "@/models/orders/ordersModel";
import UrlConstants from "@/utils/urlconstants";
const CloseModal = () => {
   router.push({ name: "OrderList" });
};

const { GetData } = new commonModule();
const { DecryptData, ConvertToUserTimeZone,checkJosnIsValid,checkXMLIsValid } = inject('GlobalFunctions') as any;


const Id = ref(DecryptData(route.query?.Id ?? ""));
const Data = ref<ordersModel>(new ordersModel());

if (Id.value > 0) {
  GetData(UrlConstants.apiOrderMaster + Id.value, async (data: ordersModel) => {
    Data.value = data;
  }, "");
}

</script>
