import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import { loadEnv } from "vite";
import prismjsPlugin from "vite-plugin-prismjs";

const prismjsPluginOption = {
    languages: ["javascript", "css", "markup", "csharp"],
    plugins: ["line-numbers"],
    theme: "twilight",
    css: true,
};

export default ({ mode }) => {
    return defineConfig({
        plugins: [vue(), prismjsPlugin(prismjsPluginOption)],
        base: loadEnv(mode, process.cwd()).VITE_APP_NAME,
        server: {
            https: true,
            port: 30000,
            host: "127.0.0.1",
        },
    });
};
