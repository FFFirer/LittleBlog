<template>
    <n-layout>
        <n-message-provider>
            <n-layout has-sider>
                <n-layout-sider>
                    <n-layout-header
                        id="app-header"
                        style="padding: 10px; padding-left: 32px"
                    >
                        <h1
                            @click="gotoHome"
                            style="cursor: pointer; margin: 0"
                        >
                            管理后台
                        </h1>
                    </n-layout-header>
                    <n-menu
                        @update:value="handleUpdateValue"
                        :options="menuOptions"
                    >
                    </n-menu>
                </n-layout-sider>
                <n-layout-content id="app-content">
                    <router-view> </router-view>
                </n-layout-content>
            </n-layout>
        </n-message-provider>
    </n-layout>
</template>

<script lang="ts">
import { defineComponent, h } from "vue";

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
    name: "App",
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
        return {
            gotoHome,
            handleUpdateValue,
            menuOptions,
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
}
</style>
