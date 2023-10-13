<template>
	<view>
		<view-grid ref="grid" :index="true" :options="options">
			<!-- 自定義slot -->
			<view @click="viewClick" slot="gridHeader" class="grid-header">
				<vol-alert>
					<view>列表數據自定義style顯示。</view>
					<view></view>
				</vol-alert>
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
	import extend from './App_TransactionExtend1.js'
	import options from './App_TransactionOptions1.js';
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
