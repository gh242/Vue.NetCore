<template>
  <div>
    <el-alert type="info">
      <h5>SellOrder.js文件中的擴展SellOrder表顯示body信息,你可以在此處定義需要顯示的信息</h5>
    </el-alert>
  </div>
</template>
<script>
export default {
  components: {
    VolBox: () => import("@/components/basic/VolBox.vue")
  },
  data() {
    return {
      model: false
    };
  },
  methods: {
    test() {
      this.$emit("parentCall", $vue => {
        let rows = $vue.$refs.table.rowData;
        //此於拿到的$vue你可以操作其中的任何對象，如:
        //props.js中的屬性，this.columns/this.editFormFields
        //或者操作其他動態擴展的組件如:$vue.$refs.gridBody/$vue.$refs.gridFooter
        //$vue.$refs.table獲取表格信息
        //$vue.refresh()刷新表格等操作
        //能操作的所有對象可參照props.js與4ViewGird.uve的屬性
        //能操作的方法methods.js/detailMethods.js
      });
    }
  }
};
</script>