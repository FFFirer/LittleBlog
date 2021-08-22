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

const noLogin = import.meta.env.VITE_NOLOGIN;

// 路由守卫
router.beforeEach((to, from, next) => {
    if (to.matched.length !== 0) {
        to.matched.some((route) => {
            if (route.meta.needLogin && !store.state.isLogin && !noLogin) {
                // 判断是否已经登录
                next({
                    name: "login",
                    params: {
                        path: route.path,
                    },
                });
            }
        });

        next();
    } else {
        next({
            path: "/errors/404",
        });
    }
});

export default router;
