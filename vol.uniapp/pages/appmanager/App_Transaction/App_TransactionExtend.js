//************************************************
//  *Author：jxx
//  *QQ：283591387
//  *自定義業務邏輯擴展
//************************************************
export default function() {
	return {
		methods: {
			onInited() { //頁面參數初始化

				this.ck = true; //設置顯示checkbox，只有水平(table)顯示類型時才生效

				this.height = this.height - 40;
				//設置table為水平顯示或者list列表顯示
				this.direction = 'horizontal' //list
				this.textInline = false;
				//this.rowIndex=true;
				this.columns.forEach(column => {
					//設置居左顯示
					column.align = 'left';
					//指定單元格寬度
					if (column.field == 'Quantity' || column.field == 'TransactionType') {
						column.width = 40
					}
					if (column.field == 'Name') {
						column.width = 50
					}
				})

				this.fabButtons.push({
					name: "獲取選中行",
					icon: "scan",
					onClick: () => {
                      let rows=this.$refs.table.getSelectRows();
					  this.$toast('共選中'+rows.length+'行');
					  console.log(rows);
					}
				})
			},
			searchFormOnChange(field, value) { //查詢彈出框下拉框或日期選中事件
				if (field == "TransactionType") {
					this.$toast(`點擊了【狀態】字段,選中值${value}`)
				}
			},
			editFormOnChange(field, value) { //新建編輯彈出框下拉框或日期選中事件
				if (field == "TransactionType") {
					//點擊【狀態】給名字随機設置一個值
					this.editFormFields.Name = ~~(Math.random() * 100000);
					this.$toast(`點擊了【狀態】字段,選中值${value}`)
				}
			},
			rowClick(index, row, column) { //行點擊事件(默認觸發編輯)
				return true;
			},
			searchBefore(params) { //查詢前
				return true;
			},
			updateBefore(formData) { //更新保存前操作
				return true;
			},
			addBefore(formData) { //新建保存前操作
				return true;
			}
		}
	}
}
