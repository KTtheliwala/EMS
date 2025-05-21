import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import { defineStore } from "pinia";
// import { AuthService } from '@/composables/api/authService'
// import { useToastStore } from "@/store/useToastStore";
import { useAuthDataStore } from "@/store/useAuthDataStore";
import  { AuthService } from "@/composables/api/authService";

const routes: Array<RouteRecordRaw> = [
    {
        path: "/notfound", name: "error404", component: () => import("@/views/errors/404.vue"),
        meta: { name: "Page not found", requiresAuth: false },
    },
    {
        path: "/:pathMatch(.*)*", name: "NotFound", component: () => import("@/views/errors/404.vue"),
        redirect: { name: "error404" },
        children:[]
    },
    {
        path: "/unauthorized", name: "unauthorized", component: () => import("@/views/errors/401.vue"),
        meta: { name: "unauthorized", requiresAuth: false },
    },
    {
        path: "/forbidden", name: "forbidden", component: () => import("@/views/errors/403.vue"),
        meta: { name: "forbidden", requiresAuth: false },
    },
    {
        path: "/", name: "Login", component: () => import("@/login.vue"),
        meta: { title: "", name: "Login", requiresAuth: false, },
    },
    {
        path: "/Login/ResetPassword", name: "Reset Password", component: () => import("@/resetPassword.vue"),
        meta: { title: "", name: "Reset Password", requiresAuth: false, },
        props: (route) => ({ Id: route.query.Id }),
    },
    {
        path: "/home", name: "Home", components: {
            default: () => import("@/home.vue"),
            layout: () => import("@/components/shared/layout/defaultLayout.vue"),
        },
        meta: { title: "", name: "Home", requiresAuth: true,MainPage:'Home'  },
        redirect: { name: "Dashboard" },
        children: [
            {
                path: "/dashboard", name: "Dashboard", component: () => import("@/views/frontEnd/dashboard.vue"),
                meta: { title: "", name: "Dashboard", requiresAuth: true,MainPage:'Dashboard'  },
            },
            {
                path: "/master", name: "Master", component: () => import("@/views/frontEnd/master.vue"),
                meta: { title: "CakeSmiths | Master", name: "Master", 
                requiresAuth: true,
                TabName:'master',
                MainPage:'Master'  },
                redirect: { name: "Company" },
                children: [
                    {
                        path: "/master/category", name: "Category", component: () => import("@/views/frontEnd/master/category/category-list.vue"),
                        meta: { title: "CakeSmiths | Master | Category", name: "Category", requiresAuth: true, children: [], 
                        TabName:'Category',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/product", name: "Product", component: () => import("@/views/frontEnd/master/products/products-list.vue"),
                        meta: { title: "CakeSmiths | Master | Product", name: "Product", requiresAuth: true, children: [], 
                        TabName:'Product',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/company", name: "Company", component: () => import("@/views/frontEnd/master/company/company-list.vue"),
                        meta: { title: "CakeSmiths | Master | Company", name: "Company", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/transpoter", name: "Transpoter", component: () => import("@/views/frontEnd/master/transpoter/transpoter-list.vue"),
                        meta: { title: "CakeSmiths | Master | Transpoter", name: "Transpoter", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/group", name: "Group", component: () => import("@/views/frontEnd/master/group/group-list.vue"),
                        meta: { title: "CakeSmiths | Master | Group", name: "Group", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/subGroup", name: "Sub Group", component: () => import("@/views/frontEnd/master/subGroup/subGroup-list.vue"),
                        meta: { title: "CakeSmiths | Master | Sub Group", name: "Sub Group", requiresAuth: true, children: [],
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/quality", name: "Quality", component: () => import("@/views/frontEnd/master/quality/quality-list.vue"),
                        meta: { title: "CakeSmiths | Master | Quality", name: "Quality", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/comapny-filed", name: "Company Filed", component: () => import("@/views/frontEnd/master/companyFiled/companyFiled-list.vue"),
                        meta: { title: "CakeSmiths | Master | Company Filed", name: "Company Filed", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/db-filed", name: "DB Filed", component: () => import("@/views/frontEnd/master/dbFiled/dbFiled-list.vue"),
                        meta: { title: "CakeSmiths | Master | DB Filed", name: "DB Filed", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/field-mapping", name: "Field Mapping", component: () => import("@/views/frontEnd/master/fieldMapping/fieldMapping-list.vue"),
                        meta: { title: "CakeSmiths | Master | Field Mapping", name: "Field Mapping", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/broker", name: "Broker", component: () => import("@/views/frontEnd/master/broker/broker-list.vue"),
                        meta: { title: "CakeSmiths | Master | Broker", name: "Broker", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                    {
                        path: "/master/customer", name: "Customer", component: () => import("@/views/frontEnd/master/customer/customer-list.vue"),
                        meta: { title: "CakeSmiths | Master | Customer", name: "Customer", requiresAuth: true, children: [], 
                        TabName:'master',
                        MainPage:'Master' 
                        },
                    },
                ]
            },
            {
                path: "/admintools", name: "AdminTools", component: () => import("@/views/frontEnd/admintools.vue"),
                meta: { title: "CakeSmiths | Admin Tools", name: "AdminTools", 
                requiresAuth: true,
                TabName:'admintools',
                MainPage:'AdminTools'  },
                redirect: { name: "User" },
                children: [
                    {
                        path: "/admintools/users", name: "User", component: () => import("@/views/frontEnd/admintools/users/user-list.vue"),
                        meta: { title: "", name: "User", requiresAuth: true,MainPage:'AdminTools'  },
                    },
                    {
                        path: "/admintools/role", name: "Role", component: () => import("@/views/frontEnd/admintools/roles/roles-list.vue"),
                        meta: { title: "", name: "Role", requiresAuth: true,MainPage:'AdminTools'  },
                    },
                    {
                        path: "/admintools/system-setting", name: "SystemSetting", component: () => import("@/views/frontEnd/admintools/websetting/websiteSettings.vue"),
                        meta: { title: "", name: "SystemSetting", requiresAuth: true,MainPage:'AdminTools'  },
                    },
                    {
                        path: "/admintools/email-template", name: "EmailTemplate", component: () => import("@/views/frontEnd/admintools/emailtemplates/emailTemplateList.vue"),
                        meta: { title: "", name: "EmailTemplate", requiresAuth: true,MainPage:'AdminTools'  },
                    },                    
                    {
                        path: "/admintools/mail-list", name: "MailList", component: () => import("@/views/frontEnd/admintools/mailList/mail-Queue.vue"),
                        meta: { title: "", name: "MailList", requiresAuth: true,MainPage:'AdminTools'  },
                    },
                    {
                        path: "/admintools/audit-logs", name: "AuditLogs", component: () => import("@/views/frontEnd/admintools/auditLog/audit-log.vue"),
                        meta: { title: "", name: "AuditLogs", requiresAuth: true,MainPage:'AdminTools'  },
                    },
                ]
            } ,
            {
                path: "/orders", name: "Orders", component: () => import("@/views/frontEnd/orders.vue"),
                meta: { title: "CakeSmiths | Orders", name: "Orders", 
                requiresAuth: true,
                TabName:'orders',
                MainPage:'Orders'  },
                redirect: { name: "OrderList" },
                children: [
                    {
                        path: "/orders/order-list", name: "OrderList", component: () => import("@/views/frontEnd/orders/order-list.vue"),
                        meta: { title: "", name: "OrderList", requiresAuth: true,MainPage:'Orders'  },
                    },
                    {
                        path: "/orders/order-manage", name: "OrderManage", component: () => import("@/views/frontEnd/orders/order-manage.vue"),
                        meta: { title: "", name: "OrderManage", requiresAuth: true,MainPage:'Orders'  },
                    },
                    {
                        path: "/orders/view-order", name: "ViewOrder", component: () => import("@/views/frontEnd/orders/view-order.vue"),
                        meta: { title: "", name: "ViewOrder", requiresAuth: true,MainPage:'Orders'  },
                    },                    
                    {
                        path: "/orders/order-report", name: "OrderReport", component: () => import("@/views/frontEnd/orders/order-report.vue"),
                        meta: { title: "", name: "OrderReport", requiresAuth: true,MainPage:'Orders'  },
                    },
                ]
            }
           
        ],
    }       

];
const router = createRouter({
    history: createWebHistory(process.env.VUE_APP_BASE_URL),
    routes,
});

router.beforeEach(async (to, from, next) => {
    const authLogin = useAuthDataStore().getAuth as any;    
    if (to.meta.requiresAuth) {
        let prm = true
        if((to?.meta?.MainPage ?? '') == 'Dashboard' && (authLogin?.Token ?? "").length > 0)
        {
            prm=true
        }
        return (authLogin?.Token ?? "").length == 0 ? next({ name: "Login", query: { redirect: to.fullPath, } }) : next();
    }
    else return next();
    

})
export default router;
