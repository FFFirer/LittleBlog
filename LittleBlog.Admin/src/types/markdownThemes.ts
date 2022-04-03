import { MarkdownTheme, ResultModel } from ".";

interface ListAllThemesResultModel extends ResultModel {
    data: Array<MarkdownTheme>;
}

interface MarkdownThemeDetailResultModel extends ResultModel {
    data: MarkdownTheme;
}

class DefaultMarkdownThemeInfo {
    markdownStyleUrl!: string;
    codeBlockStyleUrl!: string;
    defaultThemeId!: string;
    defaultCodeBlockThemeId!: string;
}

export {
    ListAllThemesResultModel,
    MarkdownThemeDetailResultModel,
    DefaultMarkdownThemeInfo,
};
