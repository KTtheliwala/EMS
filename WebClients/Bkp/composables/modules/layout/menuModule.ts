import { ref, reactive, inject } from "vue";
import constants from "@/utils/constants";
import CommonHelper from "@/utils/commonHelper";

class menuModule {
  //-------------------------------PROPERTY---------------------------------------------//
  readonly homeAppGetMenuObj = inject("AppModuleObj") as any;
  readonly GetCurrentMenuObj = inject("MenuStoreObj") as any;
  readonly sidebarActive = ref(false);
  readonly layoutStatic = ref(false);
  readonly isLogin = ref(true);
  readonly openedItems = reactive({} as any);
  readonly showMenuIcon = ref(false);
  readonly isActive = ref(false);
  readonly updatepopupRefs = ref({} as any);
  readonly Menulist = ref(this?.homeAppGetMenuObj?.Menulist ?? []);

  //----------------------------------EVENT-----------------------------------------------//
  onTopBarMenuClickBtn = (event: any) => {
    this.sidebarActive.value =event==false?event: !this.sidebarActive.value;
    this.layoutStatic.value =event==false?event: !this.layoutStatic.value;
  };
  bodyClickEvent = () => {
    if (screen.width < 992) {
      this.sidebarActive.value = false;
      this.layoutStatic.value = false;
    }
  };
  loginFunction = () => {
    this.isLogin.value = false;
  };
  // For Open submenu--------

  collspasedItem = (
    index: any,
    items: any,
    id: any,
    emits: any,
    IsMobielView: boolean
  ) => {
    this.setCurrentMenuSelection(items.MainPageCode);
    if (items.ModuleType == constants.ExternalModule) {
      this.updatepopupRefs.value.OpenPopUp(true, items.Url);
      setTimeout(
        () =>
          CommonHelper.OpenNewTab(
            items.Url,
            `CakeSmiths | ${items.MainPageCode} (external)`
          ),
        500
      );
    }
    if (items.children != null) {
      this.openedItems[index] = !this.openedItems[index];
    }
    if (items.ModuleType !== constants.SubModule && IsMobielView)
      emits("sideNavClose", null);
  };
  SubMenucollspasedItem = (items: any, emits: any, IsMobielView: boolean) => {
    this.GetCurrentMenuObj.SetChidlMenuName(items.MenuName || items.PageName);
    if (items.ModuleType == constants.ExternalModule) {
      this.updatepopupRefs.value.OpenPopUp(true, items.Url);
      setTimeout(
        () =>
          CommonHelper.OpenNewTab(
            items.Url,
            `CakeSmiths | ${items.MainPageCode} | ${items.MenuName} (external)`
          ),
        500
      );
    }
    if (items.ModuleType == constants.InternalModule && IsMobielView)
      emits("sideNavClose", null);
  };
  getCurrentMenuSelection = (MenuName: any, type: number) => {
    return (type === 1
      ? this.GetCurrentMenuObj.getParentName
      : this.GetCurrentMenuObj.getChildName) === MenuName
      ? true
      : false;
  };
  setCurrentMenuSelection = (MenuName: any) => {
    this.GetCurrentMenuObj.SetMenuName(MenuName);
    this.getCurrentMenuSelection(MenuName, 1);
  };
}
export default menuModule;
