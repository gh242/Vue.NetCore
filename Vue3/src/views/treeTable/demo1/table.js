
import { h, resolveComponent } from 'vue';
let extension = {
  components: {//動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '',
    gridBody: {
      render () {
          return [
              h(resolveComponent('el-alert'), {
                  style: { 'margin-bottom': '12px' },
                  'show-icon': true, type: 'success',
                  closable: false, title: '關於TreeTable使用'
              }, '整個頁面分為:左邊樹形菜單Tree.vue與4右邊Table.vue(代碼生成的頁面,複製過來即可)兩部份,按照此格式配置即可,具體說明見TreeTable1.vue'),
          ]
      }
  },
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  text: "點擊左邊tree加載表格數據",
  buttons: [],//擴展的按鈕
  methods: {//事件擴展
    onInit() {
      //緩存當前table頁面，點擊左邊樹形菜單時，直接刷新此頁面
      this.$store.getters.data().viewGridDemo = this;
      this.boxOptions.height = 400;
      //默認不加載表格數據,由Tree.vue中created方法來觸發默認加載數據
      this.load=false;
    },
    onInited() {
      this.height = this.height - 75;
    },
    nodeClick(treeId){ //點擊邊樹節點刷新右邊表格
       this.refresh();
    },
    searchBefore(param) {
      //點擊左邊tree時加載table數據，其他情況都不加載數據
      let treeId = this.$store.getters.data().treeDemo1.treeId;
      if (treeId === undefined) {
        return false;
      }
      //將查詢的treeid(角色id)提交到後台
      param.value = treeId;
      //生成查詢條件
      // param.wheres = [{ name: 'Role_Id', value: treeId }]
      return true;
    },
    addBefore(param) { //保存前
      let treeId = this.$store.getters.data().treeDemo1.treeId;
      if (treeId === undefined) {
        this.$Message.error("請選擇左側角色")
        return false;
      }
      //添加默認新建的值到後台
      //新建用戶的角色默認為當前樹形菜單選中的角色
      param.mainData.Role_Id = treeId;
      param.mainData.IsRegregisterPhone = 0;
      return true;
    },
    addAfter(result) {//用戶新建後，顯示随機生成的密碼
      if (!result.status) {
        return true;
      }
      return true;
    },
    modelOpenAfter() {
      //點擊彈出框後，如果是編輯狀態，禁止編輯用戶名，如果新建狀態，將用戶名字段設置為可編輯
      let isEDIT = this.currentAction == this.const.EDIT;
      this.editFormOptions.forEach(item => {
        item.forEach(x => {
          if (x.field == "UserName") {
            x.disabled=isEDIT
          }
        })
        //不是新建，性別默認值設置為男
        if (!isEDIT) {
          this.editFormFields.Gender = "0";
        }
      })
    }

  }
};
export default extension;
