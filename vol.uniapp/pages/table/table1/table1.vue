<template>
	<view style="overflow: hidden;">
		<vol-alert>
			<view>
				<view>1、表格支持水平或列表顯示、事件綁定、數據源自動轉換、行格式化、自動分頁，見table1.vue頁面</view>
				<view>2、點擊下面輸入框左側的掃描圖標可以掃描搜索</view>
			</view>
		</vol-alert>
		<view style="padding: 20rpx;">
			<u-search @search="searchClick" @custom="searchClick" placeholder="請輸入名稱搜索" :showAction="true"
				actionText="搜索" searchIcon="scan" @clickIcon="scanClick" :animation="false" v-model="searchText">
			</u-search>
			<view @click="getRows" style="text-align: right;padding-top: 30rpx;">
				<u--text type="info" text="獲取選中行"></u--text>
			</view>

		</view>
		<!--  -->
		<vol-table :ck="true" :url="tableUrl" @rowClick="rowClick" :defaultLoadPage="load" @loadBefore="loadBefore"
			:height="height" :index="rowIndex" @loadAfter="loadAfter" ref="table" :direction="direction"
			@formatter="formatter" :columns.sync="columns" :textInline="textInline">
		</vol-table>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				searchText: "",
				load: true, //默認是否加載數據
				rowIndex: true, //顯示行號
				tableUrl: "api/app_expert/getPageData",
				//設置table為水平顯示或者list列表顯示'horizontal'//list
				direction: "horizontal",
				height: 0, //表格高度，默認100%高度
				textInline: false, //表格内容超出是否換行
				columns: [{
						field: 'ExpertId',
						title: '主鍵ID',
						hidden: true
					},
					{
						field: 'HeadImageUrl',
						title: '頭像',
						align: "left",
						type: 'img',
						width: 50
					},
					{
						field: 'ExpertName',
						title: '名稱',
						align: "left",
						formatter: true
					},
					{
						field: 'UserName',
						align: "left",
						title: '帳號'
					},
					{
						field: 'AuditStatus',
						title: '狀態',
						type: "select",
						align: "left",
						bind: {
							key: 'audit',
							data: []
						}
					},
					// {
					// 	field: 'Resume',
					// 	title: '簡介',
					// 	width: 80
					// },
				]
			}
		},
		onLoad() {
			let _this = this;
			uni.getSystemInfo({
				success: function(res) {
					// // #ifdef MP-WEIXIN
					// _this.height = res.windowHeight - 180;
					// return
					// // #endif

					_this.height = res.windowHeight - 125;
				}
			});
		},
		methods: {
			getRows(){
				let rows= this.$refs.table.getSelectRows();
				this.$toast('共選中'+rows.length+'行');
				console.log(rows)
			},
			scanClick() {
				//掃一掃點擊事件
				uni.scanCode({
					success: (res) => {
						this.searchText = res.result;
						this.searchClick();
					}
				})
			},
			searchClick() { //點擊搜索
				this.$refs.table.load(null, true);
			},
			formatter(row, column, index, callback) { //格式化單元格數據
				if (column.field == 'ExpertName') {
					return callback('<a style="color:red;">' + row.ExpertName + '</a>')
				}
				return callback(row[column.field])
			},
			rowClick(index, row) {
				this.$toast(`點擊了第${index}行`)
			},
			loadAfter() {

			},
			loadBefore(params, callback) {
				//如果有搜索條件,按搜索值模糊查詢 
				if (this.searchText) {
					params.wheres.push({
						name: "ExpertName",
						value: this.searchText,
						displayType: "like"
					})
				}

				callback(true);
			}
		}
	}
</script>

<style>

</style>
