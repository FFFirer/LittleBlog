<template>
    <div>
        <ArticleSummary v-for="article in articles"></ArticleSummary>
    </div>
</template>

<script>
    import ArticleSummary from '../components/ArticleSummary.vue'
    export default {
        name: 'Home',
        components: {
            'ArticleSummary': ArticleSummary
        },
        data() {
            return {
                articles: [{
                        id: 1
                    },
                    {
                        id: 2
                    },
                    {
                        id: 3
                    }, {
                        id: 1
                    },
                    {
                        id: 2
                    },
                    {
                        id: 3
                    }
                ],
                a: null
            }
        },
        methods: {

        }
    }
</script>