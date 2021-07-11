<template>
    <n-grid :y-gap="12" :cols="1">
        <n-grid-item>
            <n-grid :x-gap="12" :cols="4">
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
                        <n-button @click="list"> 查询 </n-button>
                        <n-button type="info" @click="gotoEdit(0)">
                            新增
                        </n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item :cols="1">
            <n-data-table
                :columns="columns"
                :paging="false"
                :pagination="pagination"
            >
            </n-data-table>
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
import { ListArticlesQueryContext } from "../../types";

// 构建表列
function createColumns(
    editArticle: (id: number | string) => void,
    deleteArticle: (id: number | string) => void // 方法返回类型
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
                                        editArticle(row.id as number | string);
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
                                        deleteArticle(
                                            row.id as number | string
                                        );
                                    },
                                    onNegativeClick: () => false,
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
            } as PaginationProps,
        };
    },
    methods: {
        list() {
            // console.log(api.urls.admin.Article.list);
            let query = {} as ListArticlesQueryContext;
            let result = api.admin.articles.list(query);
            console.log(result.message);
        },
        edit(id: number | string) {
            this.gotoEdit(id);
        },
        delete(id: number | string) {},
        onPageChangd(page: number) {},
    },
    mounted() {
        let _this = this;
        const editArticle: (id: number | string) => void = (id) => {
            _this.edit(id);
        };

        const deleteArticle: (id: number | string) => void = (id) => {
            _this.delete(id);
        };

        this.columns = createColumns(editArticle, deleteArticle);
    },
    setup() {
        const router = useRouter();
        const message = useMessage();

        const gotoEdit = (id: number | string) => {
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

<style></style>
