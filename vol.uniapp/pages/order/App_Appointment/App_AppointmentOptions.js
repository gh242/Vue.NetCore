//************************************************ 
//  *Author：jxx
//  *QQ：283591387
//  *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
//  *業務請在App_Appointment.js中編寫
//************************************************ 
export default function() {
	return {
		editFormFields: {"Name":"","PhoneNo":"","Describe":"","CreateDate":"","Creator":""},
		editFormOptions: [{"title":"姓名","required":true,"field":"Name","disabled":true},
                               {"type":"group"},
                               {"title":"電話","required":true,"field":"PhoneNo","disabled":true},
                               {"title":"描述","required":true,"field":"Describe","disabled":true}],
		searchFormFields: {"Name":"","PhoneNo":"","CreateDate":"","Creator":""},
		searchFormOptions: [{"title":"姓名","field":"Name"},{"title":"電話","field":"PhoneNo"},{"title":"創建時間","field":"CreateDate","type":"datetime"},{"title":"創建人","field":"Creator"}],
		columns: [{field:'Name',title:'姓名',type:'string',link:true,readonly:true},
                       {field:'PhoneNo',title:'電話',type:'string',readonly:true},
                       {field:'Describe',title:'描述',type:'string',readonly:true},
                       {field:'CreateDate',title:'創建時間',type:'datetime',readonly:true},
                       {field:'Creator',title:'創建人',type:'string',readonly:true},
                       {field:'Modifier',title:'修改人',type:'string',readonly:true},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',readonly:true}],
		table: {
			key: 'Id',
			footer: "Foots",
			cnName: '基礎表單+編輯只讀',
			name: 'App_Appointment',
			url: "/App_Appointment/",
			sortName: "CreateDate"
		}
	}
}
