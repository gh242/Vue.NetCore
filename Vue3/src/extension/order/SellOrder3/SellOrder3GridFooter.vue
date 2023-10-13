<template>
  <div style="margin-top: 15px; padding: 0 16px">
    <el-tabs type="border-card" style="min-height: 320px; box-shadow: none">
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date"></i> 訂單明細</span>
        </template>
        <vol-table
          ref="tableList"
          :loadKey="true"
          :columns="columns"
          :pagination-hide="false"
          :height="230"
          :defaultLoadPage="false"
          @loadBefore="loadBefore"
          :endEditAfter="endEditAfter"
          url="/api/SellOrder/getDetailPage"
          :row-index="true"
          :columnIndex="true"
          :index="true"
          :click-edit="true"
        ></vol-table>
      </el-tab-pane>
      <el-tab-pane :lazy="true" label="消息中心">消息中心</el-tab-pane>
      <el-tab-pane :lazy="true" label="角色管理">角色管理</el-tab-pane>
      <el-tab-pane :lazy="true" label="定時任務補">定時任務補</el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import VolTable from "@/components/basic/VolTable.vue";
export default {
  components: {
    VolTable,
  },
  methods: {
    loadBefore(params, callback) {
      //設置查詢 條件

      var _row;
      //獲取主表選中的行
      this.$emit("parentCall", ($vue) => {
        var rows = $vue.getSelectRows();
        if (rows.length) {
          _row = rows[0];
        }
      });
      if (!_row) {
        //取消選中，清空明細
        return (this.$refs.tableList.rowData.length = 0);
        //  return this.$Message.error("請選中主表行數據");
      }

      //設置查詢條件，用主表id加載明細表數據(如果是自己定義的接口，這裡條件自己處理)
      params.value = _row.Order_Id;
      //設置排序字段(如果是自己定義的接口，排序字段不一定填寫)
      if (!params.sort) {
        params.sort = "CreateDate";
      }
      return callback(true);
    },
    endEditAfter(row) {
      //結束編輯時，可以直接調用後台的代碼進行保存
      console.log(row);
      return true;
    },
    save(row) {
      //保存
      this.$Message.info("保存");
    },
    del(row) {
      this.$confirm(
        "確認要刪除此行數據【" + row.ProductName + "】？？",
        "確認信息",
        {
          distinguishCancelAndClose: true,
          confirmButtonText: "確認刪除",
          cancelButtonText: "取消",
            center: true
        }
      )
        .then(() => {
          this.$message({
            type: "info",
            message: "提交刪除請求",
          });
        })
    },
    getRender() {
      //生成表格編輯配置
      return (h, { row, column, index }) => {
        return h(
          "div",
          {
            style: {
              cursor: "pointer",
              color: " #409eff",
              "font-size": "13px",
            },
          },
          [
            h(
              "a",
              {
                props: {},
                style: { "border-bottom": "1px solid" },
                onClick: (e) => {
                  e.stopPropagation();
                  this.del(row);
                },
              },
              "刪除"
            ),
            h(
              "a",
              {
                props: {},
                style: { "margin-left": "9px", "border-bottom": "1px solid" },
                onClick: (e) => {
                  e.stopPropagation();
                  this.$refs.tableList.edit.rowIndex = index;
                },
              },
              "編輯"
            ),
            h(
              "a",
              {
                props: {},
                style: { "margin-left": "9px", "border-bottom": "1px solid" },
                onClick: (e) => {
                  e.stopPropagation();
                  //强制結束編輯
                  this.$refs.tableList.edit.rowIndex = -1;
                  this.save();
                },
              },
              "保存"
            ),
          ]
        );
      };
    },
  },
  data() {
    return {
      tableData: [],
      //從生成的代碼sellorder2.vue裡面把明細配置複製過來就能用
      columns: [
        {
          field: "OrderList_Id",
          title: "OrderList_Id",
          type: "string",
          width: 90,
          hidden: true,
          require: true,
        },
        {
          field: "Order_Id",
          title: "訂單Id",
          type: "string",
          width: 90,
          hidden: true,
          readonly: true,
        },
        {
          field: "ProductName",
          title: "商品名稱",
          type: "string",
          bind: { key: "pn", data: [] },
          width: 150,
          edit: { type: "select" },
          require: true,
        },
        {
          field: "MO",
          title: "批次",
          type: "string",
          width: 100,
          edit: { type: "" },
          require: true,
          sort: true,
        },
        {
          field: "Qty",
          title: "數量",
          type: "int",
          width: 90,
          edit: { type: "drop" },
          summary: true, //設置求和
          require: true,
        },
        {
          field: "Weight",
          title: "重量",
          type: "decimal",
          summary: true, //設置求和
          width: 90,
          edit: { type: "" },
        },
        {
          field: "Creator",
          title: "創建人",
          type: "string",
          width: 100,
        },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 140,
          sort: true,
        },
        {
          title: "操作",
          width: 110,
          align: "center",
          render: this.getRender(),
        },
      ],
    };
  },
};
</script>
<style scoped>
h3 {
  font-weight: 500;
  padding-left: 10px;
  background: white;
  margin-top: 10px;
  padding-top: 6px;
  padding-bottom: 5px;
}
</style>