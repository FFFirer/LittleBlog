<template>
    <n-layout>
        <n-layout-header
            ref="leftHeadr"
            id="app-header"
            style="padding: 10px; padding-left: 32px"
            bordered
        >
            <span
                class="site-title"
                @click="gotoHome"
                style="cursor: pointer; margin: 0"
            >
                管理后台
            </span>
            <n-button @click="logout()"> 注销 </n-button>
        </n-layout-header>
        <n-layout has-sider>
            <n-layout-sider bordered>
                <n-menu
                    @update:value="handleUpdateValue"
                    :options="menuOptions"
                    :default-expand-all="true"
                >
                </n-menu>
            </n-layout-sider>
            <n-layout-content id="app-content" style="background-color: #eee">
                <div style="padding: 10px">
                    <router-view> </router-view>
                </div>
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
import api from "../api";

const menuArticleManageKey: string = "article-manage";
const menuCategoryManageKey: string = "category-manage";
const systemConfigKey = "system-config";
const menuSysCfgBaseManageKey: string = "base-config-manage";
const friendshipLinksManageKey: string = "friendship-links-manage";

const menuOptions: Array<MenuOption | MenuGroupOption> = [
    {
        label: () => h("a", {}, "文章管理"),
        key: menuArticleManageKey,
    },
    {
        label: () => h("a", {}, "分类管理"),
        key: menuCategoryManageKey,
    },
    {
        label: () => h("a", {}, "系统配置"),
        key: systemConfigKey,
        children: [
            {
                label: () => h("a", {}, "基础设置"),
                key: menuSysCfgBaseManageKey,
            },
            {
                label: () => h("a", {}, "友情链接配置"),
                key: friendshipLinksManageKey,
            },
        ],
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
        const toCategoryList = () => {
            router.push({
                name: "categoryList",
            });
        };
        const handleUpdateValue = (key: string) => {
            if (key == menuCategoryManageKey) {
                toCategoryList();
            }

            if (key == menuArticleManageKey) {
                toArticleList();
            }

            if (key == menuSysCfgBaseManageKey) {
                toSystemBaseSetting();
            }

            if (key == friendshipLinksManageKey) {
                toFriendshipLinksSetting();
            }
        };
        const toLogin = () => {
            router.push({
                path: "/login",
            });
        };
        const logout = () => {
            api.admin.logout().then((res) => {
                if (res.isSuccess) {
                    store.checkLogout();
                    toLogin();
                }
            });
        };
        const toSystemBaseSetting = () => {
            router.push({
                name: "baseSetting",
            });
        };
        const toFriendshipLinksSetting = () => {
            router.push({
                name: "friendlinksSetting",
            });
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
    overflow: auto;
    height: calc(100vh - 61px);
}

.rightHeader {
    padding-bottom: 10px;
}

/* .header-bar {
    display: flex;
    justify-content: right;
} */

.site-title {
    font-size: 24px;
    font-weight: 600;
}
</style>
