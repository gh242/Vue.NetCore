<template>
  <div class="container" style="padding: 30px 100px;">
    <vol-box :model.sync="viewModel" :height="450" :width="600" title="點擊表的彈出框">
      <div
        style="display: block;word-break: break-all;word-wrap: break-word;"
        slot="content"
      >{{text}}</div>
      <!--  <div slot="footer">這裡可以自己擴展box，也可不用寫 -->
      <div slot="footer">
        <Button type="info" @click="viewModel=false">確認</Button>
      </div>
    </vol-box>
    <Alert type="success" show-icon>可編輯的table,如果需要編輯又需要從後台加載數據，請與4table.vue兩個組件方法配合使用</Alert>
    <Divider>可編輯的table(雙擊表格即可編輯)</Divider>
    <div style="margin-bottom:10px;">
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
      :tableData="tableData"
      :paginationHide="true"
    ></vol-table>
  </div>
</template>
<script>
// vol-table屬性說明:
//linkView：點擊表格的連接執行的方法
//columns表頭列：顯示表格列,見下面columns配置及屬性說明
//pagination分頁配置:見下面pagination配置及屬性說明
//height表高度
//url從後台加載的數據的url
//index="true",設置為行有一個自動編號,如果要執行刪除操作，必須設置此屬性
import VolTable from "@/components/basic/VolTable.vue";
import VolBox from "@/components/basic/VolBox.vue";
export default {
  components: { VolTable, VolBox },
  methods: {
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
  },
  created() {
    //從後台加下拉框的[是否啟用的]數據源
    let keys = [];
    let columnBind = [];
    this.columns.forEach(x => {
      if (x.bind && x.bind.key && (!x.bind.data || x.bind.data.length == 0)) {
        keys.push(x.bind.key);
        if (!x.bind.data) x.bind.data = [];
        columnBind.push(x.bind);
      }
    });
    if (keys.length == 0) return;
    this.http.post("/api/Sys_Dictionary/GetVueDictionary", keys).then(dic => {
      dic.forEach(x => {
        columnBind.forEach(c => {
          if (c.key == x.dicNo) {
            c.data.push(...x.data);
          }
        });
      });
    });
  },
  data() {
    return {
      text: "",
      viewModel: false, //點擊單元格時彈出框
      tableData: [
        //表數據
        {
          ExpertId: 276,
          ExpertName: "財神爺",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/u%3D3441742992%2C2765570575%26fm%3D26%26gp%3D0.jpg",
          UserName: "13888888881",
          UserTrueName: "起名太麻煩4 ",
          AuditStatus: 0,
          Enable: 1,
          filetest:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/exceltest.xlsx", //也是設置為api服務器的文件，地址如：static/20191206/xx.xlsx
          ReallyName: "艹船借賤",
          CreateDate: "2018-09-18 17:45:54"
        },
        {
          ExpertId: 276,
          ExpertName: "你瞅啥?",
          HeadImageUrl:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x1.jpg",
          UserName: "13888888882",
          UserTrueName: "月穿潭底水無痕 ",
          AuditStatus: 1,
          Enable: 1,
          filetest:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/wordtest.docx", //也是設置為api服務器的文件，地址如：static/20191206/xx.xlsx
          ReallyName: "月穿潭底水無痕 ",
          CreateDate: "2018-09-18 17:45:54"
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
          filetest:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/wordtest.docx,https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/exceltest.xlsx", //也是設置為api服務器的文件，地址如：static/20191206/xx.xlsx
          ReallyName: "月穿潭底水無痕 ",
          CreateDate: "2018-09-18 17:45:54"
        }
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
          align: "left" //文字顯示位置
        },
        {
          field: "ExpertName",
          title: "名稱",
          type: "string",
          width: 150,
          align: "left",
          edit: { type: "text" },
          sortable: true //是否排序(目前第一個字段為排序字段，其他字段排序開發中)
        },
        {
          field: "HeadImageUrl",
          title: "頭像",
          type: "img",
          width: 160,
          align: "left"
        },
        {
          field: "UserName",
          title: "申請人帳號",
          type: "string",
          link: true, //設置link=true後此單元格可以點擊獲取當前行的數據進行其他操作
          width: 120,
          hidden: true, //是否顯示
          edit: { type: "text" },
          align: "left"
        },
        {
          field: "UserTrueName",
          title: "申請人",
          type: "string",
          width: 120,
          align: "left",
          click: (row, column) => {
            //單元格點擊事亻
            this.$message.error("此單元格沒有設置為可以編輯");
          },
          formatter: row => {
            //對單元格的數據格式化處理
            return row.UserTrueName;
          }
        },
        {
          field: "filetest",
          title: "點擊文件下載",
          width: 190,
          type: "file" //指定為file與4excel即可下載文件
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
              { key: "2", value: "審核未通過" }
            ]
          },
          width: 120,
          align: "left"
        },
        {
          field: "Enable",
          title: "是否啟用",
          type: "byte",
          bind: { key: "enable", data: [] }, //此處值為data空數據，自行從後台字典數據源加載
          width: 80,
          require: true,
          align: "left",
          edit: { type: "switch" }
        },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 150,
          readonly: true,
          align: "left",
          edit: { type: "datetime" },
          sortable: true
        },
        {
          field: "ReallyName",
          title: "真實姓名",
          type: "string",
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
            return "<a>點我</a>";
          },
          align: "left"
        },
        {
          field: "Creator",
          title: "創建人",
          type: "string",
          width: 130,
          hidden: true,
          align: "left"
        }
      ]
    };
  }
};
</script>