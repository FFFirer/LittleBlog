<template>
    <div class="article-page">
        <a-page-header :title="article.title" @back="() => toHome()">

        </a-page-header>
        <div v-html="article.content" class="article-content">

        </div>
    </div>
</template>

<script>
    import apis from '../api/index.js'
    import {
        useRouter
    } from 'vue-router'
    export default {
        name: 'Article',
        props: ['id'],
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
        methods: {
            load() {
                console.log(this.id);
                apis.articleApi.getArticle(this.id).then((resp) => {
                    if (resp.data.isSuccess) {
                        this.article = resp.data.data;
                    }
                }).catch((err) => {
                    console.log(err);
                })
            }
        },
        mounted() {
            this.load();
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
        padding: 5px;
        border-top: 1px solid #cff;
    }

    .article-page {
        margin: 10px;
        background-color: white;
        box-shadow: 1px 1px 1px #ccc;
    }
</style>