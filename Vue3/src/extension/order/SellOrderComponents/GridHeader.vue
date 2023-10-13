<template>
  <div style="border-bottom: 10px solid rgb(228 228 228)">
    <el-alert type="success" title="gridHeader擴展位置" show-icon>
      <div>
        1、此代碼位置GridHeaderExtend.vue擴展組件被SellOrder.js的屬性gridHeader引用,你可以在此彈出框定義其他業務。
        <a style="color: #409eff; cursor: pointer" @click="search"
          >點擊調用父組件查詢方法</a
        >
        <a
          style="color: #409eff; cursor: pointer; margin-left: 20px"
          @click="add"
          >點擊調用父組件新建方法</a
        >
      </div>
      <div>
        2、使用 this.$emit('parentCall', ($vue) =>
        {})可以訪問整個介面上的所有屬性(頁面具體對象與4屬性，見
        <a target="_blank" href="http://www.volcore.xyz/document/api"
          >viewgrid文檔</a
        >)
      </div>
    </el-alert>
  </div>
  <vol-box
    :lazy="true"
    v-model="model1"
    title="彈出框2"
    :width="500"
    :padding="5"
    :onModelClose="onModelClose"
  >
    <div style="height: 150px;">按鈕點擊的彈出框</div>
  </vol-box>


  <vol-box
    :lazy="true"
    v-model="model2"
    title="彈出框2"
    :width="700"
    :padding="5"
    :onModelClose="onModelClose"
  >
    <div style="height: 400px;">表格當前行數據:{{ JSON.stringify(row2) }}</div>
    <template #footer>
      <el-button type="primary" size="small" @click="model2 = false"
        >確認</el-button
      >
      <el-button type="default" size="small" @click="model2 = false"
        >關閉彈</el-button
      >
    </template>
  </vol-box>
</template>
<script>
import VolBox from '@/components/basic/VolBox.vue';

import { ref, defineComponent } from 'vue';
export default defineComponent({
  components: {
    VolBox
  },
  setup(props, context) {
    const search = () => {
      context.emit('parentCall', ($parent) => {
        $parent.search();
      });
    };
    const add = () => {
      context.emit('parentCall', ($parent) => {
        $parent.add();
      });
    };
    const model1 = ref(false);
    const model2 = ref(false);
    const open1 = () => {
      model1.value = true;
    };
    const row2 = ref(null);
    const open2 = (row) => {
      model2.value = true;
      row2.value = row;
    };
    return { search, add, open1, open2, model1, model2, row2 };
  }
});
</script>
