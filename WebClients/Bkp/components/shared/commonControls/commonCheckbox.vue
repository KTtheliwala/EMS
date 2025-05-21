<template>
    <div :class="{ error: v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length }">
        <div v-for="(item, index) of items" :key="index" class="field-checkbox">
            <Checkbox :id="index" :name="name" :value="item" v-model="selecteditem" :propname="propname"
                :disabled="disabled" @click="updateValue(item[`${Props.KeyValue}`])" />
            <label :for="item[`${Props.KeyName}`]">
                {{ item[`${Props.KeyValue}`] }}
            </label>
        </div>
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from "vue";
const emit = defineEmits(["update:modelValue"]);
const SelectItem = [];
let Props = defineProps({
    modelValue: { type: String, required: true, default: "" },
    items: Object,
    name: { type: String, required: true },
    propname: { type: String, required: false, },
    v$: Object,
    KeyName: { type: String, default: "key" },
    KeyValue: { type: String, default: "value" },
    disabled: { type: Boolean, default: false },
});
//#region ----------Update Model Value-----------------------------
const updateValue = (value) => {
    if (SelectItem.includes(value))
        SelectItem.splice(SelectItem.indexOf(value), 1);
    else SelectItem.push(value);
    emit("update:modelValue", SelectItem.join(","));
};
//#endregion-------------------------------------------------------
</script>