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

const helper = {
    FillRouteData,
    FillQueryString,
};

export default helper;
