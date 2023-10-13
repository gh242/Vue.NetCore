
import { h, resolveComponent } from 'vue';
let extension = {
  components: {//動態擴充組件或組件路徑
    gridHeader: '',
    gridBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '12px' },
            'show-icon': true, type: 'warning',
            closable: false, title: '設置屬性後textInline，表格超出換行onInit(){ this.textInline = false}；代碼生成器編輯類型選擇editor即可默認生成編輯器:App_NewsEditor.js或App_News.js'
          }, ''),
        ]
      }
    },
    gridFooter: '',
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  }, //動態擴充組件或組件路徑
  buttons: {
    view: []
  },
  tableAction: "App_News",
  methods: { //事件擴展
    onInit() {
      //設置表的最大高度
      // this.tableMaxHeight = 400;
      //table内容超出後自動換行
      this.textInline = false;
      //添加預覽操作
      this.columns.forEach(item => {
        //設置url點擊事件
        if (item.field == 'DetailUrl') {
          item.title = "頁面預覽";
          item.formatter = (row) => { return '<a>預覽</a>' }
          item.click = (row, column, event) => {
            if (!row.DetailUrl || row.DetailUrl.indexOf('.html') == -1 || !this.base.isUrl(this.http.ipAddress + row.DetailUrl)) {
              return this.$Message.error("請先發佈静態頁面")
            }
            window.open(this.http.ipAddress + row.DetailUrl);
          }
        }
      })
    },
    onInited(){
      this.tableMaxHeight=this.height-50;
    }
  }
};
export default extension;