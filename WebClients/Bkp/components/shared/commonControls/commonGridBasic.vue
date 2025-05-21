<template>
    <div class="common__Table">
        {{value}}
        <div>
            <DataTable ref="dt"
                       :value="resultData"
                       v-model:selection="selectedRecords"
                       :lazy="true"
                       :paginator="false"
                       :rows="10"
                       :totalRecords="totalRows"
                       showGridlines responsiveLayout="scroll"
                       class="p-datatable-customers">
                <template #header>
                    <slot name="header"></slot>
                </template>
                <slot name="columns"></slot>
                <template #empty>
                    <div class="no-data">
                        <img src="@/assets/images/no-data.png" />
                        <h4>No Record Found</h4>
                    </div>
                </template>
            </DataTable>
        </div>
    </div>
</template>
<script setup lang="ts">
import { ref, defineProps } from "vue";
const Props = defineProps({
	filename: {
		type: String,
		required: true,
	},
	hideExportBtn: {
		type: Boolean,
		default: true,
	},
	hidefavouriteBtn: {
		type: Boolean,
		default: false,
	},
	hidesortByDropDown: {
		type: Boolean,
		default: false,
	},
	listViewOn: {
		type: Boolean,
		default: true,
	},
	clearCallBack: Function,
	hideDeactivate: {
		type: Boolean,
		default: true,
	},
	hidesActivate: {
		type: Boolean,
		default: true,
	},
	admimActionBtn: {
		type: Boolean,
		default: true,
	},
	IsPageStateClear: {
		type: Boolean,
		default: false,
	},
    resultData: {
		type: Object,
		//default: false,
	},
});
</script>