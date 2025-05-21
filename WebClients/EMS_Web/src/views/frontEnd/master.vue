
<template>
    <div class="subnavtab-wrapper">
        <div class="tab-wrapper custom-tab-wrapper">
            <ul v-if="isTabVisible" class="nav nav-tabs" role="tablist">
                <li class="nav-item" role="presentation" v-if="AuthService.CheckPermissionsPageWise(moduleName, 'Employee')">
                    <router-link to="/master/employee" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> Employee
                        </a>
                    </router-link>
                </li>   
                <!-- <li class="nav-item" role="presentation">
                    <router-link to="/master/category" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> Category
                        </a>
                    </router-link>
                </li>   
                <li class="nav-item" role="presentation">
                    <router-link to="/master/product" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> Product
                        </a>
                    </router-link>
                </li>                 -->
            </ul>
        </div>
        <div class="content-wrap">
            <router-view v-slot="{ Component, route }">
                <transition name="fade" mode="out-in" :key="route.path">
                    <component :is="Component" @setTabVisible="setTabVisible" />
                </transition>
            </router-view>
        </div>
    </div>
</template>
<script setup lang="ts">
    import { ref } from "vue";
    import { useRoute } from "vue-router";
    import { AuthService } from "@/composables/api/authService";
    let isTabVisible = ref(true);
    const moduleName = ref('Masters')
    const setTabVisible = (visible: boolean) => {
        isTabVisible.value = AuthService.CheckPermissionsModuleWise(moduleName.value);
    };      
    const masterRoute = ref(useRoute());
    const ActiveClass = "router-link-active router-link-exact-active nav-link";
    const InActiveClass = "nav-link";
</script>
