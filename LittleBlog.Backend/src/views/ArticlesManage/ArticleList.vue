<template>
  <a-row :gutter="[8, 8]">
    <a-col :span="6">
      <a-input placeholder="请输入查询关键字">

      </a-input>
    </a-col>
    <a-col :span="6">
      <a-button type="primary">查询</a-button>
    </a-col>
  </a-row>
  <a-row style="margin-top: 5px;" :gutter="8">
    <a-col :span="24">
      <a-table bordered :data-source="data" :columns="columns">
        <template #operation="scope">
          <a-button type="primary" @click="edit(scope.record.id)">编辑</a-button>
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
  } from "vue-router";

  // 定义表格列名
  const tableColumns = [{
      title: "文章标题",
      dataIndex: "title",
      key: "title"
    },
    {
      title: "作者",
      dataIndex: "author",
      key: "author"
    },
    {
      title: "操作",
      width: 80,
      slots: {
        customRender: 'operation'
      }
    }
  ]

  const tableData = [{
    key: 1,
    id: 1,
    title: "文章一",
    author: "佚名"
  }]

  export default defineComponent({
    name: "ArticleList",
    data() {
      return {
        columns: tableColumns,
        currentPage: 1,
        totalCount: 50,
        data: tableData
      }
    },
    methods: {
      edit(id) {
        // 编辑文章
        this.gotoEdit(id);
      }
    },
    setup() {
      const router = useRouter();
      const gotoEdit = (id) => {
        router.push({
          name: "editArticle", // 命名路由
          params: {
            id: id
          }
        });
      }

      return {
        gotoEdit
      }
    }
  })

</script>
