<template>
	<view>
		<view-grid ref="grid" @testBtnClick="testBtnClick" :index="true" :options="options">
			<!-- 自定義slot -->
			<view slot="gridHeader" class="grid-header">
				<vol-alert>
					<view>1、頁面由代碼生成器生成,自動控制前後端權限</view>
					<view>2、當前頁面在後台菜單上配置的只讀不能添加與修改</view>
					<view>3、點擊搜索框掃描圖標可以掃描搜索</view>
					<view>4、示例代碼見App_Appointment.vue文件</view>
					<view>5、點擊列表數據,彈出框中也有掃一掃示例</view>
				</vol-alert>
				<view style="padding: 20rpx;border-bottom: 1px solid #f5f3f3;">
					<u-search @search="searchInputClick" @custom="searchInputClick" placeholder="請輸入電話搜索"
						:showAction="true" actionText="搜索" searchIcon="scan" @clickIcon="scanClick" :animation="false"
						v-model="searchText">
					</u-search>
				</view>
			</view>
		</view-grid>
	</view>
</template>

<script>
	//************************************************
	//  *Author：jxx
	//  *QQ：283591387
	//  *自定義業務邏輯擴展
	//************************************************
	import extend from './App_AppointmentExtend.js'
	import options from './App_AppointmentOptions.js';
	let _options = options();
	_options.extend = extend;
	export default {
		data() {
			return {
				searchText: "",
				options: _options,
				show: false,
				row: {}
			}
		},
		onShow() {},
		methods: {
			scanClick() {
				//掃一掃點擊事件
				uni.scanCode({
					success: (res) => {
						this.searchText = res.result;
						this.searchInputClick();
					}
				})
			},
			testBtnClick(){
				 this.$toast('測試按鈕');
			},
			searchInputClick() { //點擊搜索
				//生成頁面的搜索功能，見App_AppointmentExtend.js中searchInputClick操作
				this.$refs.grid.searchInputClick(this.searchText);
			},
		}
	}
</script>

<style>
</style>
