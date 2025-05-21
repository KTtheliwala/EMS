<template>
    <div class="auth-wrapper">
        <div class="auth-content">
            <div class="error-page">
                <router-link to="/" class="small-logo">
                    <img class="version-logo" src="@/assets/images/CakeSmithsPvt-Logo.png" />
                </router-link>
                <h4>401</h4>
                <h6 class="page-not-fund">Session Expired <br/> Please Sign In Again</h6>
                <Button label="Login again" class="btn btn-primary mr-0 CustomButton Width160" @click="backLogin"/>
                <p class="error-version">Version : v1.0.0</p>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">

import { useAuthDataStore } from "@/store/useAuthDataStore";
import router from '@/router';
import { useRoute } from "vue-router";
const route = useRoute();
const backLogin = () => {
    useAuthDataStore().setAuth(null);
    useAuthDataStore().setAuthMsg("Your session has expired. Please re-login again");
    router.push({ name: 'Login' }); //redirect to sign-in page.
    if (typeof route.query.redirect != "undefined") 
        {
           if(route.query?.redirect ?? "" != "")
           {
                router.push({ name: 'Login',query: { redirect: route.query?.redirect??"" } }); //redirect to sign-in page.
           }
           else
           {
                router.push({ name: 'Login' }); //redirect to sign-in page.
           }
            
        }
        else 
        {
            router.push({ name: "Dashboard" });
        }
};
</script>