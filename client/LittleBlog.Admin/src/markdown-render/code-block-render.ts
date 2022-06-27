import Prism from "prismjs";

const LINE_NUMBERS = "line-numbers";

export class CodeBlockRender {
    public static RenderCode(htmlDoc: Document | undefined) {
        if (!htmlDoc) {
            return;
        }
        // 找到所有pre>code
        let codeContainers = htmlDoc.querySelectorAll("pre>code");

        if (codeContainers.length > 0) {
            codeContainers.forEach((element, index, parents) => {
                try {
                    let codeElement = element as HTMLElement;
                    let code = codeElement.innerText;

                    let container = codeElement.parentElement;

                    if (container) {
                        codeElement?.classList.add("language-text");
                        container?.classList.add("line-numbers");

                        const language = getLanguage(element as HTMLElement);

                        codeElement.innerHTML = Prism.highlight(
                            code,
                            Prism.languages[language],
                            language
                        );
                    }
                } catch (error) {
                    console.error("highlight error", error);
                }
            });
        }
    }

    /**
     * 渲染代码
     * @param html Html内容 字符串
     * @returns 渲染代码块后的
     */
    public static RenderCodeText(html: string): string {
        if (!html) {
            return "";
        }

        if (html.length <= 0) {
            return "";
        }

        let container = document.createElement("div");
        container.innerHTML = html;

        let codeElements: Array<HTMLElement> = [];
        container.querySelectorAll("pre>code").forEach((a) => {
            codeElements.push(a as HTMLElement);
        });

        if (codeElements.length <= 0) {
            return container.innerHTML;
        }

        codeElements.forEach((el) => {
            try {
                let code = el.innerText;
                let pre = el.parentElement;

                if (pre) {
                    pre.classList.add("language-text");
                    pre.classList.add("line-numbers");

                    const language = getLanguage(el);
                    const prismLanguage =
                        Prism.languages[language] || Prism.languages["text"];

                    pre.innerHTML = Prism.highlight(
                        code,
                        prismLanguage,
                        language
                    );
                }
            } catch (error) {
                console.error("primejs render error", error);
            }
        });

        return container.innerHTML;
    }
}

const codePattern = /<pre><code.*?class="">(.*?)<\/code><\/pre>/g;

const TAGS_TO_REPLACE: { [key: string]: string } = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#x27;",
    "/": "&#x2F;",
    "\\": "&#x5C;",
};

const TAGS_TO_REPLACE_REVERSE: { [key: string]: string } = {
    "&amp;": "&",
    "&lt;": "<",
    "&gt;": ">",
    "&quot;": '"',
    "&apos;": "'",
    "&#x27;": "'",
    "&#x2F;": "/",
    "&#x5C;": "\\",
};

export function escapeString(str: string = ""): string {
    return str.replace(/[&<>"'\/\\]/g, (tag) => TAGS_TO_REPLACE[tag] || tag);
}

export function unescapeString(str: string = ""): string {
    return str.replace(
        /\&(amp|lt|gt|quot|apos|\#x27|\#x2F|\#x5C)\;/g,
        (whole) => TAGS_TO_REPLACE_REVERSE[whole] || whole
    );
}

/**
 * 获取指定的语言
 * @param codeElement <code></code>
 * @returns
 */
export function getLanguage(codeElement: HTMLElement): string {
    let languages: string[] = [];
    codeElement.classList.forEach((classItem, index) => {
        if (/^language-/.test(classItem)) {
            languages.push(classItem.substring(9));
        }
    });

    return languages[0] || "text";
}

const PLUGIN_NAME = "line-numbers";
const NEW_LINE_EXP = /\n(?!$)/g;
