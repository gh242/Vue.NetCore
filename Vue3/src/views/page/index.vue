<template>
  <div style="margin:50px;">
    <div style="margin-bottom:30px;">
      1、當前用戶名：{{ userName }}, 當前時間：{{ date }}
    </div>
    <el-row>
      <el-button @click="getUserName">獲取用戶名</el-button>
      <el-button type="primary">主要按鈕</el-button>
      <el-button type="success">成功按鈕</el-button>
      <el-button type="info">信息按鈕</el-button>
      <el-button type="warning">警告按鈕</el-button>
      <el-button type="danger">危險按鈕</el-button>
    </el-row>
  </div>


  
  <div style="margin: 20px;">
    <!-- v-model -->
    <div class="item">
      2、v-model：
      <el-input style="width: 200px" v-model="inputVal"></el-input>
      <span>{{ inputVal }}</span>
    </div>

    <!-- v-show -->
    <div class="item">
      3、
      <el-button size="small" type="primary" @click="showVal = !showVal">
        v-show
      </el-button>
      <span v-show="showVal">這裡是v-show</span>
    </div>

    <!-- v-if -->
    <div class="item">
      4、
      <el-button size="small" style="width: 70px" type="warning" @click="ifVal = !ifVal">
        v-if
      </el-button>
      <span v-if="ifVal">這裡是v-if</span>
    </div>

    <!-- v-for class style -->
    <div class="item">
      <div>5. v-for循環、class樣式綁定、style綁定、click綁定</div>
      <ul>
        <!-- :style="{ color: item.key = 'hs' ? '#44fb07' : ''}" -->
        <li v-for="(item, index) in forData" @click="itemClick(item)" :key="index" :class="{ red: item.key == 'tp' }"
          :style="{ color: item.key == 'hs' ? '#aaa' : '' }">
          {{ item.name }}
        </li>
      </ul>
    </div>

    <!-- 子父組件傳參 -->
    <div class="item">
      <div style="padding-bottom: 15px">
        6.父子組件傳參props、$emit、$refs:
      </div>
      <!-- <div style="color: gray">
        這裡是當前父組件index.vue的內容
        <el-button size="small" type="primary" @click="refClick">
          通過$refs訪問childPage.vue中的內容
        </el-button>
      </div> -->
      <!-- 顯示引用的子組件 -->
      <!-- emit-test子組件內部this.$emit來訪問父組件 -->
      <!-- <child-page ref="名字隨便寫" :FatherMsg="text" @emit-test="emitTest">
        這裡是父組件做的slot內容
      </child-page> -->
    </div>
  </div>

  <!-- <div style="border:1px solid red;padding:2rem;width:400px;margin:0 auto;"> -->
  <div>props:</div>
  <div style="border:1px solid red;padding:2rem;width:500px;">
    <h3>我是父组件 index.vue</h3>
    <div>子組件向父組件傳遞的值:{{ChildMsg}}</div>
    <child-page :FatherMsg="data" @ListenChild="ListenChild"></child-page>
  </div>

  <div>通過 ref 直接調用子組件的方法：</div>
  <!-- <div style="border: 1px solid red; width: 200px; padding: 10px; margin: 0 auto"> -->
  <div style="border: 1px solid red; width: 500px; padding: 2rem;">
    <div>我是父組件 index.vue</div>
    <Button @click="handleClick">點擊調用子組件方法</Button>
    <div style="margin-top: 10px;">
        <child-page_ref ref="child" />
    </div>
  </div>

  <div style="margin-bottom: 50px;">通過組件的 $emit、$on 方法：</div>
  <!-- <div style="border: 1px solid red; width: 200px; padding: 10px;">
    我是父组件
    <Button @click="handle_emitClick">點擊調用子組件方法</Button>
    <child_page_emit_on ref="child" />
  </div> -->

</template>
 
<script>
// import { defineComponent, ref, onMounted, getCurrentInstance } from 'vue';
//導入子組件
import childPage from './ChildPage.vue';
import childPage_ref from './ChildPage_ref.vue';
// import childPage_emit_on from './ChildPage_emit_on.vue';
export default {
  components: {
    "child-page": childPage,
    "child-page_ref": childPage_ref,
    // "child_page_emit_on": childPage_emit_on
  },
  data() {
    return {
      showVal: true,
      ifVal: true,
      text: "這裡是父組件通過props傳入的參數",
      inputVal: "123456",
      userName: '',
      date: '',
      forData: [
        { name: '台北市', key: "tp" },
        { name: '新北市', key: "nt" },
        { name: '桃園市', key: "tu" },
        { name: '新竹市', key: "hs" },
      ],
      data: 'I am your father',
      ChildMsg: '',
    };
  },
  // setup(){ //vue3語法
  //   return{}
  // },
  methods: {
    emitTest(val) {
      //childPage.vue中的按鈕點擊過來的
      this.$message.success("子組件$emit調用父組件方法");
    },
    refClick() {
      //透過refs調用子組件ChildPage.vue
      this.$refs.名字隨便寫.childTest();
    },
    itemClick(item) {
      // v-for點擊事件
      this.$message.success(item.name);
    },
    getUserName() {
      this.http.get("api/user/getUserName", {}, true).then(result => {
        this.userName = result.userName;
        this.date = result.date;
        // if(isBtn){
        this.$message.success("刷新成功");
        // }
      })
    },
    ListenChild(data) {
      console.log("子組件傳遞過來的值：" , data);
      this.ChildMsg = data;
    },
    handleClick() {
      this.$refs.child.childFun();
    },
    // handle_emitClick() {
    //   //子组件$on中的名字
    //   this.$refs.child.$emit("childFun_emit")    
    // },
  },
  created() {
    //頁面加載事件
    this.getUserName();
  },
  mounted() {
    //頁面加載完成事件
    console.log('mounted');
  },
  activated() {
    //頁面活動事件，默認所有頁面都開啟了緩存，如果標用了緩存，此方法不會生效，
    // (可參照前端開發文檔上的「禁用緩存」)
  }
}
</script>

<style scoped>
.red {
  color: red;
}

.item {
  margin-bottom: 10px;
}
</style>