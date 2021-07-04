<template>
    <n-layout>
        <n-message-provider>
            <n-layout has-sider>
                <n-layout-sider>
                    <n-layout-header id="app-header" style="padding: 10px; padding-left: 32px;">
                        <h1 @click="gotoHome" style="cursor: pointer; margin: 0;">
                            管理后台
                        </h1>
                    </n-layout-header>
                    <n-menu @update:value="handleUpdateValue" :options="menuOptions">

                    </n-menu>
                </n-layout-sider>
                <n-layout-content id="app-content">
                    <div class="app-content-container">
                        <router-view>

                        </router-view>
                    </div>
                </n-layout-content>
            </n-layout>
        </n-message-provider>
    </n-layout>
</template>

<script lang="ts">
    import {
        defineComponent,
        h
    } from 'vue'

    import {
        useRouter
    } from "vue-router";

    import {
        useMessage
    } from 'naive-ui'

    const menuOptions = [{
        label: () =>
            h(
                'a', {},
                '文章管理'
            ),
        key: 'articles-manage'
    }, {
        label: () =>
            h(
                'a', {},
                '分类管理'
            ),
        key: 'categories-manage'
    }]


    export default defineComponent({
        name: 'App',
        setup() {
            // const message = useMessage()
            const router = useRouter();

            const gotoArticlesManage = (id) => {
                console.log('goto articles manage')
                router.push({
                    name: "articleList" // 命名路由
                })
            }
            const gotoCatgoriesManage = () => {
                console.log('goto categories manage')

                router.push({
                    name: "categoryList"
                })
            }
            const gotoHome = () => {
                router.push({
                    path: '/'
                })
            }
            return {
                menuOptions,
                handleUpdateValue(key, item) {
                    if (key == 'articles-manage') {
                        gotoArticlesManage()
                    }

                    if (key == 'categories-manage') {
                        gotoCatgoriesManage()
                    }
                    // message.info('[onUpdate:value]: ' + JSON.stringify(key))
                    // message.info('[onUpdate:value]: ' + JSON.stringify(item))
                },
                gotoHome
            }
        }
    })

</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: #2c3e50;
    }

    .n-layout-header {
        /* background-color: antiquewhite; */
        border-bottom: 1px solid #ccc;

    }

    .n-layout-header a {
        text-decoration: none;
    }

    .n-layout-sider {
        /* background-color: antiquewhite; */
        border-right: 1px solid #ccc;
    }

    #app-content {
        /* background-color: antiquewhite; */
        height: 100vh;
    }

    .app-content-container {
        overflow: auto;
        padding-top: 10px;
        padding-left: 10px;
    }

    #app-content>div {
        background-color: #fff;
        padding: 5px;
        box-shadow: 1px 1px 1px #ccc;
        height: 100%;
        overflow: auto;
    }

</style>
