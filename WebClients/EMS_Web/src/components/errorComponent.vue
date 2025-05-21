<template>
  <div class="error-page-wrapper">
    <div class="error-content error-401">
      <div class="brand-wrapper">
        <router-link to="/" class="small-logo">
          <!-- <img src="@/assets/images/logo.svg" /> -->
        </router-link>
      </div>
      <h4 class="mb-4">Opps something went wrong</h4>
      <Button
        label="Go Back"
        class="btn btn-primary mr-0 CustomButton Width160"
        @click="$router.go(-1)"
      />
    </div>
  </div>
</template>
