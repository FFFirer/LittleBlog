<template>
    <n-message-provider
        ><n-dialog-provider>
            <router-view> </router-view>
        </n-dialog-provider>
    </n-message-provider>
</template>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
    name: "App",
    components: {},
});
</script>

<style>
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    /* text-align: center; */
    color: #2c3e50;
}
</style>
