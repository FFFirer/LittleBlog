import * as Prism from "prismjs";

export class CodeBlockRender {
    public static RenderCode(document: Document | undefined) {
        if (!document) {
            return;
        }
        // 找到所有pre>code
        document
            .querySelectorAll("pre>code")
            .forEach((element, index, parents) => {
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

export function getLanguage(codeElement: HTMLElement): string {
    let languages: string[] = [];
    codeElement.classList.forEach((classItem, index) => {
        if (/^language-/.test(classItem)) {
            languages.push(classItem.substring(9));
        }
    });

    return languages[0] || "text";
}
