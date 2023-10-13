
import { h, resolveComponent, defineAsyncComponent } from 'vue';
import modelBody from "./App_ReportPrice/App_ReportPriceModelBody.vue"
let extension = {
  components: {//動態擴充組件或組件路徑
    gridHeader: "",
    gridBody: {
      render() {
        return h(resolveComponent('ElAlert'), {
          style: { 'margin-bottom': '12px' },
          'show-icon': true,
          closable: false,
        }, [h('p', {}, '1、此介面與4上面的區別是：多個明細表在彈出框進行的擴展,見App_ReportPrice.js(點擊新建或編輯即可查看效果)'),
        h('p', {}, ' 2、手動創建一個頁面(可以寫任意代碼)，引入到App_ReportPrice.js的modelBody位置')]);
      }
    },
    gridFooter: '',
    modelHeader: '',
    //通過defineAsyncComponent異步引用
    modelBody: modelBody,
    // defineAsyncComponent(() => (import("./App_ReportPrice/App_ReportPriceModelBody.vue"))),
    modelFooter: ''
  },
  buttons: [],//擴展的按鈕
  text: "彈出框一對多明細",
  methods: {//事件擴展
    onInit() {
    },
    onInited() {
      //初始化後將table高度减少85,因為gridHeader加了提示會導致頁面有滾動條
      this.height = this.height - 100;
      //自定義彈出框的高與4寬
      this.boxOptions.height = document.body.clientHeight * 0.9;
      this.boxOptions.width = document.body.clientWidth * 0.8;
    },
    modelOpenAfter() {//打開彈出框時
      //是否子組件渲染完成
      //新建功時，清空 清空,從表1，從表2的數據
      this.$nextTick(() => {
        //這裡沒有給彈出框中的表格傳參，如果需要參數可以通過 this.$emit("parentCall", 獲取頁面的參數
        //具體見自定義頁面App_ReportPriceModelBody.vue中的modelOpen方法的使用 this.$emit("parentCall", ($this) => {
        this.$refs.modelBody.modelOpen();
      })
    },
    addBefore(formData) { //彈出框新建或編輯功能點保存時可以將從表1，從表2的數據提到後台
      this.setFormData(formData);
      return true;
    },
    updateBefore(formData) { //編輯時功能點保存時可以將從表1，從表2的數據提到後台
      this.setFormData(formData);
      return true;
    },
    setFormData(formData) { //新建或編輯時，將從表1、2的數據提交到後台,見後台App_ReportPriceService的新建方法
      //後台從對象裡直接取extra的值
      let extra = {
        table1List: this.$refs.modelBody.$refs.table1.rowData,//獲取從表1的行數據
        table2List: this.$refs.modelBody.$refs.table2.rowData//獲取從表2的行數據
      }
      formData.extra = JSON.stringify(extra);
    },
    resetUpdateFormAfter() { //編輯彈出框時，點重置時，可自定義重置
      console.log('resetUpdateFormAfter')
      return true;
    },
    resetAddFormAfter() { //新建彈出框時，點重置時，可自定義重置
      console.log('resetAddFormAfter')
      return true;
    }
  }
};
export default extension;
