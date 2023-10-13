
import { h, resolveComponent } from 'vue';
let extension = {
  components: {
    //動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '', //{ template: "<div>擴展組xx件</div>" },
    gridBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '12px' },
            'show-icon': true, type: 'warning',
            closable: false, title: 'onInit中設置this.maxBtnLength = 7;指定按鈕顯示的個數。見代碼：App_TransactionAvgPrice.js'
          }, ''),
        ]
      }
    },
    gridFooter: "",
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: "",
    modelBody: "",
    modelFooter: ""
  },
  text:
    "還沒想好",
  buttons: [], //擴展的按鈕
  methods: {
    //事件擴展
    onInit() {
      //設置頁面上顯示的按鈕個數(不是必須的)
      this.maxBtnLength = 7;
      //this.boxButtons彈出框的按鈕,this.detailOptions.buttons，同樣适用上面上方法
      //2021.07.17更新volform.vue組件後才能使用 
      this.editFormOptions.forEach(x => {
        x.forEach(option => {
          if (option.field == "Date") {
            option.min = "2021-07-01 00:00";
            option.max = Date.now();//日期最大值"2021-07-20 00:00" 
          }

        })
      })
      this.columns.forEach(option => {
        if (option.field == 'AgeRange') {
          option.normal = true;
        }
      })
      //如果要設置查詢的日期範圍選擇同上 
      //this.searchFormOptions.forEach 

      //設置顯示所有查詢條件
      // this.setFiexdSearchForm(true);
    },
    onInited() {
      //設置表高度
      this.height = this.height - 50;
    },
    rowClick({ row, column, event }) { //查詢介面table點擊行時選中當前行
      this.$refs.table.$refs.table.toggleRowSelection(row);
    },
  }
};
export default extension;
