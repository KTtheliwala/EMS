<template>
    <div :class="{
        error: (parent == '' && v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length) ||
            (parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length && v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]].length)
    }">

        <InputNumber :id="name" :name="name" :modelValue="modelValue" :mode="mode" :currency="currency"
            :minFractionDigits="minFractionDigits" :maxFractionDigits="maxFractionDigits" :useGrouping="useGrouping"
            :class="inputclass" :propname="propname" :readonly="IsreadOnly" :disabled="disabled"
            @blur="onBlur($event.value)" @input="updateValue($event.value)" @focus="onFocus($event)"
            :allowEmpty="allowEmpty" ref="InputValue" />
    </div>
    <div class="input-errors-wrap" v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$?.value[propname ?? name]?.$errors ?? []" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
    <div v-if="parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length">
        <div class="input-errors"
            v-for="error of v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]]"
            :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import InputNumber from "primevue/inputnumber";
import { defineProps, defineEmits, ref, inject, watch } from "vue";
const emit = defineEmits(["update:modelValue"]);
const InputValue = ref();
let Props = defineProps({
    modelValue: { type: Number, required: true, default: 0, },
    name: { type: String, required: true, },
    parent: { type: String, default: "", },
    propname: { type: String, required: false, },
    mode: { type: String, default: "decimal", },
    v$: Object,
    IsreadOnly: { type: Boolean, default: false },
    allowEmpty: { type: Boolean, default: true },
    disabled: { type: Boolean, default: false },
    currency: { type: String, default: "USD" },
    inputclass: { type: String, default: "", },
    minFractionDigits: { type: String, default: "0" },
    maxFractionDigits: { type: String, default: "3" },
    useGrouping: { type: Boolean, default: false },
    onblur: { type: Function, default: null },
    onInput: { type: Function, default: null },
    blurParam: { type: [String, Number, Object], default: null }
});
const IsValue = ref(false);
const onBlur = (value: any) => {
    updateValue(value == "" || value == null || !value ? 0 : value.trim());
    if (Props.onblur) {
        Props.onblur(Props.blurParam, value.trim());
    }
}
const updateValue = (value: any) => {
    emit("update:modelValue", value);
    if (Props.onInput)
        Props.onInput(value);
    IsValue.value = value !== undefined && value !== "";
};
//updateValue(Props.modelValue);
watch(() => Props.modelValue, () => {
    updateValue(Props.modelValue);
});
const onFocus = (e: any) => {
    if (Number(e.target.value) == 0) {
        emit("update:modelValue", null);
    }
}
</script>