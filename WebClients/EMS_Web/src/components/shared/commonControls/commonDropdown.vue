<template>
    <div
        :class="{
            error: (parent == '' && v$ && v$.value && v$.value[propname ?? name] && v$.value[propname ?? name].$errors.length) ||
                (parent != '' && v$ && v$.value && v$.value[parent] && v$.value[parent].$each.$response.$errors.length && v$.value[parent].$each.$response.$errors[propname ?? name.split('_')[0]][propname ?? name.split('_')[1]].length)
        }">
        <Dropdown :id="name" :name="name" :modelValue="modelValue" :placeholder="placeholder" :options="ListOption"
            :loading="Isloading" :optionLabel="optionLabel" :optionValue="optionValue" :filter="filter" :mode="mode"
            :showClear="showClear" :datatype="datatype" :autoFilterFocus="true" class="w-full"
            @input="updateValue($event.target.value)" @change="updateValue($event.value)" :disabled="disabled"
            :readonly="readonly" :dataurl="dataurl" :appendTo="appendTo" />
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
import { defineProps, defineEmits, onMounted, ref, watch, defineExpose } from "vue";
import DropdownModel from "@/models/controls/Dropdown/DropdownModel";
import { DropDownValueType } from "@/models/controls/Dropdown/DropdownType";
import requestMethod from "@/composables/api/requestMethod";
import { useToast } from "primevue/usetoast";
const toast = useToast();
const emit = defineEmits(["update:modelValue", "update:title"]);
var dropDownItems = ref(null);
const Props = defineProps({
    modelValue: [Number, String, Boolean],
    name: { type: String, required: true },
    parent: { type: String, default: "", },
    propname: { type: String, required: false },
    placeholder: { type: String, default: "" },
    v$: Object,
    options: Object,
    optionLabel: { type: String, default: "Text" },
    appendTo: {
        type: String,
        default: 'body'
    },
    optionValue: { type: [Number, String, Boolean], default: "Value" },
    filter: { type: Boolean, default: true },
    mode: String,
    disabled: { type: Boolean, default: false },
    readonly: { type: Boolean, default: false },
    showClear: { type: Boolean, default: true },
    callBack: Function,
    afterBind: Function,
    datatype: String,
    allowZero: { type: Boolean, default: false },
    dataurl: { type: String, default: "" },
    isWatch: { type: Boolean, default: true }
});

const ListOption = ref(Props.options);
const Isloading = ref(false);

const BindData = () => {
    Isloading.value = true;
    requestMethod.Get(Props.dataurl).then((data: any) => {
        if (data.isAxiosError) {
            toast.add({ severity: 'error', icon: 'pi pi-times', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
        }
        else {
            if (data.data.StatusCode == 200) {
                ListOption.value = data.data.Data;
                Isloading.value = false;
                let type = Props.datatype === "String" ? DropDownValueType.String : DropDownValueType.Number;
                ListOption.value.forEach((item: { Value: string | number | boolean; }) => {
                    switch (type) {
                        case DropDownValueType.String:
                            item.Value = String(item.Value);
                            break;
                        case DropDownValueType.Number:
                            item.Value = Number(item.Value);
                            break;
                        case DropDownValueType.Boolean:
                            item.Value = Boolean(item.Value);
                            break;
                    }
                });
                if (typeof Props.afterBind != "undefined") {
                    Props.afterBind(ListOption.value);
                }
            } else {
                Isloading.value = false;
            }
        }
        // if(Props.modelValue){
        //   updateValue(Props.modelValue);
        // }
    });
}

const setDropDownValueType = (data: DropdownModel[], type: DropDownValueType): Promise<DropdownModel[]> => {
    return new Promise<DropdownModel[]>((resolve) => {
        try {
            data.forEach((item) => {
                switch (type) {
                    case DropDownValueType.String:
                        item.Value = String(item.Value);
                        break;
                    case DropDownValueType.Number:
                        item.Value = Number(item.Value);
                        break;
                    case DropDownValueType.Boolean:
                        item.Value = Boolean(item.Value);
                        break;
                }
            });
            resolve(data);
        } catch (error) {
            throw new Error(
                "Unable to cast drop-down value type to given type. The given type is :" +
                String(type)
            );
        }
    });
};
const SetData = (list: any) => {
    if (list) {
        let type: DropDownValueType = DropDownValueType.String;
        if (Props.datatype === "Boolean")
            type = DropDownValueType.Boolean;
        else if (Props.datatype === "Number")
            type = DropDownValueType.Number;
        else
            type = DropDownValueType.String;
        setDropDownValueType(list, type).then((data: any) => {
            ListOption.value = data;
            updateValue(Props.modelValue);
            if (typeof Props.afterBind != "undefined") {
                Props.afterBind(ListOption.value);
            }
        });
    }
}
const RefreshData = () => {
    BindData();
}
onMounted;
{
    if (Props.dataurl == "") {
        SetData(ListOption.value);
    }
    else {
        BindData();
    }
}
watch(() => Props.options, () => {
    SetData(Props.options);
});
watch(() => Props.modelValue, () => {
    if (Props.isWatch)
        updateValue(Props.modelValue, true);
});
const updateValue = (value: any, isCall: boolean) => {
    if (value != null && value != undefined && (ListOption?.value ?? []).length > 0) {
        if (ListOption.value.find(x => x[Props.optionValue] == value)) {
            emit("update:title", ListOption.value.find(x => x[Props.optionValue] == value)[Props.optionLabel]);
        }
        else {
            emit("update:title", "");
            value = null;
        }
    } else {
        if (!Props.allowZero && isCall != true) {
            emit("update:title", "");
            value = null;
        }
    }
    if (Props.modelValue != value) {
        emit("update:modelValue", value);
    }
    if (isCall || Props.modelValue != value) {
        if (typeof Props.callBack != "undefined") {
            Props.callBack(value);
        }
    }
};
defineExpose({ SetData, RefreshData });
</script>