<template>
    <div :class="{ error: v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length }">
        <Calendar :id="name" :name="name" :modelValue="modelValue" :autocomplete="autocomplete" :dateFormat="dateFormat" :minDate="minDate" :maxDate="maxDate"
            :selectionMode="selectionMode" :disabled="disabled" :propname="propname" :showIcon="showIcon" class="w-full"
            :timeOnly="timeOnly" @dateSelect="updateValue($event)" @input="updateValue($event.target.value)"
            @clear-click="updateValue($event)" :showButtonBar="showButtonBar" :selectOtherMonths="selectOtherMonths"
            :showTime="showTime" />
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>
<script setup lang="ts">
import moment from "moment";
import { defineProps, defineEmits, ref, watch, onUpdated } from "vue";
const Props = defineProps({
    modelValue: { type: null, required: true },
    name: { type: String, required: true },
    propname: { type: String, required: false },
    placeholder: { type: String, default: "" },
    v$: Object,
    manualInput: { type: Boolean, default: true },
    autocomplete: { type: String, default: "off" },
    dateFormat: { type: String, default: "MM/DD/YYYY" },
    disabled: { type: Boolean, default: false },
    showIcon: { type: Boolean, default: true },
    minDate: { type: Date, default: null },
    maxDate: { type: Date, default: null },
    selectionMode: { type: String, default: "single" },
    showButtonBar: { type: Boolean, default: true },
    view: { type: String, default: "date" },
    numberOfMonths: { type: String, default: "1" },
    showWeek: { type: Boolean, default: false },
    dateSelect: { type: Function, default: null },
    timeOnly: { type: Boolean, default: false },
    momentdateFormat: { type: String, default: "DD-MM-YYYY" },
    selectOtherMonths: { type: Boolean, default: true },
    showTime: { type: Boolean, default: false }
});
const emit = defineEmits(["update:modelValue"]);
const isUpdate = ref(false)

watch(() => Props.modelValue, () => {
    if (!isUpdate.value) {
        if (typeof Props.modelValue != "undefined") {
            if (Props.modelValue) {
                let NewDate = moment(new Date(Props.modelValue), Props.momentdateFormat);
                if (NewDate.isValid()) {
                    emit("update:modelValue", NewDate["_d"]);
                    isUpdate.value = true
                }
            }
        }
    }
    else {
        isUpdate.value = false
    }
}
);
const updateValue = (value: any, IsCall: boolean) => {
    let PaidDateCheck = moment(new Date(value), Props.momentdateFormat, true);
    if (PaidDateCheck.isValid()) {
        emit("update:modelValue", PaidDateCheck["_d"]);
        isUpdate.value = true
    }
    else { emit("update:modelValue", undefined); isUpdate.value = false }
    // emit("update:modelValue", value);
    if (Props.dateSelect)
        Props.dateSelect(value);
};
</script>
