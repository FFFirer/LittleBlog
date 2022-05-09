<template>
    <n-spin :show="editLoading">
        <div style="max-width: calc(100% - 5px)">
            <n-form
                ref="formRef"
                label-placement="left"
                label-align="right"
                label-width="60"
            >
                <n-form-item label="标题">
                    <n-input
                        v-model:value="article.title"
                        placeholder="请输入标题"
                    ></n-input>
                </n-form-item>
                <n-form-item label="作者">
                    <n-input
                        v-model:value="article.author"
                        placeholder="请输入作者"
                    ></n-input>
                </n-form-item>
                <n-form-item label="摘要">
                    <n-input
                        v-model:value="article.abstract"
                        placeholder="请输入摘要"
                    ></n-input>
                </n-form-item>
                <n-form-item v-show="!article.id" label=" ">
                    <n-checkbox v-model:checked="article.useMarkdown">
                        使用Markdown编辑器
                    </n-checkbox>
                </n-form-item>
                <n-form-item label="正文">
                    <div v-if="article.useMarkdown" class="markdown-box">
                        <markdown-editor
                            v-model:markdown="article.markdownContent"
                            v-model:html="article.content"
                            :height="600"
                        >
                        </markdown-editor>
                    </div>
                    <div v-else class="tinymce-box">
                        <Editor
                            v-model="article.content"
                            :init="init"
                            :disabled="disabled"
                        >
                        </Editor>
                    </div>
                </n-form-item>
                <n-form-item label="分类">
                    <n-select
                        v-model:value="article.categoryName"
                        placeholder="请选择分类"
                        :options="categoryOptions"
                        :filterable="true"
                        :loading="selectCategoryLoading"
                        tag
                        clearable
                        @update:value="handleSelectionUpdate"
                        @create="handleSelectionCreate"
                        :fallback-option="handleFallbackOption"
                    >
                    </n-select>
                </n-form-item>
                <n-form-item label="发布">
                    <n-checkbox v-model:checked="article.isPublished">
                    </n-checkbox>
                </n-form-item>
                <n-form-item label="">
                    <n-button @click="backToList"> 取消 </n-button>
                    <n-button type="info" @click="save"> 保存 </n-button>
                </n-form-item>
            </n-form>
        </div>
    </n-spin>
</template>

<script lang="ts">
//引入tinymce编辑器
import Editor from "@tinymce/tinymce-vue";

//引入node_modules里的tinymce相关文件文件
import tinymce, { TinyMCE, RawEditorSettings } from "tinymce/tinymce"; //tinymce默认hidden，不引入则不显示编辑器
import "tinymce/themes/silver"; //编辑器主题，不引入则报错
import "tinymce/icons/default"; //引入编辑器图标icon，不引入则不显示对应图标

// 引入编辑器插件（基本免费插件都在这儿了）
import "tinymce/plugins/advlist"; //高级列表
import "tinymce/plugins/anchor"; //锚点
import "tinymce/plugins/autolink"; //自动链接
import "tinymce/plugins/autoresize"; //编辑器高度自适应,注：plugins里引入此插件时，Init里设置的height将失效
import "tinymce/plugins/autosave"; //自动存稿
import "tinymce/plugins/charmap"; //特殊字符
import "tinymce/plugins/code"; //编辑源码
import "tinymce/plugins/codesample"; //代码示例
import "tinymce/plugins/directionality"; //文字方向
import "tinymce/plugins/emoticons"; //表情
import "tinymce/plugins/fullpage"; //文档属性
import "tinymce/plugins/fullscreen"; //全屏
import "tinymce/plugins/help"; //帮助
import "tinymce/plugins/hr"; //水平分割线
import "tinymce/plugins/image"; //插入编辑图片
import "tinymce/plugins/importcss"; //引入css
import "tinymce/plugins/insertdatetime"; //插入日期时间
import "tinymce/plugins/link"; //超链接
import "tinymce/plugins/lists"; //列表插件
import "tinymce/plugins/media"; //插入编辑媒体
import "tinymce/plugins/nonbreaking"; //插入不间断空格
import "tinymce/plugins/pagebreak"; //插入分页符
import "tinymce/plugins/paste"; //粘贴插件
import "tinymce/plugins/preview"; //预览
import "tinymce/plugins/print"; //打印
import "tinymce/plugins/quickbars"; //快速工具栏
import "tinymce/plugins/save"; //保存
import "tinymce/plugins/searchreplace"; //查找替换
// import 'tinymce/plugins/spellchecker'  //拼写检查，暂未加入汉化，不建议使用
import "tinymce/plugins/tabfocus"; //切入切出，按tab键切出编辑器，切入页面其他输入框中
import "tinymce/plugins/table"; //表格
import "tinymce/plugins/template"; //内容模板
import "tinymce/plugins/textcolor"; //文字颜色
import "tinymce/plugins/textpattern"; //快速排版
import "tinymce/plugins/toc"; //目录生成器
import "tinymce/plugins/visualblocks"; //显示元素范围
import "tinymce/plugins/visualchars"; //显示不可见字符
import "tinymce/plugins/wordcount"; //字数统计

import { defineComponent } from "vue";
import { useRouter } from "vue-router";
import { SelectOption, useMessage } from "naive-ui";
import { ArticleDto, UploadInfo, UploadTypes } from "../../types";
import api from "../../api";
import MarkdownEditor from "../../components/MarkdownEditor.vue";

const viteAppName = import.meta.env.VITE_APP_NAME || "/";
const serverBaseUrl = import.meta.env.VITE_REMOTE_API_ADDRESS;

const TinyMCE_LANGUAGE_Zh_CN_URL = viteAppName + "tinymce/langs/zh_CN.js";
const TinyMCE_SKIN_URL = viteAppName + "tinymce/skins/ui/oxide";
const TinyMCE_CONTENT_CSS_URL =
    viteAppName + "tinymce/skins/content/default/content.css";

let TIMYMCE_IMAOE_PREPEND_URL = serverBaseUrl;

if (
    TIMYMCE_IMAOE_PREPEND_URL.indexOf("/") ===
    TIMYMCE_IMAOE_PREPEND_URL.length - 1
) {
    TIMYMCE_IMAOE_PREPEND_URL = TIMYMCE_IMAOE_PREPEND_URL.substring(
        0,
        TIMYMCE_IMAOE_PREPEND_URL.length - 1
    );
}

export default defineComponent({
    name: "ArticleEdit",
    components: {
        Editor,
        MarkdownEditor,
    },
    props: {
        id: {
            type: Number,
            default: 0,
        },
        value: {
            type: String,
            default: "",
        },
        disabled: {
            type: Boolean,
            default: false,
        },
        plugins: {
            type: [String, Array],
            default:
                "print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount textpattern autosave ",
        },
        toolbar: {
            type: [String, Array],
            default:
                "fullscreen undo redo restoredraft | cut copy paste pastetext | forecolor backcolor bold italic underline strikethrough link anchor | alignleft aligncenter alignright alignjustify outdent indent | \
              styleselect formatselect fontselect fontsizeselect | bullist numlist | blockquote subscript superscript removeformat | \
              table image media charmap hr pagebreak insertdatetime print preview | code selectall searchreplace visualblocks | indent2em lineheight formatpainter axupimgs",
        }
    },
    data() {
        return {
            init: {
                language_url: TinyMCE_LANGUAGE_Zh_CN_URL, //引入语言包文件
                language: "zh_CN", //语言类型

                skin_url: TinyMCE_SKIN_URL, //皮肤：浅色
                // skin_url: '/tinymce/skins/ui/oxide-dark',//皮肤：暗色

                plugins: this.plugins, //插件配置
                toolbar: this.toolbar, //工具栏配置，设为false则隐藏
                // menubar: 'file edit',  //菜单栏配置，设为false则隐藏，不配置则默认显示全部菜单，也可自定义配置--查看 http://tinymce.ax-z.cn/configure/editor-appearance.php --搜索“自定义菜单”

                fontsize_formats:
                    "12px 14px 16px 18px 20px 22px 24px 28px 32px 36px 48px 56px 72px", //字体大小
                font_formats:
                    "微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;", //字体样式
                lineheight_formats: "0.5 0.8 1 1.2 1.5 1.75 2 2.5 3 4 5", //行高配置，也可配置成"12px 14px 16px 20px"这种形式

                height: 400, //注：引入autoresize插件时，此属性失效
                placeholder: "在这里输入文字",
                branding: false, //tiny技术支持信息是否显示
                resize: false, //编辑器宽高是否可变，false-否,true-高可变，'both'-宽高均可，注意引号
                // statusbar: false,  //最下方的元素路径和字数统计那一栏是否显示
                elementpath: false, //元素路径是否显示

                // content_style: "img {max-width:100%;}", //直接自定义可编辑区域的css样式
                content_css: TinyMCE_CONTENT_CSS_URL, //以css文件方式自定义可编辑区域的css样式，css文件需自己创建并引入

                // images_upload_url: '/apib/api-upload/uploadimg',  //后端处理程序的url，建议直接自定义上传函数image_upload_handler，这个就可以不用了
                // images_upload_base_path: '/demo',  //相对基本路径--关于图片上传建议查看--http://tinymce.ax-z.cn/general/upload-images.php
                paste_data_images: true, //图片是否可粘贴
                // images_upload_handler: (blobInfo, success, failure) => {
                // if (blobInfo.blob().size / 1024 / 1024 > 2) {
                //   failure("上传失败，图片大小请控制在 2M 以内")
                // } else {
                //   let params = new FormData()
                //   params.append('file', blobInfo.blob())
                //   let config = {
                //     headers: {
                //       "Content-Type": "multipart/form-data"
                //     }
                //   }
                //   this.$axios.post(`${api.baseUrl}/api-upload/uploadimg`, params, config).then(res => {
                //     if (res.data.code == 200) {
                //       success(res.data.msg) //上传成功，在成功函数里填入图片路径
                //     } else {
                //       failure("上传失败")
                //     }
                //   }).catch(() => {
                //     failure("上传出错，服务器开小差了呢")
                //   })
                // }
                // },
                file_picker_callback: (callback, value, meta) => {
                    let _this = this;
                    var uploadInput = document.createElement("input");
                    uploadInput.setAttribute("type", "file");

                    if (meta.filetype === "image") {
                        uploadInput.setAttribute("accept", "image/*");

                        uploadInput.onchange = function () {
                            let file = (uploadInput.files || [])[0];

                            if (!file) {
                                alert("请选择要上传的图片！");
                            }

                            let uploadInfo: UploadInfo = {
                                fileName: file.name,
                                uploadPath: "/images",
                                index: 1,
                                total: 1,
                                group: "articles",
                                type: UploadTypes.Image,
                                data: file,
                            };
                            api.admin.file.upload(uploadInfo).then((res) => {
                                if (res.isSuccess) {
                                    if (res.data.isFinish) {
                                        callback(res.data.url);
                                    }
                                } else {
                                    console.error(res.message);
                                }
                            });
                        };
                    }

                    uploadInput.click();
                },
                image_prepend_url: TIMYMCE_IMAOE_PREPEND_URL,
            } as RawEditorSettings,
            article: {
                markdownContent: "",
            } as ArticleDto,
            editLoading: false as boolean,
            file_picker_types: "image",
            categories: [] as string[],
            categoryOptions: [] as SelectOption[],
            currentCategoryInput: "" as string,
            selectCategoryLoading: false,
        };
    },
    computed: {},
    mounted() {
        tinymce.init({});

        this.loadBase();
        if (this.id > 0) {
            this.load();
        }
    },
    methods: {
        onClick(e: Event) {
            this.$emit("onClick", e, tinymce);
        },
        clear() {
            this.article.content = "";
        },
        save() {
            this.editLoading = true;
            api.admin.articles
                .save(this.article)
                .then((res) => {
                    if (res.isSuccess) {
                        this.message.success("保存成功");
                        this.backToList();
                    } else {
                        this.message.warning(res.message);
                    }
                })
                .catch((err) => {
                    this.message.error("保存出错！");
                    console.error("save", err);
                })
                .finally(() => {
                    this.editLoading = false;
                });
        },
        load() {
            this.editLoading = true;
            api.admin.articles
                .getOne(this.id)
                .then((res) => {
                    if (res.isSuccess) {
                        this.article = res.data;
                    } else {
                        this.message.warning(res.message, {
                            closable: true,
                            duration: 5000,
                        });
                    }
                })
                .catch((err) => {
                    this.message.error("无法加载文章内容！");
                    console.error("get", err);
                })
                .finally(() => {
                    this.editLoading = false;
                });
        },
        uploadFile() {},
        loadBase() {
            // 加载类别
            api.admin.categories
                .list()
                .then((res) => {
                    if (res.isSuccess) {
                        this.categories = res.data;
                        this.categoryOptions = res.data.map((v) => {
                            return {
                                label: v,
                                value: v,
                            };
                        }) as SelectOption[];
                    }
                })
                .catch((err) => {
                    console.error(err);
                });
        },
        saveCategoryName() {
            let currentValue = this.article.categoryName;
            console.log("current value", currentValue);
        },
        filterCategory(value: string): boolean {
            return (
                this.categories.filter((c) => {
                    return c === value;
                }).length > 0
            );
        },
        handleSelectionUpdate(value: string) {
            console.log("update", value);

            this.handleFallbackOption = () => {
                return false;
            };
            this.selectCategoryLoading = true;
            if (!this.filterCategory(value)) {
                api.admin.categories
                    .save(value)
                    .then((res) => {
                        if (res.isSuccess) {
                            this.loadBase();
                        } else {
                            this.message.warning(res.message);
                        }
                    })
                    .catch((err) => console.error(err))
                    .finally(() => {
                        this.selectCategoryLoading = false;
                    });
            } else {
                this.selectCategoryLoading = false;
            }
        },
        handleSelectionCreate(value: string): SelectOption {
            return {
                label: (this.filterCategory(value) ? "" : "添加: ") + value,
                value,
            } as SelectOption;
        },
        handleFallbackOption(value: string): SelectOption | boolean {
            return {
                label: "（已删除）" + value,
                value,
            };
        },
    },
    setup() {
        const router = useRouter();
        const message = useMessage();

        const backToList = () => {
            router.push({
                name: "articleList",
            });
        };

        return {
            backToList,
            message,
        };
    },
});
</script>

<style>
.n-form-item-blank {
    overflow-x: auto;
}

.tinymce-box {
    width: 100%;
}

.markdown-box {
    width: 100%;
}
</style>
