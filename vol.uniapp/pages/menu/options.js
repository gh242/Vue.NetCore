//************************************************ 
//  *Author：jxx
//  *QQ：283591387
//  *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
//  *業務請在#TableName.js中編寫
//************************************************ 
export default function() {
	return {
		columns: [{
				field: 'Id',
				title: '主鍵ID',
				type: 'int',
				hidden: true,
				readonly: true,
				require: true,
				align: 'left'
			},
			{
				field: 'Name',
				title: '姓名',
				type: 'string',
				width: 140,
				require: true,
				sort: true
			},
			{
				field: 'PhoneNo',
				title: '電話',
				type: 'string',
				link: true,
				width: 90,
				require: true,
				align: 'left'
			},
			{
				field: 'Describe',
				title: '描述',
				type: 'string',
				require: true,
				sort: true,
				align: 'left'
			},
			{
				field: 'Creator',
				title: '創建人',
				sort: true,
				type: 'string'
			},
			{
				field: 'CreateDate',
				title: '創建時間',
				type: 'date'
			}
		],
		detail: [],
		editFormFields: {
			"Name": "",
			"Id": "",
			"Describe": "",
			"PhoneNo": "",
			"CreateDate": "",
			"Creator": "",
			"Modifier": "",
			"ModifyDate": "",
			Enable: 1
		},
		editFormOptions: [{
				"title": "姓名",
				"required": true,
				"field": "Name",
				"disabled": true
			}, {
				"title": "描述",
				"required": true,
				"field": "Describe",
				"disabled": true
			},
			{
				"title": "電話",
				"required": true,
				"field": "PhoneNo",
				"disabled": true
			}, {
				type: 'group'
			}, {
				"title": "創建時間",
				"field": "CreateDate",
				type: 'date'
			}, {
				"title": "是否值",
				"field": "Enable",
				type: 'number',
				// data:[{key:1,value:'是'},{key:0,value:'否'}]
			}
		],
		searchFormFields: {
			"Name": "",
			"Id": "",
			"Describe": "",
			"PhoneNo": "",
			"CreateDate": ["", ""],
			"Creator": "",
			"Modifier": "",
			"ModifyDate": "",
			Enable: 1
		},
		searchFormOptions: [{
				"title": "姓名",
				"required": true,
				"field": "Name",
				"disabled": true
			},
			{
				"title": "電話",
				"required": true,
				"field": "PhoneNo",
				"disabled": true
			}, {
				"title": "創建時間",
				"field": "CreateDate",
				range: true,
				type: 'date'
			}, {
				"title": "是否值",
				"field": "Enable",
				type: 'select',
				data: [{
					key: 1,
					value: '是'
				}, {
					key: 0,
					value: '否'
				}]
			}
		],
		table: {
			key: '#key',
			footer: "Foots",
			cnName: '#cnName',
			name: '#TableName',
			url: "#url",
			sortName: "#SortName"
		}
	}
}
