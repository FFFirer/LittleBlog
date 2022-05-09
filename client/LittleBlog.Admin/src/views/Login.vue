<template>
    <n-layout>
        <div class="login">
            <n-spin :show="loading">
                <div class="login-form">
                    <h1>登录</h1>
                    <n-form ref="formRef" :model="loginInfo" :rules="rules">
                        <n-space vertical>
                            <n-form-item label="邮箱" path="email">
                                <n-input
                                    placeholder="请输入用户邮箱"
                                    type="text"
                                    name="username"
                                    id="username"
                                    v-model:value="loginInfo.email"
                                />
                            </n-form-item>

                            <n-form-item label="密码" path="password">
                                <n-input
                                    placeholder="请输入密码"
                                    name="password"
                                    id="password"
                                    type="password"
                                    v-model:value="loginInfo.password"
                                    @keydown.enter="login"
                                />
                            </n-form-item>
                        </n-space>
                        <n-button @click="login"> 登录 </n-button>
                    </n-form>
                </div>
            </n-spin>
        </div>
    </n-layout>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import store from "../store/index";
import api from "../api/index";
import { LoginModel } from "../types";
import { FormRules, useMessage } from "naive-ui";

const rules: FormRules = {
    email: [
        {
            required: true,
            message: "请填写邮箱地址！",
            trigger: ["blur"],
        },
    ],
    password: [
        {
            required: true,
            message: "请填写密码！",
            trigger: ["blur"],
        },
    ],
};

export default defineComponent({
    name: "Login",
    data() {
        return {
            loginInfo: {
                email: null,
                password: null,
                rememberMe: false,
            } as LoginModel,
            loading: false as boolean,
            rules: rules,
        };
    },
    props: {
        return: {
            type: String,
            default: "/",
        },
    },
    methods: {
        login(e: any) {
            e.preventDefault();

            (this.formRef as any).validate((errors: any) => {
                if (!errors) {
                    this.loading = true;
                    api.admin
                        .login(this.loginInfo)
                        .then((result) => {
                            if (result.isSuccess) {
                                store.checkLogin();
                                this.returnPage();
                            } else {
                                this.message.warning(result.message);
                            }
                        })
                        .catch((err) => {
                            if (typeof err == "string") {
                                this.message.error(err);
                            } else {
                                this.message.error(err.message);
                            }
                        })
                        .finally(() => {
                            this.loading = false;
                        });
                } else {
                    console.log(errors);
                }
            });
        },
    },
    setup(props) {
        const route = useRouter();
        const message = useMessage();
        const formRef = ref(null);

        const returnPath = props["return"] || "/";
        const returnPage = () => {
            route.push({
                path: returnPath,
            });
        };

        return {
            returnPage,
            message,
            formRef,
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
    max-height: 90%;
    background-color: white;
    padding: 20px;
}
</style>
