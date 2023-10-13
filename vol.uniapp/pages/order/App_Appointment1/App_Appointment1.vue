<template>
	<view>
		<view-grid ref="grid" @delTest="delTest" :index="true" :options="options">
			<view slot="gridHeader" class="grid-header">
				{{gridHeaderText}}
				<vol-alert>
					<view>1.設置direction屬性改變數據顯示方向</view>
					<view>2.此示例包括:列隱藏,自定義超連接,自定義列點擊事件</view>
					<view>3.見App_Appointment1.vue/.js文件</view>
				</vol-alert>
			</view>
		</view-grid>

		<u-modal title="提示" @cancel="show=false" width="500rpx" @confirm="confirm" showCancelButton showConfirmButton
			confirmColor="red" :show="show">
			<view style="text-align: center;">確定刪除數據嗎?</view>
		</u-modal>
	</view>
</template>

<script>
	//************************************************
	//  *Author：jxx
	//  *QQ：283591387
	//  *自定義業務邏輯擴展
	//************************************************
	import extend from './App_AppointmentExtend1.js'
	import options from './App_AppointmentOptions1.js';

	export default {
		data() {
			let _options = options();
			_options.extend = extend;
			return {
				gridHeaderText: "", //随便輸入字符後可以顯示出來
				options: _options,
				show: false,
				row: {}
			}
		},
		onShow() {},
		methods: {
			delTest(row) {
				this.row = row;
				this.show = true;
			},
			confirm() {
				//這裡只是舉例。後台的接口都可以自己寫
				//刪除方法，框架自帶，點擊行數據彈中會有刪除按鈕（菜單上需要配置刪除按鈕），可以看其他頁面
				const url = 'api/App_Appointment/del';
				this.http.post(url, [this.row.Id], true).then(result => {
					this.$toast(result.message)
					if (!result.status) {
						return
					}
					//關閉彈框
					this.show=false;
					//刪除成功後,調用生成頁面的刷新方法
					this.$refs.grid.search();
				})
			}
		}
	}
</script>

<style>
</style>
