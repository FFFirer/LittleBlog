<template>
    <div class="shadow-preview" id="shadowContainer" ref="shadowRootRef"></div>
</template>

<script lang="ts">
import { defineComponent, PropType, onMounted, ref, watch } from "vue";

const INNER_STYLE_TAG = "inner-style";
const OUTER_STYLE_TAG = "outer-style";

/**html preview props defination */
const HtmlPreviewProps = {
    html: {
        type: String,
        default: "",
    },
    innerStyles: {
        type: Array as PropType<Array<{ id: string; style: string }>>,
        default: [],
    },
    outerStyles: {
        type: Array as PropType<Array<{ id: string; url: string }>>,
        default: [],
    },
};

export default defineComponent({
    name: "HtmlPreview",
    watch: {
        innerStyles: {
            handler(v, o) {
                this.reloadInnerStyles();
            },
            deep: true,
        },
        outerStyles: {
            handler(v, o) {
                this.reloadOuterStyles();
            },
            deep: true,
        },
    },
    props: HtmlPreviewProps,
    methods: {
        reloadInnerStyles() {
            let allStyles = this.getInnerStyles();
            let allStyleEls = allStyles
                .filter((a) => {
                    let linkId = a.dataset["id"];
                    return linkId != undefined;
                })
                .map((s) => {
                    let styleId = s.dataset["id"];
                    return {
                        id: styleId || "",
                        el: s,
                        hasChecked: false,
                    };
                });

            let allStyleElsDict = allStyleEls.reduce((prev, curr) => {
                prev[curr.id] = curr;
                return prev;
            }, {} as { [key: string]: { id: string; el: HTMLStyleElement; hasChecked: boolean } });

            this.innerStyles.forEach((inner) => {
                if (allStyleElsDict[inner.id] != undefined) {
                    allStyleElsDict[inner.id].el.innerText = inner.style;
                    allStyleElsDict[inner.id].hasChecked = true;
                } else {
                    this.insertInnerStyle(inner.id, inner.style);
                }
            });

            allStyleEls.forEach((a) => {
                if (!a.hasChecked) {
                    a.el.remove();
                }
            });
        },
        reloadOuterStyles() {
            let allLinks = this.getOuterStyleLinks();

            let allLinkInfos = allLinks
                .filter((link) => {
                    let linkId = link.dataset["id"];
                    return linkId != undefined;
                })
                .map((a) => {
                    let linkId = a.dataset["id"];
                    return {
                        id: linkId || "",
                        el: a,
                        hasChecked: false,
                    };
                });

            let allLinkInfoDict = allLinkInfos.reduce((prev, curr) => {
                prev[curr.id] = curr;
                return prev;
            }, {} as { [key: string]: { id: string; el: HTMLLinkElement; hasChecked: boolean } });

            // 更新标签
            this.outerStyles.forEach((outer) => {
                if (allLinkInfoDict[outer.id] != undefined) {
                    allLinkInfoDict[outer.id].el.href = outer.url;
                    allLinkInfoDict[outer.id].hasChecked = true;
                } else {
                    this.insertOuterStyle(outer.id, outer.url);
                }
            });

            // 移除标签
            allLinkInfos
                .filter((a) => !a.hasChecked)
                .forEach((a) => {
                    a.el.remove();
                });
        },
        init() {},
        getInnerStyles(): Array<HTMLStyleElement> {
            if (this.shadowRoot == undefined) {
                return [];
            }

            let styles: Array<HTMLStyleElement> = [];
            let results = this.shadowRoot?.querySelectorAll(
                `style.${INNER_STYLE_TAG}`
            );

            results.forEach((a) => {
                styles.push(a as HTMLStyleElement);
            });

            return styles;
        },
        getOuterStyleLinks(): Array<HTMLLinkElement> {
            if (this.shadowRoot == undefined) {
                return [];
            }

            let links: Array<HTMLLinkElement> = [];
            this.shadowRoot
                ?.querySelectorAll(`link.${OUTER_STYLE_TAG}`)
                .forEach((a) => {
                    links.push(a as HTMLLinkElement);
                });

            return links;
        },
        insertScript() {
            if (this.shadowRoot == undefined) {
                return;
            }
        },
        insertOuterStyle(id: string, url: string) {
            if (this.shadowRoot == undefined) {
                return;
            }

            let link = document.createElement("link");
            link.dataset["id"] = id;
            link.href = url;
            link.classList.add(OUTER_STYLE_TAG);
            link.rel = "stylesheet";

            this.shadowRoot.appendChild(link);
        },
        insertInnerStyle(id: string, style: string) {
            if (this.shadowRoot == undefined) {
                return;
            }

            let styleEl = document.createElement("style");
            styleEl.classList.add(INNER_STYLE_TAG);
            styleEl.dataset["id"] = id;
            styleEl.innerText = style;

            this.shadowRoot.appendChild(styleEl);
        },
    },
    setup(props) {
        const shadowRootRef = ref<HTMLDivElement>();
        let shadowRoot = ref<ShadowRoot>();

        const getHtmlContainer = (): HTMLDivElement | undefined => {
            if (shadowRoot == undefined) {
                return;
            }

            let container =
                shadowRoot.value?.querySelector("div#html-container");

            if (container != undefined) {
                return container as HTMLDivElement;
            }

            container = document.createElement("div");
            container.id = "html-container";

            shadowRoot.value?.appendChild(container);

            return container as HTMLDivElement;
        };

        const reloadHtml = () => {
            if (shadowRoot == undefined) {
                return;
            }

            let htmlContaienr = getHtmlContainer();

            if (htmlContaienr != undefined) {
                htmlContaienr.innerHTML = props.html;
            }
        };

        watch(
            () => props.html,
            (v, o) => {
                reloadHtml();
            }
        );

        onMounted(() => {
            shadowRoot.value = shadowRootRef.value?.attachShadow({
                mode: "open",
            });

            reloadHtml();
        });

        return {
            shadowRootRef,
            shadowRoot,
            getHtmlContainer,
            reloadHtml,
        };
    },
});
</script>

<style v-scoped>
.shadow-preview {
    width: 100%;
    /* background-color: antiquewhite; */
    display: block;

    /* 解决h标签在第一个时的垂直外边距合并问题 */
    border: 1px solid rgb(98, 98, 98);
    /* padding: 1px; */
    height: 100%;
}
</style>
