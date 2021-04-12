<template>
  <a-form :label-col="{span: 2}" :wrapper-col="{span: 20}">
    <a-form-item label=" 标题">
      <a-input>

      </a-input>
    </a-form-item>
    <a-form-item label="作者">
      <a-input>

      </a-input>
    </a-form-item>
    <a-form-item label="正文">
      <MyEditor v-model="content" :init="init"></MyEditor>
    </a-form-item>
    <a-form-item label="分类">
      <a-input></a-input>
    </a-form-item>
    <a-form-item label="标签">
      <a-input></a-input>
    </a-form-item>
    <a-form-item :label-col="{span: 2}" :wrapper-col="{span: 20, offset: 2}">
      <a-checkbox>
        发布
      </a-checkbox>
    </a-form-item>
  </a-form>
  <a-row>
    <a-col :span="20" :offset="2">
      <a-space>
        <a-button @click="backToList">
          取消
        </a-button>
        <a-button type="primary">
          保存
        </a-button>
      </a-space>
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

  import MyEditor from '../../components/MyEditor.vue'

  export default defineComponent({
    name: "ArticleEdit",
    components: {
      MyEditor
    },
    data() {
      return {
        content: ""
      }
    },
    setup() {
      const router = useRouter();
      const backToList = (id) => {
        router.push({
          name: "articleList", // 命名路由
        });
      }

      return {
        backToList
      }
    }
  })

</script>
