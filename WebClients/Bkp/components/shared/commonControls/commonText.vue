<template>
    <div :class="{
        error: (v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length) ||
            (parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length && v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]].length)
    }">
        <InputText :id="name" :name="name" :value="modelValue" :placeholder="placeholder" :maxlength="length"
            :readonly="IsreadOnly" :disabled="disabled" :class="inputclass" :type="type" :propname="propname"
            @blur="onBlur($event.target.value)" @input="updateValue($event.target.value)" v-on:keypress="Validations()"
            ref="InputValue" autofocus @paste="onPaste" />
    </div>
    <div v-if="v$ && v$.value && v$.value[propname ?? name]">
        <div class="input-errors" v-for="error of v$.value[propname ?? name].$errors" :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
    <div v-if="parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length > 0">
        <div class="input-errors"
            v-for="error of v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]]"
            :key="error.$uid">
            <div class="error-msg">{{ error.$message }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, ref, inject, watch } from "vue";
const emit = defineEmits(["update:modelValue"]);
const InputValue = ref();
let Props = defineProps({
    type: { type: String, default: "text", },
    modelValue: { type: [String, Number], required: true, default: "", },
    name: { type: String, required: true, },
    parent: { type: String, default: "", },
    propname: { type: String, required: false, },
    placeholder: { type: String, default: "", },
    v$: Object,
    IsreadOnly: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    length: { type: Number, default: 250 },
    IsNumber: { type: Boolean, default: false },
    IsDecimal: { type: Boolean, default: true },
    restrictSpecialChar: Object,
    IsPhone: { type: Boolean, default: false },
    inputclass: { type: String, default: "", },
    onblur: { type: Function, default: null },
    blurParam: { type: [String, Number, Object], default: null }
});
const RestrictSpecialCharList = ref(Props.restrictSpecialChar);
const onBlur = (value: any) => {
    updateValue(value.trim());
    if (Props.onblur) {
        Props.onblur(Props.blurParam, value.trim());
    }
}
const IsValue = ref(false);
const updateValue = (value: any) => {
    if (value == null || value == "null") {
        value = "";
    }
    if (value == "" && Props.IsNumber) {
        value = null;
    }
    emit("update:modelValue", value);
    IsValue.value = value !== undefined && value !== "";
};
updateValue(Props.modelValue);
const Validations = (evt: any) => {
    if (Props.IsNumber) {
        evt = evt ? evt : window.event;
        var charCode = evt.which ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46) {
            evt.preventDefault();
        } else if (!Props.IsDecimal && charCode == 46) {
            evt.preventDefault();
        } else {
            return true;
        }
    } else if (Props.IsPhone) {
        evt = evt ? evt : window.event;
        const pattern = /[0-9 +-]/;
        const charCode = String.fromCharCode(evt.keyCode);
        let res = pattern.test(charCode);
        if (!res) {
            evt.preventDefault();
            return false;
        }
        return true;
    } else if (RestrictSpecialCharList.value != null && RestrictSpecialCharList.value.length > 0) {
        evt = evt ? evt : window.event;
        if (RestrictSpecialCharList.value.includes(evt.key)) {
            evt.preventDefault();
            return false;
        } else {
            return true;
        }
    } else {
        return true;
    }
};
watch(() => Props.modelValue, () => {
    updateValue(Props.modelValue);
});

const onPaste = async (event: any) => {
    event.preventDefault();
    let data = (event.clipboardData || window['clipboardData']).getData('Text').replace(/(?:\r\n|\r|\n)/g, '');
    if (Props.IsNumber) {

        if (data) {
            const pattern = /^[0-9]+$/g;
            if (!pattern.test(data) || (data || "").length > Props.length) {
                updateValue('')
            }
            else {
                updateValue(data)

            }
        }
    }
    else if (Props.IsPhone) {
        if (data) {
            const pattern = /[0-9 +-]/;
            if (!pattern.test(data) || (data || "").length > Props.length) {
                updateValue('')
            }
            else {
                updateValue(data)

            }
        }

    }
    else {
        if ((data || "").length > 0) {
            updateValue(data)
        }
    }
}


</script>