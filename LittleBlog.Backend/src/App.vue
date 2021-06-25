<template>
    <a-layout>
        <a-layout-header id="app-header">
            <router-link to="/">
                <h1>
                    管理后台
                </h1>
            </router-link>
        </a-layout-header>
        <a-layout>
            <a-layout-sider id="app-sider">
                <a-menu>
                    <a-menu-item key="1">
                        <router-link to="/articles/manage">
                            <span>文章管理</span>
                        </router-link>
                    </a-menu-item>
                    <a-menu-item key="2">
                        <router-link to="/categories/manage">
                            <span>分类管理</span>
                        </router-link>
                    </a-menu-item>
                    <a-menu-item key="3">
                        <router-link to="/tags/manage">
                            <span>标签管理</span>
                        </router-link>
                    </a-menu-item>
                    <a-menu-item key="4">
                        <router-link to="/site/manage">
                            <span>网站管理</span>
                        </router-link>
                    </a-menu-item>
                </a-menu>
            </a-layout-sider>
            <a-layout>
                <a-layout-content id="app-content">
                    <div>
                        <router-view></router-view>
                    </div>
                </a-layout-content>
            </a-layout>
        </a-layout>
    </a-layout>
</template>

<script lang="ts">
    import {
        defineComponent
    } from 'vue'

    export default defineComponent({
        name: 'App'
    })

</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: #2c3e50;
    }

    #app-header {
        background-color: #fff;
        border-bottom: 1px solid #ccc;
    }

    #app-sider {
        position: fixed;
        height: calc(100vh - 64px);
        border-right: 1px solid #f0f0f0;
        background-color: #fff;
        overflow: auto;
    }

    #app-sider ul {
        border-right: 0px;
    }

    #app-content {
        margin-left: 200px;
        height: calc(100vh - 64px);
        overflow: auto;
        padding: 5px;
    }

    #app-content>div {
        background-color: #fff;
        padding: 5px;
        box-shadow: 1px 1px 1px #ccc;
        height: 100%;
        overflow: auto;
    }

</style>
