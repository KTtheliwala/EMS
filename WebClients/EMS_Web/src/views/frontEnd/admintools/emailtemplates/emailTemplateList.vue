<template>
    <common-grid :config="gridConfig" ref="dt" :filters="filters" filename="emailtemplate" :AdvSearchBtnClass="'advance-search-filter-btn'">
        <template v-slot:basicfilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">From Email</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">From Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Template Code</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Subject</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                </div>
            </div>
        </template>
        <template v-slot:advancefilter>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">From Email</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[0].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">From Name</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[1].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Template Code</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[2].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Subject</label>
                    <common-input inputclass="form-control" v-model="gridConfig.filters[3].fieldValue"></common-input>
                </div>
            </div>
            <div class="col-12 md:col-3 pb-0">
                <div class="field">
                    <label class="block w-full">Active</label>
                    <common-dropdown fieldlabel="Active"
                                     placeholder="Please Select"
                                     v-model="gridConfig.filters[4].fieldValue"
                                     :options="StatusDropOption"
                                     @update:title="gridConfig.filters[4].fieldDisplayValue=$event" />
                </div>
            </div>            
        </template>
        <template v-slot:right-buttons>
            <!-- <Button icon="pi pi-plus-circle" label="Create Email Template" @click="OpenAddNewManage"></Button> -->
        </template>
        <template v-slot:columns>
            <Column header="Action" v-if="(Page?.IsEdit ?? false )"  class="action-dot-button" frozen alignFrozen="left">
                <template #body="slotProps">
                    <div>
                        <common-action-button :menuItems="gridMenuItems" :data="slotProps.data"></common-action-button>
                    </div>
                </template>
            </Column>
            <Column field="FromEmail" header="From Email" :sortable="true"></Column>
            <Column field="FromName" header="From Name" :sortable="true"></Column>
            <Column field="TemplateCode" header="Template Code" :sortable="true"></Column>
            <Column field="TemplateSubject" header="Subject" :sortable="true"></Column>
            <Column field="TemplateBody" header="Body" >
                <template #body="column">
                    <Button icon="pi pi-eye" label="Preview" @click="OpenPreview(column.data.TemplateBody)"></Button>
                </template>
            </Column>
            <Column header="Active" :sortable="true" field="IsActive" class="active-col">
                <template #body="columns">
                    <InputSwitch  v-model="columns.data.IsActive" :disabled="!(Page?.IsEdit ?? false ) ? true : false" @change="updateIsActive(columns.data.Id)" />
                </template>
            </Column>
        </template>
    </common-grid>
    <common-dialog ref="popupManage" class="large-modal"></common-dialog>
    <Dialog class="large-modal" :header="'Preview Template Body' " v-model:visible="IsPreview" :modal="true">
        <div class="preview" v-html="PreviewTemplateBody"></div>
    </Dialog>
</template>
<script setup lang="ts">
    import GridConfig from "@/models/controls/Grid/gridConfig";
    import SearchOperations from "@/models/controls/Grid/searchOperations";
    import UrlConstants from "@/utils/urlconstants";
    import { ref } from "vue";
    import emailTemplateManage from '@/views/frontEnd/admintools/emailtemplates/emailTemplateManage.vue'
    import commonModule from "@/composables/modules/commonModule";
    import { AuthService } from "@/composables/api/authService";
    const Page = AuthService.GetPagePermission('AdminTools','EmailTemplate')
    const { PatchData } = new commonModule();
    const dt = ref(null);
    const popupManage = ref();
    const PreviewTemplateBody = ref();
    const IsPreview = ref(false);
    const OpenAddNewManage = () => {
        popupManage.value.OpenModal(emailTemplateManage, "Create Email Template", 0, dt.value.reloadGrid);
    }
    const updateIsActive = (data: any) => {
        PatchData(
            UrlConstants.apiemailtemplateupdatestatus,
            data,
            "Email template Status Updated successfully.",
            null,
            null
        );
    }

    const updateNotiIsActive = (data: any) => {
        PatchData(
            UrlConstants.apiemailtemplatenotiupdatestatus,
            data,
            "Email template Status Updated successfully.",
            null,
            null
        );
    }

    const gridConfig = ref({
        api: UrlConstants.apiemailtemplatelist,
        deleteApi: UrlConstants.apiemailtemplate,
        filters: [
            { searchType: "Filter", fieldName: "FromEmail", fieldValue: "", fieldDisplayName: "From Email", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "FromName", fieldValue: "", fieldDisplayName: "From Name", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "TemplateCode", fieldValue: "", fieldDisplayName: "Template Code", fieldDisplayValue: "", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "TemplateSubject", fieldValue: "", fieldDisplayName: "Subject", fieldDisplayValue: "", opType: SearchOperations.contains },
            { searchType: "Filter", fieldName: "IsActive", fieldDisplayName: "Active?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "" },
            { searchType: "Filter", fieldName: "IsNotiActive", fieldDisplayName: "Notification?", fieldValue: null, opType: SearchOperations.equals, fieldDisplayValue: "" }
        ],
        sortcolumn: "Id",
        sortorder: 1,
        doubleClickHander: null
    } as GridConfig);

    const gridMenuItems = ref([
        {
            icon: "pi pi-pencil",
            label: "Edit",
            callback: (data: any) => {
                popupManage.value.OpenModal(emailTemplateManage, "Manage Email Template", data.Id, dt.value.reloadGrid);
            },
            visible:(Page?.IsEdit ?? false )
        }
    ]);
    const StatusDropOption = ref([
        { Text: "Active", Value: "true" },
        { Text: "Inactive", Value: "false" },
    ]);

    const OpenPreview = (data:any) => {
        IsPreview.value = true;
        PreviewTemplateBody.value = data;
    }
</script>