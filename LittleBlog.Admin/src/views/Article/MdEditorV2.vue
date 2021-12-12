<template>
    <markdown-editor
        :MarkdownContent="content"
        height="600px"
    ></markdown-editor>
</template>

<script lang="ts">
import { defineComponent, onMounted, Ref, ref, render, watch } from "vue";
import { ArticleDto } from "../../types";
import MarkdownEditor from "../../components/MarkdownEditor.vue";

export default defineComponent({
    name: "MdEditorV2",
    data() {
        return {
            content: "",
        };
    },
    components: {
        MarkdownEditor,
    },
    methods: {},
    setup() {},
});
</script>

<style>
.preview-bakcground {
    background-color: white;
}
</style>
