// VueRouter
import {
    createRouter,
    createWebHistory
}
from 'vue-router'

// pages
import Articles from '../pages/admin/Articles.vue'

const routerHistory = createWebHistory()

const routes = [{
    path: '/admin',
    component: Articles
}]

const router = createRouter({
    routes: routes,
    history: routerHistory
})

export default router