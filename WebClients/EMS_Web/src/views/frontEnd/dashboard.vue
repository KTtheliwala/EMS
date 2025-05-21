<template>
  <div class="form-layout dashboard-wrapper">
    <div class="grid guter-change bottom-seperator">
       <h4 class="p-3">Welcome to dashboard</h4>
     </div>
    <div class="form-body dashboard-body p-0">
      
    </div>
  </div>
</template>

<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useVuelidate } from "@vuelidate/core";
import UrlConstants from "@/utils/urlconstants";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import orderStatus from "@/models/controls/Order/orderStatus";
import {dashboardSummary} from "@/models/dashboard/dashboardSummary";
import { helpers, minLength, required, requiredIf, sameAs } from "@vuelidate/validators";
import commonModule from "@/composables/modules/commonModule";
import GridConfig from "@/models/controls/Grid/gridConfig";
const { GetData, PostData } = new commonModule();
const { EncryptData,ConvertToUserTimeZone,GetDecimalTwoPlace } = inject('GlobalFunctions');

const dashboardSummaryList = ref([]);

const v$ = ref();


// const fetchData = async () => {
//   await GetData(
//     UrlConstants.apiDashboardMaster,
//     async (data: any) => {
//       dashboardSummaryList.value = data; // Assign the API data to the reactive variable
      
//     },
//     ""
//   );
// };
//fetchData();
onMounted(async() => {    
  //
});
const generateClassName = (name: string) => {
  return name.replace(/\s+/g, "");
};

</script>
