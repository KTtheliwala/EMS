<template>
    <div :class="{ error: v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length }">
        <Textarea :id="name" :name="name" :value="modelValue" :autoResize="autoResize" :rows="rows" :cols="cols"
            :readonly="IsreadOnly" :disabled="disabled" :class="inputclass" :showLength="showLength"
            :propname="propname" :maxlength="maxLength" @input="updateValue($event.target.value)" ref="InputValue"
            @keyup='charCount' />
        <p class="info__msg" v-if="Props.showLength">{{ totalChar }} characters remaining</p>
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, ref, onMounted, watch } from "vue";
const InputValue = ref();
const emit = defineEmits(["update:modelValue"]);
const Props = defineProps({
    fieldlabel: { type: String, default: "" },
    type: { type: String, default: "text" },
    modelValue: { type: String, required: true, default: "" },
    name: { type: String, required: true },
    propname: { type: String, required: false },
    autoResize: Boolean,
    cols: { type: Number, default: 30 },
    rows: { type: Number, default: 5 },
    IsreadOnly: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    inputclass: { type: String, default: "", },
    v$: Object,
    showLength: { type: Boolean, default: false },
    maxLength: { type: Number, default: 250 }
});
const updateValue = (value: any) => {
    emit("update:modelValue", value);
};
const totalChar = ref();
const charCount = () => {
    if (Props.modelValue != null && Props.modelValue != '')
        totalChar.value = (Props.maxLength - Props.modelValue.length);
}
onMounted(() => {
    totalChar.value = Props.maxLength;
});
watch(() => Props.modelValue, () => {
    updateValue(Props.modelValue);
    charCount();
});
</script>
