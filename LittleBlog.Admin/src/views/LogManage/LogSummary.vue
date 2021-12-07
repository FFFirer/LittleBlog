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
                            <th>序号</th>
                            <th>等级</th>
                            <th>Logger</th>
                            <th>Message</th>
                            <th>Exception</th>
                            <th>时间</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(log, index) in logs" :key="index">
                            <td>
                                {{ index + 1 }}
                            </td>
                            <td>
                                {{ log.logLevel }}
                            </td>
                            <td>
                                {{ log.logger }}
                            </td>
                            <td>
                                {{ log.message }}
                            </td>
                            <td>
                                {{ log.exception }}
                            </td>
                            <td>
                                {{ log.logged }}
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
</template>

<script lang="ts">
import { useMessage } from "naive-ui";
import { defineComponent, ref } from "vue";
import api from "../../api";
import { ListLogsQueryContext, LogModel } from "../../types";
export default defineComponent({
    name: "LogSummary",
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
    },
    setup() {
        const message = useMessage();

        return {
            message,
        };
    },
});
</script>
