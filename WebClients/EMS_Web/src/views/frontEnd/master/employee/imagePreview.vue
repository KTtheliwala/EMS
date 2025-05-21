<template>
    <div class="employee-manage-body" >
        <div class="grid guter-change">
            <img :src="Id" v-if="Id != null"
                  class="w-full"  alt="Icon"  />
        </div>
      
        </div>  
            <div class="p-dialog-footer">
                <div></div>
                <div>
                    <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
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
          //
    });
    const CloseModal = async() => {
    dialogRef.value.close();
     }
</script>
