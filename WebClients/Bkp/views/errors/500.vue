<template>
    <div class="auth-wrapper">
        <div class="auth-content">
            <div class="error-page">
                <router-link to="/" class="small-logo">
                    <img class="version-logo" src="@/assets/images/CakeSmithsPvt-Logo.png" />
                </router-link>
                <h4>500</h4>
                <h6 class="page-not-fund">Opps something went wrong</h6>
                <Button label="Go Back" class="btn btn-primary mr-0 CustomButton Width160" @click="$router.go(-1)"/>
                <p class="error-version">Version : v1.0.0</p>
            </div>
        </div>
    </div>
</template>