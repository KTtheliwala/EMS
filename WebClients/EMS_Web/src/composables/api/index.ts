import axios from "axios";
//import { useToastStore } from "@/store/useToastStore";
import { useAuthDataStore } from "@/store/useAuthDataStore";
import router from "@/router";
import {  ref } from "vue";
// import { useRoute } from "vue-router";
// const route = useRoute(); 
const config = {
    //baseURL: "https://new1.common.com.sg/VuePractical/api/v1/",
    baseURL: process.env.VUE_APP_ROOT_API,
    timeout: process.env.VUE_APP_REQUEST_TIMEOUT,
    headers: { Accept: "text/plain", "Content-Type": "application/json", "Authorization": "", "AccessToken": "", "RefreshToken": "", "ProfitCenterId": "" },
};
const Isstatus = ref(false)
const call = axios.create(config);

call.interceptors.request.use((request: any) => {    
    //Isstatus.value = false
    // const timeStamp = new Date().getTime().toString()
    // request.url = request.url?.indexOf('?') == -1 ? request.url + '?timestamp=' + timeStamp : request.url + '&timestamp=' + timeStamp;
    // -------------------------JWT-----------------------        
    const authLogin = useAuthDataStore().getAuth as any;
    request.headers.Authorization = "Bearer " + authLogin?.Token ?? "";    
    return request;
});
call.interceptors.response.use(
    (response) => {
        // useToastStore().setLoading(false);
        return response;
    },
    (error: any) => {
        //useToastStore().setLoading(false);
        if ((error?.response?.status ?? 0) == 401) {  
            useAuthDataStore().setAuth(null);            
            useAuthDataStore().setAuthForProduct(null);
            useAuthDataStore().setAuthMsg("Your session has expired. Please re-login again");
            error.showMessage = false
            const path = router?.currentRoute?.value?.fullPath ?? "" as any 
            if(!Isstatus.value)
            {   
                
                
                Isstatus.value = true
               // router.push({ name: 'unauthorized' , query: { redirect: router?.currentRoute?.value?.fullPath ?? "" } })
               const routeData = router.resolve({ name: "unauthorized",  query: { redirect: router?.currentRoute?.value?.fullPath ?? "" } });         
               window.location.href = routeData.href

            }
        }
        else  if ((error?.response?.status ?? 0) == 403) {
            if(!Isstatus.value)
            {
                Isstatus.value = true
                //router.push({ name: 'forbidden' })
                const routeData = router.resolve({ name: 'forbidden' });         
                window.location.href = routeData.href
            }
        }
        return error;
    }
);
export default call;