<template>
	<view>
		<view-grid ref="grid" :index="true" :options="options">
			<!-- 自定義slot -->
			<view @click="viewClick" slot="gridHeader" class="grid-header">
				<vol-alert>
					<view>1、移動端同樣支持編輯/新建/查詢數據源自動綁定。</view>
					<view>2、開發與配置方式與PC端相同,都由代碼生成器完成。</view>
					<view>3、[城市]字段省市區縣選擇</view>
					<view>4、2023.03.20更新components文件夾後才能使用</view>
					<view>5、省市區縣見App_TransactionAvgPriceExtend.js</view>
				</vol-alert>
			</view>

			<view slot="searchHeader">
				<vol-alert>
					<view>頁面已增加省市區縣選擇</view>
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
	import extend from './App_TransactionAvgPriceExtend.js'
	import options from './App_TransactionAvgPriceOptions.js';
	let _options = options();
	_options.extend = extend;
	export default {
		data() {
			return {
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
