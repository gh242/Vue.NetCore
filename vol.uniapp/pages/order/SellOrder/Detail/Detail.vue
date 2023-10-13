<template>
	<view class="order-detail">
		<vol-alert>
			<view>1、訂單詳情頁面由自己維護</view>
			<view>2、框架已封装vol-table與vol-form組件可直接使用</view>
			<view>3、可參照此頁面實現一對一或者一對多</view>
		</vol-alert>
		<!--主表信息 -->
		<view class="order-main">
			<view class="title">訂單信息</view>
			<vol-form class="main-form" :load-key="true" ref="form" :form-options="editFormOptions"
				:formFields.sync="editFormFields">
			</vol-form>
		</view>
		<!-- 明細列表信息 -->
	
		<view class="order-detail-list">
			<view class="detail-btns">
				<view class="detail-btn" @click="showDetailBtnClick">
					<u-button icon="plus" type="primary" shape="circle" size="small" text="添加"></u-button>
				</view>
				<view class="detail-btn" @click="save">
					<u-button icon="checkmark" type="success" shape="circle" size="small" text="保存"></u-button>
				</view>
			</view>
			<u-sticky bgColor="#fff">
				<u-tabs :list="list"></u-tabs>
			</u-sticky>
			<vol-table :height="300" @rowClick="rowClick" :tableData="rows" direction="horizontal"
				:columns.sync="columns" ref="table">
			</vol-table>
		</view>

		<u-popup @touchmove.prevent class="form-popup" :zIndex="999999" :show="detailModel" @close="detailModel=false;">
			<view class="vol-action-sheet-select-container" style="max-height:500px">
				<view class="vol-action-sheet-select-title" @click="detailModel=false;">明細操作
					<text class="vol-action-sheet-select-confirm">取消</text>
				</view>
				<vol-form :load-key="true" ref="detail" :form-options="detailFormOptions"
					:formFields.sync="detailFormFields">
				</vol-form>
				<view style="padding: 15px;">
					<view  v-show="!isAdd" style="margin-bottom: 28rpx;">
						<u-button @click="showDel=true" icon="trash" type="error"  shape="circle" text="刪除"></u-button>
					</view>
					<u-button @click="addRow" icon="checkmark" type="primary" shape="circle" text="確認"></u-button>
				</view>
			</view>
		</u-popup>
		<!-- 刪除提示 -->
		<u-modal :show="showDel" cancelText="取消" class="del-u-modal" :showCancelButton="true" :showConfirmButton="true"
			@cancel="showDel=false" @confirm="confirmDel" title="警告">
			<view style="color: red;">確定要刪除此數據嗎!</view>
		</u-modal>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				showDel: false, //刪除提示框
				currentRow: {},
				currendtIndex: -1, //當前編輯或刪除的是第幾行
				isAdd: false, //當前是操作是新建還是編輯
				orderId: null, //編輯時傳過來的訂單id
				list:[{
						name: '訂單明細',
					},{
						name: '訂單詳情',
					},{
						name: '訂單統計',
					}],
				//主表配置
				editFormFields: {
					"OrderType": "",
					"TranNo": "",
					"Qty": "",
					"SellNo": ""
				},
				editFormOptions: [{
						"key": "ordertype",
						"data": [],
						"title": "訂單類型",
						"required": true,
						"field": "OrderType",
						"type": "select"
					},
					{
						"title": "Id",
						"required": true,
						"field": "ttt",
						hidden: true
					},
					{
						"title": "運單號",
						"required": true,
						"field": "TranNo"
					},
					{
						"title": "銷售數量",
						"required": true,
						"field": "Qty",
						"type": "number"
					},
					{
						"title": "銷售訂單號",
						"required": true,
						"field": "SellNo"
					}
				],
				//明細表配置，具體見表單vol-form菜單
				rows: [], //數據
				columns: [{
						field: 'OrderList_Id',
						title: '明細表主鍵',
						hidden: true
					},
					{
						field: 'ProductName',
						title: '商品名稱',
						type: "select",
						bind: {
							key: "pn",
							data: []
						},
					},
					{
						field: 'MO',
						title: '批次',
						type: 'string',
						require: true
					},
					{
						field: 'Qty',
						title: '數量',
						type: 'int',
						require: true
					},
					{
						field: 'Creator',
						title: '創建人',
						type: 'string',
						readonly: true
					}
				],
				detailModel: false,
				detailFormFields: {
					ProductName: "",
					MO: "",
					Qty: 0
				},
				detailFormOptions: [{
						field: 'ProductName',
						title: '商品名稱',
						type: "select",
						"data": [],
						key: "pn",
					},
					{
						field: 'MO',
						title: '批次',
						type: 'string',
						require: true
					},
					{
						field: 'Qty',
						title: '數量',
						type: 'int',
						require: true
					}
				],
			}
		},
		methods: {
			save() { //保存
				let url = ''
				if (this.orderId) { //編輯操作
					url = "api/SellOrder/update"
				} else {
					url = "api/SellOrder/add"
				}
				let params = {
					mainData: this.editFormFields,
					detailData: this.rows
				}
				this.http.post(url, params, true).then(result => {
					this.$toast(result.message);
					if (!result.status) {
						return;
					}
					this.getOrderData();
					this.getOrderListData();
					//保存成功後刷新頁面數據
				})
			},
			confirmDel() { //刪除行數據
				let url = "api/SellOrder/delDetail?orderList_Id=" + this.currentRow.OrderList_Id;
				//從後台刪除數據，這裡自己寫下delDetail接口
				// this.http.get(url,{},true).then(result=>{
				//  if(result.status){
				this.showDel = false;
				this.detailModel=false;
				this.rows.splice(this.currnetDelIndex, 1);
				this.$toast("刪除成功");
				return;
				//  }
				// })
			},
			showDetailBtnClick() { //添加行
				this.isAdd = true;
				this.showDetail(-1, {
					ProductName: '',
					MO: null,
					Qty: null
				});
			},
			rowClick(index, row) { //點擊新建或者編輯彈出明細表的操作
				this.isAdd = false;
				this.showDetail(index, row);
			},
			showDetail(index, row) {
				this.currendtIndex = index;
				this.detailFormFields = JSON.parse(JSON.stringify(row));
				this.detailModel = true;
			},
			addRow() { //彈出框點擊的確定
				if (!this.$refs.detail.validate()) {
					return false
				}
				//編輯操作
				if (this.currendtIndex != -1) {
					Object.assign(this.rows[this.currendtIndex], this.detailFormFields);
				} else { //添加行的數據
					this.rows.push(this.detailFormFields);
				}
				this.detailModel = false
			},
			getOrderData() { //獲取主表數據(現在用的是框架的方法，可以自己寫接口返回數據)
				let params = {
					page: 1,
					row: 1
				};
				//生成查詢參數
				params.wheres = JSON.stringify([{
					name: "Order_Id",
					value: this.orderId
				}]);
				this.http.post("api/SellOrder/getPageData", params, true).then(result => {
					Object.assign(this.editFormFields, result.rows[0])
				})
			},
			getOrderListData() { //獲取明細表數據(現在用的是框架的方法，可以自己寫接口返回數據)
				//此頁面沒做分頁，可以用uview的list來處理分頁
				let params = {
					page: 1,
					rows: 30
				};
				//生成查詢參數
				params.value = this.orderId;
				this.http.post("api/SellOrder/getDetailPage", params, true).then(result => {
					this.rows = result.rows;
				})
			}
		},
		onShow() {
			//獲取主表主鍵值
			let routes = getCurrentPages(); // 獲取當前打開過的頁面路由數組
			let params = routes[routes.length - 1].options; //獲取路由參數 
			if (params && params.orderId) {
				this.orderId = params.orderId;
				//獲取主表數據
				this.getOrderData();
				//獲取明細表數據
				this.getOrderListData();
			} else {
				this.orderId = null;
			}
			uni.setNavigationBarTitle({
				title: this.orderId ? '訂單編輯' : '新建訂單'
			})
		}
	}
</script>

<style scoped lang="less">
	.order-detail {
		margin-top: -20rpx;
		background: #fbfbfb;
		padding-top: 20rpx;
		overflow-y: scroll;
		overflow-x: hidden;
	}

	.order-main,
	.order-detail-list {
		// margin: 20rpx;
		border-radius: 10rpx;
	}

	.main-form,
	.detail-form {
		border: 1px solid #ebebeb;
		border-bottom: 0;
		border-radius: 4px;
		display: inline-block;
		width: 100%;
	}

	.title {
		text-align: left;
		margin: 9px 0 7px 0;
		font-size: 15px;
		border-left: 8px solid #00aaff;
		line-height: 16px;
		padding-left: 5px;
		display: flex;
		position: relative;

		.detail-icon {
			position: absolute;
			width: 60rpx;
			right: 0;
			height: 44rpx;
		}
	}

	.detail-form {
		margin-bottom: 30rpx;
	}

	.detail-btns {
		margin: 15rpx 8rpx;
		display: flex;

		.detail-btn {
			margin-left: 30rpx;
		}
	}
</style>
