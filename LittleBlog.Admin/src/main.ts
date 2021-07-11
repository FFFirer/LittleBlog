import { createApp } from "vue";
import App from "./App.vue";

import naive from "naive-ui";
import router from "./route/index";

createApp(App).use(naive).use(router).mount("#app");
