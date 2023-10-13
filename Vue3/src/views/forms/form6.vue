<template>
  <div>
    <div class="layout-content">
      <div class="layout-right">
        <h5>其他組件</h5>
        <el-alert show-icon
          >table+表單數據/測試完整示例/表單布局/其他組件，都是對element-plus二次封裝，
          只需要按現的方式進行配置json數據及實現方法即可使用</el-alert
        >
        <div class="com">
          <div>
            <h3>手動打開tabs,詳細使用見：框架文檔->前端開發->手動打開tabs</h3>
            <el-button type="info" size="small" @click="openUserInfo"
              >打開個人中心</el-button
            >
            <el-button
              type="error"
              size="small"
              @click="$tabs.close('/userinfo')"
              >關閉個人中心</el-button
            >
          </div>
          <div>
            <h3>彈出框/post/get請求(http請求代碼位置:api->http.js)</h3>
            <el-button type="error" size="small" @click="httpTest1"
              >post/get請求不帶提示</el-button
            >
            <el-button type="success" size="small" @click="httpTest2"
              >post/get請求帶默認提示</el-button
            >
            <el-button type="info" size="small" @click="httpTest3"
              >post/get請求帶自定義提示</el-button
            >
          </div>
          <div>
            <h3>vuex狀態管理,vuex代碼路徑：store->index.js</h3>
            <div style="padding: 10px; max-width: 800px; word-break: break-all">
              {{ userText }}
            </div>
            <el-button type="info" size="small" @click="getUserInfo"
              >獲取本地用戶信息</el-button
            >
            <el-button type="info" size="small" @click="getPermission"
              >獲取本地用戶菜單及權限</el-button
            >
            <el-button type="info" size="small" @click="getToken"
              >獲取本地用戶Token</el-button
            >
          </div>

          <!-- 文件操作 -->
          <div class="d-group" style="margin-top: 30px">
            <div>
              <h3>上傳excel文件</h3>
              <UploadExcel
                @importExcelAfter="importAfter"
                :url="url"
              ></UploadExcel>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import UploadExcel from "@/components/basic/UploadExcel.vue";
import Icons from "@/components/basic/Icons.vue";
export default {
  components: { UploadExcel, Icons },
  methods: {
    openUserInfo() {
      this.$tabs.open({
        text: "個人中心",
        path: "/userinfo",
        query: { x: 2221 },
      });
    },
    //每次刷新頁面後會重新加載用戶的最新信息
    getUserInfo() {
      let userInfo = this.$store.getters.getUserInfo();
      this.userText = JSON.stringify(userInfo);
    },
    getPermission() {
      let permission = this.$store.state.permission;
      this.userText = JSON.stringify(permission);
    },
    getToken() {
      let token = this.$store.getters.getToken();
      this.userText = token;
    },
    importAfter() {
      //上傳完成後處理
    },
    onOpenChange(id) {
      this.$essage.error("打開菜單" + id);
    },
    onSelect(id) {
      this.$message.error("菜單點擊" + id);
    },
    httpTest1() {
      //不帶提示
      let url = "/api/test2019/GetMsg",
        param = {};
      this.http.post(url, param).then((result) => {
        this.$message.error(result);
      });
    },
    httpTest2() {
      //帶默認提示
      let url = "/api/test2019/delay",
        param = {};
      this.http.post(url, param, true).then((result) => {
        this.$message.error(result);
      });
    },
    httpTest3() {
      //自定義提示
      //第三個參數loadText為是否顯示加載提示，默認為false,不顯示任何提示，如果設置為true默認加載提示文字為[正在處理中]，也可以自下定義顯示的文字,http請求的代碼位置:src->api->http.js
      //demo上
      let url = "/api/test2019/delay",
        param = {},
        loadText = "這裡參數可以自定文字";
      this.http.post(url, param, loadText).then((result) => {
        this.$message.error(result);
      });
    },
  },
  created() {},
  data() {
    return {
      userText: "",
      viewModel: false,
      iconModel: false,
      icon: "ivu-icon ivu-icon-logo-android",
      url: "", //上傳的url
      template: {
        //下載模板的路徑及模板的文件名，如果url為空，則不會顯示下載模板
        url: "/api/App_TransactionAvgPrice/DownLoadTemplate",
        fileName: "測試模板下載",
      },
      comSrc1: "",
      comSrc:
        "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x2.jpg",
    };
  },
};
</script>
<style lang="less" scoped>
.layout-content {
  position: relative;
  width: 1000px;
  left: 0;
  margin: 0;
  margin: auto;
  display: flex;
  .layout-left {
    width: 200px;
    margin-right: 10px;
  }
  .layout-right {
    flex: 1;
  }
}
.com > div {
  margin-top: 30px;
}
.d-group {
  display: inline-block;
  width: 100%;
  > div {
    float: left;
    width: 49%;
  }
  > div:first-child {
    margin-right: 2%;
  }
}
.on-icon {
  line-height: 20px;
  position: relative;
  .remove {
    display: none;
    color: red;
    right: 7px;
    position: absolute;
    top: -14px;
    font-size: 13px;
  }
}
</style>
