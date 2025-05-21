
<template>
    <div class="subnavtab-wrapper">
        <div class="tab-wrapper custom-tab-wrapper">
            <ul v-if="isTabVisible" class="nav nav-tabs" role="tablist">
                <li class="nav-item" role="presentation" v-if="AuthService.CheckPermissionsPageWise(moduleName, 'User')">
                    <router-link to="/admintools/users" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> User
                        </a>
                    </router-link>
                </li>
                <li class="nav-item" role="presentation" v-if="AuthService.CheckPermissionsPageWise(moduleName, 'Role')">
                    <router-link to="/admintools/role" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> Role
                        </a>
                    </router-link>
                </li>     
                <li class="nav-item" role="presentation" v-if="AuthService.CheckPermissionsPageWise(moduleName, 'SystemSetting')">
                    <router-link to="/admintools/system-setting" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> System Setting
                        </a>
                    </router-link>
                </li>   
                <!-- <li class="nav-item" role="presentation">
                    <router-link to="/admintools/email-template" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-store"></i> Email Template
                        </a>
                    </router-link>
                </li>                  
                <li class="nav-item" role="presentation">
                    <router-link to="/admintools/mail-list" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-email"></i> Sent Mails
                        </a>
                    </router-link>
                </li>                   -->
                <li class="nav-item" role="presentation" v-if="AuthService.CheckPermissionsPageWise(moduleName, 'AuditTrail')">
                    <router-link to="/admintools/audit-logs" custom v-slot="{ href, route, navigate }">
                        <a :class="masterRoute.meta.name == route.name || masterRoute.meta.parentNode == route.name ? ActiveClass : InActiveClass" :href="href" @click="navigate">
                            <i class="fas fa-email"></i> Audit Logs
                        </a>
                    </router-link>
                </li>                  
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
    const moduleName = ref('AdminTools')
    const setTabVisible = (visible: boolean) => {
        isTabVisible.value = AuthService.CheckPermissionsModuleWise(moduleName.value);
    };    

    
    const masterRoute = ref(useRoute());
    const ActiveClass = "router-link-active router-link-exact-active nav-link";
    const InActiveClass = "nav-link";
</script>
