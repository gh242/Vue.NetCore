let extension = {
  components: {
    //動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '', //{ template: "<div>擴展組xx件</div>" },
    gridBody: '',
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  text: '點擊行或者點擊表格的編輯即可開啟編輯功能',
  tableAction: 'App_Transaction',
  buttons: [], //擴展的按鈕
  methods: {
    //事件擴展
    onInit() {
      //手動設置彈出框的高度與4寬度
      //判斷有沒有App_Transaction表修改的權限 ，無權限不開啟編輯功能
      if (!this.filterPermission('App_Transaction', 'Update')) {
        return;
      }
      //開啟編輯功能
      this.doubleEdit = true;
      //開啟指定列可以編輯
      //**注意，開啟字段的編輯信息，如果調用框架的保存方法，代碼生成器中必須設置了編輯行並生成了model否則驗證通不過
      this.columns.forEach((column) => {
        if (column.field == 'Describe' || column.field == 'Name') {
          column.edit = { type: 'text' };
        }
        if (column.field == 'TransactionType') {
          column.edit = { type: 'switch' };
        }
        if (column.field == 'CowType') {
          column.edit = { type: 'select' };
        }
      });
      //動態添加操作列
      this.columns.push({
        field: '操作',
        title: '操作',
        align: 'center',
        width: 120,
        render: this.getRender()
      });
    },
    beginEdit(row, column, index) {
      console.log('編輯行：'+index)
      //點擊行編輯  return false阻止編輯(可以根據row不同的值判斷當前是否可以編輯);
      return true;
    },
    endEditBefore(row, column, index) {
      //可以自動結束編輯時，自動執行保存，下面getRender的保存按鈕去掉
      // this.editSave();
      return true;
    },
    editSave(row) {
      //調用框架的默認保存方法
      this.http
        .post('/api/App_Transaction/update', { mainData: row }, true)
        .then((x) => {
          this.$Message.info('保存成功');
          console.log('保存結果：' + JSON.stringify(x));
        });
    },
    getRender() {
      //生成最後一列操作列
      return (h, { row, column, index }) => {
        return h(
          'div',
          {
            style: { color: '#0c83ff', 'font-size': '13px', cursor: 'pointer' }
          },
          [
            h(
              'a',
              {
                props: {},
                style: { 'border-bottom': '1px solid' },
                onClick: (e) => {
                  e.stopPropagation();
                  //取消其他行選中
                  this.$refs.table.$refs.table.clearSelection();
                  //設置當前後選中
                  this.$refs.table.$refs.table.toggleRowSelection(row);
                  this.del([row]);
                }
              },
              '刪除'
            ),
            h(
              'a',
              {
                props: {},
                style: { 'margin-left': '9px', 'border-bottom': '1px solid' },
                onClick: (e) => {
                  e.stopPropagation();
                  //將當前行設置為編輯行
                  this.$refs.table.edit.rowIndex = index;
                }
              },
              '編輯'
            ),
            h(
              'a',
              {
                props: {},
                style: { 'margin-left': '9px', 'border-bottom': '1px solid' },
                onClick: (e) => {
                  e.stopPropagation();
                  //强制結束編輯
                  this.$refs.table.edit.rowIndex = -1;
                  //執行保存方法
                  this.editSave(row);
                }
              },
              '保存'
            )
          ]
        );
      };
    }
  }
};
export default extension;
