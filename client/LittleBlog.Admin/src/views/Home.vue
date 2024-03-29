<template>
    <n-layout>
        <n-layout-header
            ref="leftHeadr"
            id="app-header"
            style="padding: 10px; padding-left: 32px"
            bordered
        >
            <n-space>
                <span
                    class="site-title"
                    @click="gotoHome"
                    style="cursor: pointer; margin: 0"
                >
                    管理后台
                </span>
                <n-button class="hide-min-640" @click="openMenu">
                    菜单
                </n-button>
                <n-button @click="logout()"> 注销 </n-button>
            </n-space>
        </n-layout-header>
        <n-layout has-sider>
            <n-layout-sider
                bordered
                collapse-mode="transform"
                :collapsed-with="0"
                show-trigger="bar"
                :show-collapsed-content="false"
            >
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
    <n-drawer v-model:show="active" placement="left">
        <n-drawer-content title="管理后台">
            <n-menu
                @update:value="handleUpdateValue"
                :options="menuOptions"
                :default-expand-all="true"
            >
            </n-menu>
        </n-drawer-content>
    </n-drawer>
</template>

<script lang="ts">
import { defineComponent, h, ref } from "vue";
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
const logSummaryKey: string = "logs-summary";
const mdThemeManageKey: string = "markdown-theme-manage";
const defaultMdThemeSetting: string = "default-markdown-theme-setting";

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
            {
                label: () => h("a", {}, "Markdown主题管理"),
                key: mdThemeManageKey,
            },
            {
                label: () => h("a", {}, "Markdown默认主题设置"),
                key: defaultMdThemeSetting,
            },
        ],
    },
    {
        label: () => h("a", {}, "日志管理"),
        key: logSummaryKey,
    },
];

export default defineComponent({
    name: "Home",
    data() {
        return {
            active: false,
        };
    },
    components: {},
    setup() {
        const router = useRouter();
        const active = ref(false);
        const openMenu = () => {
            active.value = true;
        };
        const closeMenu = () => {
            if (active.value) {
                active.value = false;
            }
        };
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
        const toMdThemeManage = () => {
            router.push({
                name: "mdThemeManage",
            });
        };
        const toCategoryList = () => {
            router.push({
                name: "categoryList",
            });
        };
        const toDefaultMdThemeSetting = () => {
            router.push({
                name: "defaultMdTheme",
            });
        };
        const handleUpdateValue = (key: string) => {
            if (key == menuCategoryManageKey) {
                toCategoryList();
            }

            if (key == menuArticleManageKey) {
                toArticleList();
            }

            if (key == mdThemeManageKey) {
                toMdThemeManage();
            }

            if (key == defaultMdThemeSetting) {
                toDefaultMdThemeSetting();
            }

            if (key == menuSysCfgBaseManageKey) {
                toSystemBaseSetting();
            }

            if (key == friendshipLinksManageKey) {
                toFriendshipLinksSetting();
            }

            if (key == logSummaryKey) {
                toLogSummary();
            }

            closeMenu();
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
        const toLogSummary = () => {
            router.push({
                name: "logSummary",
            });
        };

        return {
            gotoHome,
            handleUpdateValue,
            menuOptions,
            logout,
            closeMenu,
            openMenu,
            active,
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

.n-layout-sider .n-layout-toggle-bar {
    right: 0px;
}

/* 小于等于640px时 */
@media screen and (max-width: 640px) {
    .n-layout-sider {
        display: none;
    }

    #app-header {
        padding-left: 10px !important;
    }
}

/* 大于等于641px */
@media screen and (min-width: 641px) {
    .hide-min-640 {
        display: none;
    }
}
</style>
