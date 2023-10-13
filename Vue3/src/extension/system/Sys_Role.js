
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
                  'show-icon': false, type: 'success',
                  closable: false, title: '關於TreeTable使用'
              }, ' treetable同樣全部代碼自動生成，頁面生成後設置this.rowKe="xxx" tree主鍵字段,即可完成樹形table配置,具體說明見Sys_Role1.js'),
          ]
      }
  },

    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  buttons: [],//擴展的按鈕
  tableAction:"Sys_Role",
  methods: {//事件擴展
    onInited () {
      this.height = this.height - 80;
       this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == 'ParentId') {
            item.title = "上級角色";
            //設置任意節點都能選中(默認只能選中最後一個節點)
            item.changeOnSelect = true;
          }
        })
      })
    },
    onInit() {
      //設置treetable的唯一值字段(這個字段的值在表裡面必須是唯一的)
      this.rowKey="Role_Id";
    },
    /***加載後台數據見Sys_RoleController.cs文件***/
    loadTreeChildren(tree, treeNode, resolve) { //加載子節點
      let url=`api/role/getTreeTableChildrenData?roleId=${tree.Role_Id}`;
      this.http.post(url,{}).then(result=>{
        resolve(result.rows)
      })
    },
      /***加載後台數據見Sys_RoleController.cs文件***/
    searchBefore(params){//判斷加載根節點或子節點
      //沒有查詢條件，默認查詢返回所有根節點數據
      if (!params.wheres.length) {
        params.value=1;
      }
      return true;
    }
  }
};
export default extension;
