//************************************************ 
//  *Author：jxx
//  *QQ：283591387
//  *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
//  *業務請在App_Transaction.js中編寫
//************************************************ 
export default function() {
	return {
		editFormFields: {"Quantity":"","Describe":"","Name":"","PhoneNo":"","TransactionType":"","CowType":""},
		editFormOptions: [{"title":"姓名","required":true,"field":"Name"},
                               {"title":"電話","required":true,"field":"PhoneNo"},
                               {"type":"group"},
                               {"key":"cq","data":[],"title":"狀態","required":true,"field":"TransactionType","type":"select"},
                               {"key":"nav","data":[],"title":"類型","field":"CowType","type":"select"},
                               {"type":"group"},
                               {"title":"數量","required":true,"field":"Quantity","type":"number"},
                               {"title":"描述","required":true,"field":"Describe"}],
		searchFormFields: {"Name":"","PhoneNo":"","TransactionType":"","CowType":"","Creator":"","CreateDate":""},
		searchFormOptions: [{"title":"姓名","field":"Name","type":"text"},{"title":"電話","field":"PhoneNo","type":"text"},{"key":"cq","data":[],"title":"狀態","field":"TransactionType","type":"select"},{"type":"group"},{"key":"nav","data":[],"title":"類型","field":"CowType","type":"select"},{"title":"提交人","field":"Creator"},{"title":"提交時間","field":"CreateDate","type":"date"}],
		columns: [{field:'Name',title:'姓名',type:'string',link:true},
                       {field:'PhoneNo',title:'電話',type:'string'},
                       {field:'Quantity',title:'數量',type:'int'},
                       {field:'TransactionType',title:'狀態',type:'int',bind:{ key:'cq',data:[]}},
                       {field:'CreateDate',title:'提交時間',type:'date'}],
		table: {
			key: 'Id',
			footer: "Foots",
			cnName: '導入導出',
			name: 'App_Transaction',
			url: "/App_Transaction/",
			sortName: "Id"
		}
	}
}
