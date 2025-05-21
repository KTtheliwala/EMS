<template>
    <div class="header-layout" :class="{ 'layout-static': layoutStatic }">
        <div class="inner-header">
            <div class="topbar-menu-button" @click="onTopBarMenuClickBtn">
                <i class="pi pi-bars" v-on:click="$emit('topBar-MenuClck', $event)"></i>
                <i class="pi pi-times" v-on:click="$emit('topBar-MenuClck', $event)"></i>
            </div>
            <router-link to="/home" class="mob-logo">
                <label class="header-title">EMS</label>
                <!-- <img src="@/assets/images/logo.png" /> -->
            </router-link>
            <menu-component :IsAdminMenu="true" :sidebarActive="sidebarActive"
                @sideNavClose="onTopBarMenuClickBtn"></menu-component>
            <ul class="header__right-action"> 
                <li>
                    <div class="user-profile-dropdown">
                        <Button class="user__icon" type="button" @click="toggle" aria-haspopup="true"
                            aria-controls="overlay_menu">
                            <span>{{ UserNameFirst }}</span>
                        </Button>
                        <Menu id="overlay_menu" class="fixedTopMenu" ref="menu" :model="items" :popup="true" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <common-dialog ref="changePasswordManagePopup"></common-dialog>
    <common-dialog ref="userProfileManagePopup" class="large-modal"></common-dialog>
</template>
<script setup lang="ts">
import { defineProps, onMounted, inject, ref, defineEmits, watch, onUpdated } from "vue";
import { useRoute } from "vue-router";
import menuModule from "@/composables/modules/layout/menuModule";
import menuComponent from "../menu/menuComponent.vue";
import headerNotificarion from "../layout/headerNotificarion.vue";
import { useAuthDataStore } from "@/store/useAuthDataStore";
import commonModule from "@/composables/modules/commonModule";
import UrlConstants from "@/utils/urlconstants";
import router from "@/router";
import { useAdStore } from "@/store/adStore";

// import { PublicClientApplication } from "@azure/msal-browser";
import { useToast } from "primevue/usetoast";

const { layoutStatic, sidebarActive, onTopBarMenuClickBtn } = new menuModule();
//-------------------------------------Propertise--------------------------------------------------//
const { DisplayMessage, GetData, PostData } = new commonModule();
const emit = defineEmits(["reloadData1"]);
const menu = ref();
const IsAd = ref(false);
const AdStore = useAdStore();
const toast = useToast();
import changePasswordManage from "@/views/frontEnd/admintools/users/change-password.vue";
import userProfileManage from "@/views/frontEnd/admintools/users/user-profile.vue";
const changePasswordManagePopup = ref();
const userProfileManagePopup = ref();
const UserNameFirst = ref("");
const ForceChangePassword = ref(false)
if (JSON.parse(localStorage.getItem('useAuthData'))) {
    UserNameFirst.value = JSON.parse(localStorage.getItem('useAuthData'))?.userModel?.UserName[0] ?? '';
    ForceChangePassword.value = (JSON.parse(localStorage.getItem('useAuthData'))?.userModel?.ForceChangePassword ?? false);
}

const items = ref([
    {
        icon: "pi pi-user",
        label: "User Profile",
        command: () => {
          userProfileManagePopup.value.OpenModal(userProfileManage, "User Profile", true);
        },
    },
    {
        icon: "pi pi-lock",
        label: "Change Password",
        visible: !IsAd.value,
        command: () => {
          changePasswordManagePopup.value.OpenModal(changePasswordManage, "Change Password", true);
        },
    },
    {
        icon: "pi pi-sign-out",
        label: "Logout",
        command: async () => {
          
                GetData(UrlConstants.logoutUrl, (data: any) => {
                    useAuthDataStore().setAuth(null);
                    useAuthDataStore().setAuthForProduct(null);
                    router.push({ name: "Login" });
                });            
        },
    },
]);

const toggle = (event) => {
    menu.value.toggle(event);
};
// onMounted(async () => {
//     // TodayNewNotification();
//     // Last10NotificationByUser();
//     const messaging = getMessaging();
//     onMessage(messaging, (payload) => {
//         DisplayMessage(payload.notification.title, payload.notification.body);
//     });
// });

const op = ref();
const toggleNotification = (event) => {
    op.value.toggle(event);
};
watch(() => ForceChangePassword.value, () => {
    CheckChangePwd()
});
const CheckChangePwd = () => {
   // changePasswordManagePopup.value.SetClosable(true)
    if (ForceChangePassword.value) {
       // changePasswordManagePopup.value.SetClosable(false)
       //changePasswordManagePopup.value.OpenModal(changePasswordManage, "Change Password", true);
    }
};
onMounted(() => {
    CheckChangePwd()
});

</script>
