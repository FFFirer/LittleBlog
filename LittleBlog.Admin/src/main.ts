import { createApp } from "vue";
import App from "./App.vue";

import naive from "naive-ui";
import router from "./route/index";

import hljs from "highlight.js";
import VMdEditor from "@kangc/v-md-editor";

const app = createApp(App);
app.use(naive);
app.use(router);
app.mount("#app");
