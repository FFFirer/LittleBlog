interface ArticleDto {
    id: number;
    title: string;
    author: string;
    abstract: string;
    content: string;
    savePath: string;
    isPublished: boolean;
    lastEditTime: string;
    categoryId: number;
}

// 基本结果
interface ResultModel {
    isSuccess: boolean;
    message: string;
    exceptionMessage: string;
}

// 分页模型
interface PagingModel<TEntity> {
    total: number;
    rows: Array<TEntity>;
}

interface ListArticlesQueryContext {
    page: number;
    pageSize: number;
    keyword: string;
    onlyPublished: boolean;
}

interface ListArticleResultModel extends ResultModel {
    data: PagingModel<ArticleDto>;
}

interface GetArticleResultModel extends ResultModel {
    data: ArticleDto;
}

// 登录登出
interface LoginModel {
    email?: string | null;
    password?: string | null;
    rememberMe: boolean;
}

export {
    ArticleDto,
    ResultModel,
    ListArticlesQueryContext,
    ListArticleResultModel,
    GetArticleResultModel,
    LoginModel,
};
