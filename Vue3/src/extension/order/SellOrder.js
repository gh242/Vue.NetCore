//1、此文件中的代碼都是對基礎業務進行擴展，也只能在此處編寫代碼，其他地方編寫的代碼會被代碼生成器生成的代碼覆蓋
//2、此文件中的方法會與4serviceFilter.js進行合併，終終執行的就是此文件中的方法
//3、components為擴展組件，可自定義頁面的頭部、body及尾部的額外顯示的内容，分別對應的組件為:
//gridHeader、gridBody、gridFooter、modelHeader、modelBody、modelFooter共6個組件位置，如果不需要擴展組件，請忽略此處
//4、text為介面顯示額外描述信息
//5、buttons->view/box/detail分別為查詢介面、彈出框、彈出框明細表額外擴展的按鈕
//6、methods為整個頁面所有方法的業務代碼擴展，如果需要在介面上的操作前、後編寫業務，請覆蓋此方法
//7、methods方法中可獲取任意vue對象、方法、屬性,可查看ViewGridConfig路徑下的js文件及ViewGrid.vue的data屬性
//如手動刷新表數據:this.search();
//獲取表的列配置信息:this.columns  明細表表的配置信息this.detail.columns
//8、獲取子組件gridHeader、gridBody、gridFooter、modelHeader、modelBody、modelFooter則使用
//this.$refs.gridHeader獲取gridHeader為自己擴展的對象
//9、在子組件gridHeader、gridBody、gridFooter、modelHeader、modelBody、modelFooter的方法中調用並獲得父組件
// this.$emit("parentCall", $vue => {}) //$vue為父組件對象，具體使用參考order->GridHeader.vue文件

//10、子父件傳參除8、9方式，另可使用已封裝的vuex,使用方式:this.$store.getters.data().xxx = this;使用實例：App_News.js

//此處的下面的屬性與4方法都是對基礎業務代碼的擴展，如果不需要擴展則忽略對應的方法也不要寫在下面
//serviceFilter.js
//所的文件都最終會合併到ViewGrid.vue文件中

import { h, resolveComponent } from 'vue';
import gridHeader from "./SellOrderComponents/GridHeader.vue"
import gridFooter from "./SellOrderComponents/GridFooter.vue"
import modelFooter from "./SellOrderComponents/ModelFooter.vue"
import modelBody from "./SellOrderComponents/ModelBody.vue"
let extension = {
  components: {//動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: gridHeader,
    gridBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '12px' },
            'show-icon': true, type: 'warning',
            closable: false, title: '一對一全部由代碼生成不需要寫任何代碼,點擊新建或編輯查看明細表,可根據需要實現擴展gridHeader、gridBody、gridFooter、modelHeader、modelBody、modelFooter任意位置'
          }, ''),
        ]
      }
    },
    gridFooter: gridFooter,
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: "",
    modelBody: modelBody,
    modelFooter: modelFooter,
  },
  text: "示例覆蓋全部可擴展方法,前台擴展文件SellOrder.js，後台Partial->SellOrdeService.cs",
  //this.filterPermission('表名','權限值，如:Add')來判斷頁面上是否有某個按鈕的權限
  buttons: { //根據需要自行實現擴展按鈕
    //注：沒有編輯或新建權限的情況下，是不會顯示此處添加的擴展按鈕，如果仍需要顯示此處的按鈕，可以把按鈕在methods的onInited方法中添加,如：this.boxButtons.push(...)
    view: [//ViewGrid查詢介面按鈕
      
    ],
    box: //新建、編輯彈出框按鈕
      [//ViewGrid查詢介面按鈕
        {
          name: "點我1",
          icon: 'el-icon-delete',
          value: 'Edit',
          class: '',
          type: 'success',
          index: 1,//顯示的位置
          onClick: function () {
            this.$message.error("擴展的明細Box按鈕,可設置index值指定顯示位置,可使用this.$refs拿到包括自定義擴展的所有組件");
          }
        }],
    detail: //新建、編輯彈出框明細表table表按鈕
      [//ViewGrid查詢介面按鈕
        {
          name: "點我2",
          icon: 'el-icon-delete',
          value: 'Edit',
          class: '',
          type: 'success',
          index: 1,//顯示的位置
          onClick: function () {
            this.$message.error("擴展的明細table按鈕,可設置index值指定顯示位置");
          }
        }]
  },//擴展的按鈕
  methods: {
    //擴展方法使用示例,根據需要實行下面的方法
    getServiceDate() {
      this.http.post("/api/SellOrder/getServiceDate", {}, '正在調用後台數據').then(date => {
        this.$message.error("從後台獲取的服務器時間是：" + date);
      })
    },
    mounted() {
      //this.$message.success('mounted方法執行時,你可以此處編寫業務逻輯');
    },
    //方式1,通過select選擇觸發顯示與4隱藏
    onInit() {

     this.buttons.splice(1,0,...[
      {
        name: "點我彈出框",
        icon: 'md-create',
        value: 'Edit',
        class: '',
        type: 'primary',
        index: 1,//顯示的位置
        onClick: function () { //擴展按鈕執行事件
          //this可以獲取所有屬性，包括this.$refs.gridHeader/gridBody等獲取擴展組件對象
          // this.$message("測試擴展按鈕");
          this.$refs.gridHeader.open1();
        }
      }, {
        name: "調用後台",
        icon: 'md-create',
        value: 'Edit',
        class: '',
        type: 'primary',
        index: 1,//顯示的位置
        onClick: function () { //擴展按鈕執行事件
          this.getServiceDate();
        }
      }
     ])

      //獲取訂單類型select配置，當前訂單類型select改變值時，動態設置Remark,SellNo兩個字段是否顯示 
      var orderTypeOption = this.getFormOption("OrderType");
      orderTypeOption.onChange = (val) => {
        //當訂單類型select改變值時，如果選中的是發貨(對應字典編號為2)，emark,SellNo隱藏，否則顯示出來
        this.getFormOption("Remark").hidden = val == "2"
       // var sellNoOption = this.getFormOption("SellNo").hidden = val == "2"
      }

      //點擊單元格編輯與4結束編輯(默認是點擊單元格編輯，鼠標離開結束編輯)
      this.detailOptions.clickEdit = true;

  
      //設置主表合計
      this.summary = true;
      //設置明細表合計
      this.detailOptions.summary = true;
      //表格設置為單選
      // this.single=true;
      // this.detailOptions.single=true;
      //設置編輯表單數量字段的最小與4最大值
      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          //設置輸入的數量的最小值與4最大值(默認是1)
          if (item.field == "Qty") {
            item.min = 10;
            item.max =100000;
          }
        });
      })

      //動態修改table並給列添加事件
      this.columns.forEach(x => {
        if (x.field == "Qty") {
          x.formatter = (row) => {
            return '<a style="color:#2196F3;">' + row.Qty + "(彈出框)" + '</a>'
          }
          x.click = (row, column, event) => {
            this.$refs.gridHeader.open2(row)
          }
        }
      })

      //動態設置查詢介面table高度
      this.tableHeight = 300;

    },
    onInited() {
      this.detailOptions.columnIndex=false;
      this.detailOptions.ck=true;
      //設置主表求字段，後台需要實現SummaryExpress方法
      this.columns.forEach(x => {
        if (x.field == 'Qty'||x.field=='Weight') {
          x.summary = true;
        }
      })
      //設置明細表高度
      this.detailOptions.height = 220;
      this.boxOptions.height = document.body.clientHeight * 0.90;
      //設置明細表求字和段，後台需要實現GetDetailSummary方法
      this.detailOptions.columns.forEach(x => {
        if (x.field == 'Weight' || x.field == 'Qty') {
          x.summary = true;
        }
      })
      //   this.$message.success('create方法執行後', desc: '你可以SellOrder.js中編寫業務逻輯,其他方法同樣适用' });
    },
    searchBefore(param) { //查詢ViewGird表數據前,param查詢參數
      //你可以指定param查詢的參數，處如果返回false，則不會執行查詢
      // this.$message.success(this.table.cnName + ',查詢前' });
      // console.log("擴展的"this.detailOptions.cnName + '觸發loadDetailTableBefore');
      return true;
    },
    searchAfter(result) { //查詢ViewGird表數據後param查詢參數,result回返查詢的結果
      // this.$notify({
      //   title: '查詢結果',
      //   message: '返回的對象：' + JSON.stringify(result),
      //   type: 'success'
      // });
      return true;
    },
    searchDetailBefore(param) {//查詢從表表數據前,param查詢參數
      //  this.$message.success(this.detailOptions.cnName + '查詢前' });
      return true;
    },
    searchDetailAfter(data) {//查詢從表後param查詢參數,result回返查詢的結果
      // this.$notify({
      //   title: '明細查詢結果',
      //   message: '返回的對象：' + JSON.stringify(data),
      //   type: 'success'
      // });
      return true;
    },
    delBefore(ids, rows) { //查詢介面的表刪除前 ids為刪除的id數組,rows刪除的行
      let auditStatus = rows.some(x => { return x.AuditStatus > 0 });
      // if (auditStatus) {
      //   this.$message.error('只能刪除未審核的數據')
      //   return false;
      // }
      this.$message.success('刪除前，選擇的Id:' + ids.join(','));
      return true;
    },
    delAfter(result) {//查詢介面的表刪除後
      return true;
    },
    delDetailRow(rows) { //彈出框刪除明細表的行數據(只是對table操作，並沒有操作後台)
      console.log(rows)
      return true;
    },
    addBefore(formData) { //新建保存前formData為對象，包括明細表
      //formData格式：
      // {
      //     mainData: { 主表字段1: 'x1', 主表字段2: 'x2' },
      //     detailData: [{ 明細表字段1: d1 }],
      //     delKeys: null //刪除明細表行數據的id
      // }

      //formData.mainData.xxx="xxxx";//還可以繼續手動添加值

      //如果需要同時提交其他數據到後台，請設置formData.extra=xxxx
      //後台在表xxxxService.cs中重寫Add方法即可從saveDataModel參數中拿到extra提交的對象
      console.log(this.detailOptions.cnName + '新建前,提交的數據：' + JSON.stringify(formData));
      return true;
    },
    async addBeforeAsync(formData) {
      //2020.12.06增加新建前異步方法同步處理
      //功能同上，區別在於此處可以做一些異步請求處理,全：
      // var _result = await this.http.post("/api/SellOrder/getPageData", {}, true).then(result => {
      //   console.log("1、異步請求前")
      //   return result;
      // })
      // console.log("2、異步請求，同步處理結果:" + JSON.stringify(_result))
      return true;
    },
    addAfter(result) {//新建保存後result返回的狀態及表單對象
      console.log(this.detailOptions.cnName + '新建完成後,返回的數據' + JSON.stringify(result));
      return true;
    },
    updateBefore(formData) { //編輯保存前formData為對象，包括明細表、刪除行的Id
      //formData格式：(自己調試出輸看)
      // {
      //     mainData: { 主表字段1: 'x1', 主表字段2: 'x2' },
      //     detailData: [{ 明細表字段1: d1 }],
      //     delKeys: null //刪除明細表行數據的id
      // }

      //formData.mainData.xxx="xxxx";//還可以繼續手動添加值

      //如果需要同時提交其他數據到後台，請設置formData.extra=xxxx
      //後台在表xxxxService.cs中重寫Update方法即可從saveDataModel參數中拿到extra提交的對象

      console.log(this.detailOptions.cnName + '編輯前,提交的數據：' + JSON.stringify(formData));
      //獲取擴展的modelFooter屬性text
      //  console.log(this.$refs.modelFooter.text)
      return true;
    },
    async updateBeforeAsync(formData) {
      //2020.12.06增加修改前異步方法同步處理
      //功能同上，區別在於此處可以做一些異步請求處理,全：
      var _result = await this.http.post("/api/SellOrder/getPageData", {}, true).then(result => {
        console.log("1、異步請求前")
        return result;
      })
      console.log("2、異步請求，同步處理結果:" + JSON.stringify(_result))
      return true;
    },
    updateAfter(result) {//編輯保存後result返回的狀態及表單對象
      //  this.$message.success(this.detailOptions.cnName + '編輯完成後,返回的數據' + JSON.stringify(result));
      return true;
    },
    auditBefore(ids, rows) {//審核前
      if (rows.length > 2) {//每次最多只能審核2條數據
        this.$message.error('最多只能選擇兩條數據');
        return false;
      }
      return true;
    },
    auditAfter(result, rows) {// 審核後
      if (result.status) {
        result.message = "審核成功。。。。。" + result.message;
      }
      return true;
    },
    resetAddFormBefore() { //重置新建表單前的内容
      return true;
    },
    resetAddFormAfter() { //重置新建表單後的内容
      //如果某些字段不需要重置，則可以重新賦值
      this.editFormFields.Remark = '新建重置默認值66666';
      //給明細表添加默認一行
      this.$refs.detail.rowData.push({ Remark: "新建666666" });
      return true;
    },
    resetUpdateFormBefore() { //重置編輯表單前的内容
      //this.editFormFields當前值
      console.log(this.editFormFields)
      //當前明細表的行
      console.log(this.$refs.detail.rowData)
      return true;
    },
    resetUpdateFormAfter() { //重置編輯表單後的内容
      //如果某些字段不需要重置，則可以重新賦值
      this.editFormFields.Remark = '編輯重置默認值66666';
      //給明細表添加默認一行
      this.$refs.detail.rowData.push({ Remark: "編輯666666" });
      return true;
    },
    importAfter(data) { //導入excel後刷新table表格數據
      this.search(); //刷新table
    },
    modelOpenBefore(row) { //點擊編輯/新建按鈕彈出框前，可以在此處寫逻輯，如，從後台獲取數據

    },
    async modelOpenBeforeAsync(row) { //點擊編輯/新建按鈕彈出框前，可以在此處寫逻輯，如，從後台獲取數據
      //2021.01.10
      if (row) {
        console.log("編輯操作")
      } else {
        console.log("新建操作")
      }
      //打開彈出框前，http請求同步執行
      // var _result = await this.http.post("url", {}, true).then(result => {

      //   _result = result;
      // })
      // console.log(result);
      //返回false不會彈出框 
      //this.$message.error("不能打開彈出框 ");
      return true;
    },
    getFormOption(field) {
      let option;
      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == field) {
            option = item;
          }
        })
      })
      return option;
    },

    //方式2,通過打開彈出框時觸發顯示與4隱藏
    modelOpenAfter(row) {  //編輯或新建時，根據不同的情況控制字段是否顯示 
      //   this.editFormOptions.forEach(x => {
      //     x.forEach(item => {
      //       if (item.field == "Remark" || item.field == "SellNo") {
      //         //如果不是新建，則隱藏Remark,SellNo兩個字段是否顯示 
      //          //也可以根據row當前編輯行的值來處理
      //         this.$set(item, "hidden", this.currentAction != "Add")
      //       }
      //     })
      //   })
    },
    rowClick({ row, column, event }) { //查詢介面table點擊行選中當前行
      this.$refs.table.$refs.table.toggleRowSelection(row);
    },
    // detailRowClick ({ row, column, event }) {
    //   //編輯彈出框從表table點擊行選中當前行，沒有從表的不用管
    //   this.$refs.detail.$refs.table.toggleRowSelection(row);
    // }
  }
};
export default extension;
