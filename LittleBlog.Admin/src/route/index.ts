import { createRouter, createWebHashHistory } from "vue-router";
import { RouteRecordRaw } from "vue-router/dist/vue-router";
// views
import Home from "../views/Home.vue";
// article
import ArticleEdit from "../views/Article/ArticleEdit.vue";
import ArticleList from "../views/Article/ArticleList.vue";

const routes: RouteRecordRaw[] = [
    {
        path: "/",
        component: Home,
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
            id: route.params.id, // 函数模式，{id: route.params.id}作为props传递给组件
        }),
    },
];
const routerHistory = createWebHashHistory();

const router = createRouter({
    routes: routes,
    history: routerHistory,
});

// 路由守卫
router.beforeEach((to, from, next) => {
    if (to.matched.length !== 0) {
        next();
    } else {
        next({
            path: "/errors/404",
        });
    }
});

export default router;
