<template>
    <div class="editor-container" ref="editorRef">
        <div class="editor-toolbar mb-1" ref="toolbarRef">
            <button class="btn btn-sm btn-light mr-1" @click="uploadImg()">
                上传图片
            </button>
            <button class="btn btn-sm btn-light mr-1" @click="insertLink('')">
                添加链接
            </button>
            <input
                class="mr-1"
                type="text"
                v-model="language"
                placeholder="请输入语言名称"
            />
            <button
                class="btn btn-sm btn-light mr-1"
                @click="insertCodeBlock()"
            >
                插入代码片段
            </button>
            <input
                class="mr-1"
                type="checkbox"
                name="showMarkdown"
                id="showMarkdown"
                v-model="showMarkdown"
            />
            <span> Markdown </span>
            <input
                class="mr-1"
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
            <html-preview
                class="markdown-content"
                v-show="showHtml"
                :html="previewHtml"
                :innerStyles="previewInnerStyles"
                :outerStyles="previewOuterStyles"
            >
            </html-preview>
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

import {
    computed,
    ComputedRef,
    defineComponent,
    onMounted,
    PropType,
    Ref,
    ref,
    watch,
} from "vue";
import { MarkdownTheme, UploadInfo, UploadTypes } from "../types";
import api from "../api";
import { useMessage } from "naive-ui";
import HtmlPreview from "./HtmlPreview.vue";
import { CodeBlockRender } from "../markdown-render/code-block-render";

const MarkdownEditorProps = {
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
    useDefaultTheme: {
        type: Boolean,
        default: true,
    },
    appendStyleUrls: {
        type: Array as PropType<Array<string>>,
        default: [],
    },
    appendStyleCss: {
        type: String,
        default: "",
    },
};

export default defineComponent({
    name: "MarkdownEditor",
    components: {
        HtmlPreview,
    },
    data() {
        return {
            showMarkdown: true,
            showHtml: true,
        };
    },
    props: MarkdownEditorProps,
    setup(props, { emit }) {
        const ImagePrependUrl = import.meta.env.VITE_REMOTE_API_ADDRESS;

        const message = useMessage();
        const textareaRef = ref();

        /**解构 */
        const previewHtml = ref("");
        const markdownContent = ref("");

        /**要插入的语言名称 */
        const language = ref("");

        /**Markdown渲染 */
        const markdownIt = new MarkdownIt();

        /**工具栏 */
        const toolbarRef = ref<HTMLDivElement>();

        /**编辑器高度 */
        const editorHeight = ref<string>(`${props.height || 400}px`);

        // 编辑器CodeMirror
        let editor: CodeMirror.EditorFromTextArea =
            {} as CodeMirror.EditorFromTextArea;

        // 编辑器配置
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
                markdownContent.value = props.markdown || "";
                textareaRef.value.value = markdownContent.value;

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
                    let editorContent = instance.getValue();

                    // 从编辑器取数据
                    markdownContent.value = editorContent;
                });

                // 触发一次window resize
                let e = new Event("resize");

                window.dispatchEvent(e);

                if (props.useDefaultTheme) {
                    await loadStyleCssUrl();
                }
            } else {
                console.log("没有加载textarea");
            }

            updatePreview(markdownContent.value);

            loadThemes();

            // 加载附加样式
            loadAppendStyleUrls(props.appendStyleUrls);
            reloadPreviewStyle(props.appendStyleCss);
        });

        /**所有主题 */
        let themes: Ref<Array<MarkdownTheme>> = ref([]);

        /**加载所有主题 */
        const loadThemes = async () => {
            let result = await api.admin.markdownThemes.list();

            if (result.isSuccess) {
                themes.value = result.data;
            }
        };

        // markdown内容监控变化
        watch(
            () => markdownContent.value,
            (v, o) => {
                emit("update:markdown", markdownContent.value);
                updatePreview(markdownContent.value);
            }
        );

        const updatePreview = (markdown: string) => {
            const renderedHtml = markdownIt.render(markdown);

            const renrededCodeHtml =
                CodeBlockRender.RenderCodeText(renderedHtml);

            const result = `<div class="markdown-preview">${renrededCodeHtml}</div>`;

            previewHtml.value = result;
            emit("update:html", result);
        };

        /**附加样式链接, 当外部改变时重新加载 */
        watch(
            () => props.appendStyleUrls,
            () => {
                loadAppendStyleUrls(props.appendStyleUrls);
            }
        );

        /**附加样式内容，当外部样式改变时重新加载 */
        watch(
            () => props.appendStyleCss,
            () => {
                reloadPreviewStyle(props.appendStyleCss);
            }
        );

        /**
         * 拼接地址
         * @param prepend 前缀
         * @param relativeUrl 相对url
         */
        const JoinUrl = (prepend: string, relativeUrl: string): string => {
            if (prepend.indexOf("/") === ImagePrependUrl.length - 1) {
                prepend = prepend.substring(0, ImagePrependUrl.length - 1);
            }

            if (relativeUrl.indexOf("/") === 0) {
                relativeUrl = relativeUrl.substring(1);
            }

            return `${prepend}/${relativeUrl}`;
        };

        /**上传图片 */
        const uploadImg = () => {
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

        /**
         * 插入普通链接
         * @param url
         */
        const insertLink = (url: string) => {
            editor.execCommand("goLineEnd");

            let currentCursorPos = editor.getCursor();

            if (currentCursorPos.ch > 0) {
                editor.execCommand("newlineAndIndent");

                currentCursorPos = editor.getCursor();
            }

            editor.replaceRange(`[link description](${url})`, currentCursorPos);
        };

        /**
         * 插入图片链接
         * @param url
         */
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

        /**插入代码块 */
        const insertCodeBlock = () => {
            editor.execCommand("goLineEnd");

            editor.execCommand("newlineAndIndent");

            let currentCursorPos = editor.getCursor();

            if (currentCursorPos.ch > 0) {
                editor.execCommand("newlineAndIndent");
                currentCursorPos = editor.getCursor();
            }

            editor.replaceRange(
                "```" + (language.value || "txt") + "\n```",
                currentCursorPos
            );
        };

        /**
         * 重新加载css内容
         * @param styleCss
         */
        const reloadPreviewStyle = (styleCss: string) => {};

        /**服务器远程地址 */
        let remoteUrl =
            import.meta.env.VITE_REMOTE_API_ADDRESS.indexOf("/") ==
            import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
                ? import.meta.env.VITE_REMOTE_API_ADDRESS.substring(
                      0,
                      import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
                  )
                : import.meta.env.VITE_REMOTE_API_ADDRESS;

        const defaultStyleUrls: Ref<Array<string>> = ref([]);

        /**加载默认渲染主题 */
        const loadStyleCssUrl = async () => {
            let result = await api.admin.markdownThemes.getDefault();

            if (!result.isSuccess) {
                return;
            }

            let info = result.data;

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
                defaultStyleUrls.value.push(info.markdownStyleUrl);
            }

            if (info.codeBlockStyleUrl) {
                defaultStyleUrls.value.push(info.codeBlockStyleUrl);
            }
        };

        /**
         * 加载附加样式（链接）
         * @param urls 样式链接
         */
        const loadAppendStyleUrls = (urls: Array<string>): void => {};

        const showAppendStyleCssUrls = () => {
            alert(props.appendStyleUrls);
        };

        /**半个编辑器高度 */
        let halfEditorHeight = ref<string>(`${props.height / 2}px`);

        /**预览内部样式链接 */
        const previewInnerStyles: ComputedRef<
            Array<{ id: string; style: string }>
        > = computed(() => {
            return [{ id: "inner", style: props.appendStyleCss }];
        });

        /**预览外部样式链接 */
        const previewOuterStyles: ComputedRef<
            Array<{ id: string; url: string }>
        > = computed(() => {
            let appendedUrls = props.appendStyleUrls.map((item, index) => {
                return {
                    id: index.toString(),
                    url: item,
                };
            });

            let defaultUrls = defaultStyleUrls.value.map((item, index) => {
                return {
                    id: `default-${index}`,
                    url: item,
                };
            });

            return [...appendedUrls, ...defaultUrls];
        });

        let returnObj = {
            uploadImg,
            markdownIt,
            toolbarRef,
            editorHeight,
            editor,
            insertLink,
            textareaRef,
            reloadPreviewStyle,
            halfEditorHeight,
            themes,
            message,
            loadAppendStyleUrls,
            showAppendStyleCssUrls,
            language,
            insertCodeBlock,
            previewInnerStyles,
            previewOuterStyles,
            previewHtml,
            markdownContent,
        };

        return returnObj;
    },
});
</script>

<style>
.editor-container {
    border: 0px solid gray;
    /* width: 100%; */
    overflow: auto;
}

.editor-toolbar {
    min-height: 3px;
    border-bottom: 0px solid gray;
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
