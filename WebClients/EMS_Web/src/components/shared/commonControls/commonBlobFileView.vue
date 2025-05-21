<template>

	<div class="img-wrap" v-if="isImage ==true"><img  @click="downloadFile(Props.Id)" :src="blobUrl" @change.once="Fileloaded" :height="35" :width="35" /></div>
	<template v-if="isImage ==false">
		<div class="p-fileupload-file" @click="downloadFile(Props.Id)" >
		<div class="img-wrap"><img role="presentation" src="@/assets/images/document.svg" class="p-fileupload-file-thumbnail" /></div>
		 <div class="p-fileupload-file-details" >
        	<div class="p-fileupload-file-name">{{ Props.Name}}</div>
		</div>
		</div>
	</template>
	<div class="img-loaderwrap" v-if="isLoading">
		<ProgressSpinner class="small-loader" strokeWidth="5" />
	</div>
</template>

<script setup lang="ts">
import requestMethod from "@/composables/api/requestMethod";
import UrlConstants from "@/utils/urlconstants";
import Constants from "@/utils/constants";
import { ref, defineProps, watch, inject } from "vue";
import commonModule from "@/composables/modules/commonModule";
const { PatchData, GetFile } = new commonModule();
const isLoading = ref(false);
const isImage = ref(false);
const Props = defineProps({
	Id: {
            type: String,
            default: "",
    },
	Name: {
            type: String,
            default: "",
    },
	Url: {
            type: String,
            default: "",
    },
	IsImgType:{
            type: Boolean,
            default: false,
    },
	
});

const downloadFile = (id: any) => {
        GetFile(`${Props.Url}?id=${id}`, (data: any) => { 
			const blob = new Blob([data])
            const link = document.createElement('a')
            link.href = URL.createObjectURL(blob)
            link.download = Props.Name
            link.click()
            URL.revokeObjectURL(link.href)            
        });
    }

const blobUrl = ref("");
const Fileloaded = async () => {
	isImage.value =false
	if(Props.IsImgType)
	{
		isLoading.value =true
		await requestMethod
		.GetFile(`${Props.Url}?id=${Props.Id}`)
		.then((response: any) => {
			
			blobUrl.value = ""
			if (response.isAxiosError) 
			{
				isLoading.value = false;
			} 
			else 
			{
				
				if ((response.status || 0) == 200) 
				{
					if(response.headers['content-type'].indexOf('image') != -1)
					{
						isImage.value = true
						blobUrl.value =URL.createObjectURL(response.data)
					}
				} 
				isLoading.value = false;
			}
		})
		.catch((err) => {
			isLoading.value = false;
			//toast.setMessage("error", "Unsuccessful", err, 7000);
		});
	}
	
};
Fileloaded();
watch(
	() => Props.Id,
	() => {
		Fileloaded();
	}
);
</script>
