<template>
    <div class="login">
        <div class="login-form">
            <h1>登录</h1>
            <n-space vertical>
                <n-input
                    placeholder="用户名"
                    type="text"
                    name="username"
                    id="username"
                />

                <n-input
                    placeholder="密码"
                    type="text"
                    name="password"
                    id="password"
                />
                <n-button @click="login()">登录</n-button>
            </n-space>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useRouter } from "vue-router";
import store from "../store/index";
export default defineComponent({
    name: "Login",
    props: ["retuen"],
    setup(props) {
        const route = useRouter();

        const returnPath = props["retuen"] || "/";
        const returnPage = () => {
            route.push({
                path: returnPath,
            });
        };

        const login = () => {
            store.checkLogin();
            returnPage();
        };

        return {
            returnPage,
            login,
        };
    },
});
</script>
<style>
.login {
    background-color: rgb(153, 178, 245);
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 600px;
}

.login-form {
    width: 400px;
    max-width: 90%;
    height: 200px;
    max-height: 90%;
    background-color: white;
    padding: 20px;
    padding-top: 0;
}
</style>
