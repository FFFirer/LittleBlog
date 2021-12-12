<template>
    <div class="md-editor">
        <div class="md-editor-tools"></div>
        <div class="md-editor-body">
            <div class="md-editor-origin">
                <textarea ref="textareaRef"></textarea>
            </div>
            <div class="md-editor-preview">
                <div v-html="renderedHtml"></div>
            </div>
        </div>
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

interface MarkdownEditorProps {
    markdown: Ref<string>;
    html: Ref<string>;
}

export default defineComponent({
    name: "MarkdownEditor",
    props: {
        MarkdownContent: {
            type: String,
        },
        HtmlContent: {
            type: String,
            default: "",
        },
        height: {
            type: String,
            default: "300px",
        },
    },
    setup(props, { emit }) {
        const md = new MarkdownIt();
        const textareaRef = ref();

        let renderedHtml: Ref<string> = ref("");

        let { MarkdownContent, HtmlContent, height } = props;

        let editorHeight = height;

        let markdownContent: Ref<string> = ref("");

        console.log("input", MarkdownContent);

        let mdEditor: CodeMirror.EditorFromTextArea =
            {} as CodeMirror.EditorFromTextArea;

        onMounted(() => {
            if (textareaRef.value != null) {
                textareaRef.value.value = MarkdownContent;

                mdEditor = CodeMirror.fromTextArea(
                    textareaRef.value as HTMLTextAreaElement,
                    {
                        lineNumbers: true,
                    }
                );

                mdEditor.on("change", (instance, changedObj) => {
                    markdownContent.value = instance.getValue();
                });
            }
        });

        watch(
            () => markdownContent.value,
            () => {
                renderedHtml.value = md.render(markdownContent.value);
                MarkdownContent = markdownContent.value;
                HtmlContent = renderedHtml.value;
                emit("update:HtmlContent", renderedHtml.value);
                emit("update:MarkdownContent", MarkdownContent);
            }
        );

        markdownContent.value = MarkdownContent ?? "";

        const renderHtml = () => {
            if (mdEditor != null) {
                renderedHtml.value = md.render(mdEditor.getValue());
            }
        };

        return {
            md,
            textareaRef,
            markdownContent,
            renderedHtml,
            renderHtml,
            mdEditor,
            editorHeight,
        };
    },
});
</script>
<style>
.md-editor {
    overflow: auto;
}

.md-editor-body {
    display: flex;
    flex-direction: row;
    min-height: 100%;
    overflow: auto;
}

.md-editor-origin {
    flex: 1;
    overflow: auto;
    margin: 1px;
}

.md-editor-preview {
    flex: 1;
    height: v-bind(editorHeight);
    background-color: white;
    overflow: auto;
    margin: 1px;
}

.CodeMirror {
    height: v-bind(editorHeight);
}
</style>
