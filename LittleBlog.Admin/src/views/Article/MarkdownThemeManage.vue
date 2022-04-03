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
    NDialogProvider,
    NDropdown,
    NSpace,
    useDialog,
    useMessage,
} from "naive-ui";
import { InternalRowData } from "naive-ui/lib/data-table/src/interface";
import { DialogApiInjection } from "naive-ui/lib/dialog/src/DialogProvider";
import { defineComponent, h, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import api from "../../api";
import { MarkdownTheme } from "../../types";

function createColumns(
    editRow: (id: string) => void,
    deleteRow: (id: string) => void,
    dialog: DialogApiInjection
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
                                NDropdown,
                                {
                                    options: [
                                        {
                                            label: "设为默认主题",
                                            key: "setDefaultStyle",
                                        },
                                        {
                                            label: "设为默认代码主题",
                                            key: "setDefaultCodeStyle",
                                        },
                                        {
                                            label: "编辑",
                                            key: "edit",
                                        },
                                        {
                                            label: "删除",
                                            key: "remove",
                                        },
                                    ],
                                    trigger: "hover",
                                    onSelect: (key) => {
                                        if (key == "edit") {
                                            console.log("edit");
                                            editRow(row["id"] as string);
                                            return;
                                        }

                                        if (key == "setDefaultStyle") {
                                            console.log("set default");
                                            return;
                                        }

                                        if (key == "setDefaultCodeStyle") {
                                            return;
                                        }

                                        if (key == "remove") {
                                            console.log("remove");
                                            dialog.warning({
                                                title: "警告",
                                                content: `确定要删除吗？`,
                                                positiveText: "确定",
                                                negativeText: "取消",
                                                onPositiveClick: () => {
                                                    deleteRow(
                                                        row["id"] as string
                                                    );
                                                },
                                            });
                                            return;
                                        }
                                    },
                                },
                                {
                                    default: () =>
                                        h(
                                            NButton,
                                            {
                                                size: "small",
                                            },
                                            {
                                                default: () => "操作",
                                            }
                                        ),
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
        delete(id: string) {
            api.admin.markdownThemes.remove(id).then((result) => {
                if (result.isSuccess) {
                    this.message.success("删除成功！");
                    this.list();
                } else {
                    this.message.error(result.message);
                }
            });
        },
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

        this.columns = createColumns(editRow, deleteRow, _this.dialog);

        this.list();
    },
    setup() {
        const message = useMessage();
        const router = useRouter();
        const dialog = useDialog();

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
            dialog,
        };
    },
});
</script>
