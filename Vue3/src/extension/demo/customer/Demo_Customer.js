/*****************************************************************************************
**  Author:jxx 2022
**  QQ:283591387
**完整文欄見：http://v2.volcore.xyz/document/api 【代碼生成頁面ViewGrid】
**常用示例見：http://v2.volcore.xyz/document/vueDev
**後台操作見：http://v2.volcore.xyz/document/netCoreDev
*****************************************************************************************/
//此js文件是用來自定義擴展業務代碼，可以擴展一些自定義頁面或者重新配置生成的代碼

let extension = {
  components: {
    //查詢介面擴展組件
    gridHeader: '',
    gridBody: '',
    gridFooter: '',
    //新建、編輯彈出框擴展體件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  tableAction: '', //指定某張表的權限(這裡填寫表名,默認不用填寫)
  buttons: { view: [], box: [], detail: [] }, //擴展的按鈕
  methods: {
     //下面這些方法可以保留也可以刪除
    onInit() {  //框架初始化配置前，
        //示例：在按鈕的最前面添加一個按鈕
        //   this.buttons.unshift({  //也可以用push或者splice方法來修改buttons數體
        //     name: '按鈕', //按鈕名稱
        //     icon: 'el-icon-document', //按鈕圖標vue2版本見iview文欄icon，vue3版本見element ui文欄icon(注意不是element puls文欄)
        //     type: 'primary', //按鈕樣式vue2版本見iview文欄button，vue3版本見element ui文欄button
        //     onClick: function () {
        //       this.$Message.success('點擊了按鈕');
        //     }
        //   });

        //示例：設置修改新建、編輯彈出框字段標籤的長度
        // this.boxOptions.labelWidth = 150;
        this.single = true;
    },
    onInited() {
      if(this.$route.path=="/Demo_Order"){
        this.height = 550 - 250;
        // 隱藏按鈕，只保留查詢
        this.buttons.forEach(itme=>{
          this.buttons.forEach(item=>{
            if(item.value != "Search"){
              item.hidden = true;
            }
          })
        })
      }
      
      //框架初始化配置後
      //如果要配置明細表,在此方法操作
      //this.detailOptions.columns.forEach(column=>{ });
    },
    searchBefore(param) {
      //介面查詢前,可以給param.wheres添加查詢參數
      //返回false，則不會執行查詢
      return true;
    },
    searchAfter(result) {
      //查詢後，result返回的查詢數據,可以在顯示到表格前處理表格的值
      return true;
    },
    addBefore(formData) {
      //新建保存前formData為對象，包括明細表，可以給給表單設置值，自己輸出看formData的值
      return true;
    },
    updateBefore(formData) {
      //編輯保存前formData為對象，包括明細表、刪除行的Id
      return true;
    },
    rowClick({ row, column, event }) {
      //查詢介面點擊行事件
      // this.$refs.table.$refs.table.toggleRowSelection(row); //單擊行時選中當前行;
    },
    modelOpenAfter(row) {
      //點擊編輯、新建按鈕彈出框後，可以在此處寫邏輯，如，從後台獲取數據
      //(1)判斷是編輯還是新建操作： this.currentAction=='Add';
      //(2)給彈出框設置默認值
      //(3)this.editFormFields.字段='xxx';
      //如果需要給下拉框設置默認值，請遍歷this.editFormOptions找到字段配置對應data屬性的key值
      //看不懂就把輸出看：console.log(this.editFormOptions)
    }
  }
};
export default extension;
