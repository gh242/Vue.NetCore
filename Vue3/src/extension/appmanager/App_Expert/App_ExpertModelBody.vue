<template>
  <VolBox v-model="model" :lazy="true" title="選擇數據" :height="450" :width="800" :padding="15">
    <!-- 設置查詢條件 -->
    <div style="padding-bottom: 10px">
      <span style="margin-right: 20px">請選擇數據</span>
      <el-input placeholder="名稱" style="width: 200px" v-model="expertName" />
      <el-button type="primary" style="margin-left:10px" size="small" icon="Search" @click="search">搜索</el-button>
    </div>

    <!-- vol-table配置的這些屬性見VolTable組件api文件 -->
    <vol-table ref="mytable" :loadKey="true" :columns="columns" :pagination="pagination" :pagination-hide="false"
      :max-height="380" :url="url" :index="true" :single="true" :defaultLoadPage="defaultLoadPage"
      @loadBefore="loadTableBefore" @loadAfter="loadTableAfter"></vol-table>
    <!-- 設置彈出框的操作按鈕 -->
    <template #footer>
      <div>
        <el-button size="small" type="primary" icon="el-icon-plus" @click="addRow()">添加選擇的數據</el-button>
        <el-button size="mini" icon="Close" @click="model = false">關閉</el-button>
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
      defaultLoadPage: false, //第一次打開時不加載table數據，openDemo手動調用查詢table數據
      expertName: "", //查詢條件字段
      url: "api/App_Expert/getSelectorDemo",//加載數據的接口
      columns: [
        { field: "expertId", title: "主鍵id", width: 90, hidden: true },
        { field: "expertName", title: "名稱", width: 120 },
        { field: "headImageUrl", type: "img", title: "頭像", width: 120 },
        { field: "resume", title: "簡介", width: 90 },
        {
          field: "enable",
          title: "是否啟用",
          bind: { key: "enable", data: [] },
          width: 110,
        },
      ],
      pagination: {}, //分頁配置，見voltable組件api
    };
  },
  methods: {
    openDemo(row) {
      this.model = true;
      //打開彈出框時，加載table數據

      this.$refs.mytable && this.$refs.mytable.load();

    },
    search() {
      //點擊搜索
      this.$refs.mytable.load();
    },
    addRow() {
      var rows = this.$refs.mytable.getSelected();
      if (!rows || rows.length == 0) {
        return this.$message.error("請選擇行數據");
      }
      //回寫數據到表單
      this.$emit("parentCall", ($parent) => {
        //將選擇的數據合併到表單中(注意框架生成的代碼都是大寫，後台自己寫的接口是小寫的)
        $parent.editFormFields.ExpertId = rows[0].expertId;
        $parent.editFormFields.ExpertName = rows[0].expertName;
        $parent.editFormFields.HeadImageUrl = rows[0].headImageUrl;
        $parent.editFormFields.Resume = rows[0].resume;
        $parent.editFormFields.Enable = rows[0].enable + ""; //enable是數字類型，框架綁定下拉框的時候要轉換成字符串
      });
      //關閉當前窗口
      this.model = false;
    },
    //這裡是從api查詢後返回數據的方法
    loadTableAfter(row) { },
    loadTableBefore(params) {
      //查詢前，設置查詢條件
      if (this.expertName) {
        params.wheres = [{ name: "expertName", value: this.expertName }];
      }
      return true;
    },
  },
};
</script>
