<template>
  <div>
    <el-Alert type="success" show-icon>
  
        <h4 style="color: #2196F3;">
          -- 此後台驗證封裝目的是為了增加代碼復用性、盡量避免自己寫if esle判斷提交的參數。
        </h4>
        <p>1、後台實體復用：同一個實體，不同的接口驗證的字段不同，需要注冊對應字段。</p>
        <p>2、普通多參數驗證：普通參數可用於任何參數名相同的接口。</p>
        <p>3、具體使用參照：ObjectActionValidatorExampleController，具體注入驗證規則參照：ValidatorContainer.cs</p>
    </el-Alert>
    <br />
    <div class="validator">
      <div class="general">
        <div class="v1">
          <h2>驗證所有普通參數</h2>
          <div class="v-input">
            <label>
              <i class="require">*</i>用戶名：
            </label>
            <el-input  v-model="userName1" style="width: 230px" placeholder="輸入用戶名" />
          </div>
          <div class="v-input">
            <label style="font-size: 12px;">
              <i class="require">*</i>手機號：
            </label>
            <el-input  v-model="phoneNo1" style="width: 230px" placeholder="輸入手機號" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test1()" long>提交驗證test1</el-button>
          </div>
        </div>

        <div class="v2">
          <h2>只驗證手機號</h2>
          <div class="v-input">
            <label>用戶名：</label>
            <el-input  v-model="userName2" style="width: 230px" placeholder="輸入用戶名" />
          </div>
          <div class="v-input">
            <label style="font-size:12px;">
              <i class="require">*</i>手機號：
            </label>
            <el-input  v-model="phoneNo2" style="width: 230px" placeholder="輸入手機號" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test2()" long>提交驗證test2</el-button>
          </div>
        </div>
        <div class="v3">
          <h2>驗證字符長度與4數字大小</h2>
          <div class="v-input">
            <label>
              <i class="require">*</i>所在地：
            </label>
            <el-input  v-model="local" style="width: 230px" placeholder="長度[6-10]" />
          </div>
          <div class="v-input">
            <label>
              <i class="require">*</i>存貨量：
            </label>
            <el-input  v-model="qty" style="width: 230px" placeholder="值範圍[200-500]" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test3()" long>提交驗證test3</el-button>
          </div>
        </div>
      </div>

      <div class="object-model">
        <div class="v4">
          <h2>實體校驗指定字段：用戶名、密碼</h2>
          <div class="v-input">
            <label>
              <i class="require">*</i>用戶名：
            </label>
            <el-input  v-model="loginInfo1.userName" style="width: 230px" placeholder="輸入用戶名" />
          </div>
          <div class="v-input">
            <label>
              <i class="require">*</i>密 碼：
            </label>
            <el-input  v-model="loginInfo1.passWord" style="width: 230px" placeholder="輸入密碼" />
          </div>
          <div class="v-input">
            <label>驗證碼：</label>
            <el-input  v-model="loginInfo1.VerificationCode" style="width: 230px" placeholder="輸入驗證碼" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test4()" long>提交驗證test4</el-button>
          </div>
        </div>

        <div class="v5">
          <h2>實體校驗指定字段：密碼</h2>
          <div class="v-input">
            <label>用戶名：</label>
            <el-input  v-model="loginInfo2.userName" style="width: 230px" placeholder="輸入用戶名" />
          </div>
          <div class="v-input">
            <label>
              <i class="require">*</i>密 碼：
            </label>
            <el-input  v-model="loginInfo2.passWord" style="width: 230px" placeholder="輸入密碼" />
          </div>
          <div class="v-input">
            <label>驗證碼：</label>
            <el-input  v-model="loginInfo2.VerificationCode" style="width: 230px" placeholder="輸入驗證碼" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test5()" long>提交驗證test5</el-button>
          </div>
        </div>

        <div class="v6">
          <h2>實體字段：用戶名、密碼，普通參數</h2>
          <div class="v-input">
            <label>
              <i class="require">*</i>用戶名：
            </label>
            <el-input  v-model="loginInfo3.userName" style="width: 230px" placeholder="輸入用戶名" />
          </div>
          <div class="v-input">
            <label>
              <i class="require">*</i>密 碼：
            </label>
            <el-input  v-model="loginInfo3.passWord" style="width: 230px" placeholder="輸入密碼" />
          </div>
          <div class="v-input"  style="font-size:12px;">
            <i class="require">*</i>手機號：
            <el-input  v-model="phoneNo6" style="width: 230px" placeholder="輸入手機號" />
          </div>
          <div class="btn">
            <el-button type="success" size="mini" @click="test6()" long>提交驗證test6</el-button>
          </div>
        </div>
      </div>
    </div>
    <br />
    <br />
  </div>
</template>
<script>
export default {
  data() {
    return {
      userName1: "",
      phoneNo1: "",
      userName2: "",
      phoneNo2: "",
      local: "",
      qty: "",
      phoneNo6: "",
      loginInfo1: {
        userName: "",
        passWord: "",
        VerificationCode: ""
      },
      loginInfo2: {
        userName: "",
        passWord: "",
        VerificationCode: ""
      },
      loginInfo3: {
        userName: "",
        passWord: ""
      }
    };
  },
  methods: {
    test1() {
      let url =
        "validatorExample/test1?userName=" +
        this.userName1 +
        "&phoneNo=" +
        this.phoneNo1;
      this.http.post(url, {}, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    },
    test2() {
      let url =
        "validatorExample/test2?userName=" +
        this.userName1 +
        "&phoneNo=" +
        this.phoneNo1;
      this.http.post(url, {}, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    },
    test3() {
      let url =
        "validatorExample/test3?local=" + this.local + "&qty=" + this.qty;
      this.http.post(url, {}, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    },
    test4() {
      let url = "validatorExample/test4";
      this.http.post(url, this.loginInfo1, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    },
    test5() {
      let url = "validatorExample/test5";
      this.http.post(url, this.loginInfo2, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    },
    test6() {
      let url = "validatorExample/test6?phoneNo=" + this.phoneNo6;
      this.http.post(url, this.loginInfo3, "正在驗證參數").then(x => {
        this.$Message.info(x.message || x);
      });
    }
  }
};
</script>
<style lang="less" scoped>
.validator {
  text-align: center;
  padding: 10px 30px;
}
.general,
.object-model {
  display: inline-block;
  //display: flex;
  > div {
    // flex: 1;
    width: 300px;
    float: left;
    // padding: 0 30px;
    margin-right: 50px;
  }
  .v-input,
  .btn {
    margin-top: 20px;
  }
  .btn {
    padding-right: 4px;
    padding-left: 13px;
  }
  .require {
    color: red;
    position: relative;
    top: 2px;
    margin-right: 5px;
  }
  label {
    display: inline-block;
    width: 60px;
    text-align: right;
    font-size: 12px;
  }
}
.object-model {
  margin-top: 40px;
}
h2 {
  font-size: 16px;
  text-align: left;
  font-weight: 500;
  padding-left: 14px;
}
</style>