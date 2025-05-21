import { Component, createApp, ref } from "vue";

import App from "@/App.vue";
import router from "@/router";

//PRIMEVUE
//import "primevue/resources/primevue.min.css";
import 'primevue/resources/themes/aura-light-blue/theme.css'
import PrimeVue from "primevue/config";
//PRIMEFLEX
import "primeflex/primeflex.css";
import "primeicons/primeicons.css";
//CUSTOM THEME
import "@/assets/css/themeprimevue.scss";
import "@/assets/css/primevuestyle.scss";
import "@/assets/css/style.scss";
// import "@/assets/css/ac.scss";

//PRIMEVUE COMPONENTS
import Badge from "primevue/badge"; // Badge
import Avatar from "primevue/avatar"; // Avtar
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";
import Toast from "primevue/toast";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Dialog from "primevue/dialog";
import DynamicDialog from 'primevue/dynamicdialog';
import DialogService from 'primevue/dialogservice';
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import Toolbar from "primevue/toolbar";
import Menubar from "primevue/menubar";
import SplitButton from "primevue/splitbutton";
import TabView from "primevue/tabview";
import TabPanel from "primevue/tabpanel";
import InputSwitch from "primevue/inputswitch";
import ScrollPanel from "primevue/scrollpanel";
import ToastService from "primevue/toastservice";
import ConfirmationService from "primevue/confirmationservice";
import ConfirmDialog from 'primevue/confirmdialog';
import Calendar from "primevue/calendar";
import Password from "primevue/password";
import MultiSelect from "primevue/multiselect";
import ProgressSpinner from "primevue/progressspinner";
import Menu from "primevue/menu";
import RadioButton from "primevue/radiobutton";
import Checkbox from "primevue/checkbox";
import FileUpload from "primevue/fileupload";
import Tooltip from "primevue/tooltip";
import Chip from "primevue/chip";
import ToggleButton from "primevue/togglebutton";
import Galleria from "primevue/galleria";
import Accordion from "primevue/accordion";
import AccordionTab from "primevue/accordiontab";
import PanelMenu from "primevue/panelmenu";
import AutoComplete from "primevue/autocomplete";
import Sidebar from "primevue/sidebar";
import Textarea from "primevue/textarea";
import Fieldset from 'primevue/fieldset';
import Chips from 'primevue/chips';
import OverlayPanel from 'primevue/overlaypanel';
import Tag from 'primevue/tag';
import Editor from 'primevue/editor';
import Stepper from 'primevue/stepper';
import StepperPanel from 'primevue/stepperpanel';
import Divider from 'primevue/divider';

// ---------------------- common Component --------------------------
import asyncComponent from "@/components/asyncComponent.vue";

// Shared Component
import commonForm from "@/components/shared/commonControls/commonForm.vue";
import commonInput from "@/components/shared/commonControls/commonText.vue";
import commonNumber from "@/components/shared/commonControls/commonNumber.vue";
import commonButton from "@/components/shared/commonControls/commonButton.vue";
import commonPassword from "@/components/shared/commonControls/commonPassword.vue";
import commonDropdown from "@/components/shared/commonControls/commonDropdown.vue";
import commonMultiSelect from "@/components/shared/commonControls/commonMultiSelect.vue";
import commonDate from "@/components/shared/commonControls/commonDate.vue";
import commonArea from "@/components/shared/commonControls/commonArea.vue";
import commonGrid from "@/components/shared/commonControls/commonGrid.vue";
import commonDialog from "@/components/shared/commonControls/commonDialog.vue";
import commonActionButton from "@/components/shared/commonControls/commonActionButton.vue";
import commonLoader from "@/components/shared/commonControls/commonLoader.vue";
import commonFileUpload from "@/components/shared/commonControls/commonFileUpload.vue";
import commonBlobFileView from "@/components/shared/commonControls/commonBlobFileView.vue";
import commonGroupMultiSelect from "@/components/shared/commonControls/commonGroupMultiSelect.vue";
import commonEditor from "@/components/shared/commonControls/commonEditor.vue";
import commonImportFile from "@/components/shared/commonControls/commonImportFile.vue";


//#endregion ------------------------------------------------
//#region  :-----------STATE MENEGMENT -------------------------------
import { createPinia } from "pinia";
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
//#endregion----------------------------------------------------------------
import filters from '@/helpers/filters';
import VueDOMPurifyHTML from 'vue-dompurify-html';

export default class Application {
    readonly application;
    readonly json = ref([] as any);
    constructor() {
        this.application = createApp(App);
        this.application.config.globalProperties.$filters = filters;
        this.application.provide("msalInstance", {});
        this.application.use(router);
        this.setDefaultPlugins();
        this.setDefaultRequiredPrimeVueComponents();
        this.ExtraComponent();
    }
    mount(): void {
        this.application.directive("tooltip", Tooltip);
        const pinia = createPinia()
        pinia.use(piniaPluginPersistedstate)
        this.application.use(pinia);
        this.application.use(ToastService);
        this.application.use(ConfirmationService);
        this.application.use(DialogService);
        this.application.use(VueDOMPurifyHTML,{
            default: {
                ALLOWED_ATTR: ['target','src','href','style','width','height']
            }
        })
        this.application.config.warnHandler = () => null;
        this.application.mount("#app");
    }
    addGlobalComponent(componentName: string, component: Component): void {
        this.application.component(componentName, component);
    }
    private setDefaultPlugins() {
        this.application.use(PrimeVue, {
            ripple: true,
            inputStyle: "outlined",
            csp: {
                nonce: '56b+97z7RhNTcMh4VKGXJg=='
            }        
        });
    }
    private setDefaultRequiredPrimeVueComponents() {
        /** Add only those components which are required for overall application (Global) */
        this.application.component("Badge", Badge); // Badge
        this.application.component("Avatar", Avatar); // Avtar
        this.application.component("Button", Button);
        this.application.component("Dropdown", Dropdown);
        this.application.component("Dialog", Dialog);
        this.application.component("DynamicDialog", DynamicDialog);
        this.application.component("Divider", Divider);
        this.application.component("InputText", InputText);
        this.application.component("InputNumber", InputNumber);
        this.application.component("Toast", Toast);
        this.application.component("Toolbar", Toolbar);
        this.application.component("DataTable", DataTable);
        this.application.component("Column", Column);
        this.application.component("Menubar", Menubar);
        this.application.component("TabView", TabView);
        this.application.component("TabPanel", TabPanel);
        this.application.component("InputSwitch", InputSwitch);
        this.application.component("ScrollPanel", ScrollPanel);
        this.application.component("Calendar", Calendar);
        this.application.component("Password", Password);
        this.application.component("MultiSelect", MultiSelect);
        this.application.component("ProgressSpinner", ProgressSpinner);
        this.application.component("SplitButton", SplitButton);
        this.application.component("Menu", Menu);
        this.application.component("RadioButton", RadioButton);
        this.application.component("Checkbox", Checkbox);
        this.application.component("FileUpload", FileUpload);
        this.application.component("Chip", Chip);
        this.application.component("ToggleButton", ToggleButton);
        this.application.component("Galleria", Galleria);
        this.application.component("Accordion", Accordion);
        this.application.component("AccordionTab", AccordionTab);
        this.application.component("PanelMenu", PanelMenu);
        this.application.component("AutoComplete", AutoComplete);
        this.application.component("Sidebar", Sidebar);
        this.application.component("Textarea", Textarea);
        this.application.component("Fieldset", Fieldset);
        this.application.component("Chips", Chips);
        this.application.component("ConfirmDialog", ConfirmDialog);
        this.application.component("OverlayPanel", OverlayPanel);
        this.application.component("Tag", Tag);
        this.application.component("Editor", Editor);
        this.application.component("Stepper", Stepper);
        this.application.component("StepperPanel", StepperPanel);
        
    }
    private ExtraComponent() {
        this.application.component("commonForm", commonForm);
        this.application.component("asyncComponent", asyncComponent);
        this.application.component("commonInput", commonInput);
        this.application.component("commonNumber", commonNumber);
        this.application.component("commonButton", commonButton);
        this.application.component("commonPassword", commonPassword);
        this.application.component("commonDropdown", commonDropdown);
        this.application.component("commonMultiSelect", commonMultiSelect);
        this.application.component("commonDate", commonDate);
        this.application.component("commonArea", commonArea);
        this.application.component("commonGrid", commonGrid);
        this.application.component("commonDialog", commonDialog);
        this.application.component("commonActionButton", commonActionButton);
        this.application.component("commonLoader", commonLoader);
        this.application.component("commonFile", commonFileUpload);
        this.application.component("commonBlobFile", commonBlobFileView);
        this.application.component("commonGroupMultiSelect", commonGroupMultiSelect);
        this.application.component("commonEditor", commonEditor);
        this.application.component("commonImportFile", commonImportFile);
        
    }
}

