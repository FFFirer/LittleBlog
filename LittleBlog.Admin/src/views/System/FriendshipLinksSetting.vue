<template>
    <n-grid cols="1" y-gap="6">
        <n-gi>
            <n-space>
                <span class="form-title"> 友情链接设置 </span>

                <n-button type="info" @click="save">保存</n-button>
                <n-button @click="load">取消</n-button>
            </n-space>
        </n-gi>
    </n-grid>
    <n-dynamic-input v-model:value="linkGroups" :on-create="onCreateGroup">
        <template #="group">
            <div style="width: 100%">
                <n-input-group>
                    <n-input-group-label :style="{ width: '120px' }"
                        >分组名称</n-input-group-label
                    >
                    <n-input
                        v-model:value="group.value.groupName"
                        placeholder="请输入分组名称"
                    >
                    </n-input>
                </n-input-group>

                <n-dynamic-input
                    v-model:value="group.value.links"
                    :on-create="onCreateLink"
                    item-style="margin-bottom:0"
                >
                    <template #="link">
                        <div
                            style="
                                display: flex;
                                align-items: center;
                                width: 100%;
                            "
                        >
                            <n-input-group>
                                <n-input
                                    style="width: 50%"
                                    v-model:value="link.value.link"
                                    placeholder="请输入链接地址"
                                >
                                </n-input>
                                <n-input
                                    style="width: 50%"
                                    v-model:value="link.value.description"
                                    placeholder="请输入链接描述"
                                >
                                </n-input>
                            </n-input-group>
                        </div>
                    </template>
                </n-dynamic-input>
            </div>
        </template>
    </n-dynamic-input>
</template>

<script lang="ts">
import { useMessage } from "naive-ui";
import { defineComponent, ref } from "vue";
import api from "../../api";
import { FriendshipLink } from "../../types";

interface linksGroup {
    groupName: string;
    links: FriendshipLink[];
}

export default defineComponent({
    name: "FriendshipLinksSetting",
    data() {
        return {
            linkGroups: [] as linksGroup[],
            linkGroupsDict: {} as { [key: string]: FriendshipLink[] },
            itemStyle: {
                "margin-bottom": 0,
            },
        };
    },
    computed: {
        groups(): string[] {
            return Object.keys(this.linkGroupsDict);
        },
        groupsDict(): { [key: string]: FriendshipLink[] } {
            return this.linkGroups.reduce((prev, curr) => {
                prev[curr.groupName] = curr.links;
                return prev;
            }, {} as { [key: string]: FriendshipLink[] });
        },
    },
    methods: {
        load() {
            let links = api.admin.syscfg.friendshipLinks.list().then((resp) => {
                if (resp.isSuccess) {
                    console.log("resp data", resp.data);
                    this.linkGroupsDict = resp.data.reduce(function (
                        prev: any,
                        curr: FriendshipLink
                    ) {
                        // console.log("curr", curr);
                        let arr = prev[curr.group] || [];
                        arr.push(curr);
                        prev[curr.group] = arr;
                        return prev;
                    },
                    {});

                    this.linkGroups = [];
                    Object.keys(this.linkGroupsDict).map((g) => {
                        this.linkGroups.push({
                            groupName: g,
                            links: this.linkGroupsDict[g],
                        });
                    });
                } else {
                    this.message.warning(resp.message);
                }
            });
        },
        save() {
            let toSave = this.linkGroups.reduce(
                (prev: FriendshipLink[], curr: linksGroup) => {
                    let links = curr.links.map((link) => {
                        link.group = curr.groupName;
                        return link;
                    });

                    prev = [...prev, ...links];
                    return prev;
                },
                []
            );

            api.admin.syscfg.friendshipLinks.save(toSave).then((resp) => {
                if (resp.isSuccess) {
                    this.message.success("保存成功！");
                } else {
                    this.message.warning(resp.message);
                }
            });
        },
        addGroup() {
            this.linkGroups.push({
                groupName: "",
                links: [],
            });
        },
        removeGroup(groupIndex: number) {
            this.linkGroups.splice(groupIndex, 1);
        },
        addLinkToGroup(groupIndex: number) {
            let group = this.linkGroups[groupIndex];

            if (!group) {
                group = {
                    groupName: "",
                    links: [],
                };

                this.linkGroups.push(group);
            }

            group.links.push({
                link: "",
                description: "",
                group: "",
            });
        },
        removeLinkFromGroup(groupIndex: number, linkIndex: number) {
            console.log("delete", groupIndex, linkIndex);

            let group = this.linkGroups[groupIndex];

            if (group) {
                console.log("find group", group);
                let link = this.linkGroups[groupIndex].links[linkIndex];

                if (link) {
                    console.log("find link", link);
                    this.linkGroups[groupIndex].links.splice(linkIndex, 1);
                }
            }
        },
    },
    mounted() {
        this.load();
    },
    setup() {
        const message = useMessage();

        const onCreateGroup = () => {
            return {
                groupName: "",
                links: [
                    {
                        group: "",
                        link: "",
                        description: "",
                    },
                ],
            } as linksGroup;
        };

        const onCreateLink = () => {
            return {
                group: "",
                link: "",
                description: "",
            } as FriendshipLink;
        };

        return {
            message,
            onCreateGroup,
            onCreateLink,
        };
    },
});
</script>

<style>
.form-title {
    font-size: 24px;
    font-weight: 800;
}

.link-group {
    border: 2px solid lightgrey;
    border-radius: 4px;
}
</style>
