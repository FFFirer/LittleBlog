import {
    createApp
} from 'vue'
import App from './App.vue'
import './index.css'
import naive from 'naive-ui'


import router from './static/router'

createApp(App)
    .use(naive)
    .use(router)
    .mount('#app')