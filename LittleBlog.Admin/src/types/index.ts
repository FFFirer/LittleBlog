interface ArticleDto {
    id: number;
    title: string;
    author: string;
    abstract: string;
    content: string;
    savePath: string;
    isPublished: boolean;
    lastEditTime: string;
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

export { ArticleDto, ListArticlesQueryContext, ListArticleResultModel };
