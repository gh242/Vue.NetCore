<template>
  <div>
    <vol-box
      v-model="model"
      :padding="30"
      title="修改密碼"
      :width="500"
      :height="250"
    >
      <el-alert type="success">
        <h3>
          <span>帳號：{{ row.UserName }}</span>
          <span>用戶：{{ row.UserTrueName }}</span>
        </h3>
      </el-alert>
      <div>
        <el-input
          placeholder="請輸入密碼"
          v-model="password"
          size="large"
          style="width: 100%; margin-top: 15px"
        />
      </div>
      <template #footer>
        <el-button
          type="primary"
          @click="savePwd()"
          >修改密碼</el-button
        >
        <el-button
          @click="model = false"
          >關閉</el-button
        >
      </template>
    </vol-box>
  </div>
</template>
<script>
import { defineComponent, defineAsyncComponent } from "vue";
export default defineComponent({
  components: {
    VolBox: defineAsyncComponent(() => import("@/components/basic/VolBox.vue"))
  },
  data() {
    return {
      row: {},
      password: "",
      model: false,
    };
  },
  methods: {
    open(row) {
      this.password = "";
      this.row = row;
      this.model = true;
    },
    savePwd() {
      if (!this.password) return this.$Message.error("請輸密碼");
      if (this.password.length < 6)
        return this.$Message.error("密碼長度至少6位");
      let url =
        "/api/user/modifyUserPwd?password=" +
        this.password +
        "&userName=" +
        this.row.UserName;
      this.http.post(url, {}, true).then((x) => {
        if (!x.status) {
          return this.$message.error(x.message);
        }
        this.model = false;
        this.$Message.success(x.message);
      });
    },
  },
  created() {},
})
</script>
<style lang="less" scoped>
h3 {
  font-weight: 500;
  > span:last-child {
    margin-left: 30px;
  }
}
</style>