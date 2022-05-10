<template>
    <n-form label-placement="left" label-align="right" label-width="120">
        <n-form-item label="名称">
            <n-input v-model:value="theme.name" placeholder="请输入主题名称">
            </n-input>
        </n-form-item>
        <n-form-item label="URL">
            <n-input
                :disabled="!theme.useOuterLink"
                v-model:value="theme.url"
                placeholder="请输入URL"
            ></n-input>
        </n-form-item>
        <n-form-item label="使用外部样式">
            <n-checkbox v-model:checked="theme.useOuterLink"> </n-checkbox>
        </n-form-item>
        <n-form-item label="存储路径">
            <n-input
                :disabled="true"
                v-model:value="theme.physicalPath"
                placeholder="请输入存储路径"
            >
            </n-input>
        </n-form-item>
        <n-form-item label="备注">
            <n-input
                v-model:value="theme.remark"
                placeholder="请输入备注"
            ></n-input>
        </n-form-item>
        <n-form-item label="导入样式">
            <n-button> 打开文件 </n-button>
        </n-form-item>
        <n-form-item label="附加样式">
            <n-select
                v-model:value="selectedToAppendThemes"
                multiple
                :options="allThemes"
            ></n-select>
            <n-button type="info" @click="getUrls()">
                查看当前已加载样式
            </n-button>
        </n-form-item>
        <n-form-item label="编辑主题" v-show="!theme.useOuterLink">
            <textarea ref="styleCssRef"> </textarea>
        </n-form-item>
        <n-form-item label="预览效果">
            <div class="markdown-box">
                <markdown-editor
                    :height="300"
                    :useDefaultTheme="false"
                    v-model:appendStyleUrls="appendStyleCssUrls"
                    v-model:appendStyleCss="appendStyleCss"
                >
                </markdown-editor>
            </div>
        </n-form-item>
        <n-form-item label="">
            <n-button @click="cancelEdit()"> 取消 </n-button>
            <n-button type="info" @click="save()"> 保存 </n-button>
        </n-form-item>
    </n-form>
</template>

<script lang="ts">
import CodeMirror from "codemirror";
import { SelectOption, useMessage } from "naive-ui";
import { computed, defineComponent, onMounted, Ref, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import api from "../../api";
import { MarkdownTheme } from "../../types";
import MarkdownEditor from "../../components/MarkdownEditor.vue";

import "codemirror/lib/codemirror.css";
import "codemirror/mode/css/css";

// 自动提示核心
import "codemirror/addon/hint/show-hint.css";
import "codemirror/addon/hint/show-hint";
import "codemirror/addon/hint/css-hint";

import "codemirror/theme/rubyblue.css";
import "codemirror/addon/edit/matchbrackets";
import "codemirror/addon/selection/active-line";

import "codemirror/addon/edit/closebrackets";
// 折叠代码要用到的东西
import "codemirror/addon/fold/foldgutter.css";
import "codemirror/addon/fold/foldgutter";
import "codemirror/addon/fold/xml-fold";
import "codemirror/addon/fold/foldcode";
import "codemirror/addon/fold/brace-fold";
import "codemirror/addon/fold/indent-fold.js";
import "codemirror/addon/fold/markdown-fold.js";
import "codemirror/addon/fold/comment-fold.js";

let remoteUrl =
    import.meta.env.VITE_REMOTE_API_ADDRESS.indexOf("/") ==
    import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
        ? import.meta.env.VITE_REMOTE_API_ADDRESS.substring(
              0,
              import.meta.env.VITE_REMOTE_API_ADDRESS.length - 1
          )
        : import.meta.env.VITE_REMOTE_API_ADDRESS;

export default defineComponent({
    name: "MarkdownThemeEditor",
    components: {
        MarkdownEditor,
    },
    methods: {
        getUrls() {
            alert(this.appendStyleCssUrls);
        },
    },
    props: {
        id: {
            type: String,
            default: "",
        },
    },
    setup(props) {
        const message = useMessage();
        const router = useRouter();

        const styleCssRef = ref();
        const appendStyleCss = ref("");

        const appendStyleCssUrls = computed(() => {
            if (theme.value.useOuterLink) {
                return [...selectedToAppendThemes.value, theme.value.url];
            } else {
                return [...selectedToAppendThemes.value];
            }
        });

        let { id } = props;

        let theme = ref<MarkdownTheme>(new MarkdownTheme());
        let editor: CodeMirror.EditorFromTextArea =
            {} as CodeMirror.EditorFromTextArea;

        /**保存 */
        const save = () => {
            api.admin.markdownThemes.save(theme.value).then((result) => {
                if (result.isSuccess) {
                    message.success("保存成功!");
                    backToList();
                } else {
                }
            });
        };

        /**取消 */
        const cancelEdit = () => {
            backToList();
        };

        /**加载 */
        const load = async () => {
            if (id) {
                let result = await api.admin.markdownThemes.get(id);

                if (result.isSuccess) {
                    theme.value = result.data;
                }
            } else {
                theme.value = new MarkdownTheme();
                theme.value.content = "";
                theme.value.name = "";
            }
            await loadAllThemes();
        };

        const backToList = () => {
            router.push({
                name: "mdThemeManage",
            });
        };

        const allThemes = ref<Array<SelectOption>>([]);

        const loadAllThemes = () => {
            api.admin.markdownThemes.list().then((result) => {
                allThemes.value = result.data.map((d) => {
                    if (d.url.indexOf("/") == 0) {
                        const url = `${remoteUrl}/${d.url.substring(1)}`;
                        return {
                            label: d.name,
                            value: url,
                        };
                    } else {
                        return {
                            label: d.name,
                            value: d.url,
                        } as SelectOption;
                    }
                });
            });
        };

        const selectedToAppendThemes: Ref<Array<SelectOption>> = ref([]);

        watch(
            () => theme.value.useOuterLink,
            (nv, ov) => {
                if (nv == true) {
                    // 重新加载外部样式
                    appendStyleCss.value = "";
                    appendStyleCssUrls.value.push(theme.value.url);
                } else {
                    // 重新加载内联样式
                    appendStyleCss.value = theme.value.content;
                }
            }
        );

        watch(
            () => theme.value.url,
            () => {
                if (theme.value.useOuterLink) {
                    appendStyleCssUrls.value = [theme.value.url];
                }
            }
        );

        // mounted
        onMounted(async () => {
            await load();
            if (styleCssRef.value != null) {
                editor = CodeMirror.fromTextArea(
                    styleCssRef.value as HTMLTextAreaElement,
                    {
                        lineNumbers: true,
                        mode: { name: "text/css" },
                        theme: "rubyblue",
                    }
                );

                editor.setValue(theme.value.content);

                editor.on("change", (instance, changeObj) => {
                    // 使用外部样式时不使用内联样式
                    if (!theme.value.useOuterLink) {
                        theme.value.content = instance.getValue();

                        appendStyleCss.value = theme.value.content;
                    }
                });

                if (!theme.value.useOuterLink) {
                    appendStyleCss.value = theme.value.content;
                }
            }
        });

        const markdown = ref("");

        const setupOption = {
            message,
            backToList,
            styleCssRef,
            theme,
            editor,
            save,
            load,
            cancelEdit,
            id,
            appendStyleCss,
            allThemes,
            appendStyleCssUrls,
            markdown,
            selectedToAppendThemes,
        };

        return setupOption;
    },
});
</script>

<style>
.CodeMirror {
    width: 100%;
    height: 400px;
}

.markdown-box {
    width: 100%;
}
</style>
