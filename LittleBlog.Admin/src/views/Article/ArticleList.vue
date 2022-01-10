<template>
    <n-grid y-gap="12" cols="1">
        <n-grid-item>
            <n-grid x-gap="12" y-gap="3" cols="1 640:4">
                <n-grid-item>
                    <n-input
                        type="input"
                        v-model:value="keyword"
                        placeholder="请输入查询关键词"
                    >
                    </n-input>
                </n-grid-item>
                <n-grid-item>
                    <n-space>
                        <n-button @click="list()"> 查询 </n-button>
                        <n-button type="info" @click="gotoEdit(0)">
                            新增
                        </n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item :cols="1">
            <n-spin :show="isLoading">
                <n-data-table
                    :data="data"
                    :columns="columns"
                    :paging="false"
                    :pagination="pagination"
                    :row-class-name="checkRowClass"
                >
                </n-data-table>
            </n-spin>
        </n-grid-item>
    </n-grid>
</template>

<script lang="ts">
import { defineComponent, h, VNode } from "vue";
import { useRouter } from "vue-router";

import api from "../../api/index";

import {
    DataTableColumn,
    NButton,
    NPopconfirm,
    NSpace,
    PaginationProps,
    useMessage,
} from "naive-ui";
import { InternalRowData } from "naive-ui/lib/data-table/src/interface";
import { ArticleDto, ListArticlesQueryContext } from "../../types";

// 构建表列
function createColumns(
    editArticle: (id: number) => void,
    deleteArticle: (id: number) => void // 方法返回类型
): Array<DataTableColumn> {
    return [
        {
            title: "标题",
            key: "title",
        },
        {
            title: "作者",
            width: 200,
            key: "author",
        },
        {
            title: "状态",
            width: 200,
            key: "status",
            render(row: InternalRowData) {
                if (row["isPublished"]) {
                    return "已发布";
                } else {
                    return "未发布";
                }
            },
        },
        {
            title: "最后修改日期",
            width: 200,
            key: "lastEditTime",
            render(row: InternalRowData) {
                return (row["lastEditTime"] as string)
                    .substr(0, 19)
                    .replace("T", " ");
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
                        // 默认插槽default
                        default: () => [
                            h(
                                NButton,
                                {
                                    onClick: () => {
                                        editArticle(row.id as number);
                                    },
                                    type: "info",
                                    size: "small",
                                },
                                {
                                    default: () => "编辑",
                                }
                            ),
                            h(
                                NPopconfirm,
                                {
                                    onPositiveClick: () => {
                                        console.log("delete start");
                                        deleteArticle(row.id as number);
                                    },
                                    negativeText: "取消",
                                    positiveText: "确认",
                                    filp: true,
                                    placement: "top-start",
                                },
                                {
                                    trigger: () =>
                                        h(
                                            NButton,
                                            {
                                                size: "small",
                                                type: "warning",
                                            },
                                            {
                                                default: () => "删除",
                                            }
                                        ),
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
    name: "ArticleList",
    data() {
        return {
            columns: [] as DataTableColumn[], // 返回数据类型注释
            keyword: "" as string,
            data: [] as any[],
            pagination: {
                page: 1,
                pageSize: 20,
                itemCount: 0,
            },
            isLoading: false,
        };
    },
    methods: {
        list() {
            // console.log(api.urls.admin.Article.list);
            let query = {
                keyword: this.keyword,
                page: this.pagination.page,
                pageSize: this.pagination.pageSize,
                onlyPublished: false,
            } as ListArticlesQueryContext;
            this.isLoading = true;
            api.admin.articles
                .list(query)
                .then((res) => {
                    console.log(res.message);
                    if (res.isSuccess) {
                        this.data = res.data.rows;
                        this.pagination.itemCount = res.data.total;
                    } else {
                        this.message.warning(res.message);
                    }
                })
                .catch((err) => {
                    if (typeof err == "string") {
                        this.message.error(err);
                        return;
                    } else {
                        this.message.error(err.message);
                        return;
                    }
                })
                .finally(() => {
                    this.isLoading = false;
                });
        },
        edit(id: number) {
            this.gotoEdit(id);
        },
        delete(id: number) {
            api.admin.articles
                .delete(id)
                .then((res) => {
                    if (res.isSuccess) {
                        this.pagination.page = 1;
                        this.list();
                    } else {
                        this.message.warning(res.message);
                    }
                })
                .catch((err) => {
                    this.message.error("删除失败！");
                    console.log(err);
                });
        },
        onPageChangd(page: number) {},
        checkRowClass(row: InternalRowData, index: number) {
            if (row["isPublished"]) {
                return "article-published";
            } else {
                return "article-pending";
            }
        },
    },
    mounted() {
        let _this = this;
        const editArticle: (id: number) => void = (id) => {
            _this.edit(id);
        };

        const deleteArticle: (id: number) => void = (id) => {
            _this.delete(id);
        };

        this.columns = createColumns(editArticle, deleteArticle);

        this.list();
    },
    setup() {
        const router = useRouter();
        const message = useMessage();

        const gotoEdit = (id: number) => {
            router.push({
                name: "articleEdit",
                params: {
                    id: id,
                },
            });
        };

        return {
            message,
            gotoEdit,
        };
    },
});
</script>

<style>
tr.article-published td[data-col-key="status"] {
    background-color: rgb(0, 128, 0) !important;
    color: white;
}

tr.article-pending td[data-col-key="status"] {
    background-color: rgb(255, 255, 0) !important;
}
</style>
