<template>
  <div class="vol-tabs" style="height:390px ;">
    <el-tabs
      type="border-card"
      style="
        min-height: 370px;
        box-shadow: none;
        border-bottom: 0;
        border-top: 1px solid #eee;
      "
    >
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date"></i> 從表1</span>
        </template>
        <!-- 顯示操作按鈕 -->
        <div>
          <el-button
            type="primary"
            icon="Close"
            link
            @click="del"
            >刪除行</el-button
          >
          <el-button
            type="success"
            icon="Plus"
            link
            @click="add"
            >添加行</el-button
          >
          <el-button
            type="warning"
            icon="Check"
            link
            @click="getRows"
            >獲取選中的行</el-button
          >
          <el-button
            type="info"
            icon="Refresh"
            link
            @click="$refs.table1.load()"
            >刷新</el-button
          >
        </div>
        <el-alert
          title="雙擊行可以編輯,或者可以根據菜單： table使用->table編輯(二)在行上配置操作,完整文檔見:組件api->voltable"
          type="warning"
          style="margin: 10px 0"
          show-icon
        >
        </el-alert>
        <!-- :defaultLoadPage="false"打開窗口禁用默認加載數據 -->
        <vol-table
          ref="table1"
          :clickEdit="true"
          :loadKey="true"
          :columns="tableColumns1"
          :pagination-hide="false"
          :height="205"
          :url="table1Url"
          :index="true"
          :defaultLoadPage="false"
          @loadBefore="loadTableBefore1"
          @loadAfter="loadTableAfter1"
        ></vol-table>
      </el-tab-pane>

      <!-- 從表2 -->
      <el-tab-pane :lazy="false" label="從表2">
        <template #label>
          <span><i class="el-icon-date"></i> 從表2</span>
        </template>
        <!-- 從表2配置 ,雙擊可以開啟編輯-->
        <div style="padding-bottom: 10px">
          <el-button
            type="primary"
             link
            icon="Plus"
            @click="$refs.table2.addRow({})"
            >添加行</el-button
          >
          <el-button
            icon="Close"
            link
            type="success"
            ghost
            @click="$refs.table2.load()"
            >刷新</el-button
          >
        </div>
        <vol-table
          ref="table2"
          :loadKey="true"
          :clickEdit="true"
          :columns="tableColumns2"
          :pagination-hide="false"
          :height="275"
          :url="table2Ur"
          :defaultLoadPage="false"
          @loadBefore="loadTableBefore2"
          :index="true"
        ></vol-table>
      </el-tab-pane>
      <el-tab-pane :lazy="false" label="從表3">從表3</el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
//開發一對多從表需要參照voltable與4viewgrid組件api
import VolTable from "@/components/basic/VolTable.vue";
export default {
  components: { VolTable },
  data() {
    return {
      //從表1
      table1Url: "api/App_ReportPrice/getTable1Data", //table1獲取數據的接口
      //表配置的字段注意要與4後台返回的查詢字段大小寫一致
      tableColumns1: [
        { field: "id", title: "主鍵ID", type: "int", width: 80, hidden: true },
        {
          field: "title",
          title: "標題",
          type: "string",
          width: 400,
          require: true,
          sortable: true,
          edit: { type: "text" }, //keep:true始終開啟編輯，false雙擊才能編輯
        },
        { field: "imageUrl", title: "封面圖片", type: "file", width: 170 },
        {
          field: "dailyRecommend",
          title: "内容推薦",
          type: "sbyte",
          bind: { key: "dr", data: [] },
          edit: { type: "switch" },
          width: 120,
          require: true,
          onChange: (column, row, tableData, value) => {
            this.$Message.info(value + "");
          },
        },
        {
          field: "enable",
          title: "是否啟用",
          type: "int",
          width: 90,
          bind: { key: "enable", data: [] }, //自動綁定數據源
          //配置為select編輯類型,keep=true始終處於編輯狀態(如果想要雙擊編輯，去掉keep屬性，具體配置可見voltable組件api)
          edit: { type: "select" },
          onChange: (column, row, tableData) => {
            this.$Message.info(row.enable + "");
          },
        },
        {
          field: "createDate",
          title: "發佈時間",
          type: "datetime",
          width: 150,
          readonly: true,
          sortable: true,
          edit: { type: "datetime" },
        },
      ],
      //從表2
      table2Ur: "api/App_ReportPrice/getTable2Data", //table1獲取數據的接口
      //表配置的字段注意要與4後台返回的查詢字段大小寫一致
      tableColumns2: [
        { field: "id", title: "主鍵ID", type: "int", width: 80, hidden: true },
        {
          field: "name",
          title: "姓名",
          edit: { type: "text" },
          type: "text",
          width: 120,
        },
        {
          field: "phoneNo",
          title: "電話",
          type: "text",
          edit: { type: "text" },
          width: 150,
        },

        {
          field: "createDate",
          title: "創建時間",
          type: "text",
          edit: { type: "datetime" },
          width: 150,
        },
        {
          field: "describe",
          title: "描述",
          type: "text",
          edit: { type: "text" },
          width: 500,
        },
      ],
    };
  },
  methods: {
    //如果要獲取頁面的參數請使用 this.$emit("parentCall",見下面的使用方法
    modelOpen() {
      let $parent;
      //獲取生成頁面viewgrid的對象
      this.$emit("parentCall", ($this) => {
        $parent = $this;
      });
      //當前如果是新建重置兩個表格數據
      if ($parent.currentAction == "Add") {
        this.$refs.table1.reset();
        this.$refs.table2.reset();
      } else {
        //如果是編輯，添加兩個表格的數據
        this.$refs.table1.load();
        this.$refs.table2.load();
      }
    },
    loadTableBefore1(param, callBack) {
      let $parent = null;
      //當前是子頁面，獲取查詢viewgrid頁面的對象()
      this.$emit("parentCall", ($this) => {
        $parent = $this;
      });
      //如果是新建功能，禁止刷新操作
      if ($parent.currentAction == "Add") {
        return callBack(false);
      }
      //獲取當前編輯主鍵id值
      let id = $parent.currentRow.Id;
      //添加從表的查詢參數(條件)
      //將當前選中的行主鍵傳到後台用於查詢(後台在GetTable2Data(PageDataOptions loadData)會接收到此參數)
      param.wheres.push({ name: "Id", value: id });
      callBack(true);
    },
    //從表2加載數據數前(操作與4上面一樣的,增加查詢條件)
    loadTableBefore2(param, callBack) {
      callBack(true);
    },
    //從後台加載從表1數據後
    loadTableAfter1(data, callBack) {
      return true;
    },


    del() {
      let rows = this.$refs.table1.getSelected();
      if (rows.length == 0) {
        return this.$Message.error("請先選中行");
      }
      this.$refs.table1.delRow();
      //可以this.http.post調用後台實際執行查詢
    },
    clear() {
      this.$refs.table1.reset();
    },
    add() {
    //  this.$refs.table1.addRow({});
    this.$refs.table1.rowData.unshift({title:"測試....."});
    },
    getRows() {
      //獲取選中的行
      let rows = this.$refs.table1.getSelected();
      if (rows.length == 0) {
        return this.$Message.error("請先選中行");
      }
      this.$Message.error(JSON.stringify(rows));
    },
  },
};
</script>
<style lang="less" scoped>
.vol-tabs {
  background: white;
}
</style>
