//************************************************
//  *Author：jxx
//  *QQ：283591387
//  *自定義業務邏輯擴展
//************************************************
export default function() {
	return {
		methods: {
			onInited() { //頁面參數初始化
				this.height = this.height - 40;
				//設置table超出換行顯示
				//this.textInline = false;

				//設置列寬度
				//this.columns[1].width = 70;

				//設置table為水平顯示或者list列表顯示
				//this.direction = 'horizontal'//list
				//如果為list列表顯示，指定list的標題列
				//this.titleField="字段";

				//設置自定義樣式
				this.columns.forEach(column => {
					if (column.field == 'Quantity' || column.field == 'TransactionType') {
						//自定義格式化顯示,在下面的formatter實現具體邏輯
						column.formatter = true;
						column.title="888888";

					}
				})
				console.log("11")
				//頁面打開時禁用加載數據
				//this.load=false;
				//頁面打開時默認彈出查詢框
				//this.searchModel = true;
			},
			formatter(row, column) { //自定義格式化
				if (column.field == 'TransactionType') {
					if (row.TransactionType == 1) {
						return `<span style="color: #ffff;background: #00aaff;padding: 2px 10px;
						         border-radius: 3px;font-size: 10px;"> 是</span>`;
					}
					return `<span style="color: #ffff;background: #3eb703;padding: 2px 10px;
					         border-radius: 3px;font-size: 10px;"> 否</span>`;
				}
				if (column.field == 'Quantity' && row[column.field] >= 100) {
					return '<a style="color:red;">' + row[column.field] + '</a>';
				}
				return row[column.field]
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
