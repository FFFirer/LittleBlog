// 将axios和项目结合

import axios from "axios";
import { useMessage } from "naive-ui";

const message = useMessage();

axios.defaults.baseURL = import.meta.env.VITE_REMOTE_API_ADDRESS as
    | string
    | undefined;

axios.interceptors.request.use(
    (config) => {
        // 在请求之前做些什么
        // TODO: 判断是否已经登录或者需要登录
        return config;
    },
    (err) => {
        // 对请求错误做些什么
        console.log(err);
        return Promise.reject(err);
    }
);

axios.interceptors.response.use(
    (resp) => {
        // 2xx 范围内的函数都会触发该函数
        // 对响应数据做些什么
        return resp;
    },
    (err) => {
        // 超过2xx 范围的状态码都会触发该函数
        // 对响应错误做点什么
        if (err.response.status) {
            switch (err.response.status) {
            }
            return Promise.reject(err);
        } else {
            // TODO：处理断网的情况, 显示一个全局的断网提示
            message.error("网络断开，请检查网络后再试！");
        }
    }
);
