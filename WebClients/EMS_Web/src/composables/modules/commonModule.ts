import requestMethod from "@/composables/api/requestMethod";
import { defineProps, inject, ref, defineExpose, readonly } from "vue";
import { useToast } from "primevue/usetoast";
import DeleteModel from "@/models/common/deleteModel";
import { useLoading } from 'vue3-loading-overlay';
import CommonHelper from "@/utils/commonHelper";

class commonModule {
    readonly toast = useToast();
    ShowError = (data: any) => {
        if (data.response.status == 403)
            this.toast.add({ severity: 'error', summary: 'Permission denied', detail: data?.response?.data?.Message ?? "Something went wrong. Please try again after sometime.", life: 3000 });
        else {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: data?.response?.data?.Message ?? "Something went wrong. Please try again after sometime.", life: 3000 });
        }
    }
    GetData = async (url: string, callback: any,failure:any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        await requestMethod.Get(url).then((data: any) => {
            if (data.isAxiosError) {
                this.ShowError(data);
            }
            else {
                if (data.data.StatusCode == 200) {
                    if (callback) {
                        if (data.data.Data)
                            callback(data.data.Data);
                        else
                            callback(data.data);
                    }
                }
                else {
                    if ((data?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: data.data.Message, life: 3000 });
                        if(failure){
                            failure(data.data);
                        }
                    }
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Server Error', life: 3000 });
            this.HideLoader(loader);
        });
    }
    GetFile = (url: string, callback: any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.GetFile(url).then((data: any) => {
            if (data.isAxiosError) {
                this.ShowError(data);
            }
            else {
                if (callback) {
                    if (data.headers['content-disposition'] !== undefined) {
                        const fileName = data.headers['content-disposition'].split('filename=')[1].split(';')[0];
                        data.data["filename"] = fileName.replaceAll('"', "").trim();
                    }

                    if (data.data.Data)
                        callback(data.data.Data);
                    else
                        callback(data.data);
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Server Error', life: 3000 });
            this.HideLoader(loader);
        });
    }
    PostData = (url: string, data: any, successMsg: string, callback: any, failureCallback: any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.Post(url, JSON.stringify(data)).then((response: any) => {
            if (response.isAxiosError) {
                this.ShowError(response);
            } else {
                if (response.data.StatusCode == 200) {
                    if (callback) {
                        if (response.data.Data)
                            callback(response.data.Data);
                        else
                            callback(response.data);
                    }
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'success', summary: 'Successful', detail: response.data.Message, life: 3000 });
                    }
                } else {
                    if (failureCallback)
                        failureCallback(response.data);
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: response.data.Message, life: 3000 });
                    }
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
    CreateFormData = (obj: any, formData: FormData, subKeyStr = '') => {
        for (const i in obj) {
            const value = obj[i]; let subKeyStrTrans = i;
            if (subKeyStr) {
                if (i.indexOf(' ') > -1 || !isNaN(parseInt(i)))
                    subKeyStrTrans = subKeyStr + '[' + i + ']';
                else
                    subKeyStrTrans = subKeyStr + '.' + i;
            }
            if (typeof (value) === 'object' && !(value instanceof Date) && !(value instanceof File)) {
                this.CreateFormData(value, formData, subKeyStrTrans);
            }
            else {
                formData.append(subKeyStrTrans, value ?? '');
            }
        }
    }
    PostFormData = async (url: string, data: any, callback: any, failureCallback: any) => {

        const loader = useLoading();
        this.ShowLoader(loader);
        const formData = new FormData();
        this.CreateFormData(data, formData);
        await requestMethod.PostFormData(url, formData).then((response: any) => {
            if (response.isAxiosError) {
                this.ShowError(response);
            } else {
                if (response.data.StatusCode == 200) {
                    if (callback) {
                        if (response.data.Data)
                            callback(response.data.Data);
                        else
                            callback(response.data);
                    }
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'success', summary: 'Successful', detail: response.data.Message, life: 3000 });
                    }
                } else {
                    if (failureCallback)
                        failureCallback(response.data);
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: response.data.Message, life: 3000 });
                    }
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
    DeleteData = (url: string, deleteIds: DeleteModel, callback: any, failureCallback: any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.DeleteList(url, deleteIds.Ids).then((data: any) => {
            if (data.isAxiosError) {
                this.ShowError(data);
            }
            else {
                if (data.data.StatusCode == 200) {
                    if (callback) {
                        if (data.data.Data)
                            callback(data.data.Data);
                        else
                            callback(data.data);
                    }
                    if ((data?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'success', summary: 'Successful', detail: 'You have successfully removed ' + deleteIds.Ids.length + ' records.', life: 3000 });
                    }
                }
                else {
                    if (failureCallback)
                        failureCallback(data.data);
                    if ((data?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: data.data.Message, life: 3000 });
                    }
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
    PatchData = (url: string, data: any, successMsg: string, callback: any, failureCallback: any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.Patch(url, JSON.stringify(data)).then((response: any) => {
            if (response.isAxiosError) {
                this.ShowError(response);
            } else {
                if (response.data.StatusCode == 200) {
                    if (callback) {
                        if (response.data.Data)
                            callback(response.data.Data);
                        else
                            callback(response.data);
                    }
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'success', summary: 'Successful', detail: response.data.Message, life: 3000 });
                    }
                } else {
                    if (failureCallback)
                        failureCallback(response.data);
                    if ((response?.data?.Message ?? "") != "") {
                        this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: response.data.Message, life: 3000 });
                    }
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
    ShowLoader = (loader: any) => {
        loader.show({
            color: '#1b406a',
            loader: 'spinner',
            width: 60,
            height: 60,
            backgroundColor: '#ffffff',
            opacity: 0.5,
            zIndex: 9999,
        });
    }
    HideLoader = (loader: any) => {
        loader.hide();
    }
    DisplayMessage = (title: string, body: string, url: string) => {
        this.toast.add({ severity: 'info', summary: title, detail: body, life: 3000 });
    }

    ImportData = (url: string, data: any, callback: any, failureCallback: any, IsCustomeMsg: any = false) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.Post(url, JSON.stringify(data)).then((response: any) => {
            if (response.isAxiosError) {
                this.ShowError(response);
            }
            else {
                if (response.data.StatusCode == 200) {
                    if (callback) {
                        if (response.data.Data)
                            callback(response.data.Data);
                        else
                            callback(response.data);
                    }

                    if (IsCustomeMsg == true) {
                        let msg = ''
                        if ((response.data.Data?.vendingResponse?.SuccessList || []).length > 0) {
                            msg = `${(response.data?.Data?.vendingResponse?.SuccessList || []).length} records have been imported  ${(response?.data?.Data?.vendingResponse?.FailureList || []).length}  records have not been imported e.g.${response?.data?.Data.Failure} etc.`
                            this.toast.add({ severity: 'success', summary: 'Successful', detail: msg, life: 3000 });
                        }
                        else {

                            msg = `${(response.data?.Data?.vendingResponse?.FailureList || []).length}  records have not been imported e.g.${response?.data?.Data.Failure} etc.`
                            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: msg, life: 3000 });
                        }
                    }
                    else {
                        this.toast.add({ severity: 'success', summary: 'Successful', detail: response.data.Message, life: 3000 });
                    }


                }
                else {
                    if (failureCallback)
                        failureCallback(response.data);
                    this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: response.data.Message, life: 3000 });
                }
            }
            this.HideLoader(loader);
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
    ExportData = (url: string, data: any, filename: string, contType: string, exportType: string, callback: any) => {
        const loader = useLoading();
        this.ShowLoader(loader);
        requestMethod.ExportData(url, data).then((data) => {
            const file = new Blob([data.data], { type: contType });
            // process to auto download it
            const fileURL = URL.createObjectURL(file);
            const link = document.createElement("a");
            link.href = fileURL;
            link.download = filename + "." + exportType;
            link.click();
            link.remove();
            this.HideLoader(loader);
            if (callback) {
                callback();
            }
        }).catch((err) => {
            this.toast.add({ severity: 'error', summary: 'Unsuccessful', detail: 'Something went wrong. Please try again after sometime.', life: 3000 });
            this.HideLoader(loader);
        });
    }
}
export default commonModule;
