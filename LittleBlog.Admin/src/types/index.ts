interface ArticleDto {
    id: number;
    title: string;
    author: string;
    abstract: string;
    content: string;
    savePath: string;
    isPublished: boolean;
    lastEditTime: string;
    categoryName: string;
    tags: string[];
}

// 基本结果
interface ResultModel {
    isSuccess: boolean;
    message: string;
    exceptionMessage: string;
}

interface BaseResultModel<T> extends ResultModel {
    data: T;
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

interface UploadInfo {
    [index: string]: any; // 类型索引
    fileName: string;
    uploadPath: string;
    index: number;
    total: number;
    group: string;
    type: string | UploadTypes;
    data: File;
}

interface UploadResult {
    group: string;
    url: string;
    fileId: string;
    fileName: string;
    isFinish: boolean;
}

interface UploadFileResultModel extends ResultModel {
    data: UploadResult;
}

enum UploadTypes {
    Default = "default",
    Image = "image",
    Pdf = "pdf",
}

interface Category {
    name: string;
    createTime: string;
}

interface ListCategoriesResultModel extends ResultModel {
    data: Category[];
}

export {
    ArticleDto,
    ResultModel,
    ListArticlesQueryContext,
    ListArticleResultModel,
    GetArticleResultModel,
    LoginModel,
    UploadInfo,
    UploadResult,
    UploadTypes,
    UploadFileResultModel,
    BaseResultModel as TResultModel,
    Category,
    ListCategoriesResultModel,
};
