<template>
  <a-layout id="main">
    <a-layout-header id="header">
      <h1>Welcome to LittleBlog!</h1>
    </a-layout-header>
    <a-layout>
      <a-layout-content id="main-content">
        <a-row>
          <a-col :span="4"></a-col>
          <a-col :span="16">
            <div>
              <router-view></router-view>
            </div>
          </a-col>
          <a-col :span="4">
            <div class="list-group">
              <a-list class="list-custom" bordered :data-source="categories">
                <template #header>
                  <div>
                    分类
                  </div>
                </template>
                <template #renderItem="scope">
                  <a-list-item style="text-align: left;">
                    {{ scope.item.title }}
                  </a-list-item>
                </template>
              </a-list>
              <a-list class="list-custom" bordered :data-source="archives">
                <template #header>
                  <div>
                    归档
                  </div>
                </template>
                <template #renderItem="scope">
                  <a-list-item style="text-align: left;">
                    {{ scope.item.title }}
                  </a-list-item>
                </template>
              </a-list>
              <a-list class="list-custom" bordered :data-source="tags">
                <template #header>
                  <div>
                    标签
                  </div>
                </template>
                <template #renderItem="scope">
                  <a-list-item style="text-align: left;">
                    {{ scope.item.title }}
                  </a-list-item>
                </template>
              </a-list>
            </div>
          </a-col>
        </a-row>
      </a-layout-content>
    </a-layout>
    <a-layout-footer>
      &copy; 2021 LittleBlog.fffirer.top
    </a-layout-footer>
  </a-layout>
</template>

<script>
  import {
    useRouter
  } from 'vue-router'

  export default {
    name: 'App',
    data() {
      return {
        categories: [{
            title: "分类"
          },
          {
            title: "分类"
          },
          {
            title: "分类"
          },
        ],
        archives: [{
            title: "归档"
          },
          {
            title: "归档"
          },
          {
            title: "归档"
          },
          {
            title: "归档"
          },
        ],
        tags: [{
            title: "标签"
          },
          {
            title: "标签"
          },
          {
            title: "标签"
          },
          {
            title: "标签"
          },
          {
            title: "标签"
          },
          {
            title: "标签"
          },
        ]
      }
    }
  }
</script>

<style>
  @media (min-with: 1500px) {
    #main .ant-layout-sider {
      width: 450px;
    }
  }

  #main {
    min-height: 100vh;
  }

  #main-content {
    overflow: auto;
  }

  #main .ant-layout-header,
  #main .ant-layout-sider {
    background-color: inherit;
  }

  .ant-list-split .ant-list-header {
    border-bottom: 1px solid rgb(217, 217, 217) !important;
  }

  .ant-list-bordered .ant-list-item {
    border-bottom: 1px solid rgb(217, 217, 217) !important;
  }

  #sidebar {
    background-color: white;
    padding: 5px;
  }

  .list-group .list-custom {
    margin-bottom: 10px;
  }

  .list-group .ant-list-item:hover {
    cursor: pointer;
    font-weight: bold;
    background-color: rosybrown;
    color: white;
  }

  .list-group .ant-list-header {
    font-size: 20px;
  }
</style>