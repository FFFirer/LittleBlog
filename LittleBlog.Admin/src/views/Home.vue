<template>
    <n-layout>
        <n-layout has-sider>
            <n-layout-sider bordered>
                <n-layout-header
                    ref="leftHeadr"
                    id="app-header"
                    style="padding: 10px; padding-left: 32px"
                    bordered
                >
                    <h1 @click="gotoHome" style="cursor: pointer; margin: 0">
                        LittleBlog 管理后台
                    </h1>
                </n-layout-header>
                <n-menu
                    @update:value="handleUpdateValue"
                    :options="menuOptions"
                >
                </n-menu>
            </n-layout-sider>
            <n-layout-content
                id="app-content"
                content-style="padding-left: 10px; padding-right: 10px"
            >
                <n-layout-header ref="rightHeader" bordered class="rightHeader">
                    <div class="header-bar">
                        <n-button @click="logout()"> 注销 </n-button>
                    </div>
                </n-layout-header>
                <router-view> </router-view>
            </n-layout-content>
        </n-layout>
    </n-layout>
</template>

<script lang="ts">
import { defineComponent, h } from "vue";
import store from "../store/index";

// naive-ui 类型定义
import { MenuOption } from "naive-ui/lib/menu/index";
import { MenuGroupOption } from "naive-ui/lib/menu/src/interface";

import { useRouter } from "vue-router";

const menuArticleManageKey: string = "article-manage";
const menuCategoryManageKey: string = "category-manage";
const menuOptions: Array<MenuOption | MenuGroupOption> = [
    {
        label: () => h("a", {}, "文章管理"),
        key: menuArticleManageKey,
    },
    {
        label: () => h("a", {}, "分类管理"),
        key: menuCategoryManageKey,
    },
];

export default defineComponent({
    name: "Home",
    components: {},
    setup() {
        const router = useRouter();
        const gotoHome = () => {
            router.push({
                path: "/",
            });
        };
        const toArticleList = () => {
            router.push({
                name: "articleList",
            });
        };
        const toCategoryList = () => {};
        const handleUpdateValue = (key: string) => {
            if (key == menuCategoryManageKey) {
                toCategoryList();
            }

            if (key == menuArticleManageKey) {
                toArticleList();
            }
        };
        const toLogin = () => {
            router.push({
                path: "/login",
            });
        };
        const logout = () => {
            store.checkLogout();
            toLogin();
        };
        return {
            gotoHome,
            handleUpdateValue,
            menuOptions,
            logout,
        };
    },
});
</script>

<style>
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    /* text-align: center; */
    color: #2c3e50;
}

#app-content {
    padding-top: 10px;
    height: 100vh;
}

.rightHeader {
    padding-bottom: 10px;
}

.header-bar {
    display: flex;
    justify-content: right;
}
</style>
