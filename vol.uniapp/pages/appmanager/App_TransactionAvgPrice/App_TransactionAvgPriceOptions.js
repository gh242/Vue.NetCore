//************************************************ 
//  *Author：jxx
//  *QQ：283591387
//  *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
//  *業務請在App_TransactionAvgPrice.js中編寫
//************************************************ 
export default function() {
	return {
		editFormFields: {"Variety":[],"AgeRange":"","City":"","AvgPrice":"","Date":"","IsTop":"","Creator":"","CreateDate":""},
		editFormOptions: [{"key":"pz","data":[],"title":"多選","required":true,"field":"Variety","type":"checkbox"},
                               {"key":"age","data":[],"title":"值範圍","required":true,"field":"AgeRange","type":"select"},
                               {"key":"city","data":[],"title":"城市","required":true,"field":"City","type":"select"},
                               {"title":"價格","required":true,"field":"AvgPrice","type":"decimal"},
                               {"title":"日期","required":true,"field":"Date","type":"date"},
                               {"key":"top","data":[],"title":"置頂","required":true,"field":"IsTop","type":"select"}],
		searchFormFields: {"Variety":"","AgeRange":"","City":"","Date":"","IsTop":"","Enable":""},
		searchFormOptions: [{"key":"pz","data":[],"title":"多選","field":"Variety","type":"select"},{"key":"age","data":[],"title":"值範圍","field":"AgeRange","type":"select"},{"key":"city","data":[],"title":"城市","field":"City","type":"select"},{"type":"group"},{"title":"日期","field":"Date","type":"datetime"},{"key":"top","data":[],"title":"置頂","field":"IsTop","type":"select"},{"key":"enable","data":[],"title":"是否啟用","field":"Enable","type":"select"}],
		columns: [{field:'Variety',title:'多選',type:'string',bind:{ key:'pz',data:[]}},
                       {field:'AgeRange',title:'值範圍',type:'string',bind:{ key:'age',data:[]}},
                       {field:'City',title:'城市',type:'string',bind:{ key:'city',data:[]}},
                       {field:'AvgPrice',title:'價格',type:'decimal',link:true},
                       {field:'Date',title:'日期',type:'date'},
                       {field:'IsTop',title:'置頂',type:'int',bind:{ key:'top',data:[]}},
                       {field:'Creator',title:'創建人',type:'string',readonly:true},
                       {field:'CreateDate',title:'創建時間',type:'datetime',readonly:true}],
		table: {
			key: 'Id',
			footer: "Foots",
			cnName: '自動綁定下拉框',
			name: 'App_TransactionAvgPrice',
			url: "/App_TransactionAvgPrice/",
			sortName: "Id"
		}
	}
}
