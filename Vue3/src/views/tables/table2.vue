<template>
  <div class="example-tb">
    <el-alert
      title="table從後台api加載數據源"
      type="warning"
      show-icon
      :closable="false"
    >
      <p>
        1、只需要配置好列信息即可從後台加載(自動求和見代碼中的備註說明或table組件api)。
      </p>
      <p>
        2、如果需要篩選條件，在loadTableBefore方法中按demo格式提交查詢參數即可
      </p>
      <p>
        3、具體使用見：框架文檔->組件api->voltable。<a
          href="http://v2.volcore.xyz/document/api"
          target="_blank"
          >點擊查看文檔
        </a>
      </p>
    </el-alert>
    <div class="tb">
      <div class="search-info">
        <vol-form
          :label-width="80"
          ref="myform"
          class="my-form"
          :formFields="searchFields"
          :formRules="formOptions"
        >
        </vol-form>
        <div class="btns">
          <el-button type="danger" size="small" @click="getSelectRows"
            >獲取選中行</el-button
          >
          <el-button type="primary" size="small" @click="search"
            ><i class="el-icon-search"></i>搜索</el-button
          >
        </div>
      </div>
      <vol-table
        ref="table"
        :columns="columns"
        :pagination-hide="false"
        :max-height="400"
        :url="url"
        :index="true"
        @loadBefore="loadTableBefore"
        @loadAfter="loadTableAfter"
      ></vol-table>
    </div>
  </div>
</template>
<script>
import VolTable from "@/components/basic/VolTable.vue";
import VolForm from "@/components/basic/VolForm.vue";
export default {
  components: { VolTable, VolForm },
  created() {},
  data() {
    return {
      //查詢條件字段
      searchFields: {
        TranNo: "",
        CreateDate: [],
        OrderType: null,
      },
      formOptions: [
        //表單配置見表單組件文檔
        [
          {
            title: "運單號",
            field: "TranNo",
            placeholder: "模糊查詢",
          },
          {
            title: "訂單類型",
            field: "OrderType",
            dataKey: "ordertype",
            placeholder: "訂單類型",
            data: [],
            type: "select",
          },
          {
            title: "創建時間",
            range: true,
            colSize: 6,
            field: "CreateDate",
            type: "date",
            onChange:(val)=>{
           
            }
          },
        ],
      ],
      viewModel: false, //點擊單元格時彈出框
      url: "api/SellOrder/getPageData", //後從加載數據的url
      columns: [
        { field: "Order_Id", title: "Id", width: 90, hidden: true },
        {
          field: "TranNo",
          title: "運單號",
          type: "string",
          width: 130,
          sort: true,
        },
        {
          field: "OrderType",
          title: "訂單類型",
          type: "int",
          bind: { key: "ordertype", data: [] },
          width: 90,
          sort: true,
        },
        {
          field: "Qty",
          title: "銷售數量",
          type: "int",
          width: 90,
          sort: true,
          summary: true, //前端只設置summary: true 即可求後，後台見SellOrderService.cs中查詢方法說明
        },
        {
          field: "AuditStatus",
          title: "審核狀態",
          type: "int",
          bind: { key: "audit", data: [] },
          width: 90,
          sort: true,
        },
        {
          field: "AuditDate",
          title: "審核時間",
          type: "datetime",
          width: 120,
          sort: true,
          sort: true,
        },
        {
          field: "Remark",
          title: "備註",
          type: "string",
          width: 100,
          sort: true,
        },
        {
          field: "Creator",
          title: "創建人",
          type: "string",
          width: 100,
          sort: true,
        },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 90,
          sort: true,
        },
      ],
    };
  },
  methods: {
    //點擊查詢時生成查詢條件
    loadTableBefore(param, callBack) {
      console.log("加載數據前" + param);
      //生成查詢條件
      param.wheres = [
        //設置為like模糊查詢
        {
          name: "TranNo",
          value: this.searchFields.TranNo,
          displayType: "like",
        },
        {
          name: "OrderType",
          value: this.searchFields.OrderType,
        },
        {
          name: "CreateDate",
          value: this.searchFields.CreateDate[0],
          displayType: "thanorequal", //>=
        },
        {
          name: "CreateDate",
          value: this.searchFields.CreateDate[1],
          displayType: "lessorequal", //<=
        },
      ];
      callBack(true); //此處必須進行回調，返回處理結果，如果是false，則不會執行後台查詢
    },
    loadTableAfter(data, callBack) {
      //此處是從後台加數據後，你可以在渲染表格前，預先處理返回的數據
      console.log("加載數據後" + data);
      callBack(true); //同上
    },
    search() {
      this.$refs.table.load();
    },
    del() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行");
      }
      this.$refs.table.delRow();
      //此處可以接著寫刪除後台的代碼
    },
    getSelectRows() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行1");
      }
      let text = "當前選中的行數據：" + JSON.stringify(rows);
      this.$Message.info(text);
    },
  },
};
</script>
<style lang="less" scoped>
.example-tb {
  padding: 15px;
}
.tb {
  margin-top: 15px;
}
.v-header {
  background: white;
  padding: 10px;
}
.search-info {
  display: flex;
  .my-form {
    width: 840px !important;
    margin-bottom: -15px;
  }
  .btns {
    margin-left: 15px;
    position: relative;
    margin-top: 2px;
  }
}
</style>
