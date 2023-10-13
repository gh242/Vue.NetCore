<template>
  <div class="container">
    <el-alert type="success" style=" margin: 20px 20px 0 20px;" show-icon>
      treetable
      <div>
    頁面為手動引用的voltable的組件，具體使用見：TreeTable3.vue
      </div>
    </el-alert>
    <!-- 查詢配置 -->
    <div style="padding: 0px 20px">
      <VolHeader
        icon="md-apps"
        text="TreeTable"
        style="margin-bottom: 10px; border: 0px; margin-top: 15px"
      >
        <slot>
          <div style="text-align: right">
            <el-button type="primary" size="mini"   @click="search" 
              >查詢</el-button
            >
            <el-button type="success" size="mini" style="margin-left: 10px"  @click="getRows">獲取選中的行</el-button>
          </div>
        </slot>
      </VolHeader>
      <vol-table
        ref="table"
        :loadKey="true"
        row-key="Role_Id"
        :columns="columns"
        :pagination-hide="false"
        :max-height="450"
        url="/api/Sys_Role/getPageData"
        :index="true"
        :loadTreeChildren="loadTreeChildren"
        @loadBefore="loadTableBefore"
      ></vol-table>
    </div>
  </div>
</template>
<script>
import VolTable from "@/components/basic/VolTable.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
export default {
  components: { VolTable, VolHeader },
  created() {},
  data() {
    return {
      columns: [
        {
          field: "Role_Id",
          title: "角色ID",
          type: "int",
          width: 70,
          readonly: true,
          require: true,
          align: "left",
          sortable: true,
        },
        {
          field: "RoleName",
          title: "角色名稱",
          type: "string",
          width: 90,
          require: true,
          align: "left",
        },
        {
          field: "Dept_Id",
          title: "部門ID",
          type: "int",
          width: 90,
          hidden: true,
          align: "left",
        },
        {
          field: "DeptName",
          title: "部門名稱",
          type: "string",
          width: 90,
          align: "left",
        },
        {
          field: "Enable",
          title: "是否啟用",
          type: "byte",
          bind: { key: "enable", data: [] },
          width: 90,
          align: "left",
        },
        {
          field: "OrderNo",
          title: "排序",
          type: "int",
          width: 90,
          hidden: true,
          align: "left",
        },
        {
          field: "Creator",
          title: "創建人",
          type: "string",
          width: 130,
          readonly: true,
          align: "left",
        },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 90,
          readonly: true,
          align: "left",
          sortable: true,
        },
      ],
    };
  },
  methods: {
    loadTableBefore(params) {
      //Sys_RoleController中始終只加載根節點數據
      params.value = 1;
      return true;
    },
    loadTreeChildren(tree, treeNode, resolve) {
      //加載子節點數據
      let url = `api/role/getTreeTableChildrenData?roleId=${tree.Role_Id}`;
      this.http.post(url, {}).then((result) => {
        resolve(result.rows);
      });
    },
    search() {
      this.$refs.table.load();
    },
    getRows() {
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