<template>
    <n-grid y-gap="12" cols="1">
        <n-grid-item>
            <n-grid x-gap="12" y-gap="3" cols="1 640:4">
                <n-grid-item>
                    <n-space>
                        <n-button @click="list()"> 查询 </n-button>
                        <n-button type="info" @click="add()"> 新增 </n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item :cols="1">
            <n-spin :show="isLoading">
                <n-data-table :data="data" :columns="columns"> </n-data-table>
            </n-spin>
        </n-grid-item>
    </n-grid>
</template>

<script lang="ts">
import CodeMirror from "codemirror";
import {
    DataTableColumn,
    NButton,
    NPopconfirm,
    NSpace,
    useMessage,
} from "naive-ui";
import { InternalRowData } from "naive-ui/lib/data-table/src/interface";
import { defineComponent, h, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import api from "../../api";
import { MarkdownTheme } from "../../types";

function createColumns(
    editRow: (id: string) => void,
    deleteRow: (id: string) => void
): Array<DataTableColumn> {
    return [
        {
            title: "名称",
            key: "name",
        },
        {
            title: "URL",
            key: "url",
        },
        {
            title: "路径",
            key: "physicalPath",
        },
        {
            title: "最后修改日期",
            width: 200,
            key: "lastEditTime",
            render(row: InternalRowData) {
                return (row["lastEditTime"] as string)
                    ?.substring(0, 19)
                    ?.replace("T", " ");
            },
        },
        {
            title: "操作",
            width: 140,
            key: "operation",
            render(row: InternalRowData) {
                return h(
                    NSpace,
                    {},
                    {
                        default: () => [
                            h(
                                NButton,
                                {
                                    onClick: () => {
                                        editRow(row["id"] as string);
                                    },
                                    type: "info",
                                    size: "small",
                                },
                                { default: () => "编辑" }
                            ),
                            h(
                                NPopconfirm,
                                {
                                    onPositiveClick: () => {
                                        deleteRow(row["id"] as string);
                                    },
                                    negativeText: "取消",
                                    positiveText: "确认",
                                    flip: true,
                                    placement: "top-start",
                                },
                                {
                                    trigger: () => {
                                        h(
                                            NButton,
                                            {
                                                size: "small",
                                                type: "warning",
                                            },
                                            {
                                                default: () => "删除",
                                            }
                                        );
                                    },
                                    default: () => "确认要删除这个吗？",
                                }
                            ),
                        ],
                    }
                );
            },
        },
    ];
}

export default defineComponent({
    name: "MarkdownThemeManage",
    data() {
        return {
            isLoading: false,
            columns: [] as DataTableColumn[],
            data: [] as any[],
        };
    },
    methods: {
        list() {
            api.admin.markdownThemes.list().then((result) => {
                if (result.isSuccess) {
                    this.data = result.data;
                }
            });
        },
        edit(id: string) {
            this.gotoEdit(id);
        },
        delete(id: string) {},
        add() {
            this.gotoCreate();
        },
    },
    mounted() {
        let _this = this;
        const editRow: (id: string) => void = (id) => {
            _this.edit(id);
        };
        const deleteRow: (id: string) => void = (id) => {
            _this.delete(id);
        };

        this.columns = createColumns(editRow, deleteRow);

        this.list();
    },
    setup() {
        const message = useMessage();
        const router = useRouter();

        const StyleCssRef = ref();
        let editor: CodeMirror.EditorFromTextArea =
            {} as CodeMirror.EditorFromTextArea;

        const gotoEdit = (id: string) => {
            router.push({
                name: "mdThemeEdit",
                params: {
                    id: id,
                },
            });
        };

        const gotoCreate = () => {
            router.push({
                name: "mdThemeCreate",
            });
        };

        return {
            message,
            editor,
            gotoEdit,
            gotoCreate,
        };
    },
});
</script>
