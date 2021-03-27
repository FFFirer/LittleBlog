// VueRouter
import {
    createRouter,
    createWebHistory
}
from 'vue-router'

// pages
import Home from '../pages/Home.vue'
import Article from '../pages/Article.vue'

const routerHistory = createWebHistory()

const routes = [{
    path: '/',
    component: Home
}, {
    path: '/article/:id',
    component: Article
}]

const router = createRouter({
    routes: routes,
    history: routerHistory
})

export default router