<template>
  <div class="tabs-group">
    <el-scrollbar style="height: 100%">
      <div class="tabs-btns">
        <button
          v-for="(btn, bIndex) in tabsButtons"
          @click="onClick(btn, bIndex)"
          :class="{ actived: bIndex == index }"
          :key="bIndex"
        >
          <i :class="btn.icon"></i>
          {{ btn.name }}
        </button>
        <span class="message"
          ><i class="el-icon-chat-line-round"
            >關於多頁籤：可以將生成的頁面或自定義頁面引用到此處,具體使用見:tabsTable.vue</i
          ></span
        >
      </div>
      <keep-alive>
        <component ref="com" v-bind:is="currentComponent"></component>
      </keep-alive>
    </el-scrollbar>
  </div>
</template>
<script>
import table1 from "../order/App_Appointment";
import table2 from "../appmanager/App_Transaction";
import table3 from "../formsMulti/multi3";
//注意必看，App_Appointment與4App_Transaction是自動生成的頁面，在頁籤引用時需要指定表名權限，否則普通帳號打開頁面會出現401
//具體指定權限方式見：App_Appointment.js與4App_Transaction.js中的tableAction屬性
export default {
  components: {
    table1,
    table2,
    table3,
  },
  data() {
    return {
      index: 0,
      currentComponent: "",
      open: false,
      tabsButtons: [],
    };
  },
  created() {
    //判斷權限(初始化頂tabs選項)
    this.tabsButtons = [
      {
        name: "頁籤1",
        icon: "el-icon-document",
        component: "table1", //上面引用的組件名
        table: "/App_Appointment", //這裡填寫實際表名用於權限判斷(與4菜單設置上的表path一樣)，沒有權限的不會顯示
      },
      {
        name: "頁籤2",
        icon: "el-icon-shopping-cart-full",
        component: "table2",
        table: "/App_Transaction",
      },
      {
        name: "頁籤3",
        icon: "el-icon-location-outline",
        component: "table3",
        table: "/table3",
      },
    ].filter((x) => {
      return this.$store.getters.getPermission(x.table);
    });

    //默認顯示第一個頁面
    if (this.tabsButtons.length) {
      this.currentComponent = this.tabsButtons[0].component;
    }
  },
  activated() {},
  methods: {
    //可以通過this.$refs.com.$refs.grid訪問生成頁面裡的數據
    onClick(item, index) {
      if (this.index == index) {
        return;
      }
      this.index = index;
      this.currentComponent = item.component;
      //頁籤切換時，可以調用此方法刷新生成頁面的數據
      this.$nextTick(() => {
        if (this.$refs.com.$refs.grid) {
          this.$refs.com.$refs.grid.search();
          //如果需要調用自定義方法，可以在表的js中定義xxx方法
          //this.$refs.com.$refs.grid.xxx();
        }
      });
      // togglePage.call(this, { item, index });
    },
  },
};
</script>
<style lang="less" scoped>
.tabs-group {
  position: absolute;
  height: 100%;
  width: 100%;
}
.tabs-btns {
  border-bottom: 1px solid #eee;
  button {
    cursor: pointer;
    border: none;
    outline: none;
    background: none;
    margin-right: 20px;
    position: relative;
    padding: 10px 10px 10px 15px;
  }
  button:after {
    content: "/";
    position: absolute;
    right: -16px;
    bottom: 10px;
    color: #b3b3b3;
  }
  button:last-child:after {
    content: "";
    display: none;
  }
  .actived {
    color: black;
    font-weight: bold;
  }
  .message {
    font-size: 12px;
    color: #a2a19e;
    margin-left: 30px;
  }
}
</style>