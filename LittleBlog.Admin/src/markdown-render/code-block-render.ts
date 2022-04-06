import * as Prism from "prismjs";

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
                        container?.classList.add("language-text");
                        container?.classList.add("line-numbers");

                        const language = getLanguage(element as HTMLElement);

                        container.innerHTML = Prism.highlight(
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
}

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

    console.log("get language", languages[0] || "text");
    return languages[0] || "text";
}

const PLUGIN_NAME = "line-numbers";
const NEW_LINE_EXP = /\n(?!$)/g;
