<template>
    <div></div>
    
</template>
<script setup lang="ts">
    import { useDialog } from "primevue/usedialog";
    import { defineProps, defineExpose, ref } from "vue";
    import { useLoading } from 'vue3-loading-overlay';
    import commonModule from "@/composables/modules/commonModule";
    let Props = defineProps({
        title: { type: String, default: "Manage Data" },
        class: { type: String, default: "" },
        closable: { type: Boolean, default: true },
    });
    const closable = ref(Props.closable)
    const dialog = useDialog();
    const { ShowLoader, HideLoader } = new commonModule();
    const loader = useLoading();
    const OpenModal = (CustomerComponent: any,title:string, data: any, closeCallback: any) => {
        ShowLoader(loader);
        var newdata = JSON.stringify(data);
        const dialogRef = dialog.open(CustomerComponent, {
            props: {
                header: title&&title!=''? title: Props.title,
                modal: true,
                class: Props.class,
                draggable: false,
                closeOnEscape:false,
                closable:closable.value,
            },
            onClose: (options) => {
                if (closeCallback)
                    closeCallback(options.data);
            },
            data: JSON.parse(newdata),
            containerClass: 'large-modal'
        });
        HideLoader(loader);
    }
    const CloseModel=()=>{
        dialog.close();
    }
    const SetClosable=(Itm:any)=>{
        closable.value=Itm
    }
    defineExpose({ OpenModal,CloseModel,SetClosable })

</script>