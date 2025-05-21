<template>
    <div class="vld-parent">
        <loading :active="isLoading"
                 :can-cancel="true"
                 :on-cancel="onCancel"
                 :is-full-page="fullPage"></loading>
    </div>
</template>
<script setup lang="ts">
    import { ref } from 'vue';
    import { useLoading } from 'vue3-loading-overlay';
    import 'vue3-loading-overlay/dist/vue3-loading-overlay.css';
    const isLoading = ref(false);
    const fullPage = ref(true);
    const onCancel = () => {
        isLoading.value = false;
    }
</script>