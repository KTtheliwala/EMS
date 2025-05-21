<template>
    <div
        :class="{
            error: (parent == '' && v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length) ||
                (parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length && v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]].length)
        }">

        <MultiSelect :id="name" :name="name" :modelValue="modelValue" :placeholder="placeholder" :options="ListOption"
            :optionLabel="optionLabel" :optionValue="optionValue" :filter="filter" :showToggleAll="showToggleAll"
            :maxSelectedLabels="maxSelectedLabels" :loading="Isloading" @input="updateValue($event.target.value)"
            @change="updateValue($event.value)" :disabled="disabled" :readonly="readonly" :display="chip"/>
        <button class="clearValueBtn" @click="updateValue('')" v-if="(modelValue || []).length > 0"><i
                class="pi pi-times"></i></button>
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
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
import { defineProps, defineEmits, ref, onMounted, defineExpose, watch } from "vue";
const emit = defineEmits(["update:modelValue", "update:title"]);
const IsValue = ref(false);
const Props = defineProps({
    fieldlabel: { type: String, default: "" },
    modelValue: [Number, String, Boolean, Array],
    name: { type: String, required: true },
    parent: { type: String, default: "", },
    propname: { type: String, required: false },
    placeholder: { type: String, default: "" },
    v$: Object,
    options: Object,
    optionLabel: { type: String, default: "Text" },
    optionValue: { type: [Number, String], default: "Value" },
    filter: { type: Boolean, default: false },
    mode: String,
    disabled: { type: Boolean, default: false },
    readonly: { type: Boolean, default: false },
    callBack: Function,
    datatype: String,
    showToggleAll: { type: Boolean, default: false },
    maxSelectedLabels: { type: Number, default: 1 },
    selectedItemsLabel: { type: String, default: "" },
    chip: { type: String, default: "chip" },
});
const Isloading = ref(false);
const updateValue = (value: any) => {
    IsValue.value = true;
    if (Props.modelValue != value) {
        emit("update:modelValue", value);
        try {
            if ((value || []).length > 0) {
                const records = (Props.options || []).filter(i => value.includes(i[Props.optionValue]))
                const CSVOf_arr = records.map((item: any) => { return item[Props.optionLabel] }).join(', ');
                emit("update:title", CSVOf_arr);
            }
            else {
                emit("update:title", "");
            }

        } catch (err) {
            //nothing to do
        }
        if (typeof Props.callBack != "undefined") {
            Props.callBack(value);
        }
    }
};
watch(() => Props.options, () => {
    SetData(Props.options);
});
const ListOption = ref(Props.options);
const SetData = (list: any) => {
    if (list) {
        ListOption.value = list;
        updateValue(Props.modelValue);
    }
}
const SetValues = (value: any) => {
    updateValue(value);
}
defineExpose({ SetData, SetValues });
</script>