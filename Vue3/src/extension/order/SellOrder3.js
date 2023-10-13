
import SellOrder3GridFooter from "./SellOrder3/SellOrder3GridFooter.vue"
let extension = {
  components: {//動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '',//{ template: "<div>擴展組xx件</div>" },
    gridBody:'',
    gridFooter: SellOrder3GridFooter, //() => import("./SellOrder3/SellOrder3GridFooter.vue"),
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: "",
    modelBody: '',
    modelFooter: "", //() => import("./SellOrderComponents/ModelFooter.vue"),
  },
  text: "用法與4【主從一對一(2)】一致(見SellOrder3.js)",
  buttons: { //根據需要自行實現擴展按鈕
  },//擴展的按鈕
  methods: {
    mounted() {
    },
    //方式1,通過select選擇觸發顯示與4隱藏
    onInit() {
      //設置為單選，用於明細表加載數據時獲取主表選行的id
      this.single = true;
    },
    onInited() {
      this.height = this.height - 350;
    },

    rowClick({ row, column, event }) { //查詢介面table點擊行選中當前行
      this.$refs.table.$refs.table.toggleRowSelection(row);
      if (this.$refs.gridFooter && this.$refs.gridFooter.$refs.tableList) {
        //添加明細表的數據(觸發明細表加載數據，見SellOrder3GridFooter方法loadBefore)
        //load方法可參照voltable組件api文檔
        this.$refs.gridFooter.$refs.tableList.load()
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
    },
    modelOpenAfter() {
      //新建彈出框時，設置設置默認訂單類型
      if (this.currentAction == "Add") {
        this.editFormOptions.forEach(item => {
          item.forEach(x => {
            //如果是編輯帳號設置為只讀
            if (x.field == "OrderType") {
              //新建時默認選擇中第一個下拉框的值，如果要選中其他的值，請遍歷x.data獲取key
              /*注意:如果下拉框的數據源是自定義sql，並且key是數字，請將（x.data[0].key*1）轉換成數字*/
              this.editFormFields.OrderType = x.data[0].key;
              //可以指定其他input標籤的默認值
              this.editFormFields.TranNo="8888"
            }
          })

        })
      }
    }
  }
};
export default extension;
