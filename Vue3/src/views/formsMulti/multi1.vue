<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/appmanager/App_TransactionAvgPrice.js此處編寫
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
    import extend from "../formsMulti/multil1Extension/multi1.js"
    var vueParam = {
        data() {
            return {
                table: {
                    key: 'Id',
                    footer: "Foots",
                    cnName: '表單一對多table',
                    name: 'App_TransactionAvgPrice1',
                    url: "/App_TransactionAvgPrice/",
                    sortName: "Id"
                },
                extend:extend,
                editFormFields: {"Variety":"","AgeRange":"","City":"","AvgPrice":"","Date":"","IsTop":"","Creator":"","CreateDate":"","Modifier":"","ModifyDate":""},
                editFormOptions: [[{"dataKey":"pz","title":"種類","required":true,"field":"Variety","type":"select"},
                              {"dataKey":"city","title":"城市","required":true,"field":"City","type":"select"}],
                              [{"title":"價格","required":true,type:"number","field":"AvgPrice"},
                              {"title":"成交日期","required":true,"field":"Date","type":"date"}],
                              [{"dataKey":"enable","title":"狀態","required":true,"field":"IsTop","type":"select"},
                               {"type":"datetime","title":"創建時間","field":"CreateDate","disabled":true}]],
                searchFormFields: {"Variety":"","City":""},
                searchFormOptions: [[{"dataKey":"pz","title":"品種","field":"Variety","type":"drop"},{"dataKey":"city","title":"城市","field":"City","type":"dropList"}]],
                columns: [{field:'Id',title:'主鍵ID',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Variety',title:'種類',type:'string',bind:{ key:'pz',data:[]},width:90,require:true,align:'left',sortable:true},
                       {field:'City',title:'城市',type:'string',bind:{ key:'city',data:[]},width:90,require:true,align:'left'},
                       {field:'AvgPrice',title:'價格',type:'decimal',link:true,width:80,require:true,align:'left'},
                       {field:'Date',title:'成交日期',type:'datetime',width:90,require:true,align:'left',sortable:true},
                       {field:'IsTop',title:'狀態',type:'int',bind:{ key:'enable',data:[]},width:90,require:true,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'byte',bind:{ key:'enable',data:[]},width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true}],
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
