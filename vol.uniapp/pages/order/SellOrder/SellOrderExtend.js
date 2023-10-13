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

				//設置自定義格式顯示
				//this.columns.forEach(column=>{
				// 	if(column.field=='字段'){
				//      //自定義格式化顯示,在下面的formatter實現具體邏輯
				// 		//column.formatter=true;

				//      //指定字段為date類型不顯示時分秒
				//      //column.type="date";

				//      //設置列寬度
				//      //column.width = 70;
				// 	}
				// })

				//頁面打開時禁用加載數據
				//this.load=false;
				//頁面打開時默認彈出查詢框
				//this.searchModel = true;
			},

			//審核操作：
			//1、實現rowButtons方法返回審批按鈕（注意表必须有AuditStatus字段int類型）
			rowButtons(rowindex, row) {
				//沒有審核權限按鈕的不顯示
				// let hasAudit = this.permission.some(x => {
				// 	return x.tableName == 'SellOrder' && x.permission.indexOf('Audit') != -1
				// });
				// if (!hasAudit) {
				// 	return [];
				// }
				//AuditStatus === 0審核中的數據
				if (row.AuditStatus === 0) {
					return [{
						text: "審核",
						type: "error",
						shape: "circle"
					}]
				}
				//返回查看審批按鈕
				return [{
					text: "查看審核",
					type: "primary",
					shape: "circle"
				}]
			},
			//2、實現rowButtons按鈕點擊方法，並跳轉到審批頁面(創建一個審批頁面SellOrderAduit.vue並在pages.json中添加頁面配置)
			//3、具體實現見SellOrderAduit.vue頁面
			rowButtonClick(btn, rowindex, row) {
				uni.navigateTo({
					url: "/pages/order/SellOrder/SellOrderAduit?orderId=" + row.Order_Id
				})
			},
			formatter(row, column) { //自定義格式化
				// if(column.field=='xx'){
				//  return '<a style="color:red;">' + row[column.field] + '</a>';
				// }
				//return row[column.field]
			},
			rowClick(index, row, column) { //行點擊事件(默認觸發編輯)
				//跳轉到詳情頁面
				let url = "/pages/order/SellOrder/Detail/Detail?orderId=" + row.Order_Id;
				uni.navigateTo({
					url: url
				})
				return false;
			},
			gridAdd() { //點擊添加跳轉到詳情頁面
				let url = "/pages/order/SellOrder/Detail/Detail";
				uni.navigateTo({
					url: url
				})
			}
		}
	}
}
