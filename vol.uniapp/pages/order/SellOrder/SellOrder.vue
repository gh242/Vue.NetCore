<template>
	<view>
		<view-grid ref="grid" :index="true" :options="options">
			<!-- 自定義slot -->
			<view @click="viewClick" slot="gridHeader" class="grid-header">
				<vol-alert>
					<view>點擊編輯或新建跳轉到主從表頁面,見SellOrder.vue</view>
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
	import extend from './SellOrderExtend.js'
	import options from './SellOrderOptions.js';
	let _options = options();
	_options.extend = extend;
	export default {
		data() {
			return {
				gridHeaderText: "", //随便輸入字符後可以顯示出來
				options: _options
			}
		},
		onShow() {
		},
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
