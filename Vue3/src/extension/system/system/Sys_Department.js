/*****************************************************************************************
**  Author:jxx 2022
**  QQ:283591387
**完整文檔見：http://v2.volcore.xyz/document/api 【代碼生成頁面ViewGrid】
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
    //新建、編輯彈出框擴展組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  tableAction: '', //指定某張表的權限(這裡填寫表名,默認不用填寫)
  buttons: { view: [], box: [], detail: [] }, //擴展的按鈕
  methods: {
    //下面這些方法可以保留也可以刪除
    onInit() {  //框架初始化配置前，
      this.rowKey = "DepartmentId";
    },
    loadTreeChildren(tree, treeNode, resolve) { //加載子節點
      let url = `api/Sys_Department/getTreeTableChildrenData?departmentId=${tree.DepartmentId}`;
      this.http.post(url, {}).then(result => {
        resolve(result.rows)
      })
    },
    /***加載後台數據見Sys_RoleController.cs文件***/
    searchBefore(params) {//判斷加載根節點或子節點
      //沒有查詢條件，默認查詢返回所有根節點數據
      if (!params.wheres.length) {
        params.value = 1;
      }
      return true;
    },
    onInited() {
      let hasUpdate, hasDel, hasAdd;
      this.buttons.forEach((x) => {
        if (x.value == 'Update') {
          x.hidden = true;
          hasUpdate = true;
        } else if (x.value == 'Delete') {
          hasDel = true;
          x.hidden = true;//隱藏按鈕
        }
        else if (x.value == 'Add') {
          x.type="primary";
          hasAdd = true;
        }
      });
      if (!(hasUpdate || hasDel || hasAdd)) {
        return;
      }
      this.columns.push({
        title: '操作',
        field: '操作',
        width: 80,
        fixed: 'right',
        align: 'center',
        render: (h, { row, column, index }) => {
          return (
            <div>
              <el-button
                onClick={($e) => {
                  this.addBtnClick(row)
                }}
                type="primary"
                link
                v-show={hasAdd}
                icon="Plus"
              >
              </el-button>
              <el-button
                onClick={($e) => {
                  this.edit(row);
                }}
                type="success"
                link
                v-show={hasUpdate}
                icon="Edit"
              >
              </el-button>
              <el-tooltip
                class="box-item"
                effect="dark"
                content="刪除"
                placement="top"
              >
                <el-button
                  link
                  onClick={($e) => {
                    this.del(row);
                  }}
                  v-show={hasDel}
                  type="danger"
                  icon="Delete"
                >
                </el-button>
              </el-tooltip>
            </div>
          );
        }
      });
    },
    addBtnClick(row) {
      //這裡是動態addCurrnetRow屬性記錄當前點擊的行數據,下面modelOpenAfter設置默認值
      this.addCurrnetRow = row;
      this.add();
    },
    addAfter() {//添加後刷新字典
      this.initDicKeys();
      return true;
    },
    updateAfter() {
      this.initDicKeys();
      return true;
    },
    delAfter(result) {//查詢介面的表刪除後
      this.initDicKeys();
      return true;
    },
    modelOpenAfter(row) {
      //點擊行上的添加按鈕事件
      if (this.addCurrnetRow) {

        //獲取當前組織構架的所有父級id,用於設置新建時父級id的默認值

        //獲取數據數據源
        let data = [];
        this.editFormOptions.forEach(options => {
          options.forEach(option => {
            if (option.field == 'ParentId') {
              data = option.orginData;
            }
          })
        })
        let parentIds = this.base.getTreeAllParent(this.addCurrnetRow.DepartmentId, data).map(x => { return x.id });
        //設置編輯表單上級組織的默認值
        this.editFormFields.ParentId = parentIds;
        this.addCurrnetRow = null;

      }
    }
  }
};
export default extension;
