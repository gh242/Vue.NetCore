<template>
  <div class="com-tree">
    <div class="left-tree">
      <tree-demo :onChange="onChange1"></tree-demo>
    </div>
    <div class="right-table">
      <table-demo></table-demo>
    </div>
  </div>
</template>
<script>
import treeDemo from "./demo1/Tree.vue";
import TableDemo from "./demo1/Table.vue";
export default {
  components: {
    treeDemo,
    TableDemo
  },
  created() {
    //初始化一個對象全局緩存起來，當點擊左邊樹形菜單時，把點擊菜單的treeId(角色id)存到treeDemo1裡
    this.$store.getters.data().treeDemo1 = { treeId: undefined };
  },
  methods: {
    onChange1(treeId) {
      //調用table(viewGird.vue)刷新方法
      this.$store.getters.data().viewGridDemo.nodeClick(treeId); //這裡可以寫任意參數
    }
  }
};
</script>

<style lang="less" scoped>
.com-tree {
  display: flex;
  > .left-tree {
    width: 215px;
  }
  > .right-table {
    flex: 1;
  }
}
</style>
