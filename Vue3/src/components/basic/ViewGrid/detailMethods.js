//從表方法
let detailMethods = {
  //查詢從表前先做内部處理
  loadInternalDetailTableBefore(param, callBack,data) {
    //加載明細表數據之前,需要設定查詢的主表的ID
    //每次只要加載明細表格數據就重置刪除明細的值
    if (this.detailOptions.delKeys.length > 0) {
      this.detailOptions.delKeys = [];
    }
    let key = this.table.key;
    if (this.currentRow && this.currentRow.hasOwnProperty(key)) {
      param.value = this.currentRow[key];
    }
    return this.loadDetailTableBefore(param, callBack,data);
  },
  detailRowOnChange(row) {
    this.detailRowChange(row);
  },
  detailRowChange(row) {
    //checkbox選中行事件
  },
  detailRowOnClick({ row, column, event }) {
    //明細表點擊行事件2020.11.07
    this.detailRowClick({ row, column, event });
  },
  detailRowClick({ row, column, event }) {},
  resetDetailTable(row) {
    //編輯和查看明細時重置從表數據
    if (!this.detailOptions.columns || this.detailOptions.columns.length == 0) {
      return;
    }
    let key = this.table.key;
    let query = { value: row ? row[key] : this.currentRow[key] };
    this.$nextTick(() => {
      if (this.$refs.detail) {
        this.$refs.detail.reset();
        this.$refs.detail.load(query);
      }
    });
  },
  //從後面加載從表數據
  refreshRow() {
    this.resetDetailTable();
  },
  addRow() {
    this.$refs.detail.addRow({});
    this.$refs.detail.edit.rowIndex=-1;
    this.updateDetailTableSummaryTotal();
  },
  delRow() {
    let rows = this.$refs.detail.getSelected();
    if (!rows || rows.length == 0) {
      return this.$message.error('請選擇要刪除的行!');
    }
    if (!this.delDetailRow(rows)) {
      return false;
    }

    let tigger = false;
    this.$confirm('確認要刪除選擇的數據嗎?', '警告', {
      confirmButtonText: '確定',
      cancelButtonText: '取消',
      type: 'warning',
      center: true
    }).then(() => {
      if (tigger) return;
      tigger = true;
      rows = this.$refs.detail.delRow();
      let key = this.detailOptions.key;
      //記錄刪除的行數據
      rows.forEach((x) => {
        if (x.hasOwnProperty(key) && x[key]) {
          this.detailOptions.delKeys.push(x[key]);
        }
      });
      this.updateDetailTableSummaryTotal();
    });
  },
  updateDetailTableSummaryTotal() {
    //2021.09.25增加明細表刪除、修改時重新計算行數與4汇總
    //2021.12.12增加明細表判斷(强制刷新合計時會用到)
    if (!this.$refs.detail) {
      return;
    }
    //刪除或新增行時重新設置顯示的總行數
    this.$refs.detail.paginations.total = this.$refs.detail.rowData.length;
    //重新設置合計
    if (this.$refs.detail.summary) {
      this.$refs.detail.columns.forEach((column) => {
        if (column.summary) {
          this.$refs.detail.getInputSummaries(null, null, null, column);
        }
      });
    }
  },
  detailSelectable(row, index){
    //明細表CheckBox 是否可以勾選
       return true;
  }
};

export default detailMethods;
