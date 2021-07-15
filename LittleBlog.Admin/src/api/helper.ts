import { compile } from "path-to-regexp";
// 构建QueryStrings
const FillRouteData = (url: string, data: any): string => {
    for (let key of Object.keys(data)) {
        let value = data[key];
        url = url.replace("{" + key + "}", value);
    }

    return url;
};

const FillQueryString = (url: string, data: any): string => {
    let querys: string[] = [];
    for (let key of Object.keys(data)) {
        let value = data[key];
        querys.push(key + "=" + value);
    }

    url = url + "?" + querys.join("&");
    return url;
};

const regexpCompileCache: {
    [key: string]: Function;
} = Object.create(null);

// 将参数填充进url里
const fillUrlParams = (path: string, params: Object): string => {
    params = params || {};
    try {
        const filler =
            regexpCompileCache[path] ||
            (regexpCompileCache[path] = compile(path));

        return filler(params, {
            pretty: true,
        });
    } catch (error) {
        console.warn(`missing params ${error.message}`);
        return "";
    } finally {
    }
};

const helper = {
    FillRouteData,
    FillQueryString,
    fillUrlParams,
};

export default helper;
