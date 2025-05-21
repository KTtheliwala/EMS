<template>
    <div class="detail-wrap">
        <div class="grid guter-change">
            <div class="col-12 md:col-12">
                <h3 class="role-title"><span>Role:</span> {{ IsPopup.Name }}</h3>
            </div>
            <div class="col-12 md:col-12">
                <div class="Role-table-wrapper mb-0">
                    <table class="Role-table">
                        <thead>
                            <tr>
                                <th>Modules</th>
                                <th class="Role-Drop-Selection">
                                    <label for="View">View</label>
                                    &nbsp;
                                    <Checkbox inputId="ViewAll" name="ViewAll" value="ViewAll" :binary="true"
                                        @change="CheckViewAll(ViewAll)" v-model="ViewAll" />
                                </th>
                                <th class="Role-Drop-Selection">
                                    <label for="Create">Create</label>
                                    &nbsp;
                                    <Checkbox inputId="CreateAll" name="CreateAll" value="CreateAll" :binary="true"
                                        @change="CheckCreateAll(CreateAll)" v-model="CreateAll"
                                        :disabled="IsDisable" />
                                </th>
                                <th class="Role-Drop-Selection">
                                    <label for="Edit">Edit</label>
                                    &nbsp;
                                    <Checkbox inputId="EditAll" name="EditAll" value="EditAll" :binary="true"
                                        @change="CheckEditAll(EditAll)" v-model="EditAll" :disabled="IsDisable" />
                                </th>
                                <th class="Role-Drop-Selection">
                                    <label for="Delete">Delete</label>
                                    <Checkbox inputId="DeleteAll" name="DeleteAll" value="DeleteAll" :binary="true"
                                        @change="CheckDeleteAll(DeleteAll)" v-model="DeleteAll"
                                        :disabled="IsDisable" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="item of uniqueModule" :key="item">
                                <tr role="row">
                                    <td class="title-strong">{{ item.ModuleName }}
                                    </td>
                                    <td class="title-strong">
                                        <Checkbox :inputId="item.ModuleName" :name="item.ModuleName" :value="item"
                                            :binary="true" @change="CheckRole(item.IsView, item, 'IsView')"
                                            v-model="item.IsView" v-if="item.IsShowView" />
                                    </td>
                                    <td class="title-strong">
                                        <Checkbox :inputId="item.ModuleName" :name="item.ModuleName" :value="item"
                                            :binary="true" @change="CheckRole(item.IsAdd, item, 'IsAdd')"
                                            v-model="item.IsAdd" :disabled="!item.IsView" v-if="item.IsShowAdd" />
                                    </td>
                                    <td class="title-strong">
                                        <Checkbox :inputId="item.ModuleName" :name="item.ModuleName" :value="item"
                                            :binary="true" @change="CheckRole(item.IsEdit, item, 'IsEdit')"
                                            v-model="item.IsEdit" :disabled="!item.IsView" v-if="item.IsShowEdit" />
                                    </td>
                                    <td class="title-strong">
                                        <Checkbox :inputId="item.ModuleName" :name="item.ModuleName" :value="item"
                                            :binary="true" @change="CheckRole(item.IsDelete, item, 'IsDelete')"
                                            v-model="item.IsDelete" :disabled="!item.IsView"
                                            v-if="item.IsShowDelete" />
                                    </td>
                                </tr>
                                <tr role="row"
                                    v-for="item1 of objPermissionList.filter(x => x.ModuleName == item.ModuleName)"
                                    :key="item1">
                                    <td>
                                        {{ item1.PageName }}
                                    </td>
                                    <td>
                                        <Checkbox :inputId="'IsView' + item1.PageId" :name="'IsView' + item1.PageId"
                                            :value="item1"
                                            @change="SelectCheckRole(item1.PageId, item1.IsView, 'IsView', item1.Id, item1)"
                                            :binary="true" v-model="item1.IsView" v-if="item1.IsShowView" />
                                    </td>
                                    <td>
                                        <Checkbox :inputId="'IsAdd' + item1.PageId" :name="'IsAdd' + item1.PageId"
                                            :value="item1"
                                            @change="SelectCheckRole(item1.PageId, $event, 'IsAdd', item1.Id, item1)"
                                            :binary="true" v-model="item1.IsAdd" :disabled="!item1.IsView"
                                            v-if="item1.IsShowAdd" />
                                    </td>
                                    <td>
                                        <Checkbox :inputId="'IsEdit' + item1.PageId" :name="'IsEdit' + item1.PageId"
                                            :value="item1"
                                            @change="SelectCheckRole(item1.PageId, $event, 'IsEdit', item1.Id, item1)"
                                            :binary="true" v-model="item1.IsEdit" :disabled="!item1.IsView"
                                            v-if="item1.IsShowEdit" />
                                    </td>
                                    <td>
                                        <Checkbox :inputId="'IsDelete' + item1.PageId"
                                            :name="'IsDelete' + item1.PageId" :value="item1"
                                            @change="SelectCheckRole(item1.PageId, $event, 'IsDelete', item1.Id, item1)"
                                            :binary="true" v-model="item1.IsDelete" :disabled="!item1.IsView"
                                            v-if="item1.IsShowDelete" />
                                    </td>
                                </tr>
                            </template>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="p-dialog-footer">
        <div></div>
        <div>
            <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseModal" />
            <Button :label="'Save'" icon="pi pi-plus-circle" type="button" @click="savePermissions" autofocus />
        </div>
    </div>
    <comnet-dialog ref="popupManage"></comnet-dialog>
</template>

<script setup lang="ts">
import { inject, ref } from "vue";
import commonModule from "@/composables/modules/commonModule";
import UrlConstants from "@/utils/urlconstants";

const dialogRef = inject('dialogRef') as any;
let IsPopup = dialogRef?.value?.data
const { GetData, PostData } = new commonModule();
const uniqueModule = ref([]);
const objPermissionList = ref([]);
const savePermissionList = ref([]);
const IsDisable = ref(true);
const ViewAll = ref(false);
const CreateAll = ref(false);
const EditAll = ref(false);
const DeleteAll = ref(false);

const BindPermissionData = async () => {
    await GetData(UrlConstants.apiGetPermissionBasedOnRole + '?id=' + IsPopup.Id, (data: any) => {
        objPermissionList.value = data.PermissionList;
        data.PermissionList.forEach(function (item: any) {
            const i = uniqueModule.value.findIndex((x: any) => x.ModuleName == item.ModuleName);
            if (i <= -1) {
                uniqueModule.value.push({
                    ModuleName: item.ModuleName,
                    Id: item.Id,
                    IsView: false,
                    IsAdd: false,
                    IsEdit: false,
                    IsDelete: false,
                    IsDisable: true,
                    IsUpdate: false,
                    IsShowMenu: false,
                    IsShowSearch: false,
                    IsShowAdd: false,
                    IsShowEdit: false,
                    IsShowDelete: false,
                    IsShowView: false,
                    ModuleTypeId: item.ModuleTypeId
                });
            }
        });
        
        uniqueModule.value.forEach(function (item: any) {            
            item.IsView = (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsView).length > 0) ? true : false;
            item.IsAdd = (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsAdd).length > 0) ? true : false;
            item.IsEdit = (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsEdit).length > 0) ? true : false;
            item.IsDelete = (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsDelete).length > 0) ? true : false;
            item.IsUpdate = (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsUpdate).length > 0) ? true : false;
            item.IsDisable = ((data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsShowView && x.IsView).length > 0) ? false : true);
            item.IsShowSearch= false;
            item.IsShowAdd= (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsShowAdd).length > 0) ? true : false;
            item.IsShowEdit= (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsShowEdit).length > 0) ? true : false;
            item.IsShowDelete= (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsShowDelete).length > 0) ? true : false;
            item.IsShowView= (data.PermissionList.filter((x: any) => x.Id == item.Id && x.IsShowView).length > 0) ? true : false;
        });
        //End 
        IsDisable.value  =((data.PermissionList.filter((x: any) => x.IsShowView && x.IsView).length > 0) ? false : true);
        objPermissionList.value = data.PermissionList;
        ResetTopHeaderCheckbox();
    }, null);
};
BindPermissionData();


const CheckViewAll = (value: boolean) => {
  

    objPermissionList.value.forEach((x: any) => {
        x.IsView = value;
    });
    if (!value) {
        objPermissionList.value.forEach((x: any) => {
            x.IsAdd = value;
            x.IsEdit = value;
            x.IsDelete = value;
            x.IsUpdate = value;
        });
        uniqueModule.value.forEach((x: any) => {
            x.IsAdd = value;
            x.IsEdit = value;
            x.IsDelete = value;
            x.IsUpdate = value;            
        });
        ViewAll.value = value
        CreateAll.value = value;
        EditAll.value = value;
        DeleteAll.value = value;
    }
    IsDisable.value  =((objPermissionList.value.filter((x: any) => x.IsShowView == true && x.IsView == true).length > 0) ? false : true);
    uniqueModule.value.forEach(function (item: any) {
        item.IsView = value;        
        item.IsDisable = ((objPermissionList.value.filter((x) => x?.Id == item.Id && x?.IsShowView == true && x?.IsView == true).length > 0) ? false : true);
    });
};
const CheckCreateAll = (value: boolean) => { 

    let lstDataEditUniqModel =  uniqueModule.value.filter((x: any) => x.IsView == true && x.IsShowAdd == true);
    lstDataEditUniqModel.forEach((x: any) => {
            x.IsAdd = value;
        });
   let lstDataEdit =  objPermissionList.value.filter((x: any) => x.IsView == true && x.IsShowAdd == true);
   lstDataEdit.forEach((x: any) => {
            x.IsAdd = value;
        });

};
const CheckEditAll = (value: boolean) => {    
    let lstDataEditUniqModel =  uniqueModule.value.filter((x: any) => x.IsView == true && x.IsShowEdit == true);
    lstDataEditUniqModel.forEach((x: any) => {
            x.IsEdit = value;
        });
   let lstDataEdit =  objPermissionList.value.filter((x: any) => x.IsView == true && x.IsShowEdit == true);
   lstDataEdit.forEach((x: any) => {
            x.IsEdit = value;
        });
    
};
const CheckDeleteAll = (value: boolean) => {
 
    let lstDataEditUniqModel =  uniqueModule.value.filter((x: any) => x.IsView == true && x.IsShowDelete == true);
    lstDataEditUniqModel.forEach((x: any) => {
            x.IsDelete = value;
        });
   let lstDataEdit =  objPermissionList.value.filter((x: any) => x.IsView == true && x.IsShowDelete == true);
   lstDataEdit.forEach((x: any) => {
            x.IsDelete = value;
        });
};
const CheckRole = async (flag: any, val: any, event: any) => {
    if (val.ModuleName !== "" && event === "IsView") {
        let lstData = objPermissionList.value.filter((x: any) => x.ModuleName == val.ModuleName && x.IsShowView == true);
        let uniqueModuleData = uniqueModule.value.filter((x: any) => x.ModuleName == val.ModuleName);
        lstData.forEach((x: any) => {
            x.IsView = flag;
        });
        if (!flag) {
            lstData.forEach((x: any) => {
                x.IsAdd = false;
                x.IsEdit = false;
                x.IsDelete = false;
                x.IsUpdate = false;
            });
            uniqueModuleData.forEach(function (item: any) {
                item.IsAdd = false;
                item.IsEdit = false;
                item.IsDelete = false;
                item.IsUpdate = false;
            });
        }
    }

    if (val.ModuleName !== "" && event === "IsAdd") {
        let lstData = objPermissionList.value.filter((x: any) => x.ModuleName == val.ModuleName && x.IsView == true && x.IsShowView == true);
        lstData.forEach((x: any) => {
            x.IsAdd = flag;
        });
    }

    if (val.ModuleName !== "" && event === "IsEdit") {
        let lstData = objPermissionList.value.filter((x: any) => x.ModuleName == val.ModuleName && x.IsView == true && x.IsShowEdit == true);
        lstData.forEach((x: any) => {
            x.IsEdit = flag;
        });
    }

    if (val.ModuleName !== "" && event === "IsUpdate") {
        let lstData = objPermissionList.value.filter((x: any) => x.ModuleName == val.ModuleName && x.IsUpdate == true && x.IsShowEdit == true);
        lstData.forEach((x: any) => {
            x.IsUpdate = flag;
        });
    }

    if (val.ModuleName !== "" && event === "IsDelete") {
        let lstData = objPermissionList.value.filter((x: any) => x.ModuleName == val.ModuleName && x.IsView == true && x.IsShowDelete == true);
        lstData.forEach((x: any) => {
            x.IsDelete = flag;
        });
    }


    ResetTopHeaderCheckbox();

};
const SelectCheckRole = async (val: any, event: any, opnFor: any, moduleId: number, obj: any) => {
    if (!obj.IsView) {
        let lstData = objPermissionList.value.filter((x: any) => x.PageId == obj.PageId);
        lstData.forEach((x: any) => {
            x.IsAdd = false;
            x.IsEdit = false;
            x.IsDelete = false;
            x.IsUpdate = false;
        });
    }
    uniqueModule.value.forEach(function (item: any) {
        item.IsView = (objPermissionList.value.filter((x: any) => x.Id == item.Id && x.IsView == true).length > 0) ? true : false;
        item.IsAdd = (objPermissionList.value.filter((x: any) => x.Id == item.Id && x.IsAdd == true).length > 0) ? true : false;
        item.IsEdit = (objPermissionList.value.filter((x: any) => x.Id == item.Id && x.IsEdit == true).length > 0) ? true : false;
        item.IsDelete = (objPermissionList.value.filter((x: any) => x.Id == item.Id && x.IsDelete == true).length > 0) ? true : false;
        item.IsUpdate = (objPermissionList.value.filter((x: any) => x.Id == item.Id && x.IsUpdate == true).length > 0) ? true : false;
        item.IsDisable = ((objPermissionList.value.filter((x) => x?.Id == item.Id && x?.IsShowView == true && x?.IsView == true).length > 0) ? false : true);
    });
    
    ResetTopHeaderCheckbox();
};
const ResetTopHeaderCheckbox = () => {
    ViewAll.value = (objPermissionList.value.filter((x: any) => x.IsView == false).length > 0) ? false : true;
    EditAll.value = (objPermissionList.value.filter((x: any) => x.IsEdit == false && x.IsShowEdit == true).length > 0) ? false : true;
    CreateAll.value = (objPermissionList.value.filter((x: any) => x.IsAdd == false && x.IsShowAdd == true).length > 0) ? false : true;
    DeleteAll.value = (objPermissionList.value.filter((x: any) => x.IsDelete == false && x.IsShowDelete == true).length > 0) ? false : true;
};
const savePermissions = async () => {    
    savePermissionList.value = [];
    objPermissionList.value.forEach((newitems: any) => {
        if (newitems.PageId > 0) {            
            savePermissionList.value?.push({
                Id: newitems.PermissionId,
                RoleId: IsPopup.Id,
                PageId: newitems.PageId,
                PageName: newitems.PageName,
                IsAdd: newitems.IsAdd,
                IsEdit: newitems.IsEdit,
                IsDelete: newitems.IsDelete,
                IsView: newitems.IsView,
                IsUpdate: newitems.IsUpdate
            })
        }
    });

    PostData(UrlConstants.apiSavePermissions, savePermissionList.value, "Permission successfully added.", (data: any) => {
        CloseModal()
    }, null);
}


const CloseModal = () => {
    dialogRef.value.close();
}
</script>