import axios, { ResponseType } from "axios";
import {
    ArticleDto,
    GetArticleResultModel,
    ListArticleResultModel,
    ListArticlesQueryContext,
    ResultModel,
} from "../types/index";
import helper from "./helper";
import querystring from "querystring";

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
        if (err.response) {
            switch (err.response.status) {
            }
            return Promise.reject(err);
        } else {
            // TODO：处理断网的情况, 显示一个全局的断网提示
            return Promise.reject(
                "网络断开，无法连接到服务器，请检查网络后再试！"
            );
        }
    }
);

// const querystring = require("querystring");

// const apiSeverAddress: string | undefined = import.meta.env
//     .VITE_REMOTE_API_ADDRESS as string | undefined;

// if (apiSeverAddress == undefined) {
//     console.error("没有设置接口地址");
// }

// 接口文件
// 如果路径中含有参数，则是使用冒号表示，/api/with/:params
const urls = {
    common: {
        Articles: {
            list: "/api/Articles/List",
            get: "/api/Articles/{id}",
        },
    },
    admin: {
        Article: {
            delete: "/api/Admin/Articles/Delete",
            save: "/api/Admin/Articles/Save",
            get: "/api/Admin/Articles/:id",
            list: "/api/Admin/Articles/List",
        },
        Tags: {
            list: "/api/Admin/Tags/List",
            getOne: "/api/Admin/Tags/GetOne",
            delete: "/api/Admin/Tags/Delete",
            save: "/api/Admin/Tags/Save",
            create: "/api/Admin/Tags/CreateTag",
        },
    },
};

const api = {
    urls,
    common: {},
    admin: {
        articles: {
            list: async (
                query: ListArticlesQueryContext
            ): Promise<ListArticleResultModel> => {
                console.log("request admin list articles api");
                return await axios
                    .get(urls.admin.Article.list, {
                        params: query,
                    })
                    .then((resp) => {
                        if (resp.status == 200) {
                            return resp.data;
                        } else {
                            throw new Error("can't list articles");
                        }
                    });
            },
            save: async (article: ArticleDto): Promise<ResultModel> => {
                return await axios
                    .post(urls.admin.Article.save, article)
                    .then((resp) => {
                        if (resp.status == 200) {
                            return resp.data;
                        } else {
                            throw new Error("save failed");
                        }
                    });
            },
            delete: async (id: number): Promise<ResultModel> => {
                let targetUrl =
                    urls.admin.Article.delete +
                    "?" +
                    querystring.stringify({ id: id });
                return await axios.post(targetUrl).then((resp) => {
                    if (resp.status == 200) {
                        return resp.data;
                    } else {
                        throw new Error("delete failed");
                    }
                });
            },
            getOne: async (id: number): Promise<GetArticleResultModel> => {
                let targetUrl = helper.fillUrlParams(urls.admin.Article.get, {
                    id: id,
                });
                return await axios.get(targetUrl).then((resp) => {
                    if (resp.status == 200) {
                        return resp.data;
                    } else {
                        throw new Error("get article failed");
                    }
                });
            },
        },
    },
};

export default api;
