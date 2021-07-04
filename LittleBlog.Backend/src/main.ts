import { createApp } from 'vue'
import App from './App.vue'
// import Antd from 'ant-design-vue'
// import 'ant-design-vue/dist/antd.css'
import router from './router/index'
import naive from 'naive-ui'

createApp(App)
    .use(router)
    .use(naive)
    // .use(Antd)
    .mount('#app')
