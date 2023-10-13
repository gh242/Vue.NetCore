import gridFooter from "./SellOrder2/SellOrder2GridFooter.vue"

//自定義從表選擇數據源頁面
import modelBody from "./SellOrder2/SellOrder2ModelBody.vue"
import { h, resolveComponent } from 'vue';
let extension = {
  components: {//動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '',//{ template: "<div>擴展組xx件</div>" },
    gridBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '12px' },
            'show-icon': true, type: 'success',
            closable: false, title: '介面由代碼生成器生成,只需幾行代碼即可實現點擊主表行顯示明細表數據(直接擴展gridFooter位置的代碼,具體代碼見此示例:SellOrder2.js)'
          }, ''),
        ]
      }
    },
    gridFooter: gridFooter,
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: "",
    modelBody: modelBody,//自定義從表選擇數據源頁面
    modelFooter: "",
  },
  text: "點擊主表行加載明細表數據,如果本地看不到此菜單,可以按照演示環境配置下此頁面菜單",
  buttons: { //根據需要自行實現擴展按鈕
  },//擴展的按鈕
  methods: {
    mounted() {
    },
    //方式1,通過select選擇觸發顯示與4隱藏
    onInit() {
      //點擊單元格編輯與4結束編輯(默認是點擊單元格編輯，鼠標離開結束編輯)
      this.detailOptions.clickEdit = true;
      //設置主表合計
      this.summary = true;
    },
    onInited() {
      //調整介面table高度
      this.height = this.height - 310;

      //明細表選擇數據源操作
      //獲取明細表備註列，給備註列添加選擇數據操作

      this.detailOptions.buttons.unshift({
        'name': '選擇數據', 
        icon: "el-icon-plus",
        onClick: () => {
          this.$refs.modelBody.open();
        }
      })
      // let _column = this.detailOptions.columns.find(x => { return x.field == "Remark" });
      // _column.title="(備註)點擊選擇數據"
      // //移除編輯操作
      // _column.edit = null;
      // //給備註列添加選擇數據操作
      // _column.render = (h, { row, column, index }) => {
      //   return h("div", { style: {} },
      //     [
      //       h("i", {
      //         class: ["el-icon-zoom-out"],
      //         style: {
      //           cursor: "pointer",
      //           "margin-right": "8px",
      //           color: "#409eff",
      //         },
      //         onClick: (e) => {
      //           e.stopPropagation();
      //           //彈出選擇數據源
      //           this.$refs.modelBody.open(row);
      //         },
      //       }),
      //     ], row.Remark)
      // };
    },
    // rowDbClick({ row, column, event }){//雙擊事件
    // },
    rowClick({ row, column, event }) { //查詢介面table點擊行選中當前行
      //取消其他行選中
      this.$refs.table.$refs.table.clearSelection();
      //設置選中當前行
      this.$refs.table.$refs.table.toggleRowSelection(row);
      if (this.$refs.gridFooter && this.$refs.gridFooter.$refs.tableList) {
        //load方法可參照voltable組件api文檔
        this.$refs.gridFooter.$refs.tableList.load({ value: row.Order_Id, sort: "CreateDate" })
      }
    },
    searchAfter(rows) {
      //頁面加載或者刷新數據後直接顯示第一行的明細
      if (rows.length) {
        // this.$nextTick(() => {
        this.$refs.gridFooter.$refs.tableList.load({ value: rows[0].Order_Id, sort: "CreateDate" })
        // })
      } else {
        //沒有數據時，清空明細數據
        this.$refs.gridFooter.$refs.tableList.rowData.splice(0)
      }
      return true;
    }
  }
};
export default extension;
