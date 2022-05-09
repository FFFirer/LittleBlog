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
                        <n-button @click="list()"> 查询 </n-button>
                        <n-button type="primary" @click="editArticle(0)">
                            新增
                        </n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item :cols="1">
            <n-data-table
                :data="data"
                :columns="columns"
                :paging="false"
                :pagination="pagination"
            >
            </n-data-table>
        </n-grid-item>
    </n-grid>
</template>

<script lang="ts">
import { defineComponent, h } from 'vue'

import { useRouter } from 'vue-router'

import Apis from '../../api/index.ts'

import { NButton, NSpace, NPopconfirm, useMessage } from 'naive-ui'

import moment from 'moment'

// 定义表格列名
const tableColumns = [
    {
        title: '文章标题',
        key: 'title',
    },
    {
        title: '作者',
        key: 'author',
    },
    {
        title: '操作',
        width: 80,
        key: 'oper',
        render(row) {
            return h(
                NButton,
                {
                    onClick: () => gotoEdit(row.id),
                    type: 'info',
                },
                '编辑'
            )
        },
    },
]

const createColumns = ({ editArticle, deleteArticle }) => {
    return [
        {
            title: '标题',
            key: 'title',
        },
        {
            title: '作者',
            width: 200,
            key: 'author',
        },
        {
            title: '最后修改日期',
            width: 200,
            key: 'lastEditTime',
            render(row) {
                // let lastEditTime = moment(row.lastEditTime)
                // return lastEditTime.format('YYYY-MM-DD HH:mm:ss')
                return row.lastEditTime.substr(0, 19).replace('T', ' ')
            },
        },
        {
            title: '操作',
            width: 140,
            key: 'operation',
            render(row) {
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
                                        editArticle(row.id)
                                    },
                                    type: 'info',
                                    size: 'small',
                                },
                                {
                                    default: () => '编辑',
                                }
                            ),
                            h(
                                NPopconfirm,
                                {
                                    onPositiveClick: () => {
                                        console.log('delete start')
                                        deleteArticle(row.id)
                                    },
                                    onNegativeClick: () => false,
                                    negativeText: '取消',
                                    positiveText: '确认',
                                    filp: true,
                                    placement: 'top-start',
                                },
                                {
                                    trigger: () =>
                                        h(
                                            NButton,
                                            {
                                                size: 'small',
                                                type: 'warning',
                                            },
                                            {
                                                default: () => '删除',
                                            }
                                        ),
                                    default: () => '确认要删除这个吗？',
                                }
                            ),
                        ],
                    }
                )
            },
        },
    ]
}

const tableData = [
    {
        key: 1,
        id: 1,
        title: '文章一',
        author: '佚名',
    },
]

export default defineComponent({
    name: 'ArticleList',
    data() {
        return {
            // columns: null,
            currentPage: 1,
            totalCount: 50,
            data: [],
            keyword: null,
            onlyPublished: false,
            pagination: {
                pageSize: 20,
                itemCount: 0,
            },
            columns: [],
        }
    },
    methods: {
        editArticle(id) {
            this.gotoEdit(id)
        },
        deleteArticle(id) {
            console.log('delete article ' + id)
            Apis.ArticlesManageApi.Delete(id)
                .then((resp) => {
                    if (resp.data.isSuccess) {
                        this.list()
                    } else {
                        this.message.warning(resp.data.message, {
                            closable: true,
                            duration: 5000,
                        })
                    }
                })
                .catch((err) => {
                    console.log(err)
                    this.message.error(err, {
                        closable: true,
                        duration: 5000,
                    })
                })
        },

        onPageChanged(page) {
            this.pagination.page = page
            this.list()
        },
        edit(id) {
            // 编辑文章
            this.gotoEdit(id)
        },
        list() {
            let queryData = {
                keyword: this.keyword,
                onlyPublished: this.onlyPublished,
                page: this.pagination.page,
                pageSize: this.pagination.pageSize,
            }

            console.log(queryData)

            Apis.ArticlesManageApi.List(queryData)
                .then((resp) => {
                    if (resp.data.isSuccess) {
                        this.data = resp.data.data.rows
                        this.pagination.itemCount = resp.data.data.total
                    } else {
                        console.log(resp.data.message)
                    }
                })
                .catch((err) => {
                    console.log(err)
                })
        },
    },
    mounted() {
        this.list()
        let _this = this

        const editArticle = (id) => {
            this.editArticle(id)
        }

        const deleteArticle = (id) => {
            this.deleteArticle(id)
        }

        this.columns = createColumns({
            editArticle,
            deleteArticle,
        })
    },
    setup() {
        const router = useRouter()

        const gotoEdit = (id) => {
            router.push({
                name: 'editArticle', // 命名路由
                params: {
                    id: id,
                },
            })
        }

        const message = useMessage()

        return {
            gotoEdit,
            message,
        }
    },
})
</script>
