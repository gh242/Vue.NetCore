<template>
  <div id="vol-container" :class="['vol-theme-' + theme]">
    <div class="vol-aside" :style="{ width: menuWidth + 'px' }">
      <div class="header" :style="{ width: menuWidth - 1 + 'px' }">
        <!-- <img v-show="!isCollapse" v-bind:src="logo" /> -->
        <span style="color:white; font-size: 20px;">Psi 測試環境</span>
        <i @click="toggleLeft" class="el-icon-s-fold collapse-menu" />
      </div>
      <div class="vol-menu">
        <el-scrollbar style="height: 100%">
          <VolMenu
            :currentMenuId="currentMenuId"
            :on-select="onSelect"
            :enable="true"
            :open-select="false"
            :isCollapse="isCollapse"
            :list="menuOptions"
          ></VolMenu>
        </el-scrollbar>
      </div>
    </div>
    <div class="vol-container" :style="{ left: menuWidth - 1 + 'px' }">
      <div class="vol-header">
        <!-- <div class="project-name">Vol開發框架Vue3版本</div> -->
        <div class="project-name">Vol開發框架Vue3版本</div>
        <div class="header-text">
          <div class="h-link">
            <a
              href="javascript:void(0)"
              @click="to(item)"
              v-for="(item, index) in links.filter((c) => {
                return !c.icon;
              })"
              :key="index"
            >
              <span v-if="!item.icon"> {{ item.text }}</span>
              <i v-else :class="item.icon"></i>
            </a>
          </div>
        </div>
        <div class="header-info">
          <div class="h-link">
            <a
              href="javascript:void(0)"
              @click="to(item)"
              v-for="(item, index) in links.filter((c) => {
                return c.icon;
              })"
              :key="index"
            >
              <span v-if="!item.icon"> {{ item.text }}</span>
              <i v-else :class="item.icon"></i>
            </a>
          </div>
          <!--消息管理-->
          <div class="h-link" @click="messageModel = true">
            <a><i class="el-icon-message-solid"></i></a>
          </div>
          <div>
            <img class="user-header" :src="userImg" :onerror="errorImg" />
          </div>
          <div class="user">
            <span>{{ userName }}</span>
            <span id="index-date"></span>
          </div>
          <div class="settings">
            <i
              style="font-size: 20px"
              class="el-icon-s-tools"
              @click="drawer_model = true"
            />
          </div>
        </div>
      </div>
      <div class="vol-path">
        <el-tabs
          @tab-click="selectNav"
          @tab-remove="removeNav"
          @contextmenu.prevent="bindRightClickMenu(false)"
          type="border-card"
          class="header-navigation"
          v-model="selectId"
          :strtch="false"
        >
          <el-tab-pane
            v-for="(item, navIndex) in navigation"
            type="card"
            :name="navIndex + ''"
            :closable="navIndex > 0"
            :key="navIndex"
            :label="item.name"
          >
            <span style="display: none">{{ navIndex }}</span>
          </el-tab-pane>
        </el-tabs>
        <!-- 右鍵菜單 -->
        <div v-show="contextMenuVisible">
          <ul
            :style="{ left: menuLeft + 'px', top: menuTop + 'px' }"
            class="contextMenu"
          >
            <li v-show="visibleItem.all">
              <el-button link @click="closeTabs()">
                <i class="el-icon-close"></i>
                {{
                  navigation.length == 2 ? "關閉菜單" : "關閉所有"
                }}</el-button
              >
            </li>
            <li v-show="visibleItem.left">
              <el-button link @click="closeTabs('left')" 
                ><i class="el-icon-back"></i>關閉左邊</el-button
              >
            </li>
            <li v-show="visibleItem.right">
              <el-button link @click="closeTabs('right')" >
                <i class="el-icon-right"></i>關閉右邊</el-button
              >
            </li>
            <li v-show="visibleItem.other">
              <el-button
              link
                @click="closeTabs('other')"
             
                ><i class="el-icon-right"></i>關閉其他
              </el-button>
            </li>
          </ul>
        </div>
      </div>
      <div class="vol-main" id="vol-main">
        <el-scrollbar style="height: 100%" v-if="permissionInited">
          <loading v-show="$store.getters.isLoading()"></loading>
          <router-view v-slot="{ Component }">
            <keep-alive>
              <component
                :is="Component"
                :key="$route.name"
                v-if="!$route.meta ||($route.meta && !$route.meta.hasOwnProperty('keepAlive'))"
              />
            </keep-alive>
            <component
              :is="Component"
              :key="$route.name"
              v-if="$route.meta && $route.meta.hasOwnProperty('keepAlive')"
            />
          </router-view>
        </el-scrollbar>
      </div>
    </div>
    <el-drawer
      title="選擇主題"
      v-model="drawer_model"
      direction="rtl"
      destroy-on-close
    >
      <div class="theme-selector">
        <div
          @click="changeTheme(item.name)"
          class="item"
          v-for="(item, index) in theme_color"
          :key="index"
          :style="{ background: item.color }"
        >
          <div
            v-show="item.leftColor"
            :style="{ background: item.leftColor }"
            style="height: 100%; width: 20px"
            class="t-left"
          ></div>
          <div class="t-right"></div>
        </div>
      </div>
    </el-drawer>

    <el-drawer
      title="消息列表"
      v-model="messageModel"
      direction="rtl"
      destroy-on-close
    >
      <Message :list="messageList"></Message>
    </el-drawer>
  </div>
</template>
<style lang="less" scoped>
@import "./index/index.less";
</style>
<script>
import loading from "@/components/basic/RouterLoading";
import VolMenu from "@/components/basic/VolElementMenu.vue";
import Message from "./index/Message.vue";
import MessageConfig from "./index/MessageConfig.js";
var imgUrl = require("@/assets/imgs/logo.png");
var $this;
var $interval;
var $indexDate;
import {
  defineComponent,
  reactive,
  ref,
  watch,
  onMounted,
  getCurrentInstance,
} from "vue";
import { useRouter, useRoute } from "vue-router";
import store from "../store/index";
import http from "@/../src/api/http.js";
export default defineComponent({
  components: {
    VolMenu,
    loading,
    Message,
  },

  data() {
    return {
      allTabs: true,
      leftTabs: true,
      rightTabs: true,
      otherTabs: true,
      menuLeft: 0,
      menuTop: 0,
      //  contextMenuVisible: false, // 右鍵關閉顯/隱
    };
  },
  setup(props, context) {
    // 獲取全局屬性和方法
    const { proxy } = getCurrentInstance();

    // 菜單導航默認寬度
    const menuWidth = ref(200);
    const contextMenuVisible = ref(false);
    const isCollapse = ref(false);
    const drawer_model = ref(false);
    const messageModel = ref(false);
    const theme_color = ref([
      { name: "blue", color: "rgb(45, 140, 240)" },
      { name: "blue2", color: "rgb(45, 140, 240)", leftColor: "#0068d6" },
      { name: "red", color: "rgb(237, 64, 20)" },
      { name: "red2", color: "rgb(237, 64, 20)", leftColor: "#a90000" },
      { name: "dark", color: "#272929" },
      { name: "orange", color: "#ff9900" },
      { name: "orange2", color: "#ff9900", leftColor: "rgb(232 141 5)" },
      { name: "green", color: "rgb(25, 190, 107)" },
      { name: "green2", color: "rgb(25, 190, 107)", leftColor: "#019e4f" },
      { name: "white", color: "#fff" },
    ]);
    const links = ref([
      {
        text: "框架視頻",
        path: "https://www.cctalk.com/m/group/90268531",
        id: -3,
      },
      { text: "大屏數據", path: "/bigdata", id: -3 },
      {
        text: "框架文檔",
        path: "http://v2.volcore.xyz/document/guide",
        id: -2,
      },   {
        text: "框架企業版",
        path: "http://pro.volcore.xyz/",
        id: 10,
      },
      { text: "個人中心", path: "/UserInfo", id: -1, icon: "el-icon-s-custom" },
      {
        text: "安全退出",
        path: "/login",
        id: -4,
        icon: "el-icon-switch-button",
      },
    ]);
    const errorImg = ref(
      'this.src="' + require("@/assets/imgs/error-img.png") + '"'
    );
    const selectId = ref("1");
    // 【首頁】標籤序號(當前右鍵選中的菜單)
    const selectMenuIndex = ref("0");
    //2022.05.29增加tab選項與4菜單聯動功能
    const currentMenuId = ref(0);
    const userName = ref("--");
    const userInfo = ref({});
    const visibleItem = reactive({
      left: false,
      right: false,
      all: false,
      other: false,
    });
    const userImg = ref("");
    const navigation = reactive([
      { orderNo: "0", id: "1", name: "首頁", path: "/home" },
    ]);
    const logo = ref(imgUrl);
    const theme = ref("blue2");
    const menuOptions = ref([]);
    const permissionInited = ref(false);
    const messageList = reactive([]);
    let _config = getCurrentInstance().appContext.config.globalProperties;
    let router = useRouter();
    const toggleLeft = () => {
      isCollapse.value = !isCollapse.value;
      menuWidth.value = isCollapse.value ? 63 : 200;
    };
    //2021.08.28開放手動折疊菜單方法
    _config.menu = {
      show() {
        toggleLeft();
      },
      hide() {
        toggleLeft();
      },
    };
    const changeTheme = (name) => {
      if (theme.value != name) {
        theme.value = name;
      }
      localStorage.setItem("vol3_theme", name);
    };
    const to = (item) => {
      /* 2020.07.31增加手動打開tabs*/
      if (item.path == "#") {
        window.open("https://github.com/cq-panda/Vue.NetCore");
        return;
      }
      if (item.path.indexOf("http") != -1) {
        window.open(item.path);
        return;
      }
      if (typeof item == "string" || item.path == "/login") {
        if (item == "/login" || item.path == "/login") {
          store.commit("clearUserInfo", "");
          window.location.href = "/";
          return;
        }
        router.push({ path: item });
        return;
      }
      if (item.path == "#") return;
      open(item);
    };
    const open = (item, useRoute) => {
      /* 2020.07.31增加手動打開tabs*/
      let _index = navigation.findIndex((x) => {
        return x.path == item.path;
      });
      if (_index == -1) {
        navigation.push({
          //  orderNo: String(navigation.length),// 序號
          id: item.id + "",
          name: item.name || item.text || "無標題",
          path: item.path,
          query: item.query, //2021.03.20修復自定義二次打開$tabs時參數丢失的問題
        });
        //新打開的tab移至最後一個選項
        selectId.value = navigation.length - 1 + "";
      } else {
        selectId.value = _index + "";
      }
      if (useRoute === undefined) {
        //非標準菜單，記錄最後一次跳轉的頁面，用於刷新
        setItem(item);
        router.push(item);
        // this.$router.push(item);
      }
      currentMenuId.value = item.id * 1;
      // tab菜單綁定右鍵事件
      proxy.$nextTick(function (e) {
        proxy.bindRightClickMenu(true);
      });
    };
    const close = (path) => {
      /* 2020.07.31增加手動打開tabs*/
      let index = navigation.findIndex((x) => {
        return x.path == path;
      });
      if (index == -1) {
        return _config.$Message.error("未找到菜單");
      }
      removeNav(index);
    };
    const setItem = (item) => {
      /* 2020.07.31增加手動打開tabs*/
      localStorage.setItem(
        window.location.origin + "_tabs",
        JSON.stringify(item)
      );
    };
    const getItem = () => {
      /* 2020.07.31增加手動打開tabs*/
      let nav = localStorage.getItem(window.location.origin + "_tabs");
      return nav ? JSON.parse(nav) : null;
    };
    const selectNav = (item) => {
      //升級element正式版修改
      selectId.value = item.props.name;
      let _path = navigation[item.index].path;
      currentMenuId.value = (
        menuOptions.value.find((c) => {
          return c.path == _path;
        }) || { id: 0 }
      ).id;

      router.push({
        path: navigation[item.index].path,
        query: navigation[item.index].query,
      });
    };

    const removeNav = (_index) => {
      return new Promise(() => {
        //關閉的當前項,跳轉到前一個頁面
        if (selectId.value == _index + "") {
          console.log(navigation[_index - 1]);
          setItem(navigation[_index - 1]);
          router.push({
            path: navigation[_index - 1].path,
            //2022.06.27修復tabs二次切換後參數丢失的問題
            query: navigation[_index - 1].query,
          });
          navigation.splice(_index, 1);
          selectId.value = selectId.value - 1 + "";
          return;
        }
        if (_index < selectId.value) {
          selectId.value = selectId.value - 1 + "";
        }
        navigation.splice(_index, 1);
        currentMenuId.value = (
          menuOptions.value.find((c) => {
            return c.path == navigation[selectId.value * 1].path;
          }) || { id: 0 }
        ).id;
      });
    };

    const getSelectMenuName = (id) => {
      return menuOptions.value.find(function (x) {
        return x.id == id;
      });
    };
    const onSelect = (treeId) => {
      /* 2020.07.31增加手動打開tabs*/
      var item = getSelectMenuName(treeId);
      open(item, false);
    };

    /**
     * 顯示右鍵菜單
     * @param {*} e 事件對象
     */
    const openTabsMenu = function (e) {
      e.preventDefault(); // 防止默認菜單彈出
      let tabId = e.target.id.split("-")[1] * 1;

      //記錄當前選中的菜單index
      selectMenuIndex.value =
        document.getElementById("pane-" + tabId).children[0].textContent * 1;
      //只有首頁時不顯示
      if (navigation.length == 1) {
        return;
      }

      //首頁設置顯示關閉右邊菜單
      if (!selectMenuIndex.value) {
        visibleItem.all = false;
        visibleItem.right = true;
        visibleItem.left = false;
        visibleItem.other = false;
      } else {
        visibleItem.all = true;
        //不是最後一個顯示關閉右邊菜單
        visibleItem.right = selectMenuIndex.value != navigation.length - 1;
        //只有兩個菜單時不顯示關閉左邊
        visibleItem.left = navigation.length != 2;
        //只有兩個菜單時不顯示關閉其他
        visibleItem.other = navigation.length != 2;
      }
      contextMenuVisible.value = true;
      // 設置右鍵菜單顯示的位置
      proxy.menuLeft =
        e.target.getBoundingClientRect().left - (isCollapse.value ? 63 : 198); //-e.target.clientWidth
      proxy.menuTop = 36;
    };

    /**
     * 關閉右鍵菜單
     */
    const closeTabsMenu = () => {
      contextMenuVisible.value = false;
    };
    const toHome = () => {
      open({
        text: navigation[0].name,
        path: navigation[0].path,
      });
    };
    /**
     * 關閉其它標籤頁
     * @param {*} par 關閉類型(left,right,other)
     */
    const closeTabs = (value) => {
      let _menuId = navigation[selectId.value * 1].id;
      let currnetIndex = selectId.value * 1; // navigation.findIndex(c => { return c.id == selectId.value });
      switch (value) {
        case "left": {
          // 刪除左側tab標籤
          navigation.splice(1, currnetIndex - 1); // 刪除左側tab標籤
          break;
        }
        case "right": {
          // 刪除右側tab標籤
          if (selectMenuIndex.value == 0) {
            navigation.splice(currnetIndex); // 刪除右側tab標籤
            toHome();
          } else {
            navigation.splice(currnetIndex + 1); // 刪除右側tab標籤
            if (selectMenuIndex.value < currnetIndex) {
              navigation.splice(
                selectMenuIndex.value,
                currnetIndex - selectMenuIndex.value
              );
            }
          }
          break;
        }
        case "other": {
          // 刪除其他所有tab標籤
          navigation.splice(currnetIndex + 1); // 刪除右側tab標籤(這裡必須按照右→左順序刪除)
          navigation.splice(1, currnetIndex - 1); // 刪除左側tab標籤
          break;
        }
        default: {
          //關閉所有
          navigation.splice(1, navigation.length);
          toHome();
          break;
        }
      }
      selectId.value =
        navigation.findIndex((c) => {
          return c.id == _menuId;
        }) + "";
      closeTabsMenu();
    };

    watch(
      () => contextMenuVisible.value,
      (newVal, oldVal) => {
        // 監視
        if (newVal) {
          document.body.addEventListener("click", closeTabsMenu);
        } else {
          document.body.removeEventListener("click", closeTabsMenu);
        }
      }
    );

    /**
     * 系統創建開始
     */
    const created = () => {
      let _theme = localStorage.getItem("vol3_theme");
      if (_theme) {
        theme.value = _theme;
      }

      let _userInfo = store.getters.getUserInfo();
      if (_userInfo) {
        userName.value = _userInfo.userName;
        if (_userInfo.img) {
          userImg.value = _config.base.getImgSrc(_userInfo.img, http.ipAddress);
        }
      }
      Object.assign(_config.$tabs, { open: open, close: close });

      http.get("api/menu/getTreeMenu", {}, true).then((data) => {
        data.push({ id: "1", name: "首頁", url: "/home" }); // 為了獲取選中id使用
        data.forEach((d) => {
          d.path = (d.url || "").replace("/Manager", "");
          d.to = (d.url || "").replace("/Manager", "");
          if (!d.icon || d.icon.substring(0, 3) != "el-") {
            d.icon = "el-icon-menu";
          }
        });
        store.dispatch("setPermission", data);
        menuOptions.value = data;
        permissionInited.value = true;

        //開啟消息推送（main.js中設置是否開啟signalR）2022.05.05
        if (_config.$global.signalR) {
          MessageConfig(http, (result) => {
            messageList.unshift(result);
            //    console.log(result)
          });
        }

        //當前刷新是不是首頁
        if (router.currentRoute.value.path != navigation[0].path) {
          //查找系統菜單
          let item = menuOptions.value.find((x) => {
            return x.path == router.currentRoute.value.path; //this.$route.path;
          });
          if (item) return onSelect(item.id);
          //查找頂部快捷連接
          item = links.value.find((x) => {
            return x.path == router.currentRoute.value.path; //this.$route.path;
          });
          //查找最後一次跳轉的頁面
          if (!item) {
            item = getItem();
          }
          if (item) {
            return open(item, false);
          }
        }
        selectId.value = "1";
      });
    };
    created();
    return {
      menuWidth,
      isCollapse,
      drawer_model,
      theme_color,
      errorImg,
      userInfo,
      userName,
      userImg,
      selectId,
      selectMenuIndex,
      navigation,
      links,
      onSelect,
      openTabsMenu,
      selectNav,
      getSelectMenuName,
      removeNav,
      logo,
      theme,
      menuOptions,
      permissionInited,
      changeTheme,
      to,
      toggleLeft,
      messageModel,
      messageList,
      contextMenuVisible,
      visibleItem,
      closeTabsMenu,
      closeTabs,
      currentMenuId,
    };
  },
  /**
   * 掛載钩子函數
   */
  mounted() {
    let _date = showTime();
    $indexDate = document.getElementById("index-date");
    $indexDate.innerText = _date;
    $interval = setInterval(function () {
      $indexDate.innerText = showTime();
    }, 1000);

    this.bindRightClickMenu(true);
  },

  methods: {
    /**
     * 綁定右鍵事件
     * @param {*} enable 是否啟用右鍵事件[true:啟用;false:禁用;]
     * @param {*} $event 事件
     */
    bindRightClickMenu(enable) {
      if (!enable) return;
      let that = this;
      // 使用原生js 為單個dom綁定鼠標右擊事件
      that.$nextTick(() => {
        let tab_top_dom = Object.assign(
          [],
          document.getElementsByClassName("el-tabs__item is-top")
        );
        tab_top_dom.forEach((item, index) => {
          item.oncontextmenu = that.openTabsMenu;
        });
      });
    },
  },

  /**
   * 銷毀钩子函數
   */
  destroyed() {
    $this = null;
    clearInterval($interval);
  },
});
const week = new Array(
  "星期一",
  "星期二",
  "星期三",
  "星期四",
  "星期五",
  "星期六",
  "星期日"
);
function showTime() {
  let date = new Date();
  let year = date.getFullYear();
  let month = date.getMonth() + 1;
  let day = date.getDate();
  let hour = date.getHours();
  let minutes = date.getMinutes();
  let second = date.getSeconds();

  return (
    year +
    "." +
    (month < 10 ? "0" + month : month) +
    "." +
    (day < 10 ? "0" + day : day) + //202.08.08修復日期天數小於10時添加0
    "" +
    " " +
    (hour < 10 ? "0" + hour : hour) +
    ":" +
    (minutes < 10 ? "0" + minutes : minutes) +
    ":" +
    (second < 10 ? "0" + second : second) +
    " " + //2020.08.30修復首頁日期星期天不顯示的問題
    (week[date.getDay() - 1] || week[6])
  );
}
</script>

<style lang="less" scoped>
.vol-container .vol-path ::v-deep(.el-tabs__content) {
  padding: 0;
}

.contextMenu {
  width: 120px;
  margin: 0;
  border: 1px solid #eaeaea;
  background: #fff;
  z-index: 30000;
  position: absolute;
  list-style-type: none;
  padding: 5px 0;
  border-radius: 4px;
  font-size: 14px;
  color: #333;
  box-shadow: 2px 2px 3px 0 rgb(182 182 182 / 20%);
  i,button{
    font-size: 14px !important;
  }
}

.contextMenu li {
  margin: 0;
  padding: 5px 17px;
}

.contextMenu li:hover {
  background: #fafafa;
  cursor: pointer;
}

.contextMenu li button {
  color: #626060;
  font-size: 14px;
  letter-spacing: 1px;
}

.el-tabs.el-tabs--top.el-tabs--border-card.header-navigation
  > .el-tabs__header
  .el-tabs__item:last-child,
.el-tabs--top.el-tabs--border-card.header-navigation
  > .el-tabs__header
  .el-tabs__item:nth-child(2) {
  padding: 0;
}

.header-navigation ::v-deep(.el-tabs__item.is-top) {
  padding: 0 15px;
}
</style>
<style>
.horizontal-collapse-transition {
  transition: 0s width ease-in-out, 0s padding-left ease-in-out,
    0s padding-right ease-in-out;
}
</style>
