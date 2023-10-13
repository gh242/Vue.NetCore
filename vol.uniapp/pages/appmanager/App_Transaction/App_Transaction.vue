<template>
	<view>
		<view-grid ref="grid" :index="true" :options="options">
			<!-- 自定義slot -->
			<view @click="viewClick" slot="gridHeader" class="grid-header">
				<vol-alert>
					<view>1、搜索、編輯或新建彈出框點擊下拉框查看效果。</view>
					<view>2、設置columns的align=true内容居左顯示。</view>
					<view>3、設置this.ck=true顯示checkbox。</view>
					<view>4、示例見:App_TransactionExtend.js文件說明。</view>
				</vol-alert>
			</view>

			<vol-alert slot="search">
				<view>點擊【狀態】下拉框可監聽事件</view>
			</vol-alert>
			<vol-alert slot="edit">
				<view>點擊【狀態】下拉框給【姓名】随機設置一個值</view>
			</vol-alert>
		</view-grid>
	</view>
</template>

<script>
	//************************************************
	//  *Author：jxx
	//  *QQ：283591387
	//  *自定義業務邏輯擴展
	//************************************************
	import extend from './App_TransactionExtend.js'
	import options from './App_TransactionOptions.js';
	let _options = options();
	_options.extend = extend;
	export default {
		data() {
			return {
				gridHeaderText: "", //随便輸入字符後可以顯示出來
				options: _options
			}
		},
		onShow() {},
		methods: {
			viewClick() {
				//獲取生成頁面grid的對象
				console.log(this.$refs.grid.searchFormFields)
			}
		}
	}
</script>

<style>
</style>
