<template>
  <div class="content-wrap">
    <div class="detail-wrap">
      <div class="grid guter-change">
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="pageName3">Page Name</label>
            <common-input placeholder="" name="pageName3" v-model="Data.Entity"
              :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="logType3">Log Type</label>
            <common-input placeholder="" name="logType3" v-model="Data.LogType" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="action3">Action</label>
            <common-input placeholder="" name="action3" v-model="Data.Action" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="entityId3">Entity Id</label>
            <common-input placeholder="" name="entityId3" v-model="Data.EntityId" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="user3">User</label>
            <common-input placeholder="" name="user3" v-model="Data.Username" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-4 pb-0">
          <div class="field">
            <label class="block w-full" for="date3">Date</label>
            <common-input placeholder="" name="date3" v-model="Data.TimeStamp" :disabled="true"></common-input>
          </div>
        </div>
        <div class="col-12 md:col-12">
          <div class="field">
            <label class="block w-full" for="message3">Message</label>
            <div class="p-inputtext-div disabled-div">              
              <div v-if="checkJosnIsValid(Data.Message)">
                      <vue-json-pretty :deep="1" :showIcon="true" :data="JSON.parse(Data.Message)"></vue-json-pretty>
                  </div>
                  <div class="text-wrap" v-html="Data.Message" v-else></div>
            </div>
          </div>
        </div>
        <div class="col-12 md:col-12">
          <div class="field">
            <label class="block w-full" for="properties3">Properties</label>
            <div class="p-inputtext-div disabled-div">              
              <div v-if="checkXMLIsValid(Data.Properties)" class="properties-body">
                <ul>
                  <li v-for="(value, key) in parseXML(Data.Properties)" :key="key">
                    <strong>{{ key }}:</strong> {{ value }}
                  </li>
                </ul>
                </div>
                <div class="text-wrap" v-html="Data.Properties" v-else></div>             
            </div>
          </div>
        </div>
        <div class="col-12 md:col-12">
          <div class="field">
            <label class="block w-full" for="exception3">Exception</label>
            <div class="p-inputtext-div disabled-div" v-if="Data.Exception != null">              
              <div v-if="checkJosnIsValid(Data.Exception)">
                      <vue-json-pretty :deep="0" :showIcon="true" :data="JSON.parse(Data.Exception)"></vue-json-pretty>
                  </div>
                  <div class="text-wrap exception-body" v-html="formatErrorMessage(Data.Exception)" v-else></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="p-dialog-footer">
    <div></div>
    <div>
      <common-button btntype="button" :class="'p-button-aux'" label="Cancel"
        @click="CloseModal"></common-button>
    </div>
  </div>
</template>
<script setup lang="ts">
import { inject, ref } from "vue";
import VueJsonPretty from "vue-json-pretty";
import "vue-json-pretty/lib/styles.css";
import commonModule from "@/composables/modules/commonModule";
import { AuditLogModel } from "@/models/auditLog/auditLogModel";
import UrlConstants from "@/utils/urlconstants";
const CloseModal = () => {
  dialogRef.value.close();
};

const { GetData } = new commonModule();
const { DecryptData, ConvertToUserTimeZone,checkJosnIsValid,checkXMLIsValid } = inject('GlobalFunctions') as any;
const dialogRef = inject("dialogRef") as any;
const Id = DecryptData(dialogRef.value.data ?? "");
const Data = ref<AuditLogModel>(new AuditLogModel());

if ((Id || "").length > 0) {
  GetData(UrlConstants.apiAuditLog + Id, async (data: AuditLogModel) => {
    Data.value = data;
    Data.value.TimeStamp = ConvertToUserTimeZone(Data.value.TimeStamp)
  }, "");
}
const parseXML = (xmlString:any)=> {
  const parser = new DOMParser();
  const xmlDoc = parser.parseFromString(xmlString, 'text/xml');
  const properties = xmlDoc.getElementsByTagName('property');
  const obj = {};

  for (let property of properties) {
    const key = property.getAttribute('key');
    const value = property.textContent;
    obj[key] = value;
  }
  return obj;
}

const formatErrorMessage = (errorMessage:any)=>  {
      const lines = errorMessage.split('\n');
      return lines.map((line, index) => `<p key=${index}>${line.trim()}</p>`).join('');
    }
</script>
