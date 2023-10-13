<template>
  <div class="m-detail">
    <div class="m-title">
      <h2>訂單詳情</h2>
    </div>
    <div class="m-tabs">
      <Tabs>
        <TabPane label="配置要求" icon="md-apps">
          <div style="margin-bottom:10px;text-align: right;">
            <Button type="info" ghost @click="clear">清空表</Button>
            <Button type="info" ghost @click="del">刪除行</Button>
            <Button type="info" ghost @click="add">添加行</Button>
            <Button type="info" ghost @click="getRows">獲取選中的行</Button>
          </div>
          <vol-table
            ref="table"
            :linkView="_linkView"
            :columns="columns"
            :height="400"
            :index="true"
            url="/api/App_Appointment/getPageData"
            @loadBefore="loadBefore"
            :defaultLoadPage="false"
            :pagination-hide="false"
          ></vol-table>
        </TabPane>
        <TabPane label="訂單明細" icon="md-apps">
          <VolForm ref="myform" :loadKey="loadKey" :formFileds="formFileds" :formRules="formRules"></VolForm>
          <div class="m-btn">
            <Button type="info" @click="()=>{}">保存</Button>
          </div>
        </TabPane>
        <TabPane label="回款計劃" icon="md-apps">2</TabPane>
        <TabPane label="回款情況" icon="md-apps">2</TabPane>
      </Tabs>
    </div>
    <br />
    <br />
    <br />
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
import VolTable from "@/components/basic/VolTable.vue";
import VolBox from "@/components/basic/VolBox.vue";
export default {
  components: {
    VolForm,
    VolTable,
    VolBox
  },
  data() {
    return {
      row: {},
      loadKey: true, //如果有下拉框的表單，這裡設置為true,自動綁定下拉框數據
      formFileds: {
        Variety: "",
        AgeRange: "",
        DateRange: [],
        City: "",
        AvgPrice: 8.88,
        Date: ""
      },
      formRules: [
        [
          {
            dataKey: "age", //後台下拉框對應的數據字典編號
            data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
            title: "月齡",
            filter: true,
            required: true, //設置為必選項
            field: "AgeRange",
            type: "select"
          },
          {
            title: "品種",
            dataKey: "pz",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [
              { key: "1", value: "1" },
              { key: "2", value: "2" }
            ],
            required: false,
            field: "Variety",
            type: "select"
          },
          {
            dataKey: "city",
            title: "城市",
            required: true,
            data: [],
            field: "City",
            type: "select"
          }
        ],
        [
          {
            type: "decimal",
            title: "成交均價",
            required: true,
            field: "AvgPrice"
          },
          {
            title: "日期",
            required: true,
            field: "Date",
            placeholder: "你可以設置colSize屬性决定標籤的長度，可選值12/8/6/4",
            colSize: 8, //設置寬度為2/3
            type: "date"
          }
        ]
      ],
      columns: [
        {
          field: "Name",
          title: "姓名",
          type: "string",
          width: 100,
          sortable: true,
          edit:{type:"text"}
        },
        { field: "Describe", title: "描述", type: "string", width: 180, edit:{type:"text"} },
        { field: "PhoneNo", title: "電話", type: "string", width: 130 , edit:{type:"text"}},
        {
          field: "CreateID",
          title: "CreateID",
          type: "int",
          width: 80,
          hidden: true
        },
        { field: "Creator", title: "創建人", type: "string", width: 130 }
      ]
    };
  },
  methods: {
    rowChangeLoadData(row) {
      //選中行後觸發擴展tabs的方法，可以在此處加載tabs裡的數據
      // this.http.post("/xxx",{},'正在加載數據').then(x=>{
      // })
      if (this.$refs.table) {
        this.row = row; //保存當前觸發的行數據，下面查詢時用
        //觸發table查詢
        this.$refs.table.load(null, true);
      }
      console.log("選中行觸發tabs方法：" + JSON.stringify(row));
    },
    loadBefore(param, callBack) {
      //單選時觸發查詢前方法
      //查詢前設置查詢條件及分頁信息
      //根據當前字段查詢
      param.wheres = [{ name: "Name", value: this.row.Name }];
      callBack(true);
    },
    _linkView(row, column) {
      this.text =
        "點擊單元格的彈出框，當前點擊的行數據：" + JSON.stringify(row);
      this.viewModel = true;
      //  this.$message.error(JSON.stringify(row));
    },
    del() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行");
      }
      this.$refs.table.delRow();
    },
    clear() {
      this.tableData.splice(0);
    },
    add() {
      this.$refs.table.addRow({});
      //  this.tableData.push({});
    },
    getRows() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行1");
      }
      this.text = "當前選中的行數據：" + JSON.stringify(rows);
      this.viewModel = true;
    }
  }
};
</script>

<style lang="less" scoped>
.m-detail {
  min-height: 400px;
  margin-top: 20px;
  .m-tabs {
    background: white;
    padding: 0 15px;
  }
  .m-title {
    padding: 0 15px;
    h2 {
      font-weight: 500;
    }
  }
  .m-btn {
    text-align: right;
    padding: 10px 0;
  }
}
</style>