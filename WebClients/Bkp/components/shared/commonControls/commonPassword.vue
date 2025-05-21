<template>
    <div :class="{ error: v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length }">
        <Password :id="name" :name="name" :disabled="Isdisabled" :modelValue="modelValue" :placeholder="placeholder"
            :feedback="feedback" :toggleMask="toggleMask" :propname="propname" class="w-full" ref="InputValue"
            @input="updateValue($event.target.value)">
            <template #footer="sp">
                {{ sp.level }}
                <h3 class="m-0 mb-2">Requirements</h3>
                <ul class="pl-2 ml-2 mt-0" style="line-height: 1.5">
                    <li>Minimum 6 characters long</li>
                    <li>At least one lowercase character</li>
                    <li>At least one uppercase character</li>
                    <li>At least one number</li>
                    <li>At least 1 special character</li>
                </ul>
            </template>
        </Password>
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, watch, ref } from "vue";
import Divider from 'primevue/divider';
const emit = defineEmits(["update:modelValue"]);
let Props = defineProps({
    modelValue: { type: String, required: true, default: "" },
    name: { type: String, required: true },
    propname: { type: String, required: false },
    placeholder: { type: String, default: "" },
    v$: Object,
    feedback: { type: Boolean, default: false },
    toggleMask: { type: Boolean, default: false },
    Isdisabled: { type: Boolean, default: false }
});
const updateValue = (value: any) => {
    emit("update:modelValue", value);
};
</script>
