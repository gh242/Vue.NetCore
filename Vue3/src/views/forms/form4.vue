<template>
  <div class="t-dis">
    <div style="padding: 15px 23px 0 23px;">
      <VolHeader
        icon="el-icon-warning-outline"
        text="表單顯示"
        style="margin-bottom: 20px"
      >
        <template #content>
          <div style="color: #909090; font-size: 13px">
            這家伙很懶,什麼也沒留下
          </div>
        </template>
        <div>
          <el-button size="mini" type="primary">
            <i class="el-icon-edit"></i>設置</el-button
          >
        </div>
      </VolHeader>

      <VolForm
        ref="myform2"
        :label-width="90"
        :formFields="formFields"
        :formRules="formRules"
      ></VolForm>
    </div>
    <div style="height: 10px; background: #eee"></div>
    <table2></table2>
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
import table2 from "../tables/table2";
export default {
  components: { VolForm, VolHeader, table2 },
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
      },
      formRules: [
        [
          {
            title: "商品類型",
            dataKey: "age",
            // 如果這裡綁定了data數據，後台不會加載此數據源
            data: [],
            field: "Variety",
            type: "select",
          },
          {
            dataKey: "city",
            title: "所在城市",
            field: "City",
            type: "select",
            data: [],
          },
          {
            title: "成交均價",
            field: "AvgPrice",
          },
          {
            title: "詳細地址",
            field: "City2",
          },
        ],
        [
          {
            dataKey: "city",
            title: "所在地區",
            field: "City1",
            type: "select",
            data: [],
          },
          {
            title: "生產價格",
            field: "AvgPrice1",
          },
          {
            type: "date",
            title: "生產日期",
            field: "DateRange2",
          },
          {
            title: "審核日期",
            field: "DateRange",
            type: "date",
          },
        ],
      ],
    };
  },
};
</script>
<style lang="less" scoped>
.t-dis {
  margin-bottom: 15px;
  background: white;
}
.t-dis ::v-deep(.example-tb .el-alert) {
  display: none;
}
.t-dis ::v-deep(.example-tb .tb) {
  margin-top: 0 !important;
}
.t-dis ::v-deep(.example-tb .search-info) {
margin-left: -19px;
}
</style>
