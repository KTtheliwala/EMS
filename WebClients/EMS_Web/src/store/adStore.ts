import { defineStore } from "pinia";
export const useAdStore = defineStore("adStore", {
    state: () => {
        return {
            msalConfig: {
                auth: {
                    clientId: process.env.VUE_APP_AZURE_CLIENTID,
                    authority: process.env.VUE_APP_AZURE_AUTHORITY,
                    redirectUri: process.env.VUE_APP_AZURE_REDIRECTURI
                },
                cache: {
                    cacheLocation: 'localStorage',
                },
            },
            accessToken: ''
        };
    },
    getters: {
        getAuth(state) {
            return state;
        }
    },
    actions: {
        setAccessToken(state: any, token: any) {
            state.accessToken = token;
        }
    },
});
