import { createRouter, createWebHashHistory } from 'vue-router'

import Home from '../views/Home.vue'

// articles manage
import ArticleEdit from '../views/ArticlesManage/ArticleEdit.vue'
import ArticleList from '../views/ArticlesManage/ArticleList.vue'

// categories manage
import CategoryList from '../views/CategoriesManage/CategoryList.vue'

// tags manage
import TagList from '../views/TagsManage/TagList.vue'

// error view
import NotFound from '../views/Errors/NotFound.vue'

const routerHistory = createWebHashHistory()

const routes = [
  {
    path: '/',
    component: Home,
  },
  {
    name: 'articleList',
    path: '/articles/manage',
    component: ArticleList,
  },
  {
    name: 'editArticle', // 命名路由
    path: '/articles/edit/:id',
    component: ArticleEdit,
    props: true,
  },
  {
    path: '/tags/manage',
    component: TagList,
  },
  {
    path: '/categories/manage',
    component: CategoryList,
  },
  {
    path: '/errors/404',
    component: NotFound,
  },
]

const router = createRouter({
  routes: routes,
  history: routerHistory,
})

// 路由守卫, 导航守卫
router.beforeEach((to, from, next) => {
  if (to.matched.length !== 0) {
    next()
  } else {
    next({
      path: '/errors/404',
    })
  }
})

export default router
