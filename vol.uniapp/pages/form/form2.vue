<template>
	<view class="form-test">
		<vol-alert>
			<view>vol-form封装了事件綁定、下拉框自動綁定數據源、級聯組件、多選、單選、日期、日期範圍選擇等常用組件操作,見form2.vue文件</view>
		</vol-alert>
		<vol-form @input-confirm="inputConfirm" @extraClick="extraClick" @onChange="onChange" :load-key="true"
			ref="form" :form-options.sync="editFormOptions" :formFields.sync="editFormFields">
		</vol-form>

		<view class="btns">
			<view class="btn">
				<u-button type="primary" @click="reset" shape="circle" text="重置表單"></u-button>
			</view>
			<view class="btn">
				<u-button type="success" @click="vailForm" shape="circle" text="校驗表單"></u-button>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				test: {
					a: 1,
					b: 2
				},
				editFormFields: {
					inputText: "",
					inputText2: "",
					customInput: "",
					textarea: "這裡的文字有點長文字有點長。。",
					pwd: "12345",
					readonlyText: "只讀輸入框",
					cascader1: null,
					cascader2: null,
					cascader3: '004',
					selectVal: "",
					selectListVal: [], //多選這裡的值是數組 
					dateValue: this.base.getDate(), //設置默認日期為當天
					datetimeValue: "2022-03-27 20:15",
					dateRange: ["2022-03-10", "2022-06-20"], //數組 
					inputRange: [100000000, 900000000], //區間是數組


					province: "北京市,北京市,海淀區", //省市區縣值必须以逗號隔開

					inputDecimal: null, //小數
					inputNumber: null, //數字
					switchValue: 1,
					radioVal: null, //單選
					selectClickValue: "",
					dateClickValue: null,
					imgs: [{
						url: "http://api.volcore.xyz/Upload/Tables/Sys_User/202006191408112343/1111s.jpg"
					}, {
						url: "http://api.volcore.xyz/Upload/Tables/App_News/202204201140571762/20-05.png"
					}]
				},
				editFormOptions: [{
						"title": "輸入框",
						"required": true,
						"field": "inputText"
					},

					{
						"title": "自定義按鈕",
						"field": "customInput",
						extra: {
							style: "background: #00aaff;margin-left:16rpx;border-radius: 30rpx;font-size: 24rpx;padding: 4rpx 16rpx;color: #ffff;",
							text: "按鈕",
							icon: "map",
							color: "#ffff",
							size: 12
						}
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "選中輸入框,鍵盤點擊搜索或者掃描槍掃描觸發回車事件"
					},
					{
						"title": "回車事件",
						"required": false,
						"field": "inputText2"
					},
					{
						"title": "表單字段定義按鈕及點擊事件,示例見form2.vue",
						style: "padding-left:16rpx;font-weight: 500;color: #9e9e9e;font-size: 26rpx;",
						type: "group"
					},
					{
						"title": "多文本",
						"field": "textarea",
						type: "textarea"
					}, {
						"title": "密碼框",
						"field": "pwd",
						"type": "password"
					},
					{
						"title": "只讀框",
						"field": "readonlyText",
						"type": "text",
						readonly: true
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "省市區縣type設置city(2023.03.20更新components文件夾)"
					},
					{
						"title": "省市區縣",
						"field": "province",
						type: "city" //type必须為city
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "設置checkStrictly=true,只能選擇最後一級節點"
					},
					{
						"title": "樹形級聯",
						"field": "cascader1",
						type: "cascader",
						"required": true,
						checkStrictly: true, //是否只能選擇最後一個節點,默認可以選擇任意節點
						data: [],
						key: "tree_roles"
					},

					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "設置checkStrictly=false,可以選擇任意節點"
					},
					{
						"title": "樹形級聯2",
						"field": "cascader2",
						type: "cascader",
						"required": true,
						checkStrictly: false, //是否只能選擇最後一個節點,默認可以選擇任意節點
						data: [],
						key: "tree_roles"
					},

					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "自定義級聯data數據源，格式見：form2.vue文件"
					},
					{
						"title": "自定義級聯",
						"field": "cascader3",
						type: "cascader",
						"required": true,
						checkStrictly: false, //是否只能選擇最後一個節點,默認可以選擇任意節點
						data: [{
							id: "001",
							parentId: null,
							name: "一級節點"
						}, {
							id: "002",
							parentId: "001",
							name: "二級節點"
						}, {
							id: "003",
							parentId: null,
							name: "三級節點"
						}, {
							id: "004",
							parentId: "003",
							name: "四級節點"
						}]
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "下拉框綁定"
					},
					{
						"title": "下拉框",
						"field": "selectVal",
						type: "select",
						"required": true,
						data: [],
						key: "pn"
					},
					{
						"title": "多選框",
						"field": "selectListVal",
						type: "selectList",
						"required": true,
						data: [],
						key: "pn"
					},
					{
						type: "group" //表單分組
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "日期設置min與max屬性限制選擇範圍"
					},
					{
						"title": "日期",
						"required": true,
						"type": "date",
						"field": "dateValue",
						//設置時間選擇範圍，如果日期是datetim類型，時間後面加上時分秒
						//2023.04.02更新util->common.js才能使用獲取日期的方法
						// min:'2023-04-01',
						// max:'2023-07-02'

						//設置只能選擇半個月内的數據
						min: this.base.addDay(this.base.getDate(), -15),
						max: this.base.getDate()
					},
					{
						"title": "日期時分秒",
						"type": "datetime",
						"field": "datetimeValue"
					},
					{
						"title": "日期範圍",
						"type": "date",
						range: true, //區間輸入
						"field": "dateRange"
					},
					{
						"title": "區間輸入",
						"type": "decimal", //number數字，text文本輸入
						range: true, //區間輸入
						"field": "inputRange"
					},
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "只能輸入小數與整數(在手機上查看)"
					},
					{
						"title": "小數",
						"type": "decimal",
						field: "inputDecimal" //只能輸入小數

					},
					{
						"title": "整數",
						"type": "number",
						field: "inputNumber" //只能輸入數字 
					},
					{
						type: "group" //表單分組
					},
					//placement
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "單選添加事件,並隱藏【單選】字段"
					},
					{
						"title": "是否值",
						"type": "switch",
						"field": "switchValue"
					},

					{
						"title": "單選",
						"type": "radio",
						data: [],
						key: "pn",
						placement:'row',//布局方式，row横向，column纵向	,具體見uvivew文欄
						//placement:"column",	
						"field": "radioVal"
					},
					//placement
					{
						type: "group", //表單分組
						style: "margin-top: 10px;font-weight: 500;font-size: 26rpx;color: #848383;",
						title: "註冊選擇事件,見onChange方法說明"
					},
					{
						"title": "下拉框事件",
						"type": "select",
						"field": "selectClickValue",
						data: [],
						key: "pn"
					},
					{
						"title": "日期事件",
						"type": "date",
						"field": "dateClickValue"
					}, {
						type: "group" //表單分組
					},
					{
						"title": "圖片上傳",
						"type": "img",
						//readonly:true,//設置圖片只讀
						"url": "api/sys_user/upload", //後台框架自帶的上傳方法，如果涉及權限問題，請參照後台開發文欄上重寫權限來重寫upload方法的權限
						"multiple": true, //從圖上傳
						"maxCount": 3, //最多只能上傳3張圖片
						"field": "imgs"
					},
				],
			}
		},
		methods: {
			extraClick(item) {
				//點擊後給表字段設置值
				this.editFormFields.customInput = ~~(Math.random() * 10000000)
				this.$toast('表單按鈕點擊:' + item.title)
			},
			onChange(field, value, item, data) { //日期與下拉框選擇事件
				if (field == "selectClickValue" || field == "dateClickValue") {
					this.$toast(`選擇字段${field}值${value}`)
					return;
				}
				if (field == 'switchValue') {
                    //獲取單選字段配置
					let op = this.editFormOptions.find(c => {
						return c.field == 'radioVal'
					});
					//選擇是時，隱藏【單選】字段
					this.$set(op, 'hidden', value + '' === "1")
					return;
				}
			},
			vailForm() {
				if (this.$refs.form.validate()) {
					this.$toast("表單校驗成功")
				}
			},
			reset() {
				this.$refs.form.reset();
				this.$toast("表單已重置")
			},
			inputConfirm(field, e) {
				this.$toast(`字段${field}回車事件`)
				console.log(field)
			}
		},
		onShow() {

		},
		onReady() {
			//設置表單默認值請寫在onReady中
			this.editFormFields.inputText = 'vol框架';
		},
		onLoad() {

		}
	}
</script>

<style lang="less" scoped>
	.form-test {
		height: auto;
		margin-top: -20rpx;
		background: #f6f6f6;
		padding-top: 20rpx;
	}

	.btns {
		display: flex;
		padding: 0rpx 20rpx;

		.btn {
			flex: 1;
			padding: 20rpx;
		}
	}
</style>
