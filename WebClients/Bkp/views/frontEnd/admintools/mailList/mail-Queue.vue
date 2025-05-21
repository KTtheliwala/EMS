<template>
    <div class="content-wrap">
        <div class="page-title">
            <h2>{{ $route.name }}</h2>
        </div>
        <common-grid :config="gridConfig" ref="dt" selectionMode="single" :filters="gridConfig.filters" filename="MailQueue" >
            <template v-slot:basicfilter>
                <div class="col-12 md:col-3 pb-0 ">
                    <div class="field">
                        <label class="block w-full" for="fromEmail">From Email</label>
                        <common-input name="fromEmail" inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="fromName">From Name</label>
                        <common-input name="fromName" inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="toEmail">To Email</label>
                        <common-input name="toEmail" inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="mailSubject">MailSubject</label>
                        <common-input name="mailSubject" inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                    </div>
                </div>
            </template>
            <template v-slot:advancefilter>
                <div class="col-12 md:col-3 pb-0 ">
                    <div class="field">
                        <label class="block w-full" for="fromEmail2">From Email</label>
                        <common-input name="fromEmail2" inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="fromName2">From Name</label>
                        <common-input name="fromName2" inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="toEmail2">To Email</label>
                        <common-input name="toEmail2" inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="mailSubject2">MailSubject</label>
                        <common-input name="mailSubject2" inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="createdDate">Created Date (Range)</label>
                        <Calendar v-model="gridConfig.filters[4].fieldValue" dateFormat="dd/mm/yy" selectionMode="range" :selectOtherMonths="true" :showButtonBar="true" :showIcon="true" :showTime="false" :timeOnly="false" class="w-full" inputId="createdDate"></Calendar>
                    </div>
                </div>
                <div class="col-12 md:col-3 pb-0">
                    <div class="field">
                        <label class="block w-full" for="Status">Send Status</label>
                        <common-dropdown placeholder="Please Select" :name="'Status'" v-model="gridConfig.filters[5].fieldValue" @update:title="gridConfig.filters[5].fieldDisplayValue=$event" :options="StatusDropOption"  />
                    </div>
                </div>
            </template>
            <template v-slot:columns>
                <Column field="CreatedDate" header="Created Date" class="width-180" :sortable="true">
                    <template #body="columns">
                        <span @click="OpenAddEditManage(columns.data)" class="cursor-pointer underline">
                            {{ ConvertToUserTimeZone(columns.data.CreatedDate) }}
                        </span>
                    </template>
                </Column>
                <Column field="FromEmail" header="From Mail" class="width-210" :sortable="true">
                    <template #body="columns">
                        {{ columns.data.FromEmail }}
                    </template>
                </Column>
                <Column field="FromName" header="From Name" class="width-210" :sortable="true">
                    <template #body="columns">
                        {{ columns.data.FromName }}
                    </template>
                </Column>
                <Column field="ToEmail" header="To Email" class="width-300" :sortable="true">
                    <template #body="columns">
                        <span class="overflow-ellipsis">
                            {{ columns.data.ToEmail }}
                        </span>
                    </template>
                </Column>
                <Column field="MailSubject" header="Mail Subject" class="width-300" :sortable="true">
                    <template #body="columns">
                        <span class="overflow-ellipsis">
                            {{ columns.data.MailSubject }}
                        </span>
                    </template>
                </Column>
                <Column field="StatusName" header="Send Status" class="text-center width-100" :sortable="true">
                    <template #body="columns">
                        {{ columns.data.StatusName }}
                    </template>
                </Column>
                <Column field="Retry" header="Last Retry" :sortable="true" class="text-center">
                    <template #body="columns">
                        {{ columns.data.Retry }}
                    </template>
                </Column>
                <Column field="Action" header="Action" class="table-action-2" frozen alignFrozen="right">
                    <template #body="slotProps">
                        <div class="action-btns">
                            <common-button btntype="button" :icon="'pi pi-eye'" :class="'p-button-aux'"
                            @click="OpenAddEditManage(slotProps.data)"
                                v-tooltip.top="'View'"></common-button>
                        </div>
                    </template>
                </Column>
            </template>
        </common-grid>
    </div>
    <common-dialog ref="popupManage" class="large-modal"></common-dialog>
</template>
<script setup lang="ts">

import GridConfig from "@/models/controls/Grid/gridConfig";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import UrlConstants from "@/utils/urlconstants";
import { inject, ref } from "vue";
import commonModule from "@/composables/modules/commonModule";
import mailQueueManage from "@/views/frontEnd/admintools/mailList/mail-Queue-Manage.vue";
const { ConvertToUserTimeZone } = inject('GlobalFunctions') as any;
const { PatchData, GetData } = new commonModule();

const dt = ref(null);
const gridConfig = ref({
api: UrlConstants.apiMailQueueList,
deleteApi: "",
filters: [
    { searchType: "Filter", fieldName: "FromEmail", fieldValue: null, fieldDisplayName: "From Email", opType: SearchOperations.contains },
    { searchType: "Filter", fieldName: "FromName", fieldValue: null, fieldDisplayName: "From Name ", opType: SearchOperations.equals },
    { searchType: "Filter", fieldName: "ToEmail", fieldValue: null, fieldDisplayName: "To Email ", opType: SearchOperations.contains },
    { searchType: "Filter", fieldName: "MailSubject", fieldValue: null, fieldDisplayName: "Mail Subject ", opType: SearchOperations.contains },
    { searchType: "Filter", fieldName: "CreatedDate", fieldValue: null, fieldDisplayName: "Created Date ", opType: SearchOperations.dateRange },
    { searchType: "Filter", fieldName: "Status", fieldValue: null, fieldDisplayName: "Status", opType: SearchOperations.contains },
],
sortcolumn: "Id",
sortorder: -1,
doubleClickHander: null,
} as GridConfig);

const popupManage = ref();
const OpenAddEditManage = (data: any) => {
  popupManage.value.OpenModal(mailQueueManage, 'Email Details', data, dt.value.reloadGrid);
}

const StatusDropOption = ref([
    { Text: "Failed", Value: "-1" },
    { Text: "Success", Value: "-2   " },
]);



</script>
