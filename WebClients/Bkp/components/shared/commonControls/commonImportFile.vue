<template>
    <div class="import-file-wrapper">
        <Stepper :activeStep="activeStep">
            <StepperPanel header="Upload">
                <template #content="{ nextCallback }">
                    <div class="flex flex-column">
                        <div class="import-file__inner">
                            <div class="grid guter-change">
                                <div class="col-12 md:col-6 pb-0 mx-auto" :class="IsShowMapping ? 'md:col-6' : 'md:col-12 file-upload-fullWidth'">
                                    <common-file :maxFileSize="100000000" chooseLabel="Browse File" accept=".xlsx" :class="'importFile'"
                                        v-model="excelFile" :AfterSelectFile="onSelectFile"></common-file>
                                </div>
                                <div class="col-12 md:col-6 pb-0" v-if="IsShowMapping">
                                    <div class="import-file-select-data">
                                        <DataTable :value="data.Mappings"
                                            v-if="data?.value?.importFrom !== 'ImportContainerModelMapping'">
                                            <Column field="serialNumber" header="No." :sortable="false"
                                                class="sr-box-center">
                                                <template #body="columns">
                                                    {{ columns.index + 1 }}
                                                </template>
                                            </Column>
                                            <Column field="Label" header="Column Name"></Column>
                                            <Column field="ColName" header="Excel Column Name">
                                                <template #body="columns">
                                                    <Dropdown v-model="columns.data.ColName" :options="ExcelCols"
                                                        placeholder="Select a Column"
                                                        :class="columns.data.ColName ? 'w-full' : 'w-full error'"
                                                        :filter="true" :showClear="true" :autoFilterFocus="true" />
                                                </template>
                                            </Column>
                                        </DataTable>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="p-dialog-footer">
                        <template v-if="importFrom == 'ImportModels'">
                            <Button label="Next" icon="pi pi-arrow-right" 
                            :disabled="(excelFile == null || (data.Mappings.length == 0 || data?.Mappings?.filter(x => (x?.ColName ?? '') == '').length > 0))" @click="ChangeStep(1)" />
                        </template>
                        <template v-else>
                            <Button label="Next" icon="pi pi-arrow-right"
                                :disabled="(excelFile == null || (data.Mappings.length == 0 || data?.Mappings?.filter(x => (x?.ColName ?? '') == '').length > 0))"
                                iconPos="right" @click="ValidateData" v-if="importFrom !== 'ImportContainerModelMapping'" />
                            <Button label="Next" icon="pi pi-arrow-right" :disabled="excelFile == null || IsShowMapping"
                                iconPos="right" @click="ValidateData" v-if="importFrom === 'ImportContainerModelMapping'" />
                        </template>
                    </div>
                </template>
            </StepperPanel>
            <StepperPanel header="Image(s) Upload" v-if="importFrom == 'ImportModels'">            
                <template #content="{ prevCallback, nextCallback }">
                    <div class="flex flex-column">
                        <div class="import-file__inner">
                            <div class="grid guter-change">
                                <div class="col-12 md:col-12 pb-0 mx-auto" :class="IsShowMapping ? 'md:col-6' : 'md:col-12 file-upload-fullWidth'">
                                    <comnetFileupload :maxFileSize="5242880" chooseLabel="Browse Files" accept=".jpg,.jpeg,.gif,.png" :class="'importFile'"
                                    :placeHolder="'Drag & drop images here to upload.'" :multiple="true" v-model="multifilesSelected" :AfterSelectFile="onMultifilesSelectFile"></comnetFileupload>                                    
                                </div>
                                <div class="col-12 md:col-12 pb-0" v-if="multifilesSelected != null && multifilesSelected?.length > 0">
                                    <P class="m-0">Total {{multifilesSelected?.length}} files selected.</P>
                                </div>
                            </div>
                        </div>
                        <p class="input-notes">Optimal size 298px*200px in .jpg/ .jpeg/ .gif/ .png (Max 5 mb file size only)</p>
                    </div>
                    <div class="p-dialog-footer">
                        <Button label="Back" class="p-button-aux" icon="pi pi-arrow-left" @click="ChangeStep(0)" />
                        <Button label="Next" icon="pi pi-arrow-right"
                                :disabled="(excelFile == null || (data.Mappings.length == 0 || data?.Mappings?.filter(x => (x?.ColName ?? '') == '').length > 0))"
                                iconPos="right" @click="ValidateData" v-if="importFrom !== 'ImportContainerModelMapping'" />
                    </div>
                </template>
            
            </StepperPanel>
            <StepperPanel header="Preview">
                <template #content="{ prevCallback, nextCallback }">
                    <div class="flex flex-column">
                        <div class="import-file__inner">
                            <DataTable class="p-datatable-customers importedDataPreviewTable" :value="previewData" :rowClass="rowClass"
                                v-if="importFrom !== 'ImportContainerModelMapping'">
                                <Column field="serialNumber" header="No." :sortable="false" class="sr-box-center">
                                    <template #body="columns">
                                        {{ columns.index + 1 }}
                                    </template>
                                </Column>
                                <!-- <Column :field="item.Label" :header="item.ColName"
                                    v-for="item of data.Mappings.filter(x => x.ColName != 'Photo')">

                                </Column> -->
                                 <template v-for="(value, index) in data.Mappings.filter(x => x.ColName != 'Photo')">
                                    <Column :field="value.Label" :header="value.ColName">
                                        <template #body="slotProps" v-if="value.ColName == 'Name'">
                                            <div v-html="slotProps.data.Name"></div>
                                        </template>
                                    </Column>
                                </template>
                                <Column field="Photo" header="Photo" v-if="importFrom == 'ImportModels'">
                                    <template #body="columns">
                                        <img :src="columns.data.objectURL"
                                            v-if="columns.data.objectURL != null" alt="Model" width="50" />
                                    </template>
                                </Column>
                                <Column field="Status" header="Status">
                                    <template #body="columns">                                        
                                        <span class="status-error" v-html="columns.data.Status || 'Ready to import'"></span>                                        
                                    </template>
                                </Column>
                                <template #empty>
                                    <div class="no-data">
                                        <img src="@/assets/images/no-items.png" alt="No data Found" />
                                        <h4>No Data Found</h4>
                                    </div>
                                </template>
                            </DataTable>
                            <DataTable :value="ContainerModel" :rowClass="rowClass" :paginator="true" 
                                v-else-if="importFrom === 'ImportContainerModelMapping'"
                                paginatorTemplate="RowsPerPageDropdown CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink"
                    currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
                    :rowsPerPageOptions="[100, 200, 300, 500]" :responsiveLayout="'scroll'"
                    :rows="100"
                                :virtualScrollerOptions="{ itemSize: 100 }" showGridlines tableStyle="min-width: 50rem">
                                <ColumnGroup type="header">
                                    <Row>
                                        <Column field="serialNumber" header="No." :sortable="false"
                                            class="sr-box-center text-center" :rowspan="3"></Column> -->
                                            <Column header="Market" :rowspan="3" />
                                        <Column header="Brand" :rowspan="3" />
                                        <Column header="Model Family Name" :rowspan="3" />
                                        <Column header="Model" :rowspan="3" />
                                        <Column header="Plant" :rowspan="3" />
                                    </Row>
                                    <Row>
                                        <Column v-for="item in packingTypes" :header="item.Text"
                                            :colspan="item.Length" />
                                        <Column header="Status" :rowspan="3" />
                                    </Row>
                                    <Row>
                                        <Column v-for="(value, index) in containerKeys" :field="value.Value"
                                            :header="value.Text"></Column>
                                    </Row>
                                </ColumnGroup>
                                <Column field="serialNumber" header="No." class="sr-box-center text-center">
                                    <template #body="columns">
                                        {{ columns.index + 1 }}
                                    </template>
                                </Column>
                                <Column field="Market" header="Market" />
                                <Column field="Brand" header="Brand" />
                                <Column field="ModelFamilyName" header="Model Family" />
                                <Column field="Model" header="Model" />
                                <Column field="Plant" header="Plant" />
                                <template v-for="(value, index) in containerKeys">
                                    <Column :field="value.Value" :header="value.Text" v-if="value != 'id'">
                                        <template #body="slotProps">
                                            <div v-if="value.Value == 'Brand'">
                                                {{ slotProps.data.Model }}
                                            </div>
                                            <div v-else-if="value.Value == 'Model'">
                                                {{ slotProps.data.Model }}
                                            </div>
                                            <div v-for="Data in slotProps.data.ContainerDetails" v-else>
                                                <div v-if="Data.ContainerTypeId == value.Value">
                                                    <comnet-input :inputclass="'w-80'" :IsNumber="true" :IsDecimal="false"
                                                        v-model='Data["ContainerValue"]'></comnet-input>
                                                </div>
                                            </div>
                                            <div v-if="value?.Value === 'Status'">
                                                 <p v-html="'<strong>This is bold text</strong> and a <br /> line break.'"></p>
  <div v-html="slotProps.data.Status || 'Ready to import'"></div>
</div>
                                        </template>
                                    </Column>
                                </template>
                                <Column field="Status" header="Status">
                                    <template #body="columns">
                                        {{ columns.data.Status }}
                                    </template>
                                </Column>
                                <template #empty>
                                    <div class="no-data">
                                        <img src="@/assets/images/no-items.png" alt="No Data Found" />
                                        <h4>No Data Found</h4>
                                    </div>
                                </template>
                            </DataTable>

                        </div>
                    </div>
                    <div class="p-dialog-footer">
                        <template v-if="importFrom !== 'ImportContainerModelMapping'">
                            <InlineMessage
                                :severity="previewData?.length === 0 ? 'info' : previewData.filter(x => !x.Status).length == previewData.length ? 'success' : 'info'">
                                {{ previewData?.length === 0 ? 'No data found' : previewData.filter(x => !x.Status).length== previewData.length ? 'All data will be import.' :'Total ' + previewData.filter(x => !x.Status).length + ' records will be import from '+previewData.length+' records.' }}</InlineMessage>
                        </template>
                        <template
                            v-else-if="importFrom === 'ImportContainerModelMapping' && ContainerModel?.length > 0">
                            <InlineMessage
                                :severity="ContainerModel.filter(x => x?.Status).length == 0 ? 'success' : 'info'">
                                {{ ContainerModel.filter(x => !x?.Status).length == 0 ? 'All data will be import.' : 'Total ' + ContainerModel.filter(x => !x?.Status).length + ' records will be import from '+ContainerModel?.length+' records.' }}</InlineMessage>
                        </template>
                        <Button label="Back" class="p-button-aux" icon="pi pi-arrow-left" @click="ChangeStep((importFrom == 'ImportModels'?1:0))" />
                        <Button label="Import Data" icon="pi pi-arrow-right" iconPos="right" @click="ImportData"
                            v-if="((importFrom !== 'ImportContainerModelMapping' && previewData?.length > 0) || (importFrom === 'ImportContainerModelMapping' && ContainerModel?.length > 0))" />
                    </div>
                </template>
            </StepperPanel>
            <StepperPanel header="Import">
                <template #content="{ prevCallback }">
                    <div class="flex flex-column">
                        <div class="import-file__inner">
                            <div class="import-file-loader-wrapper">
                                <div class="import-file-loader">
                                    <i class="pi pi-file-import"></i>
                                    <template v-if="importFrom == 'ImportModels'">
                                        
                                        <div class="col-12 md:col-12 pb-0 text-center">                                
                                            <p class="import-file-status" v-for="(item, index) in Msg?.split('.')?.filter(sentence => sentence.trim() !== '')" :key="index">{{ item.trim() }}.</p>
                                        </div>
                                        
                                    </template>
                                    <template v-else>
                                        <p class="import-file-status">{{ Msg }}</p>
                                    </template>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="p-dialog-footer">
                        <Button label="Back" class="p-button-aux" icon="pi pi-arrow-left" @click="ChangeStep((importFrom == 'ImportModels'?2:1))" />
                        <Button label="Finish" icon="pi pi-arrow-right" iconPos="right" @click="CloseModal" />
                    </div>
                </template>
            </StepperPanel>
        </Stepper>
    </div>
</template>

<script setup lang="ts">
import commonModule from "@/composables/modules/commonModule";
import UrlConstants from "@/utils/urlconstants";
import constants from "@/utils/constants";
import { inject, ref } from "vue";
import * as XLSX from 'xlsx';
const { PostFormData, PostData } = new commonModule();
const IsShowMapping = ref(false);
const dialogRef = inject('dialogRef') as any;
const data = ref(dialogRef.value.data);
const callBack=data.value.callBack;
const ExcelCols = ref([]);
const excelFile = ref();
const multifilesSelected = ref();
const multifilesSelectedFiles = ref();
const previewData = ref([]);
const activeStep = ref(0);
const Msg = ref("");
const CloseModal = () => {
    dialogRef.value.close();
}

const ContainerModel = ref([]) as any
const containerKeys = ref() as any;
const packingTypes = ref()
const callBackReturnData = ref();
const importFrom = ref(data?.value?.importFrom);
const ImportUrl = ref("");


const emit = defineEmits(['after-select-file']);
const onSelectFile = (file: any) => {
    ImportUrl.value = data?.value?.ImportUrl;
    if (data?.value?.importFrom !== 'ImportContainerModelMapping')
        IsShowMapping.value = (file?.name ?? '') != '';

    if (file?.name) {
        const reader = new FileReader();
        reader.onload = (e) => {
            if (e !== null)
                if (e.target !== null) {
                    const sheetData = e.target.result;
                    const workbook = XLSX.read(sheetData, {
                        type: 'binary',
                        cellDates: true,
                    });
                    ExcelCols.value = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0] as any;
                    data.value.Mappings.forEach(mapping => {
                        if (!ExcelCols.value.find(x => x == mapping.ColName))
                            mapping.ColName = null;
                    });
                }
        }
        reader.readAsBinaryString(file);
    }
}
const onMultifilesSelectFile = (file: any) => {
multifilesSelectedFiles.value =  file;    
}
const ValidateData = async () => {    
    let formData = { File: excelFile.value, Mappings: data.value.Mappings, importFrom: data.value.importFrom };    
    PostFormData(data.value.ValidateUrl, formData, async (data: any) => {        
        if (callBack) {
            callBackReturnData.value = await callBack(data);
        }
        if (formData?.importFrom == 'ImportContainerModelMapping') {            
            ContainerModel.value = callBackReturnData?.value?.ContainerModel;
            containerKeys.value = callBackReturnData?.value?.containerKeys;
            packingTypes.value = callBackReturnData?.value?.packingTypes;
        } else {
            previewData.value = data;    
            if(formData?.importFrom == 'ImportModels'){                    
            previewData.value = previewData.value.map(item => {
            const fileMatch = multifilesSelectedFiles?.value?.find(file => file?.name === item?.Image);
                    return {
                        ...item,
                        objectURL: fileMatch ? fileMatch.objectURL : null,
                        Image: fileMatch ? item?.Image : null,
                        File: fileMatch ? fileMatch : null // Add objectURL if found // Add objectURL if found
                    };
                });
            }
        }
        
        if(formData?.importFrom == 'ImportModels')
            ChangeStep(2);
        else 
            ChangeStep(1);
    }, (data: any) => {

    });
}
const ImportData = async () => {
    let data = previewData.value;
    if (importFrom.value === 'ImportContainerModelMapping') {
        data = ContainerModel.value;
    }    
    if (importFrom.value === 'ImportModels') {        
        let formData1 = {data };
        PostFormData(ImportUrl.value, formData1, "", async (data: any) => {
        Msg.value = data.Message;
        ChangeStep(3);
    }, (data: any) => {
        Msg.value = data;
    });       
    }else{
    PostData(ImportUrl.value, data, "", (data: any) => {
        Msg.value = data.Message;
        ChangeStep(2);
    }, (data: any) => {
        Msg.value = data;
    });
}
}

const rowClass = (data) => {
    debugger
    if(data?.Status == "This record already exists and will be updated if import." )
        return 'row-warning' 
    else if((data?.Status !=undefined && data?.Status !=null && data?.Status != ""))
        return 'row-error' 
    else return ""
};
const ChangeStep = (step: number) => {    
    activeStep.value = step;
}
</script>