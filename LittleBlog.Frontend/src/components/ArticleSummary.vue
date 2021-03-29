<template>
    <router-link to="/article/1">
        <div class="article-summary">
            <h1>这是文章标题</h1>
            <p>这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要这是文章摘要</p>
            <div class="summary">
                <span>
                    2020-01-01
                </span>
                <span>
                    赞: 0
                </span>
                <span>
                    踩: 1
                </span>
                <span>
                    看: 1
                </span>
            </div>
        </div>
    </router-link>
</template>

<script>
    import {
        useRouter
    } from 'vue-router'
    export default {
        name: 'ArticleSummary',
        props: ["article", "url"],
        methods: {

        },
        computed: {
            articleRouterLink: () => {
                return {
                    name: this.url,
                    params: {
                        id: this.article.id
                    }
                }
            }
        }
    }
</script>

<style>
    .article-summary {
        margin: 10px;
        text-align: left;
        box-shadow: 0px 1px 2px #888888;
        background-color: aliceblue;
        padding: 5px;
        cursor: pointer;
        color: black;
    }

    .article-summary:hover {
        box-shadow: 0px 1px 10px #888888;
    }

    .article-summary h1 {
        font-size: 24px;
    }

    .article-summary .summary {
        font-size: 10px;
        color: cadetblue;
    }
</style>