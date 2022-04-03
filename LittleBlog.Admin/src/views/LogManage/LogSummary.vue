<template>
    <n-grid y-gap="6" x-gap="6" cols="12" item-responsive>
        <n-gi span="12 600:4">
            <n-select
                v-model:value="queryLevel"
                :options="queryLevelOptions"
                placeholder="请选择日志等级"
            >
            </n-select>
        </n-gi>
        <n-gi span="12 600:4">
            <n-date-picker
                v-model:value="queryDateTimeRange"
                type="datetimerange"
                clearable
                start-placeholder="开始时间"
                end-placeholder="结束时间"
            >
            </n-date-picker>
        </n-gi>
        <n-gi span="12 600:4">
            <n-input
                v-model:value="queryLogger"
                placeholder="输入要筛选的Logger, 支持尾部*通配符"
            ></n-input>
        </n-gi>
        <n-gi span="12 600:4">
            <n-space>
                <n-button @click="load"> 查询 </n-button>
                <n-button @click="reload"> 重置 </n-button>
            </n-space>
        </n-gi>
        <n-gi span="12">
            <n-spin :show="isLoading">
                <n-table>
                    <thead>
                        <tr>
                            <th style="width: 50px">序号</th>
                            <th style="width: 50px">操作</th>
                            <th style="width: 50px">等级</th>
                            <th>时间</th>
                            <th>Message</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(log, index) in logs" :key="index">
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                <n-button @click="showDetail(log)" text>
                                    <template #icon>
                                        <n-icon size="16">
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                xmlns:xlink="http://www.w3.org/1999/xlink"
                                                viewBox="0 0 1024 1024"
                                            >
                                                <path
                                                    d="M120 230h496c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm0 424h496c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm784 140H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zm0-424H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8z"
                                                    fill="currentColor"
                                                ></path>
                                            </svg>
                                        </n-icon>
                                    </template>
                                </n-button>
                            </td>
                            <td>
                                {{ log.logLevel }}
                            </td>
                            <td>
                                {{ log.logged }}
                            </td>
                            <td>
                                {{ log.message }}
                            </td>
                        </tr>
                    </tbody>
                </n-table>
            </n-spin>
        </n-gi>
        <n-gi span="12">
            <n-pagination
                v-model:page="page"
                :item-count="total"
                :page-size="pageSize"
                :page-sizes="pageSizes"
                :show-size-picker="true"
            >
            </n-pagination>
        </n-gi>
    </n-grid>
    <n-modal :show="showModal" :on-mask-click="hideDetail">
        <n-card closable @close="hideDetail()">
            <n-table :bordered="true">
                <tbody>
                    <tr>
                        <td>
                            <b>级别</b>
                        </td>
                        <td>
                            {{ logDetail.logLevel }}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b> 时间 </b>
                        </td>
                        <td>
                            {{ logDetail.logged }}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b> Message </b>
                        </td>
                        <td>{{ logDetail.message }}</td>
                    </tr>
                    <tr>
                        <td>
                            <b>Exception</b>
                        </td>
                        <td style="white-space: pre-line">
                            {{ logDetail.exception ?? "无" }}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b> 应用 </b>
                        </td>
                        <td>{{ logDetail.application }}</td>
                    </tr>
                    <tr>
                        <td>
                            <b>Logger</b>
                        </td>
                        <td>{{ logDetail.logger }}</td>
                    </tr>
                    <tr>
                        <td><b> Callsite </b></td>
                        <td>{{ logDetail.callSite }}</td>
                    </tr>
                </tbody>
            </n-table>
        </n-card>
    </n-modal>
</template>

<script lang="ts">
import { useMessage } from "naive-ui";
import { defineComponent, ref } from "vue";
import api from "../../api";
import { ListLogsQueryContext, LogModel } from "../../types";
import { Times } from "@vicons/fa";
export default defineComponent({
    name: "LogSummary",
    components: { Times },
    data() {
        return {
            logs: [] as Array<LogModel>,
            total: 0 as number,
            isLoading: false as boolean,
            page: ref(1),
            pageSize: ref(50),
            pageSizes: [50, 100, 200],
            queryDateTimeRange: ref(null),
            queryLevel: ref(null),
            queryLevelOptions: [
                {
                    label: "Trace",
                    value: "Trace",
                },
                {
                    label: "Debug",
                    value: "Debug",
                },
                {
                    label: "Information",
                    value: "Info",
                },
                {
                    label: "Warning",
                    value: "Warn",
                },
                {
                    label: "Error",
                    value: "Error",
                },
                {
                    label: "Critical",
                    value: "Critical",
                },
                {
                    label: "None",
                    value: "",
                },
            ],
            queryLogger: "",
            showModal: false,
            logDetail: {} as LogModel,
        };
    },
    methods: {
        reload() {
            this.page = 1;
            this.queryLogger = "";
            this.queryLevel = null;
            this.queryDateTimeRange = null;
            this.load();
        },
        load() {
            // 组装数据
            let queryContext = {} as ListLogsQueryContext;
            queryContext.page = this.page;
            queryContext.pageSize = this.pageSize;
            if (this.queryDateTimeRange) {
                let startTimeStamp = this.queryDateTimeRange![0];
                let endTimeStamp = this.queryDateTimeRange![1];

                console.log(
                    "start timestamp",
                    startTimeStamp,
                    "end timestamp",
                    endTimeStamp
                );

                if (startTimeStamp && typeof startTimeStamp == "number") {
                    queryContext.startTime = new Date(
                        startTimeStamp
                    ).toISOString();
                }

                if (endTimeStamp && typeof endTimeStamp == "number") {
                    queryContext.endTime = new Date(endTimeStamp).toISOString();
                }

                console.log(
                    "start utc",
                    queryContext.startTime,
                    "end utc",
                    queryContext.endTime
                );
            }
            queryContext.logLevel = this.queryLevel;
            queryContext.logger = this.queryLogger;

            api.admin.logs
                .list(queryContext)
                .then((resp) => {
                    if (resp.isSuccess) {
                        this.logs = resp.data.rows.map(function (logEntity) {
                            logEntity.logged = logEntity.logged.replaceAll(
                                "T",
                                " "
                            );

                            return logEntity;
                        });
                    } else {
                        this.message.warning(resp.message);
                    }
                })
                .catch((err) => {
                    if (typeof err == "string") {
                        this.message.error(err);
                        return;
                    } else {
                        this.message.error(err.message);
                        return;
                    }
                })
                .finally(() => {
                    this.isLoading = false;
                });
        },
        showDetail(log: LogModel) {
            this.logDetail = log;
            // this.logDetail.exception = this.logDetail.exception
            //     .replaceAll("\r\n", "<br/>")
            //     .replaceAll("\n", "<br/>");
            this.showModal = true;
        },
        hideDetail() {
            this.showModal = false;
            this.logDetail = {} as LogModel;
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
