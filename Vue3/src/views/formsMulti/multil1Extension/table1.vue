<template>
  <div class="container">
    <el-alert
      type="info"
      style="margin-bottom: 10px"
      show-icon
      title="顯示多個table"
      ><div>
        1、上面table由代碼生成器生成，可以在自己添加的頁面把生成的引用進來使用
      </div>
      <div>2、見multi1.vue文件、multil1Extension</div>
    </el-alert>
    <div class="header">
      <div class="text">雙擊表編輯</div>
      <div class="btns">
        <el-button type="primary" size="mini" plain @click="clear"
          >清空表</el-button
        >
        <el-button type="primary" size="mini" plain @click="del"
          >刪除行</el-button
        >
        <el-button type="primary" size="mini" plain @click="add"
          >添加行</el-button
        >
        <el-button type="primary" size="mini" plain @click="getRows"
          >獲取選中的行</el-button
        >
      </div>
    </div>

    <vol-table
      ref="table"
      :columns="columns"
      :max-height="200"
      :index="true"
      :tableData="tableData"
      :paginationHide="true"
    ></vol-table>
    <br />
  </div>
</template>
<script>
import VolTable from "@/components/basic/VolTable.vue";

export default {
  components: { VolTable },
  methods: {
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
      this.tableData.push({});
    },
    getRows() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$Message.error("請先選中行1");
      }
      this.$Message.info("當前選中的行數據：" + JSON.stringify(rows));
    },
  },
  created() {},
  data() {
    return {
      text: "",
      tableData: [
        //表數據
        {
          ExpertId: 276,
          ExpertName: "起名太麻煩4",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x1.jpg",
          UserName: "13888888882",
          UserTrueName: "月穿潭底水無痕 ",
          AuditStatus: 1,
          Enable: 1,
          ReallyName: "月穿潭底水無痕 ",
          CreateDate: "2018-09-18 17:45:54",
        },
        {
          ExpertId: 276,
          ExpertName: "取名難好難",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/tj01.jpg",
          UserName: "13888888883",
          UserTrueName: "烏拉圭 ",
          AuditStatus: 2,
          Enable: 0,
          ReallyName: "月穿潭底水無痕 ",
          CreateDate: "2018-09-18 17:45:54",
        },
      ],
      columns: [
        //表配置
        {
          field: "ExpertId", //數據庫表字段,必須和數據庫一樣，並且大小寫一樣
          title: "主鍵ID", //表頭顯示的名稱
          type: "int", //數據類型
          isKey: true, //是否為主鍵字段
          width: 80, //表格寬度
          hidden: true, //是否顯示
          readonly: true, //是否只讀(功能未啟用)
          require: true, //是否必填(功能未啟用)
          align: "left", //文字顯示位置
        },
        {
          field: "ExpertName",
          title: "名稱",

          width: 150,
          align: "left",
          edit: { type: "text" },
          sortable: true, //是否排序(目前第一個字段為排序字段，其他字段排序開發中)
        },
        {
          field: "HeadImageUrl",
          title: "頭像",
          type: "img",
          width: 80,
          align: "left",
        },
        {
          field: "UserName",
          title: "申請人帳號",
          width: 120,
          hidden: true, //是否顯示
          edit: { type: "text" },
          align: "left",
        },
        {
          field: "UserTrueName",
          title: "申請人",

          width: 120,
          align: "left",
          click: (row, column) => {
            //單元格點擊事亻
            this.$message.error("此單元格沒有設置為可以編輯");
          },
          formatter: (row) => {
            //對單元格的數據格式化處理
            return row.UserTrueName;
          },
        },
        {
          field: "AuditStatus",
          title: "審核狀態",
          type: "int",
          bind: {
            //如果後面返回的數據為數據源的數據，請配置此bind屬性，可以從後台字典數據源加載，也只以直接寫上
            key: "audit",
            data: [
              { key: "0", value: "審核中" },
              { key: "1", value: "審核通過" },
              { key: "2", value: "審核未通過" },
            ],
          },
          width: 120,
          align: "left",
        },
        {
          field: "Enable",
          title: "是否啟用",
          type: "byte",
          bind: { key: "enable", data: [] }, //此處值為data空數據，自行從後台字典數據源加載
          width: 80,
          require: true,
          align: "left",
          edit: { type: "switch" },
        },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 150,
          readonly: true,
          align: "left",
          edit: { type: "datetime" },
          sortable: true,
        },
        {
          field: "ReallyName",
          title: "真實姓名",

          width: 120,
          click: (row, column) => {
            //單元格點擊事亻
            let msg =
              "此處可以自己自定格式顯示内容,此單元格原始值是:【" +
              row.ReallyName +
              "】";
            this.$message.error(msg);
          },
          formatter: (row, column) => {
            //對單元格的數據格式化處理
            return "<a style='color:rgb(15, 132, 255);'>點我</a>";
          },
          align: "left",
        },
        {
          field: "Creator",
          title: "創建人",

          width: 130,
          hidden: true,
          align: "left",
        },
      ],
    };
  },
};
</script>
<style lang="less" scoped>
.container {
  background: white;
  margin-top: 12px;
  padding: 15px;
  border-top: 10px solid #eee;
}
.header {
  display: flex;
  margin-bottom: 10px;
  .text {
    line-height: 30px;
    border-bottom: 2px solid #607d8b;
    font-size: 16px;
    /* color: #FF9800; */
    font-weight: 500;
  }
  .btns {
    flex: 1;
    text-align: right;
  }
}
</style>