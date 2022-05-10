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
    useMarkdown: boolean;
    markdownContent: string;
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
    pageCount: number;
    pageSize: number;
}

interface ListArticlesQueryContext {
    page: number;
    pageSize: number;
    keyword: string;
    onlyPublished: boolean;
}

interface BasePagingQueryContext {
    page: number;
    pageSize: number;
}

interface ListLogsQueryContext extends BasePagingQueryContext {
    startTime?: string | null;
    endTime?: string | null;
    logger?: string | null;
    logLevel?: number | null;
}

interface ListArticleResultModel extends ResultModel {
    data: PagingModel<ArticleDto>;
}

interface GetArticleResultModel extends ResultModel {
    data: ArticleDto;
}

interface LogModel {
    id: number;
    logLevel: string;
    message: string;
    logger: string;
    application: string;
    callSite: string;
    exception: string;
    logged: string;
}

interface ListLogResultModel extends ResultModel {
    data: PagingModel<LogModel>;
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

// 系统配置-网站信息配置
interface WebSiteBaseInfo {
    siteName: string;
    welcome: string;
}

interface WebSiteFiling {
    number: string;
}

interface SystemConfig {
    baseInfo: WebSiteBaseInfo;
    filing: WebSiteFiling;
}

interface FriendshipLink {
    description: string;
    link: string;
    group: string;
}

interface ListPagingCategoriesQueryContext extends BasePagingQueryContext {}
interface ListPagingCategoriesResultModel
    extends BaseResultModel<PagingModel<Category>> {}

class MarkdownTheme {
    name!: string;
    url!: string;
    physicalPath!: string;
    id!: string;
    content!: string;
    remark!: string;
    useOuterLink!: boolean;
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
    WebSiteFiling,
    WebSiteBaseInfo,
    SystemConfig,
    FriendshipLink,
    ListLogsQueryContext,
    ListLogResultModel,
    LogModel,
    MarkdownTheme,
    ListPagingCategoriesQueryContext,
    ListPagingCategoriesResultModel,
};
