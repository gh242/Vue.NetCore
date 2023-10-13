//************************************************
//  *Author：jxx
//  *QQ：283591387
//  *自定義業務邏輯擴展
//************************************************
export default function() {
	return {
		methods: {
			onInited() { //頁面參數初始化
				//設置table超出換行顯示
				//this.textInline = false;

				//設置列寬度
				//this.columns[1].width = 70;

				//設置table為水平顯示或者list列表顯示
				//this.direction = 'horizontal'//list
				//如果為list列表顯示，指定list的標題列
				this.titleField = "CreateDate";
				//this.height=this.height-65;
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
				this.load = false;
				//頁面打開時默認彈出查詢框
				this.searchModel = true;

                //設置查詢與編輯的城市字段為省市區縣選擇(2023.03.20更新components文件夾後才能使用)
				this.searchFormOptions.forEach(x => {
					if (x.field == 'City') {
						x.type = 'city'
					}
				})
				
				this.editFormOptions.forEach(x => {
					if (x.field == 'City') {
						x.type = 'city'
					}
				})
			},
			formatter(row, column) { //自定義格式化
				// if(column.field=='xx'){
				//  return '<a style="color:red;">' + row[column.field] + '</a>';
				// }
				//return row[column.field]
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
