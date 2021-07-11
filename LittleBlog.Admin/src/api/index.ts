import axios, { ResponseType } from "axios";
import {
    ListArticleResultModel,
    ListArticlesQueryContext,
} from "../types/index";
import helper from "./helper";

// const apiSeverAddress: string | undefined = import.meta.env
//     .VITE_REMOTE_API_ADDRESS as string | undefined;

// if (apiSeverAddress == undefined) {
//     console.error("没有设置接口地址");
// }

axios.defaults.baseURL = import.meta.env.VITE_REMOTE_API_ADDRESS as
    | string
    | undefined;

// 接口文件
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
            get: "/api/Admin/Articles/{id}",
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
            list: (query: ListArticlesQueryContext): ListArticleResultModel => {
                console.log("request admin list articles api");
                let a = {
                    message: "成功",
                } as ListArticleResultModel;
                return a;
            },
        },
    },
};

export default api;
