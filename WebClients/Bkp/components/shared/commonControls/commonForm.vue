<template>
    <form @submit.prevent="SubmitData($event)" class="p-fluid" autocomplete="off">
        <slot></slot>
    </form>
</template>
<script setup lang="ts">
    import commonModule from "@/composables/modules/commonModule";
    import { defineProps, defineExpose } from "vue";
    const { GetData, PostData } = new commonModule();
    const Props = defineProps({
        actionUrl: { type: String, required: true },
        v$: Object,
        successCallBack: { type: Function, default: null },
        failureCallBack: { type: Function, default: null },
        beforeSubmit: { type: Function, default: null },
        data: { type: Object, required: true },
        successMessage: { type: String, default: "" },
        dataId: { type: Number, default: 0 },
        dataUrl: { type: String, default: "" }
    });
    const SubmitData = async (e: any) => {
        if (Props.beforeSubmit) {
            Props.beforeSubmit();
        }
        const result = await Props.v$.value.$validate();
        if (((Props.v$?.value?.$silentErrors ?? []).length || 0) > 0) 
        {
            return;
        }
        if (result) {
            PostData(Props.actionUrl, Props.data, Props.successMessage, (data: any) => {
                if(Props.successCallBack)
                    Props.successCallBack(data);
            }, (data: any) => {
                if(Props.failureCallBack)
                    Props.failureCallBack(data.Message);
            });
        }
        else{
            if ((Props.v$.value.$silentErrors.length || 0) > 0) {
                if (e.target[Props.v$.value.$silentErrors[0].$property])
                    e.target[Props.v$.value.$silentErrors[0].$property].focus();
            }
        }
    }
    const SetValidation = async (callback: any) => {
        if (Props.dataId > 0) {
            let GetDataUrl = Props.dataUrl;
            if (!Props.dataUrl || Props.dataUrl == "")
                GetDataUrl = Props.actionUrl + Props.dataId;
            GetData(GetDataUrl, (data: any) => {
                callback(data);
            });
        }
        else {
            callback(Props.data);
        }
    }
    defineExpose({ SetValidation })
</script>