<template>
  <div class="example-tb">
    <el-alert title="關於table使用" type="success" show-icon :closable="false">
      <p>1、VolTable基於Element Table封裝的常用功能。</p>
      <p>
        2、功能包括，雙擊編輯：select、select聯動、switch、日期、圖片顯示與4預覽、單元格文件下載、input、render動態渲染等組件。
      </p>
      <p>
        3、最重要的是,封裝後的組件功能包括：自動綁定下拉框數據源、自動從後台加載數據、分頁、及table輸入驗證等常用功能(只需要照著文檔配置json即可完成)。
      </p>
      <p>
        4、具體使用見：框架文檔->組件api->voltable。<a
          href="http://v2.volcore.xyz/document/api"
          target="_blank"
          >點擊查看文檔
        </a>
      </p>
    </el-alert>
    <div class="tb">
      <div class="btns" style="margin-bottom: 10px;">
        <el-button type="success" size="small" @click="getSelect"><i class="el-icon-check"></i>獲取選中行</el-button>
  
           <el-button type="primary" size="small" @click="addRow"><i class="el-icon-plus"></i>添加行</el-button
        >
      </div>
              <!-- :single="true"設置為單選  -->
      <vol-table
        ref="table"
        :columns="columns"
        :max-height="500"
        :index="true"
        @rowClick="rowClick"
        :tableData="tableData"
        :paginationHide="true"
        :endEditBefore="endEditBefore"
        :endEditAfter="endEditAfter"
        @rowChange="rowChange"
        :single="true"
        :beginEdit="beginEdit"
      ></vol-table>
    </div>
    <el-alert
      style="margin-top: 15px"
      :closable="false"
      title="此示例給出了大部分功能所以代碼比看起來比較多，可根據需要設置"
      type="info"
      show-icon
    >
    </el-alert>
    <br />
  </div>
</template>
<script>
/*********************此示例采用的vue2語法，也可以使用vue3語法***************************/

import VolTable from "@/components/basic/VolTable.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
// import VolBox from "@/components/basic/VolBox.vue";
import { ref, getCurrentInstance } from "vue";
import VolUpload from "@/components/basic/VolUpload.vue";
export default {
  components: {
    VolTable,
    //VolBox,
    VolHeader,
    VolUpload,
  },
  setup() {},
  data() {
    return {
      url: "/api/app_news/upload", //使用後台自帶的上傳文件方法，也可以自定義方法上傳
      text: "",
      tableData: [
        //表數據
        {
          ExpertId: 276,
          ExpertName: "時間管理大濕",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x3.jpg",
          UserName: "12345",
          UserTrueName: "孤獨比夜還長 ",
          AuditStatus: 0,
          Enable: 1,
          ReallyName: "45678",
          CreateDate: "2018-09-18 17:45:54",
          Creator: 38.88,
        },
        {
          ExpertId: 276,
          ExpertName: "小短腿-鲁班",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x1.jpg",
          UserName: "56789",
          UserTrueName: "七秒種的記憶",
          AuditStatus: 1,
          Enable: 1,
          ReallyName: "七秒種的記憶1",
          CreateDate: "2018-09-18 17:45:54",
          Creator: 19.2,
        },
        {
          ExpertId: 276,
          ExpertName: "行走在冷風中",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x2.jpg",
          UserName: "13883",
          UserTrueName: "不午休的、貓 ",
          AuditStatus: 2,
          Enable: 0,
          ReallyName: "月穿潭底水無痕 ",
          CreateDate: "2018-09-18 17:45:54",
          Creator: 2.2,
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
          readonly: true, 
          require: true, 
          align: "left"//文字顯示位置left、center
        },
        {
          field: "ExpertName",
          title: "render動態渲染",
          type: "string",
          width: 100,
          require: true,
          align: "left",
          render: (h, { row, column, index }) => {
            return h("div", { style: {} }, [
              h("div", {}, [
                h("i", {
                  class: ["el-icon-zoom-out"],
                  style: {
                    cursor: "pointer",
                    "margin-right": "8px",
                    color: "#b1b1b1",
                  },
                  onClick: (e) => {
                    e.stopPropagation();
                    this.$Message.success("搜索按鈕");
                  },
                }),
                h(
                  "a",
                  {
                    class: [],
                    style: {
                      cursor: "pointer",
                      color: "#3a8ee6",
                    },
                    onClick: (e) => {
                      e.stopPropagation();
                      //彈出框編輯
                      this.$message.success(row.ExpertName);
                    },
                  },
                  row.ExpertName
                ),
              ]),
            ]);
          },
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
          title: "長度限制",
          type: "string",
          //  link: true, //設置link=true後此單元格可以點擊獲取當前行的數據進行其他操作
          width: 70,
          require: true,
          edit: { type: "text", min: 3, max: 5 },
          align: "left",
        },
        {
          field: "UserTrueName",
          title: "不可編輯",
          type: "string",
          width: 100,
          align: "left",
          require: true,
          // edit: { type: "text", min: 4, max: 7 },
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
          title: "下拉框",
          type: "int",
          bind: {
            //如果後面返回的數據為數據源的數據，請配置此bind屬性，可以從後台字典數據源加載，也只以直接寫上
            key: "audit",
            data: [
              { key: 0, value: "審核中" },
              { key: 1, value: "審核通過" },
              { key: 2, value: "審核未通過" },
            ],
          },
          edit: { type: "select" },
          width: 100,
          align: "left",
          onChange: (row, column) => {
            this.$message.success(row.AuditStatus + "");
          },
        },
        {
          field: "Enable",
          title: "Switch",
          type: "byte",
          bind: { key: "enable", data: [] }, //會自動從後台字典數據源加載
          width: 70,
          require: true,
          align: "left",
          edit: { type: "switch", keep: true }, //keep始終開啟編輯狀態
          onChange: (val, row, column) => {
            this.$message.info("switch點擊事件");
          },
        },
        {
          field: "CreateDate",
          title: "日期",
          type: "date",
          width: 100,
          align: "left",
          edit: { type: "date" }
        },
        {
          field: "Creator",
          title: "數字驗證",
          type: "string",
          width: 90,
          onKeyPress: (row, column, $event) => {
            console.log(row);
          },
          edit: { type: "decimal", min: 2.2, max: 40 },
        },
        {
          field: "操作",
          title: "操作",
          width: 120,
          fixed: "right",
          align: "center",

          render: (h, { row, column, index }) => {
            return <div onClick={($e) => {  $e.stopPropagation();}}>
            <el-button onClick={($e) => {   this.tableData.splice(index, 1); }} type="primary" plain size="small" style="padding: 10px !important;">刪除</el-button>
            <el-button onClick={($e) => { this.editClick(row, column) }} type="success" plain size="small" style="padding: 10px !important;">編輯</el-button>

            <el-dropdown  trigger="click" 
              v-slots={{
                dropdown: () => (
                <el-dropdown-menu>
                  <el-dropdown-item><div onClick={($e) => {  this.dropdownClick('京醬肉絲') }}>京醬肉絲</div></el-dropdown-item>
                  <el-dropdown-item><div onClick={($e) => {   this.dropdownClick('驢肉火烧') }}>驢肉火烧</div></el-dropdown-item>
                  <el-dropdown-item><div onClick={($e) => { this.dropdownClick('吊爐烤鸭') }}>吊爐烤鸭</div></el-dropdown-item>
                </el-dropdown-menu>
              )
            }}
            >
              <span style="font-size: 13px;color: #409eff;margin: 5px 0 0 10px;" class="el-dropdown-link">
                更多<i class="el-icon-arrow-right"></i>
              </span>
            </el-dropdown>
          </div>
          }
        }
      ],
    };
  },
  methods: {
    dropdownClick(value) {
      this.$message.success(value)
    },
    rowChange(row) {
      //選中checkbox事件
      return this.$message.success("點擊checkbox,第" + row.elementIndex + "行");
    },
    rowClick({ row, column, $event }) {
      //點擊行事件
      return this.$message.success("點擊的行：" + row.elementIndex);
    },
    beginEdit(row, column, index) {
      console.log("編輯開始前" + JSON.stringify(row));
      return true;
    },
    endEditBefore(row, column, index) {
      console.log("結束編輯前" + JSON.stringify(row));
      return true;
    },
    endEditAfter(row, column, index) {
      console.log("結束編輯後" + JSON.stringify(row));
      return true;
    },
    editClick(row,column){
      let _index = this.$refs.table.edit.rowIndex;
                      if (_index != -1) {
                        return this.$message.error(
                          "請先完成第" +
                            (_index + 1) +
                            "行的編輯,點擊表頭可完成編輯"
                        );
                      }
                      this.$refs.table.rowBeginEdit(row, column);
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
    addRow() { //添加行
      this.tableData.push({});
    },
    getSelect() { //獲取選中的行
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
