<template>
    <div
        v-for="(item, index) in sourceData"
        :key="index"
        class="checkbox-container"
    >
        <input type="checkbox" checked="item.isChecked" @change="onChange" />
        {{ item.label }}
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'CheckboxGroup',
    props: {
        selected: [],
        source: [],
        value: '',
        label: '',
    },
    methods: {
        onChange(e) {
            console.log('change: ' + e.target.checked)
            console.log(this.selected)
            let selectedItem = this.sourceData.filter(
                (current, index, array) => {
                    current.isChecked == true
                }
            )

            this.selected = []
            selectedItem.forEach((v, i, a) => {
                this.selected.push(v[value])
            })
        },
    },
    setup(props) {
        console.log('checkbox group up')
        let source = props.source // 数据源
        let selected = props.selected // 已选择/默认
        let valueProperty = props.value // 值属性
        let labelProperty = props.label // 显示属性
        let sourceData = []
        source.forEach((d) => {
            // 默认传入得source是Array<object>
            let dValue = d[valueProperty]
            let dLabel = d[labelProperty]

            let selectedValue = selected.filter((current, index, arr) => {
                current == dValue
            })

            // console.log("selected: " + selectedValue)

            if (selectedValue.length <= 0) {
                sourceData.push({
                    value: dValue,
                    label: dLabel,
                    isChecked: false,
                })
            } else {
                sourceData.push({
                    value: dValue,
                    label: dLabel,
                    isChecked: true,
                })
            }
        })

        console.log(sourceData)

        return {
            sourceData,
        }
    },
})
</script>

<style>
.checkbox-container {
    margin: 5px;
    display: inline-block;
}
</style>
