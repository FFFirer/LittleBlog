<template>
    <div>
        <ArticleSummary v-for="article in articles" :article="article" :url="articleUrl"></ArticleSummary>
        <a-pagination :default-current="1" :current="articlesListQuery.page"
            :default-pagesize="articlesListQuery.pageSize" :total="articlesListQuery.total" @change="onChanged">
        </a-pagination>
    </div>
</template>

<script>
    import ArticleSummary from '../components/ArticleSummary.vue'
    import apis from '../api/index.js'

    export default {
        name: 'Home',
        components: {
            'ArticleSummary': ArticleSummary
        },
        data() {
            return {
                articles: [],
                articlesListQuery: {
                    page: 1,
                    pageSize: 20,
                    total: 0,
                    keyword: "",
                },
                articleUrl: "/article",
            }
        },
        methods: {
            query() {
                let data = this.articlesListQuery
                apis.articleApi.getList(data)
                    .then((resp) => {
                        if (resp.data.isSuccess) {
                            console.log(resp.data);
                            this.articles = resp.data.data;
                        }
                    }).catch((err) => {
                        console.log(err);
                    });
            },
            onChanged(e) {
                console.log(e);
                this.query();
            }
        },
        mounted() {
            this.query();
        }
    }
</script>