import { createRouter, createWebHashHistory } from "vue-router";
import { RouteRecordRaw } from "vue-router/dist/vue-router";
// Home, Login
import Login from "../views/Login.vue";
import Home from "../views/Home.vue";
// views
import Welcome from "../views/Welcome.vue";
// article
import ArticleEdit from "../views/Article/ArticleEdit.vue";
import ArticleList from "../views/Article/ArticleList.vue";

// Categories
import CategoryList from "../views/Category/CategoryList.vue";

// Cookies
import Cookie from "js-cookie";

// 系统配置
import BaseSetting from "../views/System/BaseSetting.vue";
import FriendshipLinksSetting from "../views/System/FriendshipLinksSetting.vue";

// 日志
import LogSummary from "../views/LogManage/LogSummary.vue";

// 状态管理
import store from "../store/index";
const routes: RouteRecordRaw[] = [
    {
        path: "/",
        component: Home,
        meta: {
            needLogin: true,
        },
        children: [
            {
                path: "/",
                component: Welcome,
            },
            {
                name: "articleList",
                path: "/articles",
                component: ArticleList,
            },
            {
                name: "articleEdit",
                path: "/articles/edit/:id",
                component: ArticleEdit,
                props: (route) => ({
                    id: parseInt(route.params.id as string), // 函数模式，{id: route.params.id}作为props传递给组件
                }),
            },
            {
                name: "categoryList",
                path: "/categories",
                component: CategoryList,
            },
            {
                name: "baseSetting",
                path: "/system/setting/base",
                component: BaseSetting,
            },
            {
                name: "friendlinksSetting",
                path: "/system/setting/friendship-link",
                component: FriendshipLinksSetting,
            },
            {
                name: "logSummary",
                path: "/logs/summary",
                component: LogSummary,
            },
        ],
    },
    {
        path: "/login",
        component: Login,
        name: "login",
        props: (route) => ({
            return: route.params.path || "/",
        }),
    },
];
const routerHistory = createWebHashHistory();

const router = createRouter({
    routes: routes,
    history: routerHistory,
});

function checkLoginStatus() {
    let loginStatus = Cookie.get("login_status") || "";

    return store.state.isLogin || loginStatus === "logined";
}

// 路由守卫
router.beforeEach((to, from, next) => {
    if (to.matched.length !== 0) {
        if (to.matched.some((route) => route.meta.needLogin)) {
            if (!checkLoginStatus()) {
                next({
                    name: "login",
                    params: {
                        path: to.fullPath,
                    },
                });
            } else {
                next();
            }
        } else {
            next();
        }
    } else {
        next({
            path: "/errors/404",
        });
    }

    // if (to.matched.length !== 0) {
    //     to.matched.some((route) => {
    //         if (route.meta.needLogin && !store.state.isLogin) {
    //             // 判断是否已经登录
    //             next({
    //                 name: "login",
    //                 params: {
    //                     path: route.path,
    //                 },
    //             });
    //         }
    //     });
    //     next();
    // } else {
    // }
});

export default router;
