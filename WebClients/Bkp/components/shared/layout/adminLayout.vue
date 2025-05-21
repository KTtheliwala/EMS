<template>
  <!-- <div class="layout-main" :class="{ 'layout-static': layoutStatic }">
    <header-layout @topBar-MenuClck="onTopBarMenuClickBtn" /> -->
    <!-- <menu-component
      :sidebarActive="sidebarActive"
      @sideNavClose="onTopBarMenuClickBtn"
      :IsAdminMenu="false"
      :IsShowBackButton="true"
    /> -->
  <!-- </div> -->
  <div class="layout-main">
    <header-layout  />
  </div>
</template>
<script setup lang="ts">
import headerLayout from "./headerLayout.vue";
//---------------------------Name Space------------------------------------------------//
// import headerLayout from "./headerLayout.vue";
// import menuComponent from "../menu/menuComponent.vue";
// import menuModule from "@/composables/modules/layout/menuModule";
//-------------------------------------Module--------------------------------------------------//
//const { layoutStatic, sidebarActive, onTopBarMenuClickBtn } = new menuModule();
</script>
