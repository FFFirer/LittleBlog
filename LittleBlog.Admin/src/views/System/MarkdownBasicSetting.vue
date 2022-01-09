<template>
    <n-form label-placement="left" label-align="right" label-width="200">
        <n-form-item label="Markdown主题文件路径">
            <n-input
                v-model:value="setting.markdownStyleUrl"
                placeholder="请输入主题文件路径"
            >
            </n-input>
        </n-form-item>
        <n-form-item label="Markdown代码块主题文件路径">
            <n-input
                v-model:value="setting.codeBlockStyleUrl"
                placeholder="请输入代码块主题文件路径"
            >
            </n-input>
        </n-form-item>
        <n-form-item label="">
            <n-button type="info" @click="saveCurrent()"> 保存 </n-button>
        </n-form-item>
    </n-form>
</template>

<script lang="ts">
import { useMessage } from "naive-ui";
import { defineComponent, onMounted, ref, Ref } from "vue";
import api from "../../api";
import { DefaultMarkdownThemeInfo } from "../../types/markdownThemes";

export default defineComponent({
    name: "MarkdownBasicSetting",
    setup() {
        const message = useMessage();

        const setting = ref<DefaultMarkdownThemeInfo>({
            codeBlockStyleUrl: "",
            markdownStyleUrl: "",
        });

        const loadCurrent = async (): Promise<DefaultMarkdownThemeInfo> => {
            let result = await api.admin.markdownThemes.getDefault();
            if (result.isSuccess) {
                return result.data;
            }
            return {
                codeBlockStyleUrl: "",
                markdownStyleUrl: "",
            } as DefaultMarkdownThemeInfo;
        };

        const saveCurrent = async () => {
            if (!setting.value) {
                return;
            }

            let result = await api.admin.markdownThemes.saveDefault(
                setting.value
            );

            if (result.isSuccess) {
                message.success("成功！");
            } else {
                message.warning("失败！");
            }
        };

        onMounted(async () => {
            let info = await loadCurrent();
            setting.value.markdownStyleUrl = info.markdownStyleUrl;
            setting.value.codeBlockStyleUrl = info.codeBlockStyleUrl;
        });

        const setupOption = {
            message,
            setting,
            loadCurrent,
            saveCurrent,
        };

        return setupOption;
    },
});
</script>

<style></style>
