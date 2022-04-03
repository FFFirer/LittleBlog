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
import { result } from "lodash";
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
import { DefaultMarkdownThemeInfo } from "../../types/markdownThemes";

function createColumns(
    editRow: (id: string) => void,
    deleteRow: (id: string) => void,
    dialog: DialogApiInjection,
    onDropDownSelected: (key: string, row: any) => void,
    isDefaultTheme: (id: string) => boolean,
    isCodeBlockDefaultTheme: (id: string) => boolean
): Array<DataTableColumn> {
    return [
        {
            title: "名称",
            key: "name",
            render(row: InternalRowData) {
                let badges = [];
                if (isDefaultTheme(row["id"] as string)) {
                    let defautlThemeBadge = h(
                        "span",
                        {
                            class: ["badge", "badge-primary"],
                            style: "margin-right: 5px",
                        },
                        {
                            default: () => "默认主题",
                        }
                    );
                    badges.push(defautlThemeBadge);
                }
                if (isCodeBlockDefaultTheme(row["id"] as string)) {
                    let codeBlockDefaultThemeBadge = h(
                        "span",
                        {
                            class: ["badge", "badge-secondary"],
                            style: "margin-right: 5px",
                        },
                        {
                            default: () => "代码块默认主题",
                        }
                    );

                    badges.push(codeBlockDefaultThemeBadge);
                }
                badges.push(row["name"] as string);
                return badges;
            },
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
                                            key: "setDefaultTheme",
                                        },
                                        {
                                            label: "设为默认代码主题",
                                            key: "setCodeBlockDefaultTheme",
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
                                        onDropDownSelected(key, row);
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
            basicInfo: {} as DefaultMarkdownThemeInfo,
            isLoadedBasic: false,
        };
    },
    methods: {
        list() {
            if (!this.isLoadedBasic) {
                this.loadBasic();
            }
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
        loadBasic() {
            api.admin.markdownThemes.getDefault().then((result) => {
                if (result.isSuccess) {
                    this.basicInfo = result.data;
                    this.isLoadedBasic = true;
                } else {
                    this.message.error(result.message);
                }
            });
        },
        setDefaultTheme(id: string) {
            api.admin.markdownThemes.setDefaultTheme(id).then((result) => {
                if (result.isSuccess) {
                    this.message.success("设置成功");
                    this.isLoadedBasic = false;
                    this.list();
                } else {
                    this.message.error(result.message);
                }
            });
        },
        setCodeBlockDefaultTheme(id: string) {
            api.admin.markdownThemes
                .setCodeBlockDefaultTheme(id)
                .then((result) => {
                    if (result.isSuccess) {
                        this.message.success("设置成功");
                        this.isLoadedBasic = false;
                        this.list();
                    } else {
                        this.message.error(result.message);
                    }
                });
        },
        handleDropDownSelected(key: string, row: MarkdownTheme) {
            const self = this;
            if (key == "edit") {
                console.log("edit");
                this.edit(row.id);
                return;
            }

            if (key == "setDefaultTheme") {
                console.log("set default");
                this.setDefaultTheme(row.id);
                return;
            }

            if (key == "setCodeBlockDefaultTheme") {
                this.setCodeBlockDefaultTheme(row.id);
                return;
            }

            if (key == "remove") {
                console.log("remove");
                this.dialog.warning({
                    title: "警告",
                    content: `确定要删除吗？`,
                    positiveText: "确定",
                    negativeText: "取消",
                    onPositiveClick: () => {
                        this.delete(row.id);
                    },
                });
                return;
            }
        },
        isDefaultTheme(id: string): boolean {
            return id == this.basicInfo.defaultThemeId;
        },
        isCodeBlockDefaultTheme(id: string): boolean {
            return id == this.basicInfo.defaultCodeBlockThemeId;
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

        this.columns = createColumns(
            editRow,
            deleteRow,
            _this.dialog,
            _this.handleDropDownSelected,
            _this.isDefaultTheme,
            _this.isCodeBlockDefaultTheme
        );

        this.loadBasic();
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
