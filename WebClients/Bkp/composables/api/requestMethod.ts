import { AxiosRequestConfig } from "axios";
import api from "./index";
class requestMethod {
        async Get(methodname: any) {
                return await api.get(`${methodname}`).catch(function (error) {
                        throw error;
                });
        }
        async Post(methodname: any, T: any) {
                return await api.post(`${methodname}`, T).catch(function (error) {
                        throw error;
                });
        }
        async PostFormData(methodname: any, T: any) {
                return await api.post(`${methodname}`, T, { headers: { "Content-Type": "multipart/form-data" } }).catch(function (error) {
                        throw error;
                });
        }
        async ExportData(methodname: any, T: any) {
                return await api.post(`${methodname}`, T, {
                        responseType: "blob",
                }).catch(function (error) {
                        throw error;
                });
        }
        async GetFile(methodname: any) {
                return await api.get(`${methodname}`, {
                        responseType: "blob",
                }).catch(function (error) {
                        throw error;
                });
        }
        async Put(methodname: any, T: any) {
                return await api.put(`${methodname}`, T).catch(function (error) {
                        throw error;
                });
        }
        async Patch(methodname: any, T: any) {
                return await api.patch(`${methodname}`, T).catch(function (error) {
                        throw error;
                });
        }
        async DeleteList(methodname: any, T: any) {
                const config = {
                        data: T,
                } as AxiosRequestConfig;
                return await api.delete(`${methodname}`, config).catch(function (error) {
                        throw error;
                });
        }
        async PostList(methodname: any, T: any) {
                const config = {
                        data: T,
                } as AxiosRequestConfig;
                return await api.post(`${methodname}`, T).catch(function (error) {
                        throw error;
                });
        }
        async Delete(methodname: any, T: []) {
                const params = [];
                params.push(T);
                const config = {
                        data: String(T).includes(",")
                                ? String(T).split(",")
                                : params,
                } as AxiosRequestConfig;
                return await api.delete(`${methodname}`, config).catch(function (error) {
                        throw error;
                });
        }
        async GetBlob(methodname: any) {
                const result = await api
                        .get(`${methodname}`, { responseType: "blob" })
                        .then((response: any) => {
                                if (!response.isAxiosError)
                                        return URL.createObjectURL(response.data);
                                else
                                        return "";
                        }).catch(function (error) {
                                throw error;
                        });
                return result;
        }
        async DownloadData(methodname: any) {

                return await api.get(`${methodname}`, {
                        responseType: "blob",
                }).catch(function (error) {
                        throw error;
                });
        }
}
export default new requestMethod();
