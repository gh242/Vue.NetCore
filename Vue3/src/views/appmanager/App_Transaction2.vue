<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/appmanager/App_Transaction.js此處編寫
 -->
<template>
    <div>
        <view-grid :columns="columns"
                   :detail="detail"
                   :editFormFields="editFormFields"
                   :editFormOptions="editFormOptions"
                   :searchFormFields="searchFormFields"
                   :searchFormOptions="searchFormOptions"
                   :table="table"
                   :extend="extend">
        </view-grid>
    </div>
</template>

<script>
    import extend from "@/extension/appmanager/App_Transaction2.js";
    var vueParam = {
        data() {
            return {
                table: {
                    key: 'Id',
                    footer: "Foots",
                    cnName: '表格編輯',
                    name: 'App_Transaction',
                    url: "/App_Transaction/",
                    sortName: "Id"
                },
                extend: extend,
                editFormFields: {"Name":"","TransactionType":"","CowType":"","PhoneNo":"","Describe":""},
                editFormOptions: [[{"title":"姓名","required":true,"field":"Name"}],
                              [{"dataKey":"cq","data":[],"title":"是否買入","field":"TransactionType","type":"select"}],
                              [{"dataKey":"nav","data":[],"title":"購買類型","field":"CowType","type":"select"}],
                              [{"title":"電話","required":true,"field":"PhoneNo","type":"phone"}],
                              [{"title":"描述","required":true,"field":"Describe","type":"textarea"}]],
                searchFormFields: {"Name":"","PhoneNo":"","TransactionType":"","CowType":[],"Creator":"","CreateDate":""},
                searchFormOptions: [[{"title":"姓名","field":"Name","type":"like"},{"title":"電話","field":"PhoneNo","type":"text"},{"dataKey":"cq","data":[],"title":"是否買入","field":"TransactionType","type":"select"}],[{"dataKey":"nav","data":[],"title":"購買類型","field":"CowType","type":"selectList"},{"title":"提交人","field":"Creator"},{"title":"提交時間","field":"CreateDate","type":"datetime"}]],
                columns: [{field:'Id',title:'主鍵ID',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Name',title:'姓名',type:'string',width:120,require:true,align:'left',sortable:true},
                       {field:'PhoneNo',title:'電話',type:'string',link:true,width:130,require:true,align:'left'},
                       {field:'Quantity',title:'數量',type:'int',width:130,require:true,align:'left'},
                       {field:'TransactionType',title:'是否買入',type:'int',bind:{ key:'cq',data:[]},width:80,align:'left'},
                       {field:'CowType',title:'購買類型',type:'string',bind:{ key:'nav',data:[]},width:130,align:'left'},
                       {field:'Describe',title:'描述',type:'string',width:120,require:true,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'byte',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人Id',type:'int',width:90,hidden:true,align:'left'},
                       {field:'CreateDate',title:'提交時間',type:'datetime',width:150,align:'left',sortable:true},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:150,hidden:true,align:'left',sortable:true}],
                detail: {
                    cnName:"#detailCnName",
                    columns: [],
                    sortName: "",
                    key:""
                }
            };
        }
    };
    export default vueParam;
</script>
