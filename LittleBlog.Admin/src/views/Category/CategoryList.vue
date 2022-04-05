<template>
    <n-grid :y-gap="12" :cols="1">
        <n-grid-item>
            <n-grid :x-gap="12" :cols="4">
                <n-grid-item>
                    <n-space>
                        <n-button @click="list()"> 查询 </n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item :cols="1">
            <n-spin :show="isLoading">
                <n-space vertical>
                    <n-data-table
                        :data="data"
                        :columns="columns"
                        :pagination="false"
                    >
                    </n-data-table>
                    <n-pagination
                        v-show="moreThanOnePage"
                        :default-page="1"
                        :default-page-size="20"
                        :page-sizes="pagination.pageSizes"
                        :page="pagination.page"
                        :page-size="pagination.pageSize"
                        :show-size-Picker="true"
                        :item-count="pagination.itemCount"
                        @update:page="handlePaginationPageUpdated"
                        @update:page-size="handlePaginationPageSizeUpdated"
                    >
                    </n-pagination>
                </n-space>
            </n-spin>
        </n-grid-item>
    </n-grid>
</template>

<script lang="ts">
import {
    DataTableColumn,
    NButton,
    NPopconfirm,
    useMessage,
    NSpace,
    PaginationProps,
} from "naive-ui";
import { InternalRowData } from "naive-ui/lib/data-table/src/interface";
import { computed, defineComponent, h, reactive, ref } from "vue";
import { createRouter, useRouter } from "vue-router";
import api from "../../api";
import { Category, ListPagingCategoriesQueryContext } from "../../types";

function CreateColumns(
    deleteCategory: (categoryName: string) => void
): Array<DataTableColumn> {
    return [
        {
            title: "No.",
            key: "rowIndex",
            render(row: InternalRowData, index: number) {
                return index + 1;
            },
            width: 50,
        },
        {
            title: "分类",
            key: "name",
        },
        {
            title: "创建日期",
            width: 200,
            key: "createTime",
            render(row: InternalRowData) {
                return (row["createTime"] as string)
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
                        default: () => [
                            h(
                                NPopconfirm,
                                {
                                    onPositiveClick: () => {
                                        deleteCategory(row.name as string);
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
    name: "CategoryList",
    data() {
        return {
            columns: [] as DataTableColumn[],
            data: [] as Category[],
            isLoading: false,
        };
    },
    methods: {
        list() {
            this.isLoading = true;
            let query: ListPagingCategoriesQueryContext = {
                page: this.pagination.page ?? 1,
                pageSize: this.pagination.pageSize ?? 20,
            };
            api.admin.categories
                .listSummaries(query)
                .then((res) => {
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
        delete(categoryName: string) {
            this.isLoading = true;
            api.admin.categories
                .delete(categoryName)
                .then((res) => {
                    if (res.isSuccess) {
                        this.message.success("删除成功");
                        this.list();
                    } else {
                        this.message.warning(res.message);
                    }
                })
                .catch((err) => {
                    this.message.error("删除失败！");
                    console.log(err);
                })
                .finally(() => {
                    this.isLoading = false;
                });
        },
        handlePaginationPageUpdated(page: number) {
            this.pagination.page = page;
            this.list();
        },
        handlePaginationPageSizeUpdated(pageSize: number) {
            this.pagination.pageSize = pageSize;
            this.pagination.page = 1;

            this.list();
        },
    },
    mounted() {
        const deleteCategory: (categoryName: string) => void = (
            categoryName
        ) => {
            this.delete(categoryName);
        };

        this.columns = CreateColumns(deleteCategory);

        this.list();
    },
    setup() {
        const router = useRouter();
        const message = useMessage();

        const paginationReactive = reactive<PaginationProps>({
            page: 1,
            pageSize: 20,
            pageSizes: [20, 50, 100],
            showSizePicker: true,
        });

        const moreThanOnePage = computed(() => {
            console.log("computed", paginationReactive.pageCount);
            return (
                (paginationReactive.itemCount ?? 0) >
                (paginationReactive.pageSize ?? 1)
            );
        });

        return {
            message,
            pagination: paginationReactive,
            moreThanOnePage,
        };
    },
});
</script>
