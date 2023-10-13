<template>
  <div class="t-dis1">
    <VolHeader
      icon="el-icon-warning-outline"
      text="表單屬性(顯示純文本)"
      style="margin-bottom: 20px"
    >
   <template #content>
         <div style="color: #909090;">設置type='label'以純文本方式顯示(只讀表單默認對下拉框字典轉換,不需要手動處理下拉框數據源)</div>
   </template>
    </VolHeader>
    <VolForm
      ref="myform1"
      :label-width="75"
      :formFields="formFields"
      :formRules="formRules2"
    ></VolForm>
  </div>
  <el-alert style="margin: 0 20px;width: auto;" type="success" :closable="false">
    <p>設置type='label'以純文本方式顯示,設置readonly=true以組件只讀方式顯示。具體使用見：框架文檔->組件api->VolForm。<a
          href="http://v2.volcore.xyz/document/api"
          target="_blank"
          >點擊查看文檔
        </a></p>
  </el-alert>
  <div class="t-dis2">
    <VolHeader
      icon="el-icon-warning-outline"
      text="表單屬性(顯示原組件)"
      style="margin-bottom: 20px"
    >
       <template #content>
         <div style="color: #909090;">設置readonly=true以組件只讀方式顯示</div>
   </template>
    </VolHeader>
    <VolForm
      ref="myform2"
      :label-width="75"
      :formFields="formFields"
      :formRules="formRules"
    ></VolForm>
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
export default {
  components: { VolForm, VolHeader },
  methods: {
    getForm() {
      this.$message.error(JSON.stringify(this.formFields));
    },
    reset() {},
  },
  mounted() {},
  data() {
    return {
      loadKey: true,
      formFields: {
        Variety: "日用品",
        DateRange: "2019-09-01",
        City: "北京市",
        AvgPrice: 1238000,
        Variety1: "",
        DateRange1: "2019-09-02",
        City1: "上海市",
        AvgPrice1: 7.72,
        DateRange2: "2019-09-03",
        City2: "廣州市",
        AvgPrice2: 6.66,
        IsTop: "1",
      },
      formRules: [
        // 兩列的表單，formRules數據格式為:[[{},{}]]
        [
          {
            title: "商品類型",
            dataKey: "age",
            // 如果這裡綁定了data數據，後台不會加載此數據源
            data: [],
            field: "Variety",
            readonly: true,
            type: "select",
          },
          {
            dataKey: "city",
            title: "所在城市",
            field: "City",
            readonly: true,
            type: "select",
            data: [],
          },
          {
            title: "成交均價",
            field: "AvgPrice",
          },
        ],
        [
          {
            dataKey: "city",
            title: "所在地區",
            field: "City1",
            readonly: true,
            type: "select",
            data: [],
          },
          {
            title: "生產價格",
            field: "AvgPrice1",
            readonly: true,
          },
          {
            title: "審核日期",
            field: "DateRange",
            type: "date",
            readonly: true,
          },
        ],
        [
          {
            dataKey: "city",
            title: "詳細地址",
            field: "City2",
            readonly: true,
            type: "select",
            data: [],
          },
          {
            title: "平均重量",
            field: "AvgPrice2",
            type: "number",
            readonly: true,
          },
          {
            type: "date",
            title: "生產日期",
            field: "DateRange2",
            readonly: true,
          },
        ],
      ],
      formRules2: [
        [
          {
            title: "商品類型",
            dataKey: "age",
            data: [],
            field: "Variety",
            type: "label",
          },
          {
            dataKey: "city",
            title: "所在城市",
            field: "City",
            type: "label",
            data: [],
            type: "label",
          },
          {
            title: "成交均價",
            field: "AvgPrice",
            type: "label",
          },
        ],
        [
          {
            dataKey: "city",
            title: "所在地區",
            field: "City1",
            type: "label",
            data: [],
          },
          {
            title: "生產價格",
            field: "AvgPrice1",
            type: "label",
          },
          {
            title: "審核日期",
            field: "DateRange",
            type: "label",
          },
        ],
        [
          {
            title: "詳細地址",
            field: "City2",
            type: "label",
          },
          {
            title: "平均重量",
            field: "AvgPrice2",
            type: "label",
          },
          {
            type: "label",
            title: "生產日期",
            field: "DateRange2",
          },
        ],
      ],
    };
  },
};
</script>
<style lang="less" scoped>
.t-dis1,.t-dis2 {
  margin-bottom: 15px;
  background: white;
  border: 1px solid #f5f5f5;
  padding: 5px;
  border-radius: 5px;
  margin: 15px 20px 0 20px;
}
.t-dis1 ::v-deep(.el-form-item){
  margin-bottom: 5px;
}
</style>
