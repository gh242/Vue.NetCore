import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import ElementPlus from 'element-plus';
// import 'element-plus/lib/theme-chalk/index.css';
import 'element-plus/dist/index.css'
import './assets/element-icon/icon.css'
import base from './uitils/common'
import http from './api/http'
// import 'dayjs/locale/zh-cn'
// import locale from 'element-plus/lib/locale/lang/zh-cn'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'



import permission from './api/permission'
import viewgird from './components/basic/ViewGrid';
const app = createApp(App);
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
app.config.globalProperties.base = base;
app.config.globalProperties.http = http;
app.config.globalProperties.$tabs = {};
app.config.globalProperties.permission = permission;
app.config.globalProperties.$global = {
    signalR: false, //是否開啟signalR
    menuSearch:true,//菜單是否啟用搜索功能
    table: {
      smallCell:false,//表格單元格大小
      useTag: true //table組件下拉框數據源的字段是否顯示背景顏色
    },
    audit: { //審核選項
        data: [
            { text: '通過', value: 1 },
            { text: '拒絕', value: 3 },
            { text: '駁回', value: 4 }
        ],
        status:[0,2] //審核中的數據
        // 待審核 = 0,
        // 審核通過 = 1,
        // 審核中 = 2,
        // 審核未通過 = 3,
        // 駁回 = 4
    }
}
//2023.03.13，
//修改見：volupload.vue，後台AliOSSController.cs，阿裡雲OSS配置.doc
window.oss = {
    ali: { //阿裡雲
        use: false,//使用阿裡雲上傳文件
        //阿裡缩略圖壓缩大小
        //.aliyuncs.com
        small: "?x-oss-process=image/resize,m_lfit,w_200"
    }
}
app.use(store)
    .use(ElementPlus, { size: 'default' })
    .use(router)
    .use(viewgird)
    .mount('#app');
app.config.globalProperties.$Message = app.config.globalProperties.$message;

