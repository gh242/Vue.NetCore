<template>
  <div style="padding: 12px 15px;">
    <VolHeader
      style="padding-bottom: 5px; border-bottom: 0;"
      icon="md-podium"
      text="點樹形菜單自定義加載數據"
    >
      <!-- <div slot="content">當前選中的是:{{$store.getters.data().treeDemo2.text}}</div> -->
      <div style="text-align: right">
        <el-button type="primary" size="mini" @click="clear">清空表</el-button>
        <el-button type="primary" size="mini" @click="del">刪除行</el-button>
        <el-button type="primary" size="mini" @click="add">添加行</el-button>
        <el-button type="primary" size="mini" @click="getRows"
          >獲取選中的行</el-button
        >
      </div>
    </VolHeader>
  <el-alert
  style="margin-bottom: 5px;"
    title="自定義treetable頁面"
    type="warning"
    description="具體見：TreeTable2.vue"
    show-icon>
  </el-alert>
    <vol-table
      ref="editTable"
      :columns="columns"
      :maxHeight="550"
      :loadKey="true"
      :index="true"
      :tableData="tableData"
      :pagination-hide="true"
    ></vol-table>
  </div>
</template>
  <script>
import VolTable from "@/components/basic/VolTable.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
import options from "./tree_options";
export default {
  components: { VolTable, VolHeader },
  created() {
    //緩存當前table頁面，點擊左邊樹形菜單時，直接刷新此頁面table數據
    this.$store.getters.data().tableDemo2 = this;
  },
  data() {
    return {
      tableData: [], //表數據，點擊樹菜單時賦值
      columns: [
        //表配置
        {
          field: "code", //數據庫表字段,必須和數據庫一樣，並且大小寫一樣
          title: "編號", //表頭顯示的名稱
          isKey: true, //是否為主鍵字段
          //  hidden: true //是否顯示
        },
        {
          field: "address",
          title: "詳細地址",
          type: "text",
          width: 150,
        },
        {
          field: "remark",
          title: "備註",
          type: "text",
          width: 150,
        },
        {
          field: "enable",
          title: "是否可用",
          bind: { key: "enable", data: [] }, //此處值為data空數據，自行從後台字典數據源加載
          width: 150,
          edit: { type: "switch", keep: true },
        },
        {
          field: "createDate",
          title: "創建日期",
          type: "datetime",
          width: 150,
        },
      ],
    };
  },
  methods: {
    refresh() {
      //點擊左側樹形菜單時，刷新表數據
      let treeId = this.$store.getters.data().treeDemo2.treeId;
      if (treeId == undefined) {
        this.tableData = [];
      }

      let childrenIds = [];
      //options.tree的數據格式，可以從後台返回，在此處遞歸
      options.tree.forEach((x) => {
        if (x.parentId == treeId) {
          x.lv = 1;
          x.children = [];
          childrenIds.push(x.id);
          this.getTree(x.id, x, childrenIds);
        }
      });
      console.log(childrenIds);
      let rows = [];
      //options.tableData的數據格式，可以從後台返回，在此處遍歷
      options.tableData.forEach((x) => {
        if (childrenIds.indexOf(x.id) != -1 || x.id == treeId) {
          rows.push(x);
        }
      });
      this.tableData = rows;
    },
    getTree(id, data, childrenIds) {
      options.tree.forEach((x) => {
        if (x.parentId == id) {
          x.lv = data.lv + 1;
          if (!data.children) data.children = [];
          data.children.push(x);
          this.getTree(x.id, x);
        }
      });
    },
    del() {
      let rows = this.$refs.editTable.getSelected();
      if (rows.length == 0) {
        return this.$Message.error("請先選中行");
      }
      this.$refs.editTable.delRow();
    },
    clear() {
      this.tableData.splice(0);
    },
    add() {
      this.tableData.push({});
    },
    getRows() {
      let rows = this.$refs.editTable.getSelected();
      if (rows.length == 0) {
        return this.$Message.error("請先選中行");
      }
      this.$Message.error(JSON.stringify(rows));
    },
    endEdit() {
      //手動結束編輯
      this.$refs.editTable.edit.rowIndex = -1;
    },
  },
};
</script>
