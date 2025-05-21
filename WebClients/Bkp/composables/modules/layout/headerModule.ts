import { ref } from "vue";
// import { useAuthDataStore } from "@/store/useAuthDataStore";
import router from "@/router";
// import apiRequestCall from "@/composables/api/requestMethod";
import UrlConstants from "@/utils/urlconstants";
// import ImageMgtModel from "@/models/admin/imageMgtModel";
// import { AuthService } from "@/composables/api/authService";
class menuModule {
        readonly title = ref("Dashboard");
        readonly menu = ref();
        // readonly authStore = useAuthDataStore();

        readonly reloadUserImg = ref(false);
        // private newImageMgt = (): ImageMgtModel => {
        //         return {
        //                 Id: 0,
        //                 Key: '',
        //                 Description: '',
        //                 Note: '',
        //                 IsActive: true,
        //                 SocialIcon: "",

        //         } as ImageMgtModel;
        // };
        // readonly Outlook = ref<ImageMgtModel>(this.newImageMgt());
        // readonly Teams = ref<ImageMgtModel>(this.newImageMgt());
        readonly PageCodeId = ref(0)
        toggle = (event: any) => {
                this.menu.value.toggle(event);
        };
        // private setAuthData = (authData: any): void => {
        //         this.authStore.setAuth(authData);
        // }


        handleUpdateImg = async (): Promise<void> => {
                this.reloadUserImg.value = false
                this.reloadUserImg.value = true
        }
        getUserMenutems = () => {
                return [
                        {
                                label: "User Profile",
                                to: "/user-profile",
                                visible: true
                        },
                        {
                                label: "Admin Settings",                                
                                // to: AuthService.getCmsNamePermission(),
                                // visible: AuthService.checkAnyCmsPermission()
                        },
                        {
                                label: "Sign Out",
                                visible: true,
                                // command: () => {
                                //         apiRequestCall
                                //                 .Get(
                                //                         UrlConstants.logoutUrl
                                //                 )
                                //                 .then((response: any) => {
                                //                         this.setAuthData(null);
                                //                         router.push({ name: 'Login' })
                                //                 })
                                //                 .catch((err) => {
                                //                         throw err;
                                //                         router.push({ name: 'Login' })
                                //                 });

                                // }

                        },
                ];
        };


}
export default menuModule;
