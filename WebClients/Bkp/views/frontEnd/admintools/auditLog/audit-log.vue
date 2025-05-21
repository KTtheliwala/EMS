<template>
  <div class="content-wrap ">
    <div class="page-title">
      <h2>System Audit Log</h2>
    </div>
    <common-grid selectionMode="single" :config="gridConfig" ref="dt" :IsShowExport="false" :filters="filters"
      :DeleteToAllBtn="false">
      <template v-slot:basicfilter>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="entity">Entity</label>
            <common-dropdown name="entity" placeholder="All" v-model="gridConfig.filters[0].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogEntity'"
              @update:title="gridConfig.filters[0].fieldDisplayValue = $event" :datatype="'String'" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="action">Action</label>
            <common-dropdown name="action" placeholder="All" v-model="gridConfig.filters[1].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogAction'"
              @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="dateRange">Date(Range)</label>
            <Calendar v-model="gridConfig.filters[2].fieldValue" dateFormat="dd M yy" :showButtonBar="true"
              selectionMode="range" :selectOtherMonths="true" :showIcon="true" class="w-full" inputId="dateRange"></Calendar>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="logSource">Log Source</label>
            <common-dropdown name="logSource" placeholder="All" v-model="gridConfig.filters[3].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogSource'"
              @update:title="gridConfig.filters[3].fieldDisplayValue = $event" />
          </div>
        </div>
      </template>
      <template v-slot:advancefilter>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="entity2">Entity</label>
            <common-dropdown name="entity2" placeholder="All" v-model="gridConfig.filters[0].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogEntity'"
              @update:title="gridConfig.filters[1].fieldDisplayValue = $event" :datatype="'String'" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="action2">Action</label>
            <common-dropdown name="action2" placeholder="All" v-model="gridConfig.filters[1].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogAction'"
              @update:title="gridConfig.filters[1].fieldDisplayValue = $event" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="actionBy2">Action By</label>
            <common-dropdown name="actionBy2" placeholder="All" v-model="gridConfig.filters[7].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=AllUser'"
              @update:title="gridConfig.filters[7].fieldDisplayValue = $event" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="dateRange2">Date(Range)</label>
            <Calendar v-model="gridConfig.filters[2].fieldValue" dateFormat="dd M yy" :showButtonBar="true"
              selectionMode="range" :selectOtherMonths="true" :showIcon="true" class="w-full" inputId="dateRange2"></Calendar>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="keyword2">Keyword (details)</label>
            <common-input inputclass="form-control" v-model="gridConfig.filters[5].fieldValue" name="keyword2"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="logSource2">Log Source</label>
            <common-dropdown name="logSource2" placeholder="All" v-model="gridConfig.filters[3].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogSource'"
              @update:title="gridConfig.filters[3].fieldDisplayValue = $event" />
          </div>
        </div>
        <div class="col-12 md:col-3 pb-0">
          <div class="field">
            <label class="block w-full" for="logType2">Log Type</label>
            <common-dropdown name="logType2" placeholder="All" v-model="gridConfig.filters[4].fieldValue"
              :dataurl="UrlConstants.apidropdown + '?Mode=LogType'"
              @update:title="gridConfig.filters[4].fieldDisplayValue = $event" />
          </div>
        </div>
      </template>

      <template v-slot:columns>
        <Column field="Entity" header="Entity" :sortable="true">
          <template #body="columns">
            <span @click="OpenAuditDetailsPopupModel(columns.data)" class="cursor-pointer underline">
              {{ columns.data.Entity }}
            </span>
          </template>
        </Column>
        <Column field="LogTypeId" header="Log Type" :sortable="true">
          <template #body="columns">

            {{ columns.data.LogType }}

          </template>
        </Column>
        <Column field="LogSourceId" header="Log Source" :sortable="true">
          <template #body="columns">

            {{ columns.data.LogSource }}

          </template>
        </Column>
        <Column field="ActionId" header="Action" :sortable="true" class="width-150">
          <template #body="columns">
            {{ columns.data.Action }}
          </template>
        </Column>
        <Column field="Message" header="Message" :sortable="true">
          <template #body="columns">
            <div v-if="checkJosnIsValid(columns.data.Message)">
              <vue-json-pretty :deep="0" :showIcon="true" :data="JSON.parse(columns.data.Message)"></vue-json-pretty>
            </div>
            <div class="text-wrap" v-else v-dompurify-html="columns.data?.Properties ? columns.data.Properties.slice(0, 150) : ''">
            </div>
          </template>
        </Column>
        <Column field="Properties" header="Properties" :sortable="true">
          <template #body="columns">
            <div v-if="checkJosnIsValid(columns.data.Properties)">
              <vue-json-pretty :deep="0" :showIcon="true" :data="JSON.parse(columns.data.Properties)"></vue-json-pretty>
            </div>
            <div class="text-wrap"  v-else v-dompurify-html="columns.data?.Properties ? columns.data.Properties.slice(0, 150) : ''">
            </div>
          </template>
        </Column>
        <Column field="Username" header="User" :sortable="true">
          <template #body="columns">

            {{ columns.data.Username }}

          </template>
        </Column>
        <Column field="TimeStamp" header="Date" :sortable="true" class="width-150 text-center">
          <template #body="columns">
            {{ ConvertToUserTimeZone(columns.data.TimeStamp, "DD MMM YYYY hh:mm A") }}
          </template>
        </Column>
        <Column field="Action" header="Action" class="table-action-2" frozen alignFrozen="right">
          <template #body="slotProps">
            <div class="action-btns">
              <common-button btntype="button" :icon="'pi pi-eye'" :class="'p-button-aux'" v-tooltip.top="'View'"
                @click="OpenAddEditManage(slotProps.data.Id)"></common-button>
            </div>
          </template>
        </Column>
      </template>
    </common-grid>
  </div>

  <common-dialog ref="popupManage" class="large-modal"></common-dialog>
</template>
<script setup lang="ts">
import { useRoute } from 'vue-router';
import router from '@/router';
import GridConfig from "@/models/controls/Grid/gridConfig";
import SearchOperations from "@/models/controls/Grid/searchOperations";
import UrlConstants from "@/utils/urlconstants";
import systemAuditLogManage from "@/views/frontEnd/admintools/auditLog/audit-log-manage.vue";
import { inject, ref } from "vue";

const { EncryptData, ConvertToUserTimeZone } = inject('GlobalFunctions');
const dt = ref(null);
const gridConfig = ref({
  api: UrlConstants.apiAuditLogList,
  deleteApi: UrlConstants.apiAuditLog,
  filters: [
    {
      searchType: "Filter",
      fieldName: "EntityName",
      fieldValue: null,
      fieldDisplayName: "Entity",
      opType: SearchOperations.equals,
    },
    {
      searchType: "Filter",
      fieldName: "ActionId",
      fieldValue: null,
      fieldDisplayName: "Action",
      opType: SearchOperations.equals,
    },
    {
      searchType: "Filter",
      fieldName: "TimeStamp",
      fieldValue: null,
      fieldDisplayName: "Date",
      opType: SearchOperations.dateRange,
    },
    {
      searchType: "Filter",
      fieldName: "LogSourceId",
      fieldDisplayName: "Log Source",
      fieldValue: null,
      opType: SearchOperations.equals,
    },
    {
      searchType: "Filter",
      fieldName: "LogTypeId",
      fieldDisplayName: "Log Type",
      fieldValue: null,
      opType: SearchOperations.equals,
    },
    {
      searchType: "Filter",
      fieldName: "Message",
      fieldValue: "",
      fieldDisplayName: "Keyword (details)",
      opType: SearchOperations.contains,
    },
    {
      searchType: "Filter",
      fieldName: "UserId",
      fieldDisplayName: "User",
      fieldValue: null,
      opType: SearchOperations.equals,
    },
    {
      searchType: "Filter",
      fieldName: "ActionById",
      fieldValue: null,
      fieldDisplayName: "User",
      opType: SearchOperations.equals,
    },
  ],
  sortcolumn: "TimeStamp",
  sortorder: -1,
  doubleClickHander: null,
} as GridConfig);
gridConfig.value.filters[2].fieldValue = [new Date(), new Date()]
const popupManage = ref();
const OpenAddEditManage = (data: any) => {
  popupManage.value.OpenModal(systemAuditLogManage, 'System Audit Log', EncryptData(data.toString()), dt.value.reloadGrid);
}
const checkJosnIsValid = (jsn: any): boolean => {
  try {
    JSON.parse(jsn);
  } catch (e) {
    return false;
  }
  return true;
}
</script>
