<template>
    <n-grid cols="1" y-gap="6">
        <n-gi>
            <n-space>
                <span class="form-title"> 友情链接设置 </span>
                <n-button type="primary" dashed @click="addGroup"
                    >添加分组</n-button
                >
                <n-button type="info" @click="save">保存</n-button>
                <n-button @click="load">取消</n-button>
            </n-space>
        </n-gi>
        <n-gi
            class="link-group"
            v-for="(linkGroup, groupIndex) in linkGroups"
            :key="groupIndex"
        >
            <n-grid cols="1" y-gap="3">
                <n-input-group class="link-group-header">
                    <n-input
                        placeholder="请输入分组名称"
                        v-model:value="linkGroup.groupName"
                    >
                        <template #prefix> 分组： </template>
                    </n-input>
                </n-input-group>
                <n-space>
                    <n-button type="error" @click="removeGroup(groupIndex)">
                        删除分组
                    </n-button>
                    <n-button
                        type="primary"
                        dashed
                        @click="addLinkToGroup(groupIndex)"
                    >
                        添加链接
                    </n-button>
                </n-space>
                <n-grid cols="1 800:2" x-gap="6" y-gap="3">
                    <n-gi
                        v-for="(linkItem, linkIndex) in linkGroup.links"
                        :key="linkIndex"
                    >
                        <n-input-group>
                            <n-input
                                :style="{ width: '35%' }"
                                placeholder="请输入链接名称"
                                v-model:value="linkItem.description"
                            >
                                <template #prefix>链接：</template>
                            </n-input>
                            <n-input
                                placeholder="请输入链接地址"
                                v-model:value="linkItem.link"
                            >
                            </n-input>
                            <n-button
                                type="error"
                                @click="
                                    removeLinkFromGroup(groupIndex, linkIndex)
                                "
                            >
                                删除链接
                            </n-button>
                        </n-input-group>
                    </n-gi>
                </n-grid>
            </n-grid>
        </n-gi>
    </n-grid>
</template>

<script lang="ts">
import { useMessage } from "naive-ui";
import { defineComponent } from "vue";
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
        };
    },
    methods: {
        load() {
            let links = api.admin.syscfg.friendshipLinks.list().then((resp) => {
                if (resp.isSuccess) {
                    console.log("resp data", resp.data);
                    let groups = resp.data.reduce(function (
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

                    console.log("groups", groups);

                    this.linkGroups = [];

                    Object.keys(groups).map((g) => {
                        this.linkGroups.push({
                            groupName: g,
                            links: groups[g],
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

        return {
            message,
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
