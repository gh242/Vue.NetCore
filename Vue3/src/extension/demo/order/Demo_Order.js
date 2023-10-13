/*****************************************************************************************
**  Author:jxx 2022
**  QQ:283591387
**完整文欄見：http://v2.volcore.xyz/document/api 【代碼生成頁面ViewGrid】
**常用示例見：http://v2.volcore.xyz/document/vueDev
**後台操作見：http://v2.volcore.xyz/document/netCoreDev
*****************************************************************************************/
//此js文件是用來自定義擴展業務代碼，可以擴展一些自定義頁面或者重新配置生成的代碼

import gridBody from './Demo_OrderGridBody'
import modelHeader from './orderModelHeader';
let extension = {
  components: {
    //查詢介面擴展組件
    gridHeader: '',
    gridBody: gridBody,
    gridFooter: '',
    //新建、編輯彈出框擴展體件
    modelHeader: modelHeader,
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
      
      //在第二個按鈕後添加一個新的按鈕
      this.buttons.splice(2, 0, { 
        name: "測試按鈕",
        icon: 'md-refresh',
        type: 'info',
        hidden: false, //是否隱藏按鈕(如果想要隱藏按鈕，在onInited方法中，遍歷buttons，設置hidden=true)
        onClick: function () { //觸發事件
          this.$Message.info("測試按鈕");
        }
      })
      this.buttons.push({
        name: "點擊了按鈕", //按鈕名稱
        icon: 'el-icon-document', //按鈕圖標，參照iview圖標
        type: 'success',//按鈕類型，可參照
        hidden: false, //是否隱藏按鈕(如果想要隱藏按鈕，在onInited方法中，遍歷buttons，設置hidden=true)
        onClick: function () { //觸發事件
          this.http.get("api/Demo_Order/test",{},true).then(x=>{
            this.$Message.success(x);
          })
          // this.$Message.success("點擊了按鈕");
        }
      })

      // this.maxBtnLength = 4; //畫面上最多幾個按鈕

      //添加額外標籤：
      // extra: {
        //style: "color:red", //樣式 
        //icon: "ios-search",  //顯示圖標 
        //text: "點擊可觸發事件", //顯示文本
        //click: item => {} //觸發事件 
      //}
      this.editFormOptions.forEach((option)=>{
        option.forEach((item)=>{
          if(item.field == "Customer"){
            item.extra = {
              style: "color:#3a8ee6;font-size: 13px;cursor:pointer;", //樣式 
              icon: "el-icon-zoom-out",  //顯示圖標 
              text: "選擇數據", //顯示文本
              click: item => {
                //refs
                this.$refs.modelHeader.open(); // 呼叫子組件的方法
                // this.$Message.error('點擊標籤觸發的事件');
              } //觸發事件 
            };
            // item.extra= {
            //   icon: "ios-search", //显示图标
            //   text: "点击可触发事件", //显示文本
            //   //触发事件
            //   click: (item) => {
            //     this.$Message.error("点击标签触发的事件");
            //   }
            // }
          }
        })
      });
    },
    onInited() {
      //框架初始化配置後
      //如果要配置明細表,在此方法操作
      //this.detailOptions.columns.forEach(column=>{ });
      //unshift、splice、push
      this.detailOptions.buttons.unshift({
        name: "選擇數據",//按鈕名稱
        icon: 'el-icon-plus',//按鈕圖標，参照iview圖標
        // type: 'success',//按鈕類型,可参照iview buttons設置此屬性
        hidden:false,//是否隱藏按鈕(如果想要隱藏按鈕，在onInited方法中遍歷buttons，設置hidden=true)
        onClick: function () { //觸發事件
            // this.refresh();
            // alert('選擇數據');
            this.$refs.modelHeader.openDetail();
        }
      })
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
