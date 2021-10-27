import axios, { AxiosResponse, ResponseType } from "axios";
import {
    ArticleDto,
    FriendshipLink,
    GetArticleResultModel,
    ListArticleResultModel,
    ListArticlesQueryContext,
    ListCategoriesResultModel,
    LoginModel,
    ResultModel,
    SystemConfig,
    TResultModel,
    UploadFileResultModel,
    UploadInfo,
    UploadResult,
} from "../types/index";
import helper from "./helper";
import querystring from "querystring";
import { routerKey } from "vue-router";

// 携带cookie
axios.defaults.withCredentials = true;
axios.defaults.baseURL = import.meta.env.VITE_REMOTE_API_ADDRESS as
    | string
    | undefined;

axios.interceptors.request.use(
    (config) => {
        // 在请求之前做些什么
        config.withCredentials = true; // 允许携带cookie
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
            delete: "/api/Admin/Tags/Delete/:tagName",
            save: "/api/Admin/Tags/Save/:tagName",
        },
        Categories: {
            list: "/api/Admin/Categories/List",
            listAll: "/api/Admin/Categories/ListAll",
            delete: "/api/Admin/Categories/Delete/:categoryName",
            save: "/api/Admin/Categories/Save/:categoryName",
        },
        login: "/api/user/login",
        logout: "/api/user/logout",
        File: {
            upload: "/api/File/Upload",
        },
        SysCfg: {
            base: "/api/Admin/SysCfg/Base",
            friendshipLinks: "/api/Admin/SysCfg/FriendshipLinks",
        },
    },
};

const handleResponse = (
    resp: AxiosResponse<any>,
    errorMessage: string
): any => {
    if (resp.status == 200) {
        return resp.data;
    } else {
        if (errorMessage) {
            console.error(resp.status, resp.statusText);
            throw new Error(errorMessage);
        } else {
            throw new Error(resp.statusText);
        }
    }
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
        login: async (loginInfo: LoginModel): Promise<ResultModel> => {
            return await axios
                .post(urls.admin.login, loginInfo)
                .then((resp) => {
                    return handleResponse(resp, "登录失败！");
                });
        },
        logout: async (): Promise<ResultModel> => {
            return await axios.get(urls.admin.logout).then((resp) => {
                return handleResponse(resp, "注销失败！");
            });
        },
        file: {
            upload: async (
                uploadinfo: UploadInfo
            ): Promise<UploadFileResultModel> => {
                let formData = new FormData();
                Object.keys(uploadinfo).forEach(function (key) {
                    formData.append(key, uploadinfo[key]);
                });
                return await axios
                    .post(urls.admin.File.upload, formData, {
                        headers: {
                            "Content-Type": "multipart/form-data",
                        },
                    })
                    .then((resp) => {
                        return handleResponse(resp, "上传文件失败");
                    });
            },
        },
        tags: {
            list: async (): Promise<TResultModel<string[]>> => {
                return await axios.get(urls.admin.Tags.list).then((resp) => {
                    return handleResponse(resp, "获取标签失败");
                });
            },
            delete: async (tagName: string): Promise<ResultModel> => {
                let targetUrl = helper.fillUrlParams(urls.admin.Tags.delete, {
                    tagName: tagName,
                });
                return await axios.post(targetUrl).then((resp) => {
                    return handleResponse(resp, "删除标签失败");
                });
            },
            save: async (tagName: string): Promise<ResultModel> => {
                let targetUrl = helper.fillUrlParams(urls.admin.Tags.save, {
                    tagName: tagName,
                });
                return await axios.post(targetUrl).then((resp) => {
                    return handleResponse(resp, "保存标签失败");
                });
            },
        },
        categories: {
            list: async (): Promise<TResultModel<string[]>> => {
                return await axios
                    .get(urls.admin.Categories.list)
                    .then((resp) => {
                        return handleResponse(resp, "获取分类失败");
                    });
            },
            listAll: async (): Promise<ListCategoriesResultModel> => {
                return await axios
                    .get(urls.admin.Categories.listAll)
                    .then((resp) => {
                        return handleResponse(resp, "获取分类失败");
                    });
            },
            delete: async (categoryName: string): Promise<ResultModel> => {
                let targetUrl = helper.fillUrlParams(
                    urls.admin.Categories.delete,
                    {
                        categoryName: categoryName,
                    }
                );
                return await axios.post(targetUrl).then((resp) => {
                    return handleResponse(resp, "删除分类失败");
                });
            },
            save: async (categoryName: string): Promise<ResultModel> => {
                let targetUrl = helper.fillUrlParams(
                    urls.admin.Categories.save,
                    {
                        categoryName: categoryName,
                    }
                );
                return await axios.post(targetUrl).then((resp) => {
                    return handleResponse(resp, "baocu");
                });
            },
        },
        syscfg: {
            base: {
                get: async (): Promise<TResultModel<SystemConfig>> => {
                    return await axios
                        .get(urls.admin.SysCfg.base)
                        .then((resp) => {
                            return handleResponse(
                                resp,
                                "获取系统基础配置失败！"
                            );
                        });
                },
                save: async (config: SystemConfig): Promise<ResultModel> => {
                    return await axios
                        .post(urls.admin.SysCfg.base, config)
                        .then((resp) => {
                            return handleResponse(
                                resp,
                                "保存系统基础配置失败！"
                            );
                        });
                },
            },
            friendshipLinks: {
                list: async (): Promise<TResultModel<FriendshipLink[]>> => {
                    return await axios
                        .get(urls.admin.SysCfg.friendshipLinks)
                        .then((resp) => {
                            return handleResponse(resp, "获取友情链接失败！");
                        });
                },
                save: async (links: FriendshipLink[]): Promise<ResultModel> => {
                    return await axios
                        .post(urls.admin.SysCfg.friendshipLinks, links)
                        .then((resp) => {
                            return handleResponse(resp, "保存友情链接失败！");
                        });
                },
            },
        },
    },
};

export default api;
