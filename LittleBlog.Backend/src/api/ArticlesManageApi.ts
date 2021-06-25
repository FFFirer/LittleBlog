import BaseApi from './BaseApi'

const DeleteArticleApiUrl = '/api/Admin/Articles/Delete'
const SaveArticleApiUrl = '/api/Admin/Articles/Save'
const ArticleDetailApiUrl = '/api/Admin/Articles/{id}'
const ListArticlesApiUrl = '/api/Admin/Articles/List'

interface IRouteData {
    key: string
    value: object
}

interface ISaveArticleData {}

interface IListArticlesQueryParams {
    keyword: string
    onlyPublished: boolean
    page: number
    total: number
    pageCount: number
}

const FillRouteData = (url: string, routeData: Array<IRouteData>): string => {
    routeData.forEach((data) => {
        url = url.replace('{' + data.key + '}', String(data.value))
    })

    return url
}

const OnlyFillRouteData = (url: string, data: object): string => {
    for (let key of Object.keys(data)) {
        let value = key.valueOf()
        url = url.replace('{' + key + '}', value)
    }

    return url
}

const OnlyFillQueryString = (url: string, data: object): string => {
    let querys: string[] = []
    for (let key of Object.keys(data)) {
        let value = key.valueOf()
        querys.push(key + '=' + value)
    }

    url = url + '?' + querys.join('&')

    return url
}

interface IArticle {
    id: number
    title: string
    author: string
    content: string
}

interface IArticleManageApi {
    List: (data: IListArticlesQueryParams) => {}
    GetDetail: (id: number) => {}
    Delete: (id: number) => {}
    Save: (article: IArticle) => {}
}

const ArticlesManageApi: IArticleManageApi = {
    List: (data: IListArticlesQueryParams) => {
        return BaseApi.get(ListArticlesApiUrl, data, 'json')
    },
    GetDetail: (id: number) => {
        let actualUrl = OnlyFillRouteData(ArticleDetailApiUrl, { id: id })
        return BaseApi.get(actualUrl, {}, 'json')
    },
    Delete: (id: number) => {
        let actualUrl = OnlyFillQueryString(DeleteArticleApiUrl, { id: id })
        return BaseApi.post(actualUrl, {}, 'json')
    },
    Save: (article: IArticle) => {
        return BaseApi.post(SaveArticleApiUrl, article)
    },
}

export default ArticlesManageApi
