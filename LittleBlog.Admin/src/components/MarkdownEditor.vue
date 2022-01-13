<template>
    <div class="editor-container" ref="editorRef">
        <div class="editor-toolbar" ref="toolbarRef">
            <button @click="uploadImg()">上传图片</button>
            <button @click="insertLink('')">添加链接</button>
            <input
                type="checkbox"
                name="showMarkdown"
                id="showMarkdown"
                v-model="showMarkdown"
            />
            <span> Markdown </span>
            <input
                type="checkbox"
                name="showHtml"
                id="showHtml"
                v-model="showHtml"
            />
            <span> Html </span>
        </div>
        <div class="editor-body">
            <div v-show="showMarkdown" class="origin-content" id="mdEditor">
                <textarea ref="textareaRef"></textarea>
            </div>
            <div class="spliter"></div>
            <iframe
                v-show="showHtml"
                class="markdown-content"
                ref="previewRef"
                frameborder="0"
            ></iframe>
        </div>
        <div class="editor-foot"></div>
    </div>
</template>

<script lang="ts">
import "codemirror/lib/codemirror.css";
import "codemirror/mode/javascript/javascript";

// 主题样式（我直接用了纯白色的，看着比较舒服）
import "codemirror/theme/rubyblue.css";
// 括号显示匹配
import "codemirror/addon/edit/matchbrackets";
import "codemirror/addon/selection/active-line";
// 括号、引号编辑和删除时成对出现
import "codemirror/addon/edit/closebrackets";
// 折叠代码要用到一些玩意
import "codemirror/addon/fold/foldgutter.css";
import "codemirror/addon/fold/foldgutter";
import "codemirror/addon/fold/xml-fold";
import "codemirror/addon/fold/foldcode";
import "codemirror/addon/fold/brace-fold";
import "codemirror/addon/fold/indent-fold.js";
import "codemirror/addon/fold/markdown-fold.js";
import "codemirror/addon/fold/comment-fold.js";

import MarkdownIt from "markdown-it";
import CodeMirror from "codemirror";

import { defineComponent, onMounted, Ref, ref, watch } from "vue";
import { MarkdownTheme, UploadInfo, UploadTypes } from "../types";
import api from "../api";
import { useMessage } from "naive-ui";
import { CodeBlockRender } from "../markdown-render/code-block-render";

interface MarkdownEditorProps {
    markdown: Ref<string>;
    html: Ref<string>;
}

export default defineComponent({
    name: "MarkdownEditor",
    data() {
        return {
            showMarkdown: true,
            showHtml: true,
        };
    },
    watch: {
        showHtml(value) {
            console.log("show html", value);
        },
    },
    props: {
        markdown: {
            type: String,
            default: "",
        },
        html: {
            type: String,
            default: "",
        },
        height: {
            type: Number,
            default: 300,
        },
    },
    setup(props, { emit }) {
        const message = useMessage();
        const ImagePrependUrl = import.meta.env.VITE_REMOTE_API_ADDRESS;

        const markdownIt = new MarkdownIt();
        const textareaRef = ref();

        const toolbarRef = ref<HTMLDivElement>();

        const previewRef = ref<HTMLIFrameElement>();

        let { markdown, html, height } = props;

        height = height || 400;

        const editorHeight = ref<string>(`${height}px`);

        let markdownContent: Ref<string> = ref("");

        let htmlContent: Ref<string> = ref(html ?? "");

        let editor: CodeMirror.EditorFromTextArea =
            {} as CodeMirror.EditorFromTextArea;

        let editorConfig: CodeMirror.EditorConfiguration = {
            lineNumbers: true,
            mode: { name: "text/markdown" },
            theme: "rubyblue",
            indentWithTabs: false,
            scrollbarStyle: "native",
        };

        // 加载时
        onMounted(async () => {
            if (textareaRef.value != null) {
                console.log("初始化编辑器");

                textareaRef.value.value = markdown;

                // 初始化
                editor = CodeMirror.fromTextArea(
                    textareaRef.value as HTMLTextAreaElement,
                    editorConfig
                );

                editor.setOption("extraKeys", {
                    Tab: (cm) => {
                        let spaces = Array(
                            (cm.getOption("indentUnit") || 0) + 1
                        ).join(" ");
                        cm.replaceSelection(spaces);
                    },
                });

                editor.on("change", (instance, changeObj) => {
                    markdownContent.value = instance.getValue();
                });

                // 赋值
                markdownContent.value = markdown;

                // 触发一次window resize
                let e = new Event("resize");
                window.dispatchEvent(e);

                await loadStyleCssUrl();
            }

            // 编辑器高度
            else {
                console.log("没有加载textarea");
            }

            previewRef.value?.contentWindow?.document.body.classList.add(
                "markdown-preview"
            );

            loadThemes();
        });

        let themes: Ref<Array<MarkdownTheme>> = ref([]);

        const loadThemes = async () => {
            let result = await api.admin.markdownThemes.list();

            if (result.isSuccess) {
                themes.value = result.data;
            }
        };

        watch(
            () => markdownContent.value,
            () => {
                htmlContent.value = `<div class="markdown-preview">${markdownIt.render(
                    markdownContent.value
                )}</div>`;

                if (previewRef.value?.contentWindow) {
                    previewRef.value.contentWindow.document.body.innerHTML =
                        htmlContent.value;
                }

                // 重新渲染
                CodeBlockRender.RenderCode(
                    previewRef.value?.contentWindow?.document
                );

                emit("update:html", htmlContent.value);
                emit("update:markdown", markdownContent.value);
            }
        );

        const JoinUrl = (prepend: string, relativeUrl: string): string => {
            if (prepend.indexOf("/") === ImagePrependUrl.length - 1) {
                prepend = prepend.substring(0, ImagePrependUrl.length - 1);
            }

            if (relativeUrl.indexOf("/") === 0) {
                relativeUrl = relativeUrl.substring(1);
            }

            return `${prepend}/${relativeUrl}`;
        };

        const uploadImg = () => {
            console.log("click upload image");

            let uploadInput = document.createElement("input");

            uploadInput.setAttribute("type", "file");

            uploadInput.setAttribute("accept", "image/*"); // 设置过滤条件

            uploadInput.onchange = () => {
                let file = (uploadInput.files || [])[0];

                if (!file) {
                    alert("请选择要上传的图片！");
                    return;
                }

                let uploadInfo: UploadInfo = {
                    fileName: file.name,
                    uploadPath: "/images",
                    index: 1,
                    total: 1,
                    group: "articles",
                    type: UploadTypes.Image,
                    data: file,
                };

                api.admin.file.upload(uploadInfo).then((res) => {
                    if (res.isSuccess) {
                        if (res.data.isFinish) {
                            insertImageLink(
                                JoinUrl(ImagePrependUrl, res.data.url)
                            );
                        }
                    } else {
                        console.error(res.message);
                    }
                });
            };

            uploadInput.click();
        };

        const insertLink = (url: string) => {
            editor.execCommand("goLineEnd");

            let currentCursorPos = editor.getCursor();

            if (currentCursorPos.ch > 0) {
                editor.execCommand("newlineAndIndent");

                currentCursorPos = editor.getCursor();
            }

            editor.replaceRange(`[link description](${url})`, currentCursorPos);
        };

        const insertImageLink = (url: string) => {
            editor.execCommand("goLineEnd");

            editor.execCommand("newlineAndIndent");

            let currentCursorPos = editor.getCursor();

            if (currentCursorPos.ch > 0) {
                editor.execCommand("newlineAndIndent");
                currentCursorPos = editor.getCursor();
            }

            editor.replaceRange(
                `![link description](${url})`,
                currentCursorPos
            );
        };

        const reloadPreviewStyle = (styleCss: string) => {
            if (previewRef.value?.contentWindow) {
                let headerStyle =
                    previewRef.value?.contentWindow.document.head.querySelector(
                        "style"
                    );

                if (!headerStyle) {
                    headerStyle =
                        previewRef.value?.contentWindow.document.createElement(
                            "style"
                        );

                    previewRef.value?.contentWindow.document.head.appendChild(
                        headerStyle
                    );
                }

                headerStyle.innerHTML = styleCss;
            }
        };

        const loadStyleCssUrl = async () => {
            let result = await api.admin.markdownThemes.getDefault();

            if (!result.isSuccess) {
                return;
            }

            let info = result.data;

            let remoteUrl =
                import.meta.env.VITE_REMOTE_API_ADDRESS.indexOf("/") ==
                import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
                    ? import.meta.env.VITE_REMOTE_API_ADDRESS.substring(
                          0,
                          import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
                      )
                    : import.meta.env.VITE_REMOTE_API_ADDRESS;

            info.codeBlockStyleUrl =
                info.codeBlockStyleUrl.indexOf("/") == 0
                    ? info.codeBlockStyleUrl.substring(1)
                    : info.codeBlockStyleUrl;

            info.markdownStyleUrl =
                info.markdownStyleUrl.indexOf("/") == 0
                    ? info.markdownStyleUrl.substring(1)
                    : info.markdownStyleUrl;

            if (!info.codeBlockStyleUrl.startsWith("http")) {
                info.codeBlockStyleUrl = `${remoteUrl}/${info.codeBlockStyleUrl}`;
            }

            if (!info.markdownStyleUrl.startsWith("http")) {
                info.markdownStyleUrl = `${remoteUrl}/${info.markdownStyleUrl}`;
            }

            if (info.markdownStyleUrl) {
                const mdThemeStyleLink =
                    previewRef.value?.contentWindow?.document.createElement(
                        "link"
                    );

                if (!mdThemeStyleLink) {
                    return;
                }

                mdThemeStyleLink.id = "mdThemeStyle";
                mdThemeStyleLink.rel = "stylesheet";
                mdThemeStyleLink.href = info.markdownStyleUrl;

                previewRef.value?.contentWindow?.document.head.appendChild(
                    mdThemeStyleLink
                );
            }

            if (info.codeBlockStyleUrl) {
                const codeBlockStyleLink =
                    previewRef?.value?.contentWindow?.document.createElement(
                        "link"
                    );

                if (!codeBlockStyleLink) {
                    return;
                }

                codeBlockStyleLink.id = "codeBlockStyle";
                codeBlockStyleLink.rel = "stylesheet";
                codeBlockStyleLink.href = info.codeBlockStyleUrl;

                previewRef.value?.contentWindow?.document.head.appendChild(
                    codeBlockStyleLink
                );
            }
        };

        let halfEditorHeight = ref<string>(`${height / 2}px`);

        let returnObj = {
            uploadImg,
            markdownIt,
            toolbarRef,
            previewRef,
            editorHeight,
            markdownContent,
            htmlContent,
            editor,
            insertLink,
            textareaRef,
            reloadPreviewStyle,
            halfEditorHeight,
            themes,
            // loadImportMdStyle,
            message,
            // selectedTheme,
        };

        return returnObj;
    },
});
</script>
<style>
.editor-container {
    border: 1px solid gray;
    /* width: 100%; */
    overflow: auto;
}

.editor-toolbar {
    min-height: 3px;
    border-bottom: 1px solid gray;
}

.editor-body {
    display: flex;
    /* background-color: yellow; */
    overflow: auto;
}

.origin-content {
    flex: 1;
    height: v-bind(editorHeight);
    overflow: auto;
}

.spliter {
    width: 1px;
    border: 0;
    background-color: gray;
}

.markdown-content {
    flex: 1;
    height: v-bind(editorHeight);
    overflow: auto;
}

#mdEditor .CodeMirror {
    height: v-bind(editorHeight);
}
</style>
