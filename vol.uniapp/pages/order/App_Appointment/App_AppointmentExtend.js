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
				this.height = this.height - 65;

				this.fabButtons.push({
					name: "測試按鈕",
					icon: "search",
					onClick: () => {
						this.$emit('testBtnClick', {
							value: "測試"
						})
					}
				})

				//編輯彈出框姓名字段增加一個掃一掃操作
				this.editFormOptions.forEach(item => {
					if (item.field == 'Name') {
						item.extra = {
							style: "color: #0762c4;margin-left:30rpx;",
							text: "掃一掃",
							icon: "scan",
							color: "#0762c4",
							size: 20
						}
					}
				})
			},
			extraClick(option, fields) { //上面的掃一掃點擊事件觸發
				if (option.field == 'Name') {
					this.$toast('點擊了掃一掃');
					uni.scanCode({
						success: (res) => {
							this.editFormFields.Name = res.result;
						}
					})
				}

			},
			formatter(row, column) { //自定義格式化
				// if(column.field=='xx'){
				//  return '<a style="color:red;">' + row[column.field] + '</a>';
				// }
				//return row[column.field]
			},
			rowClick(index, row, column) { //行點擊事件
				return true;
			},
			updateBefore(formData) { //更新保存前操作
				return true;
			},
			addBefore(formData) { //新建保存前操作
				return true;
			},
			searchInputClick(searchText) {
				//這裡設置的是動態屬性，searchText名字可以自己随便寫
				this.searchText = searchText;
				this.search();
			},
			searchBefore(params) { //查詢前
				//介面上的掃描或者搜索框操作,與上面的searchInputClick配合使用
				if (this.searchText) {
					params.wheres.push({
						name: "PhoneNo",
						value: this.searchText,
						displayType: "like"
					})
				}
				return true;
			}
		}
	}
}
