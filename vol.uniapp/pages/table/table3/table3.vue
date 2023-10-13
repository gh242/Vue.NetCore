<template>
	<view>
		<vol-alert>
			<view>當前頁面為手動綁定的列表數據,另兩個頁面是從後台api綁定的數據</view>
		</vol-alert>
		<vol-table :tableData="rows" @rowClick="rowClick" :height="tableHeight" :titleField="titleField" ref="table"
			:direction="direction" @formatter="formatter" :columns.sync="columns" :textInline="textInline">
		</vol-table>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				load: true, //默認是否加載數據
				rowIndex: true, //顯示行號
				titleField: 'ExpertName', //列表的標題字段
				tableUrl: "",
				//設置table為水平顯示或者list列表顯示'horizontal'//list
				direction: "list",
				tableHeight: 0, //表格高度，默認100%高度
				textInline: false, //表格内容超出是否換行
				rows: [{
					"ExpertName": "測試數據1",
					"HeadImageUrl": "Upload/Tables/App_Expert/202103061753415708/060222.jpg",
					"UserName": "這是手動綁定的table數據",
					AuditStatus:null,
					"Creator": "admin",
					"CreateDate": "2018-09-04 15:49:44"
				},{
					"ExpertName": "測試數據2",
					"HeadImageUrl": "Upload/Tables/App_Expert/202103061753415708/060222.jpg",
					"UserName": "table數據",
					AuditStatus:1,
					"Creator": "admin",
					"CreateDate": "2018-09-02 15:49:44"
				},{
					"ExpertName": "測試數據3",
					"HeadImageUrl": "Upload/Tables/App_Expert/202103061753415708/060222.jpg",
					"UserName": "table數據",
					AuditStatus:1,
					"Creator": "admin",
					"CreateDate": "2018-09-08 15:49:44"
				},{
					"ExpertName": "測試數據4",
					"HeadImageUrl": "Upload/Tables/App_Expert/202103061753415708/060222.jpg",
					"UserName": "table數據",
					AuditStatus:1,
					"Creator": "admin",
					"CreateDate": "2018-09-04 15:49:44"
				}],
				columns: [{
						field: 'ExpertId',
						title: '主鍵ID',
						hidden: true
					},
					{
						field: 'ExpertName',
						title: '名稱'
					},
					{
						field: 'UserName',
						title: '帳號',
						formatter: true
					},
					{
						field: 'AuditStatus',
						title: '狀態',
						type: "select",
						bind: {
							key: 'audit',
							data: []
						}
					},
					{
						field: 'Resume',
						title: '簡介',
						width: 80
					},
					{
						field: 'HeadImageUrl',
						title: '頭像',
						type: 'img',
						width: 50
					},
					{
						field: 'Creator',
						title: '創建人'
					},
					{
						field: 'CreateDate',
						title: '申請時間',
						type: 'date'
					}
				]
			}
		},
		onLoad() {
			let _this = this;
			uni.getSystemInfo({
				success: function(res) {
					_this.tableHeight = res.windowHeight - 70;
				}
			});
		},
		methods: {
			formatter(row, column, index, callback) { //格式化單元格數據
				if (column.field == 'UserName') {
					return callback('<a style="color:#008dd4;">' + row.UserName + '</a>')
				}
				return callback(row[column.field])
			}
		}
	}
</script>

<style>

</style>
