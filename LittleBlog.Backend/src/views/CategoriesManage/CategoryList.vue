<template>
  <a-row :gutter="[8, 8]">
    <a-col :span="6">
      <a-input v-model="keyword" placeholder="请输入查询关键词"></a-input>
    </a-col>
    <a-col :span="6">
      <a-space>
        <a-button>查询</a-button>
        <a-button type="primary">新增</a-button>
      </a-space>
    </a-col>
  </a-row>
  <a-row style="margin-top: 5px" :gutter="8">
    <a-col :span="24">
      <a-table bordered :columns="columns">
        <template #opertaion>
          <a-button type="primary">编辑</a-button>
        </template>
      </a-table>
    </a-col>
  </a-row>
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
