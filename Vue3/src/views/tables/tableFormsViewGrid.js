var viewGridOptions = {  //此處的權限是使用的當前頁面的權限，而不是App_Transaction表的權限
            table: {
                key: 'Id',
                footer: "Foots",
                cnName: 'table基礎配置',
                name: 'App_Transaction',
                url: "/App_Transaction/",
                sortName: "Id"
            },
            extend: { 
                text:"此組件配置與4代碼生成器生成的配置相同,自帶基礎操作,可任意擴展",
                buttons: { //擴展按鈕
                   view: [//ViewGrid查詢介面按鈕
                   {
                     name: "擴展按鈕!",
                     icon: 'md-create',
                     value: 'Edit',
                     class: '',
                     type: 'error',
                     index: 1,//顯示的位置
                     onClick: function () { //擴展按鈕執行事件
                       this.$Message.error("擴展按鈕")
                    }
                  }]
                },
                methods: {//事件擴展
                    onInit(){
                        this.tableMaxHeight=500;
                    },
                    searchAfter(result) { //查詢ViewGird表數據後param查詢參數,result回返查詢的結果
                     console.log({ title: this.table.cnName + ',查詢結果', desc: '返回的對象：' + JSON.stringify(result) });
                        return true;
                    }
                }
            },
            editFormFields: {"Name":"","TransactionType":"","CowType":"",PhoneNo:"",Describe:""},
            editFormOptions: [[{"title":"姓名","required":true,"field":"Name"}],
                          [{"dataKey":"cq","title":"是否買入","field":"TransactionType","type":"select"}],
                          [{"dataKey":"nav","title":"購買類型","field":"CowType","type":"select"}],
                          [{"type":"phone","title":"電話","field":"PhoneNo","required":true}],
                          [{"type":"textarea","title":"描述","field":"Describe","required":true}]],
            searchFormFields: {"CowType":"","Creator":"","CreateDate":""},
            searchFormOptions: [[{"dataKey":"nav","title":"購買類型","field":"CowType","type":"dropList"},{"title":"提交人","field":"Creator"},{"title":"提交時間","field":"CreateDate","type":"datetime"}]],
            columns: [{field:'Id',title:'主鍵ID',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                   {field:'Name',title:'姓名',type:'string',width:120,require:true,align:'left',sortable:true},
                   {field:'PhoneNo',title:'電話',type:'string',link:true,width:150,require:true,align:'left'},
                   {field:'Quantity',title:'數量',type:'int',width:90,require:true,align:'left'},
                   {field:'TransactionType',title:'是否買入',type:'int',bind:{ key:'cq',data:[]},width:120,align:'left'},
                   {field:'CowType',title:'購買類型',type:'string',bind:{ key:'nav',data:[]},width:90,align:'left'},
                   {field:'Describe',title:'描述',type:'string',width:190,require:true,align:'left'},
                   {field:'Enable',title:'是否啟用',type:'byte',width:90,hidden:true,align:'left'},
                   {field:'CreateID',title:'創建人Id',type:'int',width:90,hidden:true,align:'left'},
                   {field:'Creator',title:'提交人',type:'string',width:130,align:'left'},
                   {field:'CreateDate',title:'提交時間',type:'datetime',width:150,align:'left',sortable:true},
                   {field:'Modifier',title:'修改人',type:'string',width:130,hidden:true,align:'left'},
                   {field:'ModifyDate',title:'修改時間',type:'datetime',width:150,hidden:true,align:'left',sortable:true}],
            detail: {
                cnName:"",
                columns: [],
                sortName: "",
                key:""
            }
};
export default viewGridOptions;