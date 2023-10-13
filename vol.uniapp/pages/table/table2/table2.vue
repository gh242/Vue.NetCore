<template>
	<view>
		<vol-alert>
			<view>表格支持水平或列表顯示、事件綁定、數據源自動轉換、行格式化、自動分頁，見table2.vue頁面</view>
		</vol-alert>
		<vol-table :url="tableUrl" @rowButtons="rowButtons" @rowButtonClick="rowButtonClick" @rowClick="rowClick"
			:defaultLoadPage="load" @loadBefore="loadBefore" :height="tableHeight" :titleField="titleField"
			:index="rowIndex" @loadAfter="loadAfter" ref="table" :direction="direction" @formatter="formatter"
			:columns.sync="columns" :textInline="textInline">
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
				tableUrl: "api/app_expert/getPageData",
				//設置table為水平顯示或者list列表顯示'horizontal'//list
				direction: "list",
				tableHeight: 0, //表格高度，默認100%高度
				textInline: false, //表格内容超出是否換行
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
						type: 'datetime'
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
					return callback('<a style="color:red;">' + row.UserName + '</a>')
				}
				return callback(row[column.field])
			},
			rowClick(index, row) {
				this.$toast(`點擊了第${index}行`)
			},
			loadAfter() {

			},
			loadBefore(params, callback) {
				callback(true);
			},
			rowButtons(index, row, callback) { //列表顯示的按鈕
				//if(row.xx=='')
				//自定義按鈕,僅onInited中設置：this.direction = "list"後生效
				let buttons = [{
					text: "測試",
					icon: "plus",
					type: "primary",
					shape: "circle",
					//disabled: true
				}, {
					text: "刪除",
					icon: "trash",
					type: "error",
					plain: true,
					shape: "circle",
					//shape:"",//square、circle
					//disabled: false
				}];
				callback(buttons);
			},
			rowButtonClick(btn, index, row) { //列表顯示的按鈕點擊事件
				if (btn.text == '刪除') {
					this.$toast('刪除按鈕')
				} else  {
					this.$toast('測試按鈕')
				}
			},
		}
	}
</script>

<style>

</style>
