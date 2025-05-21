<template>
	<router-view name="layout"> </router-view>
	<router-view />
	
	<Toast class="CustomToast" :position="position">
		<template #message="slotProps">
			<!-- <i :class="slotProps.message.icon"></i> -->
			<!-- <i :class="slotProps.message.severity == 'success' ? 'pi pi-check' : 'pi pi-times'"></i> -->
			
			<i class="pi pi-check" v-if="slotProps.message.severity == 'success'"></i>
			<i class="pi pi-times" v-if="slotProps.message.severity == 'error'"></i>
			<i class="pi pi-exclamation-triangle" v-if="slotProps.message.severity == 'warn'"></i>
			
			<h4>{{ slotProps.message.summary }}</h4>
			<p>
				{{ slotProps.message.detail }}
				<router-link to="#">{{ slotProps.message.link }}</router-link>
			</p>
		</template>
	</Toast>
	<ConfirmDialog></ConfirmDialog>
	<DynamicDialog />
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
const position = ref("top-right");
 

</script>

