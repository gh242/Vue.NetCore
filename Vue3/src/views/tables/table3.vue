<template>
  <div class="example-tb">
    <el-alert title="功能說明" type="info" show-icon :closable="false">
      <p>1、日期(内容)格式化。</p>
      <p>2、單元格點擊事件、雙擊事件。</p>
      <p>3、單元格背景顏色、文字換行等...</p>
      <p>
        4、具體使用見：框架文檔->組件api->voltable。<a
          href="http://v2.volcore.xyz/document/api"
          target="_blank"
          >點擊查看文檔
        </a>
      </p>
    </el-alert>
    <div class="tb">
      <div class="btns" style="margin-bottom: 10px">
        <el-button type="success" size="small" @click="getSelect"
          ><i class="el-icon-check"></i>獲取選中行</el-button
        >

        <el-button type="primary" size="small" @click="addRow"
          ><i class="el-icon-plus"></i>添加行</el-button
        >
        <el-button type="primary" size="small" @click="clear"
          ><i class="el-icon-delete"></i>清空數據</el-button
        >
      </div>
      <!-- @rowClick單擊事件 -->
      <vol-table
        ref="table"
        :columns="columns"
        :max-height="500"
        :columnIndex="false"
        :ck="true"
        :text-inline="false"
        @row-dbclick="rowDbClick"
        :tableData="tableData"
        :paginationHide="true"
      ></vol-table>
    </div>
    <br />

    <VolBox
      :lazy="true"
      v-model="model"
      :title="title"
      :height="350"
      :width="700"
    >
      <div>當前點擊的行數據<br>{{currentRow}}</div>
    </VolBox>
  </div>
</template>
<script>
/*********************此示例采用的vue2語法，也可以使用vue3語法***************************/
import VolTable from "@/components/basic/VolTable.vue";
import VolHeader from "@/components/basic/VolHeader.vue";

import VolBox from "@/components/basic/VolBox.vue";
import VolUpload from "@/components/basic/VolUpload.vue";
import { ref, getCurrentInstance } from "vue";
export default {
  components: {
    VolTable,
    VolBox,
    VolHeader,
    VolUpload,
  },
  setup() {},
  data() {
    return {
      currentRow:'',
      model: false,
      url: "/api/app_news/upload", //使用後台自帶的上傳文件方法，也可以自定義方法上傳
      text: "",
      tableData: [
        //表數據
        {
          ExpertName: "天生注定(彈出框)",
          files:
            "https://files-1256993465.cos.ap-chengdu.myqcloud.com/測試現有文件可下載1.xlsx",
          UserName: "12345",
          UserTrueName: "7月23日，豐田汽車(中國)投資，將召回共計238540輛汽車",
          AuditStatus: 0,
          Enable: 1,
          CreateDate: "2018-09-18 17:45:54",
        },
        {
          ExpertName: "還沒想好(彈出框)",
          files:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x1.jpg,https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x2.jpg",
          UserName: "56789",
          UserTrueName: '沪漂夫妻買不起房 用4個集裝箱改裝成"花園豪宅"',
          AuditStatus: 1,
          Enable: 1,
          CreateDate: "2018-09-18 17:45:54",
        },
      ],
      columns: [
        //表配置
        {
          field: "ExpertName",
          title: "點擊單元格事件",
          width: 100,
          formatter: (row, column) => {
            return `<a style="color: #1e8cff;cursor: pointer;">${
              row.ExpertName || ""
            }</a>`;
          },
          click: (row, column) => {
            this.currentRow=JSON.stringify(row);
            this.model=true;
          },
        },
        {
          field: "雙擊單元格",
          title: "雙擊單元格",
          formatter: (row, column) => {
            return `<a style="color:red;cursor: pointer;">雙擊單元格</a>`;
          },
        },
        {
          field: "files",
          title: "文件下載",
          type: "file", //img,excel,file可以選擇
          width: 150,
        },
        {
          field: "UserName",
          title: "單元格顏色",
          width: 70,
          cellStyle: (row, rowIndex, columnIndex) => {
            if (row.UserName == "12345") {
              return { background: "#ecf5ff", color: "#409EFF" };
            } else {
              return { background: "#66b1ff", color: "#ffff" };
            }
          },
        },
        {
          field: "UserTrueName",
          title: "超出換行顯示",
          width: 160,
        },
        {
          field: "AuditStatus",
          title: "下拉框",
          type: "int",
          bind: {
            key: "audit",
            data: [],
          },
          width: 100,
          onChange: (row, column) => {
            this.$message.success(row.AuditStatus + "");
          },
        },
        {
          field: "Enable",
          title: "Switch事件",
          type: "byte",
          bind: { key: "enable", data: [] }, //會自動從後台字典數據源加載
          width: 70,
          require: true,
          align: "left",
          edit: { type: "switch", keep: true }, //keep始終開啟編輯狀態
          onChange: (val, row, column) => {
            this.$message.info("switch點擊事件：" + row.Enable);
          },
        },
        {
          field: "CreateDate",
          title: "日期格式化",
          type: "datetime",
          width: 100,
          formatter: (row, column) => {
            return (row.CreateDate || "").split(" ")[0].replace(/-/g, ".");
          },
        },
      ],
    };
  },
  methods: {
    rowDbClick({ row, column, event }) {
      //雙擊事件
      this.$message.warning("雙擊當前行事件並選中當前行");
      this.$refs.table.$refs.table.toggleRowSelection(row);
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
    addRow() {
      //添加行時設置默認值
      this.tableData.push({ Enable: 1, UserName: "這裡是默認值" });
    },
    getSelect() {
      //獲取選中的行
      let rows = this.$refs.table.getSelected();

      return this.$message.error("請先選中行:" + JSON.stringify(rows));
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
</style>
