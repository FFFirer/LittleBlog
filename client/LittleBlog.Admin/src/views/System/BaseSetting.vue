<template>
    <n-form :model="systemConfig" label-placement="top" ref="formRef">
        <n-grid x-gap="12" cols="1">
            <n-form-item-gi>
                <span class="form-title"> 基础设置 </span>
            </n-form-item-gi>
            <n-form-item-gi path="baseInfo.siteName" label="站点名称">
                <n-input
                    placeholder="站点名称"
                    v-model:value="systemConfig.baseInfo.siteName"
                >
                </n-input>
            </n-form-item-gi>
            <n-form-item-gi path="baseInfo.welcome" label="欢迎语">
                <n-input
                    placeholder="首屏欢迎语"
                    v-model:value="systemConfig.baseInfo.welcome"
                >
                </n-input>
            </n-form-item-gi>
        </n-grid>

        <n-grid x-gap="12" cols="1">
            <n-form-item-gi>
                <span class="form-title"> 备案设置 </span>
            </n-form-item-gi>
            <n-form-item-gi path="filing.number" label="备案号">
                <n-input
                    placeholder="备案号"
                    v-model:value="systemConfig.filing.number"
                >
                </n-input>
            </n-form-item-gi>
        </n-grid>

        <n-grid x-gap="12" cols="1">
            <n-grid-item>
                <n-space>
                    <n-button type="info" @click="save">保存</n-button>
                    <n-button @click="load">取消</n-button>
                </n-space>
            </n-grid-item>
        </n-grid>
    </n-form>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SystemConfig } from "../../types";
import api from "../../api/index";
import { useMessage } from "naive-ui";
export default defineComponent({
    name: "BaseSetting",
    data() {
        return {
            systemConfig: {
                baseInfo: {},
                filing: {},
            } as SystemConfig,
        };
    },
    methods: {
        load() {
            api.admin.syscfg.base.get().then((resp) => {
                if (resp.isSuccess) {
                    this.systemConfig = resp.data;
                } else {
                    this.message.warning(resp.message);
                }
            });
        },
        save() {
            api.admin.syscfg.base.save(this.systemConfig).then((resp) => {
                if (resp.isSuccess) {
                    this.message.success("保存成功！");
                } else {
                    this.message.warning(resp.message);
                }
            });
        },
    },
    mounted() {
        this.load();
    },
    setup() {
        const message = useMessage();

        return {
            message,
        };
    },
});
</script>

<style>
.form-title {
    font-size: 24px;
    font-weight: 800;
}
</style>
