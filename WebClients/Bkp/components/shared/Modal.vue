<template>
  <Dialog v-model:visible="modalActive" :modal="true">
    <template #header>
      <h2 class="p-dialog-title">{{ modalHeader }}</h2>
      <!-- <Button @click="$emit('closeModal', $event)" >
            <i class="pi pi-times"></i>
        </Button> -->
    </template>
    <slot></slot>
    <template #footer v-if="footerShow">
      <Button
        label="Cancel"
        @click="$emit('closeModal', $event)"
        class="p-button-text mr-2 CustomButton Width130"
      />
      <Button
        label="Create"
        @click="$emit('closeModal', $event)"
        class="mr-0 CustomButton Width130"
        autofocus
      />
    </template>
  </Dialog>
</template>
<script setup lang="ts">
import { defineProps } from "vue";
let Props = defineProps({
  modalHeader: {
      type: String,
    },
    // modalActive: {
    //   type: Boolean,
    // },
    footerShow: {
      type: Boolean,
    },
});
</script>
