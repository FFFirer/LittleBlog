<template>
    <div>
        <n-form
            ref="formRef"
            label-placement="left"
            label-align="right"
            :label-width="60"
        >
            <n-form-item label="标题">
                <n-input v-model:value="article.title" placeholder="请输入标题">
                </n-input>
            </n-form-item>
            <n-form-item label="作者">
                <n-input
                    v-model:value="article.author"
                    placeholder="请输入作者"
                >
                </n-input>
            </n-form-item>
            <n-form-item label="摘要">
                <n-input
                    v-model:value="article.abstract"
                    placeholder="请输入摘要"
                >
                </n-input>
            </n-form-item>
            <n-form-item label="正文">
                <div class="tinymce-box">
                    <Editor
                        v-model="article.content"
                        :init="init"
                        :disabled="disabled"
                    />
                </div>
            </n-form-item>
            <n-form-item label="分类">
                <n-select
                    v-model:value="article.categoryId"
                    :options="categorySelectListOptions"
                    placeholder="选择分类"
                >
                </n-select>
            </n-form-item>
            <n-form-item label="发布">
                <n-checkbox v-model:checked="article.isPublished"> </n-checkbox>
            </n-form-item>
            <n-form-item label=" ">
                <n-space>
                    <n-button @click="backToList"> 取消 </n-button>
                    <n-button type="primary" @click="save"> 保存 </n-button>
                </n-space>
            </n-form-item>
        </n-form>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { useRouter } from 'vue-router'

import Apis from '../../api/index.ts'
import CheckboxGroup from '../../components/CheckboxGroup.vue'

import Editor from '@tinymce/tinymce-vue'

import tinymce from 'tinymce/tinymce' //tinymce默认hidden，不引入则不显示编辑器
import 'tinymce/themes/silver' //编辑器主题，不引入则报错
import 'tinymce/icons/default' //引入编辑器图标icon，不引入则不显示对应图标

// 引入编辑器插件（基本免费插件都在这儿了）
import 'tinymce/plugins/advlist' //高级列表
import 'tinymce/plugins/anchor' //锚点
import 'tinymce/plugins/autolink' //自动链接
import 'tinymce/plugins/autoresize' //编辑器高度自适应,注：plugins里引入此插件时，Init里设置的height将失效
import 'tinymce/plugins/autosave' //自动存稿
import 'tinymce/plugins/charmap' //特殊字符
import 'tinymce/plugins/code' //编辑源码
import 'tinymce/plugins/codesample' //代码示例
import 'tinymce/plugins/directionality' //文字方向
import 'tinymce/plugins/emoticons' //表情
import 'tinymce/plugins/fullpage' //文档属性
import 'tinymce/plugins/fullscreen' //全屏
import 'tinymce/plugins/help' //帮助
import 'tinymce/plugins/hr' //水平分割线
import 'tinymce/plugins/image' //插入编辑图片
import 'tinymce/plugins/importcss' //引入css
import 'tinymce/plugins/insertdatetime' //插入日期时间
import 'tinymce/plugins/link' //超链接
import 'tinymce/plugins/lists' //列表插件
import 'tinymce/plugins/media' //插入编辑媒体
import 'tinymce/plugins/nonbreaking' //插入不间断空格
import 'tinymce/plugins/pagebreak' //插入分页符
import 'tinymce/plugins/paste' //粘贴插件
import 'tinymce/plugins/preview' //预览
import 'tinymce/plugins/print' //打印
import 'tinymce/plugins/quickbars' //快速工具栏
import 'tinymce/plugins/save' //保存
import 'tinymce/plugins/searchreplace' //查找替换
// import 'tinymce/plugins/spellchecker'  //拼写检查，暂未加入汉化，不建议使用
import 'tinymce/plugins/tabfocus' //切入切出，按tab键切出编辑器，切入页面其他输入框中
import 'tinymce/plugins/table' //表格
import 'tinymce/plugins/template' //内容模板
import 'tinymce/plugins/textcolor' //文字颜色
import 'tinymce/plugins/textpattern' //快速排版
import 'tinymce/plugins/toc' //目录生成器
import 'tinymce/plugins/visualblocks' //显示元素范围
import 'tinymce/plugins/visualchars' //显示不可见字符
import 'tinymce/plugins/wordcount' //字数统计

import { useMessage } from 'naive-ui'

const Categories = [
    {
        id: 0,
        name: '测试',
    },
    {
        id: 1,
        name: '测试1',
    },
]

const Tags = [
    {
        id: 0,
        name: 'Tag 1',
    },
    {
        id: 1,
        name: 'Tag 2',
    },
]

export default defineComponent({
    name: 'ArticleEdit',
    components: {
        Editor,
        CheckboxGroup,
    },
    props: {
        id: {
            type: Number,
            default: 0,
        },
        value: {
            type: String,
            default: '',
        },
        disabled: {
            type: Boolean,
            default: false,
        },
        plugins: {
            type: [String, Array],
            default:
                'print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount textpattern autosave ',
        },
        toolbar: {
            type: [String, Array],
            default:
                'fullscreen undo redo restoredraft | cut copy paste pastetext | forecolor backcolor bold italic underline strikethrough link anchor | alignleft aligncenter alignright alignjustify outdent indent | \
              styleselect formatselect fontselect fontsizeselect | bullist numlist | blockquote subscript superscript removeformat | \
              table image media charmap hr pagebreak insertdatetime print preview | code selectall searchreplace visualblocks | indent2em lineheight formatpainter axupimgs',
        },
    },
    data() {
        return {
            article: {
                title: '',
                content: '',
                author: '',
                isPublished: false,
                categoryId: null,
                tags: [],
            },
            tags: Tags,
            categories: Categories,
            init: {
                language_url: '/tinymce/langs/zh_CN.js', //引入语言包文件
                language: 'zh_CN', //语言类型

                skin_url: '/tinymce/skins/ui/oxide', //皮肤：浅色
                // skin_url: '/tinymce/skins/ui/oxide-dark',//皮肤：暗色

                plugins: this.plugins, //插件配置
                toolbar: this.toolbar, //工具栏配置，设为false则隐藏
                // menubar: 'file edit',  //菜单栏配置，设为false则隐藏，不配置则默认显示全部菜单，也可自定义配置--查看 http://tinymce.ax-z.cn/configure/editor-appearance.php --搜索“自定义菜单”

                fontsize_formats:
                    '12px 14px 16px 18px 20px 22px 24px 28px 32px 36px 48px 56px 72px', //字体大小
                font_formats:
                    '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;', //字体样式
                lineheight_formats: '0.5 0.8 1 1.2 1.5 1.75 2 2.5 3 4 5', //行高配置，也可配置成"12px 14px 16px 20px"这种形式

                height: 400, //注：引入autoresize插件时，此属性失效
                placeholder: '在这里输入文字',
                branding: false, //tiny技术支持信息是否显示
                resize: false, //编辑器宽高是否可变，false-否,true-高可变，'both'-宽高均可，注意引号
                // statusbar: false,  //最下方的元素路径和字数统计那一栏是否显示
                elementpath: false, //元素路径是否显示

                // content_style: "img {max-width:100%;}", //直接自定义可编辑区域的css样式
                content_css: '/tinymce/skins/content/default/content.css', //以css文件方式自定义可编辑区域的css样式，css文件需自己创建并引入

                // images_upload_url: '/apib/api-upload/uploadimg',  //后端处理程序的url，建议直接自定义上传函数image_upload_handler，这个就可以不用了
                // images_upload_base_path: '/demo',  //相对基本路径--关于图片上传建议查看--http://tinymce.ax-z.cn/general/upload-images.php
                paste_data_images: true, //图片是否可粘贴
            },
        }
    },
    computed: {
        categorySelectListOptions() {
            let options = []
            this.categories.forEach(function (cate, index) {
                options.push({
                    label: cate.name,
                    value: cate.id,
                })
            })

            return options
        },
    },
    methods: {
        save() {
            let article = {
                ...this.article,
            }
            article.id = this.id
            console.log(article)

            Apis.ArticlesManageApi.Save(article)
                .then((resp) => {
                    if (resp.data.isSuccess) {
                        this.message.success('保存成功')
                        this.backToList()
                    } else {
                        // alert(resp.data.message)
                        this.message.warning(resp.data.message, {
                            closable: true,
                            duration: 5000,
                        })
                    }
                })
                .catch((err) => {
                    console.log(err)
                    // alert("保存异常")
                    this.message.error(err, {
                        closable: true,
                        duration: 5000,
                    })
                })
        },
        loadArticle() {
            Apis.ArticlesManageApi.GetDetail(this.id)
                .then((resp) => {
                    if (resp.data.isSuccess) {
                        console.log('success')
                        this.article = resp.data.data
                    } else {
                        // console.log(resp.data.message)
                        this.message.warning(resp.data.message, {
                            closable: true,
                            duration: 5000,
                        })
                    }
                })
                .catch((err) => {
                    console.log(err)
                    // alert("加载异常")
                    this.message.error('加载异常', {
                        closable: true,
                        duration: 5000,
                    })
                })
        },
    },
    mounted() {
        tinymce.init({})
        // 加载文章
        if (this.id > 0) {
            console.log('edit')
            this.loadArticle()
        } else {
            console.log('new')
        }
    },
    setup(props) {
        const router = useRouter()
        const message = useMessage()

        const backToList = (id) => {
            router.push({
                name: 'articleList', // 命名路由
            })
        }

        return {
            backToList,
            message,
        }
    },
})
</script>

<style>
.tinymce-box {
    width: 100%;
}
</style>
