<template>
  <div class="com-tree">
    <div class="left-tree">
      <tree-demo :onChange="onChange2"></tree-demo>
    </div>
    <div class="right-table">
      <table-demo ref="table2"></table-demo>
    </div>
  </div>
</template>
<script>
import TableDemo from "./demo2/Table2.vue";
import treeDemo from "./demo2/Tree2.vue";
export default {
  components: {
    "tree-demo": treeDemo,
    "table-demo": TableDemo,
  },
  created() {
    //初始化一個對象全局緩存起來，當點擊左邊樹形菜單時，把點擊菜單的treeId(角色id)存到treeDemo1裡
    this.$store.getters.data().treeDemo2 = { treeId: undefined };
  },
  methods: {
    onChange2(treeId) {
      this.$nextTick(() => {
        this.$store.getters.data().tableDemo2.refresh();
      });
    },
  },
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
