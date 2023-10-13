//************************************************ 
//  *Author：jxx
//  *QQ：283591387
//  *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
//  *業務請在SellOrder.js中編寫
//************************************************ 
export default function() {
	return {
		editFormFields: {"AuditStatus":"","AuditDate":"","OrderType":"","TranNo":"","Qty":"","SellNo":""},
		editFormOptions: [{"key":"audit","data":[],"title":"審核狀態","required":true,"field":"AuditStatus","type":"number"},
                               {"title":"審核時間","field":"AuditDate"},
                               {"type":"group"},
                               {"key":"ordertype","data":[],"title":"訂單類型","required":true,"field":"OrderType","type":"select"},
                               {"title":"運單號","required":true,"field":"TranNo"},
                               {"title":"銷售數量","required":true,"field":"Qty","type":"number"},
                               {"type":"group"},
                               {"title":"銷售訂單號","required":true,"field":"SellNo"}],
		searchFormFields: {"OrderType":"","TranNo":"","SellNo":"","Qty":"","AuditStatus":"","AuditDate":""},
		searchFormOptions: [{"title":"銷售數量","field":"Qty","type":"number"},{"type":"group"},{"title":"運單號","field":"TranNo"},{"title":"銷售訂單號","field":"SellNo"},{"key":"ordertype","data":[],"title":"訂單類型","field":"OrderType","type":"select"},{"type":"group"},{"key":"audit","data":[],"title":"審核狀態","field":"AuditStatus","type":"select"},{"title":"審核時間","field":"AuditDate","type":"datetime"}],
		columns: [{field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'ordertype',data:[]}},
                       {field:'TranNo',title:'運單號',type:'string',link:true},
                       {field:'SellNo',title:'銷售訂單號',type:'string'},
                       {field:'Qty',title:'銷售數量',type:'int'},
                       {field:'AuditStatus',title:'審核狀態',type:'int',bind:{ key:'audit',data:[]}},
                       {field:'AuditDate',title:'審核時間',type:'datetime'}],
		table: {
			key: 'Order_Id',
			footer: "Foots",
			cnName: '銷售訂單',
			name: 'SellOrder',
			url: "/SellOrder/",
			sortName: "CreateDate"
		}
	}
}
