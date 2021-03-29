import baseApi from '../baseApi'

const listArticlesUrl = "/api/Articles/List";
const getArticleUrl = "/api/Articles/{id}";

const articleApi = {
    getList: (queryData) => {
        return baseApi.get(listArticlesUrl, queryData);
    },
    getArticle: (id) => {
        let url = getArticleUrl.replace("{id}", id);
        return baseApi.get(url);
    }
}

export default articleApi;