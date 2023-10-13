<template>
  <VolBox v-model="model" title="選擇數據" :lazy="true" :height="530" :width="1100" :padding="10">
    <!-- 設置查詢條件 -->
    <div style="padding-bottom: 10px">
      <span style="margin-right: 20px">請選擇數據</span>
      <el-input placeholder="商品名稱" style="width: 200px" v-model="ProductName" />
      <el-button type="primary" plain style="margin-left: 10px" icon="Search" @click="search">搜索</el-button>
    </div>
    <!-- vol-table配置的這些屬性見VolTable組件api文件 -->
    <vol-table ref="mytable" :loadKey="true" :columns="columns" :pagination="pagination" :pagination-hide="false"
      :max-height="380" :url="url" :index="true" :single="false" :defaultLoadPage="false"
      @loadBefore="loadTableBefore"></vol-table>
    <template #footer>
      <div>
        <el-button plain type="primary" @click="addRow">選擇數據</el-button>
        <el-button @click="model = false">關閉</el-button>
      </div>
    </template>
  </VolBox>
</template>
<script>
import VolBox from "@/components/basic/VolBox.vue";
import VolTable from "@/components/basic/VolTable.vue";
export default {
  components: {
    VolBox: VolBox,
    VolTable: VolTable,
  },
  data() {
    return {
      model: false,
      ProductName: "",
      //從後台接口加載數據,這裡的接口用的框架自帶的查詢，也可以自定義接口，見App_ExpertModelBody.vue中調用的後台getSelectorDemo方法
      url: "api/sellorderlist/getPageData",
      columns: [
        {
          field: "ProductName",
          title: "商品名稱",
          type: "string",
          bind: { key: "pn", data: [] },
          width: 150,
          sort: true,
        },
        { field: "MO", title: "批次", width: 100, sort: true },
        { field: "Qty", title: "數量", width: 90, sort: true },
        { field: "Weight", title: "重量", twidth: 90, sort: true },
        { field: "Remark", title: "備註", width: 120, sort: true },
        { field: "Creator", title: "創建人", width: 130, sort: true },
        {
          field: "CreateDate",
          title: "創建時間",
          type: "datetime",
          width: 145,
          sort: true,
        },
      ],
      pagination: {},
      row: {}, //明細表選擇的行
    };
  },
  methods: {
    open(row) {
      //   this.row = row;
      this.model = true;
      //打開彈出框時，加載table數據
      this.$nextTick(() => {
        this.$refs.mytable.load();
      });
    },
    search() {
      this.$refs.mytable.load();
    },
    addRow() {
      var rows = this.$refs.mytable.getSelected();
      if (!rows || rows.length == 0) {
        return this.$Message.error("請選擇行數據");
      }

      //回寫明細表行數據，見文檔子父組件調用與4獲取明細表行數據:http://v2.volcore.xyz/document/api
      this.$emit('parentCall', $parent => {

        //這裡可以寫個判斷是否已經存在明細表，找到不存在的數據
        // rows = rows.filter(c => {
        //    return !$parent.$refs.detail.rowData.some(r => { return r.xx == c.xx })
        //  });

        //生成新的對象
        rows = rows.map(c => {
          return {
            ProductName: c.ProductName,
            MO: c.MO,
            Weight: c.Weight,
            Remark: c.Remark
          }
        })
       
        //可以清空明細表數據
        //$parent.$refs.detail.rowData.splice(0);
        //回寫到明細表格
        $parent.$refs.detail.rowData.unshift(...rows);
      })
      this.model = false;
    },
    loadTableBefore(params) {
      //查詢前，設置查詢條件
      params.wheres = [
        { name: "ProductName", value: this.ProductName, displayType: "like" },
      ];
      return true;
    },
  },
};
</script>