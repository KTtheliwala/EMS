<template>
    <Button :label="btnlabel"
            :class="btnclass"
            :type="btntype"
            :icon="btnicon"
            :disabled="disabled">
    </Button>
</template>

<script setup lang="ts">
    import { ref, defineProps } from "vue";
    const Props = defineProps({
        btnlabel: { type: String, default: "" },
        btnclass: { type: String, default: "" },
        btntype: { type: String, default: "" },
        btnicon: { type: String, default: "" },
        disabled: { type: Boolean, default: false },
    });
</script>
