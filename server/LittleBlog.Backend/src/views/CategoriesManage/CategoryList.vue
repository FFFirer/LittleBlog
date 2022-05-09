<template>
    <n-grid :y-gap="12" :cols="1">
        <n-grid-item>
            <n-grid :x-gap="12" :cols="4">
                <n-grid-item>
                    <n-input v-model="keyword" placeholder="请输入查询关键词"></n-input>
                </n-grid-item>
                <n-grid-item>
                    <n-space>
                        <n-button>查询</n-button>
                        <n-button type="primary">新增</n-button>
                    </n-space>
                </n-grid-item>
            </n-grid>
        </n-grid-item>
        <n-grid-item>

        </n-grid-item>
    </n-grid>
</template>

<script lang="ts">
    import {
        defineComponent
    } from "vue";

    import {
        useRouter
    } from 'vue-router'

    const tableColumns = [{
            title: "分类名称",
            dataIndex: "name",
            key: "name"
        },
        {
            title: "描述",
            dataIndex: "description",
            key: "description"
        },
        {
            title: "操作",
            width: 80,
            slots: {
                customRender: 'operation'
            }
        }
    ]

    export default defineComponent({
        name: "CategoryList",
        data() {
            return {
                columns: tableColumns
            }
        }
    })

</script>
