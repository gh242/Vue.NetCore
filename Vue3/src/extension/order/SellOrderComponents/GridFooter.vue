<template>
  <div style="margin-top: 10px;">
    <el-alert type="info" title="gridFooter擴展位置" show-icon>
      <div>
        此代碼位置gridFooter.vue擴展組件被SellOrder.js的屬性gridFooter引用,你可以在此彈出框定義其他業務。
        <a style="color: #409eff; cursor: pointer"
          >使用方式與4上面gridHeader一致</a
        >
      </div>
    </el-alert>
  </div>
</template>
<script>
import { ref, defineComponent } from "vue";
export default defineComponent({
  setup(props, context) {
    return {}
  },
});
</script>
