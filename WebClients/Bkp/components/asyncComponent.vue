<template>
  <loadComponent />
</template>
<script setup lang="ts">
import { defineAsyncComponent, defineProps } from "vue";
import LoadingComponent from "@/components/loadingComponent.vue";
import ErrorComponent from "@/components/errorComponent.vue";
const Props = defineProps({
  url: String,
  delay: { type: Number, default: 1000 },
  timeout: { type: Number, default: 3000 },
});
const loadComponent = defineAsyncComponent({
  loader: () => import(`@/components/${Props.url}`),
  loadingComponent: LoadingComponent /* shows while loading */,
  errorComponent: ErrorComponent /* shows if there's an error */,
  delay: Props.delay /* delay in ms before showing loading component */,
  timeout: Props.timeout /* timeout after this many ms */,
});
</script>
