<template>
  <div class="m-body">
    <a @click="close">
      這裡是自定義擴展的代碼，可以訪問整個介面上的所有屬性
      this.$emit('parentCall', ($vue) => {});點擊測試關閉彈出框</a
    >
    <el-steps :active="1" simple>
      <el-step title="步驟 1" icon="el-icon-edit"></el-step>
      <el-step title="步驟 2" icon="el-icon-upload"></el-step>
      <el-step title="步驟 3" icon="el-icon-picture"></el-step>
    </el-steps>
  </div>
</template>
<script>
import { ref, defineComponent } from "vue";
export default defineComponent({
  setup(props, context) {
    const close = () => {
      context.emit("parentCall", ($parent) => {
        $parent.boxModel = false;
      });
    };
    return { close };
  },
});
</script>
<style scoped>
.m-body {
  border-top: 1px solid #eee;
  margin-top: -14px;
  text-align: center;
  padding-top: 5px;
}
a {
  color: #2d94ff;
  cursor: pointer;
  display: inline-block;
  margin-bottom: 5px;
}
</style>