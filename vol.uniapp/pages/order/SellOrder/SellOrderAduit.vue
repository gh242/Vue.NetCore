<template>
	<view class="audit-container">
		<!-- 	這裡同樣直接複製不用修改 -->
		<view class="audit-form">
			<view style="font-weight: bolder;padding: 20rpx;">訂單信息</view>
			<vol-form ref="form" :form-options.sync="formOptions" :formFields.sync="formFields">
			</vol-form>
		</view>
		<view class="audit-action">
			<!-- 6、顯示審批操作（直接複製）,2022.09.26之前的代碼需要更新components文件架3 -->
			<vol-audit ref="audit"></vol-audit>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				//4、配置當前表要顯示的字段
				formFields: {
					"AuditStatus": "",
					"AuditDate": "",
					"OrderType": "",
					"TranNo": "",
					"Qty": "",
					"SellNo": ""
				},
				//5、配置當前顯示表單參數，可以直接從生成的SellOrderOptions.js頁面將editFormFields與editFormOptions複製過來
				formOptions: [{
						"key": "ordertype",
						"data": [],
						readonly: true,
						"title": "訂單類型",
						"field": "OrderType",
						"type": "select"
					},
					{
						"title": "運單號",
						readonly: true,
						"field": "TranNo"
					},
					{
						"title": "銷售數量",
						"field": "Qty",
						readonly: true
					},
					{
						"title": "銷售訂單號",
						"field": "SellNo",
						readonly: true
					},
					{
						"key": "audit",
						"data": [],
						"title": "審核狀態",
						"field": "AuditStatus",
						readonly: true
					},
					{
						"title": "審核時間",
						"field": "AuditDate",
						readonly: true
					},
				]
			}
		},
		methods: {},
		onLoad(options) {
			//7、配置查詢條件Order_Id改為實際表的主鍵字段
			let wheres = [{
				name: "Order_Id",
				value: options.orderId
			}]
			let params = {
				page: 1,
				rows: 30,
				wheres: JSON.stringify(wheres)
			};
			//7、調用後台接口返回數據，將SellOrder改為當前表名
			this.http.post('api/SellOrder/getPageData', params, true).then(result => {
				if (!result.rows.length) {
					this.$toast('未查到數據')
					return;
				}
				Object.assign(this.formFields, result.rows[0]);
				//獲取審批信息
				this.$nextTick(() => {
					//第一個參數為當前表的主鍵字段的值(SellOrderOptions.js執行跳轉的時候已經傳進來了)
					//第二個參數當前操作的表名
					this.$refs.audit.load(options.orderId, 'SellOrder');
				})
			})
		}
	}
</script>

<style lang="less" scoped>
	.audit-container {
		position: absolute;
		height: 100%;
		width: 100%;
		overflow-y: scroll;
		background: #f9f9f9;
		padding: 20rpx;
		box-sizing: border-box;

		.audit-form,
		.audit-action {
			border-radius: 5px;
			background: #ffff;
		}
	}
</style>
