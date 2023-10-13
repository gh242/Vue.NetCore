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
				this.direction = 'horizontal' //list


				this.columns.forEach(column => {
					//動態隱藏字段
					if (["Creator", "Modifier", "ModifyDate", "PhoneNo"].indexOf(column.field) != -1) {
						column.hidden = true;
					}
					if (column.field == 'CreateDate') {
						column.type = "date";
						column.width = 80;
					}
					if (column.field == 'Describe') {
						column.width = 140;
						//設置描述字段超連接與點擊事件
						column.formatter = true;
						column.click = true;
					}
				})
				
                //動態添加一個操作列
				//具體實現見下面formatter、cellClick方法及App_Appointment.vue中的刪除彈出框操作
				this.columns.push({
					field: "操作",
					title: "操作",
					formatter: true,
					click: true
				})

				//顯示行號
				this.rowIndex = true;
			},
			formatter(row, column) { //自定義格式化
				if (column.field == 'Describe') {
					return '<a style="color:#2485e9;">' + row[column.field] + '</a>';
				} else if (column.field == '操作') {
					return '<a style="color:#2485e9;font-size:26rpx">刪除</a>';
				}
				return row[column.field]
			},
			cellClick(index, row, column) {
				if (column.field == 'Describe') {
					this.$toast('點擊了:' + row.Describe)
				} else if (column.field == '操作') {
					//刪除操作(這裡只是舉例，框架内置了刪除方法，點擊行彈出框中(菜單分配了刪除按鈕才能看到))
					//調用刪除提示：見App_Appointment.vue中
					
					this.$emit('delTest',row)
				}
			},
			rowClick(index, row, column) { //行點擊事件
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
