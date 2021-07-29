import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { loadEnv } from 'vite'
import { def } from '@vue/shared'

export default ({ mode }) => {
    return defineConfig({
        plugins: [vue()],
        base: loadEnv(mode, process.cwd()).VITE_APP_NAME
    })
}
