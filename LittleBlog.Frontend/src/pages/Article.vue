<template>
    <a-page-header :title="article.title" @back="() => toHome()">

    </a-page-header>
    <div v-html="article.content" class="article-content">

    </div>
</template>

<script>
    import {
        useRouter
    } from 'vue-router'
    export default {
        name: 'Article',
        data() {
            return {
                title: "article title",
                subTitle: "sub title",
                article: {
                    title: "文章标题",
                    content: "<h1>文章内容</h1><p>文章内容</p>"
                }
            }
        },
        setup() {
            const router = useRouter()
            const toHome = () => {
                router.push({
                    path: '/'
                })
            };

            return {
                toHome
            }
        }
    }
</script>

<style>
    .article-content {
        text-align: left;
    }
</style>