<template>
	<view class="form-test">
		<vol-alert>
			<view>vol-form設置readonly屬性只讀</view>
		</vol-alert>
		<vol-form  @onChange="onChange" :load-key="true" ref="form" :form-options.sync="editFormOptions"
			:formFields.sync="editFormFields">
		</vol-form>
		
		<vol-alert style="margin-top: 40rpx;">
			<view>標籤顯示在上方</view>
		</vol-alert>
		<vol-form  labelPosition="left" :load-key="true" ref="form2" :form-options.sync="editFormOptions"
			:formFields.sync="editFormFields">
		</vol-form>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				editFormFields: {
					inputText: "這是必填輸入框",
					pwd: "12345",
					selectVal: "1",
					selectListVal: [], //多選這裡的值是數組 
					dateValue: "2022-03-27",
					datetimeValue: "2022-03-27 20:15"
				},
				editFormOptions: [{
						"title": "輸入框",
						"required": true,
						"field": "inputText",
						readonly: true
					}, {
						"title": "密碼框",
						"field": "pwd",
						"type": "password",
						readonly: true
					}, {
						type: "group" //表單分組
					},
					{
						"title": "下拉框",
						"field": "selectVal",
						type: "select",
						"required": true,
						data: [],
						readonly: true,
						key: "enable"
					},
					{
						"title": "多選框",
						"field": "selectListVal",
						type: "selectList",
						"required": true,
						data: [],
						key: "pn",
						readonly: true
					},
					{
						type: "group" //表單分組
					},
					{
						"title": "日期",
						"required": true,
						"type": "date",
						"field": "dateValue",
						readonly: true
					},
					{
						"title": "日期時分秒",
						"type": "datetime",
						"field": "datetimeValue",
						readonly: true
					}
				]
			}
		},
		methods: {

		},
		onShow() {

		}
	}
</script>

<style lang="less" scoped>
	.form-test {
		margin-top: -20rpx;
		background: #fbfbfb;
		padding-top: 20rpx;
	    overflow: scroll;
	}
</style>