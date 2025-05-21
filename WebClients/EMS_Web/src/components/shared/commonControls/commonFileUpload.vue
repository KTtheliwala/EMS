<template>
    <input type="hidden" :value="modelValue" />
    <div :class="{ error: v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length }">

        <FileUpload ref="FileUploadCtrl" :name="name" :accept="accept" :multiple="multiple" :auto="auto"
            :fileLimit="fileLimit" :showCancelButton="false" :maxFileSize="maxFileSize" url="" @select="onSelectFile"
            :chooseIcon="chooseIcon" :chooseLabel="chooseLabel" :showUploadButton="showUploadButton"
            :disabled="Isdisabled">
            <template #empty v-if="!Props.modelValue">
                <p>No files uploaded.</p>
            </template>
            <template #content v-if="UploadedFiles.length > 0">
                <div v-if="UploadedFiles.filter(x => !('IsDeleted' in x) || !x.IsDeleted).length == 0">
                    <p>No files uploaded.</p>
                </div>
                <div class="p-fileupload-file"
                    v-for="(obj, index) of UploadedFiles.filter(x => !('IsDeleted' in x) || !x.IsDeleted)"
                    :key="obj.name">                                     
                    <img role="presentation" v-if="obj.objectURL != undefined" @click="downloadFile(obj)" :src="obj.objectURL"
                    class="p-fileupload-file-thumbnail" />
                    <img role="presentation" v-else @click="downloadFile(obj)" src="@/assets/images/document.svg"
                        class="p-fileupload-file-thumbnail" />
                    <div class="p-fileupload-file-details" @click="downloadFile(obj)">
                        <div class="p-fileupload-file-name">{{ obj.name ?? obj.Name }}</div>
                        <span class="p-fileupload-file-size">{{ obj.size }}</span>
                        <Badge :value="obj.status ?? 'Pending'"
                            :severity="obj.status == 'Completed' ? 'success' : 'warning'"
                            class="p-fileupload-file-badge" />
                    </div>
                    <div class="p-fileupload-file-actions">
                        <Button icon="pi pi-trash" @click="onRemoveFile(index, obj.Id)"
                            class="p-button-outlined p-button-danger p-button-rounded" />
                    </div>   
                </div>
            </template>
        </FileUpload>
    </div>
    <template v-if="(ErrorFiles || []).length > 0">
        <div class="fileupload-input-errors" v-for="error of ErrorFiles" :key="error">
            <div class="error-msg">{{ error }}</div>
        </div>
    </template>

    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, ref, inject, watch, onMounted } from "vue";
import UrlConstants from "@/utils/urlconstants";
import commonModule from "@/composables/modules/commonModule";
const { GetFile } = new commonModule();
const emit = defineEmits(["update:modelValue"]);
const InputValue = ref();
let Props = defineProps({
    accept: { type: String, default: "image/*" },
    multiple: { type: Boolean, default: false },
    auto: { type: Boolean, default: false },
    maxFileSize: { type: Number, default: null },
    fileLimit: { type: Number, default: null },
    chooseIcon: { type: Boolean, default: true },
    showUploadButton: { type: Boolean, default: false },
    chooseLabel: { type: String, default: "Browse Files" },
    previewWidth: { type: Number, default: 10 },
    modelValue: [String, File],
    name: { type: String, required: true, },
    propname: { type: String, required: false, },
    v$: Object,
    Isdisabled: { type: Boolean, default: false },
    AfterSelectFile: { type: Function, default: null },
    callBack: Function,
    downloadurl: { type: String, default: "" },

});
const FileUploadCtrl = ref();
const UploadedFiles = ref([]);
const ErrorFiles = ref([]);
const onSelectFile = (event: any) => {
    UploadedFiles.value = event.files;
    emit("update:modelValue", Props.multiple ? event.files : event.files[0]);
    if (Props.AfterSelectFile)
        Props.AfterSelectFile(Props.multiple ? event.files : event.files[0]);
    if (Props.multiple) {
        ErrorFiles.value = []
        if ((FileUploadCtrl?.value?.messages || []).length > 0) {
            ErrorFiles.value = (FileUploadCtrl?.value?.messages || [])
            FileUploadCtrl.value.messages = []
        }
    }
}
const onRemoveFile = (index: number, Id: number) => {
    if (!Props.multiple) {
        FileUploadCtrl.value.files = [];
        UploadedFiles.value = [];
        emit("update:modelValue", null);
        if (Props.AfterSelectFile)
            Props.AfterSelectFile({ name: "" });
    }
    else {
        if (Id > 0) {
            index = FileUploadCtrl.value.files.findIndex(x => x.Id == Id);
        }
        else {
            index = FileUploadCtrl.value.files.findIndex(x => x == FileUploadCtrl.value.files.filter(y => !('IsDeleted' in y) || !y.IsDeleted)[index]);
        }
        if (FileUploadCtrl.value.files[index].Name)
            FileUploadCtrl.value.files[index].IsDeleted = true;
        else
            FileUploadCtrl.value.files.splice(index, 1);
        UploadedFiles.value = FileUploadCtrl.value.files;
        emit("update:modelValue", UploadedFiles.value);
    }
    if (typeof Props.callBack != "undefined") {
        Props.callBack('');
    }
}
const updateValue = (val: any) => {
    if (Props.multiple)
        UploadedFiles.value = val ? val : [];
    else {
        UploadedFiles.value = [];
        if (val) {
            UploadedFiles.value.push(val);
        }
    }
    FileUploadCtrl.value.files = UploadedFiles.value;
}
onMounted(() => {
    updateValue(Props.modelValue);
});

watch(() => Props.modelValue, () => {
    if (Props.multiple) {
        ErrorFiles.value = []
    }
    updateValue(Props.modelValue);
});

const downloadFile = (itm: any) => {
    if ((itm?.Id ?? 0) > 0 && (Props?.downloadurl ?? "").length > 0) {
        if ((itm?.FileName ?? "").length > 0) {
            GetFile(`${(Props?.downloadurl ?? "")}${itm?.FileName ?? ""}`, (data: any) => {
                const blob = new Blob([data])
                const link = document.createElement('a')
                link.href = URL.createObjectURL(blob)
                link.download = itm.name ?? itm.Name
                link.click()
                URL.revokeObjectURL(link.href)
            });
        }
    }
}
</script>