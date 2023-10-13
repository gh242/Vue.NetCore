<template>
	<view class="table-demo">
		<view style="padding:20rpx 20rpx 10rpx 20rpx;">
			<vol-alert>
				<view>1、完全自定義vol-table列表内容及樣式,見:table4.vue</view>
				<view>2、内部已處理高度自适應、自動分頁、數據源自動加載</view>
				<view>3、此示例通過tableData屬性手動綁定數據</view>
			</vol-alert>
		</view>

		<vol-table @scrolltolower="scrolltolower" :tableData="tableData" custom ref="table" :columns.sync="columns">
			<!-- 	自定義内容 -->
			<template v-slot:data="{rows}">
				<view @click="rowClick(row,index)" v-for="(row,index) in rows" class="grid-item">
					<view class="grid-content">
						<view class="grid-title">{{index+1}}、{{row.title}}</view>
						<view class="grid-bottom">
							<view class="grid-bottom-item">
								<view>發佈人</view>
								<view>{{row.creator}}</view>
							</view>
							<view class="grid-bottom-item">
								<view>發佈時間</view>
								<view>{{row.createDate}}</view>
							</view>
							<view class="grid-bottom-item">
								<view>修改人</view>
								<view>{{row.creator}}</view>
							</view>
							<view class="grid-bottom-item">
								<view>修改時間</view>
								<view>{{row.createDate}}</view>
							</view>
							<!-- 小程序不支持標籤裡面調用方法。manifest.json並且要配置	"scopedSlotsCompiler":"legacy",屬性 -->
						</view>
						<view class="btns">
							<view class="btn-item">
								<u-button @click="rowClick(row,index)" shape="circle" type="primary" size="mini">
									<u-icon name="file-text" size="12" color="#ffff"></u-icon>查看
								</u-button>
							</view>
							<view class="btn-item">
								<u-button @click="delClick(row,index)" shape="circle" type="error" size="mini">
									<u-icon name="trash" size="12" color="#ffff"></u-icon>刪除
								</u-button>
							</view>
						</view>
					</view>
				</view>
			</template>
		</vol-table>

		<u-modal showCancelButton @confirm="confirm" @cancel="show=false" :show="show" title="警告" content='確定要刪除數據嗎'>
		</u-modal>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				show: false,
				page: 0,
				tableData: [],
				columns: [{
						field: 'id',
						title: '主鍵ID'
					},
					{
						field: 'title',
						title: '標題'
					},
					{
						field: 'creator',
						title: '發佈人',
					},
					{
						field: 'createDate',
						title: '發佈時間',
						type: 'datetime',
					},
					{
						field: 'modifier',
						title: '修改人',
						type: 'string',
						width: 130,
						hidden: true,
						align: 'left'
					},
					{
						field: 'modifyDate',
						title: '修改時間',
						type: 'datetime'
					}
				],
			}
		},
		onLoad() {
			this.getData()
		},
		methods: {
			//分頁事件
			scrolltolower() {
				this.getData();
			},
			getData() {
				if (this.page < 0) {
					return;
				}
				this.page++;
				let url = 'api/app_news/getList?newsType=1&&page=' + this.page;
				this.http.get(url, {}, false).then(result => {
					result.forEach(item => {
						item.imageUrl = this.http.ipAddress + item.imageUrl;
						item.createDate = (item.createDate || '').substr(0, 10);
						item.modifyDate = (item.modifyDate || '').substr(0, 10);
					})
					if (!result.length) {
						this.page = -1;
					}
					this.tableData.push(...result);
				})
			},
			rowClick(row, index) {
				console.log('點擊了第' + index + '行')
				return this.$toast('點擊了第' + index + '行');
			},
			delClick(row, index) {
				this.row = row;
				this.index = index;
				this.show = true;
			},
			confirm() {
				this.tableData.splice(this.index, 1);
				this.show = false;
				this.$toast('刪除成功')
			}
		}
	}
</script>

<style lang="less" scoped>
	.table-demo {
		background: #f4f4f4;
	}

	.grid-item {
		margin: 10rpx 20rpx;
		padding: 16rpx;
		border-radius: 8rpx;
		display: flex;
		background: #ffff;

		.grid-content {
			display: flex;
			flex: 1;
			flex-direction: column;
			padding-left: 20rpx;

			.grid-title {

				font-size: 28rpx;
				overflow: hidden;
				display: -webkit-box;
				-webkit-line-clamp: 2;
				-webkit-box-orient: vertical;
			}

			.grid-bottom {
				flex: 1;
				align-items: flex-end;
				padding-top: 10rpx;
				display: flex;
				font-size: 24rpx;
				color: #959595;

				.grid-bottom-item {
					flex: 1;
					padding-right: 20rpx;
				}

				.grid-bottom-item>view:first-child {
					color: #353535;
					font-size: 22rpx;
				}
			}
		}
	}

	.btns {
		padding: 20rpx 20rpx 0 0;
		display: flex;
		justify-content: flex-end;

		.btn-item {
			margin-left: 20rpx;
		}
	}
</style>
