
import { h, resolveComponent } from 'vue';
let extension = {
  components: {//動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '',
    gridBody: {
      render() {
        return h(resolveComponent('ElAlert'), {
          style: { 'margin-bottom': '12px' },
          'show-icon': true,
          type: 'success',
          closable: false,
        }, [h('p', {}, '1、 onInited中重新定義表頭數據,2021.01.08更新voltable.vue後才能使用'),
        h('p', {}, ' 2、具體見：App_Appointment2.js中配置(現在只支持到二級表頭、不支持編輯與4複雜操作)')]);
      }
    },
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  tableAction:"App_Appointment",
  text: "頁面全部由代碼生成器生成，只需要在onInited中重新配置表頭數據",
  methods: {//事件擴展
    onInit() {
      //設置表的最大高度
      this.tableMaxHeight = 300;
    },
    onInited() {
      this.columns.splice(0);
      this.columns.push(...[
        {
          field: 'Name', title: '基礎信息', type: 'string', align: 'center',
          children: [
            { field: 'Name', title: '用戶姓名', type: 'string', link: true, width: 100, sortable: true },
            { field: 'PhoneNo', title: '電話', type: 'string', width: 130 },
            { field: 'Describe', title: '描述信息', type: 'string', width: 180 },
          ]
        },
        {
          field: 'Name', title: '創建人信息', type: 'string', align: 'center',
          children: [
            { field: 'Id', title: '主鍵ID', type: 'string', width: 90, hidden: true },
            { field: 'CreateDate', title: '創建時間', type: 'datetime', width: 120, sortable: true },
            { field: 'Creator', title: '創建人', type: 'string', width: 130, align: 'left' }]
        },
        {
          field: 'Name', title: '修改人信息', type: 'string', align: 'center',
          children: [
            { field: 'Modifier', title: '修 改 人', type: 'string', width: 130, align: 'left' },
            { field: 'ModifyDate', title: '修改時間', type: 'datetime', width: 150, sortable: true }]
        }
      ]
      )
    }
  }
};
export default extension;