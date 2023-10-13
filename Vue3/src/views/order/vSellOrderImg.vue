<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/order/SellOrder.js此處編寫
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
    import extend from "@/extension/order/vSellOrderImg.js";
    var vueParam = {
        data() {
            return {
                table: {
                    key: 'Order_Id',
                    footer: "Foots",
                    cnName: '從表圖片上傳',
                    name: 'SellOrder',
                    url: "/SellOrder/",
                    sortName: "CreateDate"
                },
                extend: extend,
                editFormFields: {"OrderType":"","TranNo":"","Qty":"","SellNo":"","Remark":"","Creator":"","CreateDate":"","ModifyDate":""},
                editFormOptions: [[{"dataKey":"ordertype","title":"訂單類型","required":true,"field":"OrderType","type":"select"},
                               {"title":"運單號","required":true,"field":"TranNo"},
                               {"title":"銷售數量","required":true,"field":"Qty","type":"number"}],
                              [{"title":"備註","field":"Remark","colSize":8,"type":"textarea"},
                               {"title":"銷售訂單號","required":true,"field":"SellNo"}],
                              [{"title":"創建人","field":"Creator","disabled":true},
                               {"title":"創建時間","field":"CreateDate","disabled":true},
                               {"title":"修改時間","field":"ModifyDate","disabled":true}]],
                searchFormFields: {"OrderType":"","TranNo":"","SellNo":"","AuditStatus":"","AuditDate":"","Auditor":"","Creator":"","CreateDate":"","ModifyDate":""},
                searchFormOptions: [[{"title":"運單號","field":"TranNo"},{"title":"銷售訂單號","field":"SellNo"},{"dataKey":"ordertype","title":"訂單類型","field":"OrderType","type":"select"}],[{"dataKey":"audit","title":"審核狀態","field":"AuditStatus","type":"select"},{"title":"審核時間","field":"AuditDate","type":"datetime"},{"title":"審核人","field":"Auditor"}],[{"title":"創建人","field":"Creator"},{"title":"創建時間","field":"CreateDate","type":"datetime"},{"title":"修改時間","field":"ModifyDate","type":"datetime"}]],
                columns: [{field:'Order_Id',title:'Id',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'ordertype',data:[]},width:90,require:true,align:'left',sortable:true},
                       {field:'TranNo',title:'運單號',type:'string',link:true,width:150,require:true,align:'left'},
                       {field:'SellNo',title:'銷售訂單號',type:'string',width:200,require:true,align:'left'},
                       {field:'Qty',title:'銷售數量',type:'int',width:90,require:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:100,hidden:true,readonly:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true}],
                detail: {
                    cnName:"從表明細",
                    columns: [{field:'OrderList_Id',title:'OrderList_Id',type:'string',width:90,hidden:true,require:true,align:'left'},
                       {field:'Order_Id',title:'訂單Id',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'ProductName',title:'商品名稱',type:'string',bind:{ key:'pn',data:[]},width:150,edit:{type:'select'},require:true,align:'left',sortable:true},
                       {field:'MO',title:'批次',type:'string',width:100,edit:{type:''},require:true,align:'left'},
                       {field:'Qty',title:'數量',type:'int',width:90,edit:{type:'drop'},require:true,align:'left'},
                       {field:'Weight',title:'重量',type:'decimal',width:90,edit:{type:''},align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:120,edit:{type:''},align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left',sortable:true}],
                    sortName: "CreateDate",
                    key:"OrderList_Id"
                }
            };
        }
    };
    export default vueParam;
</script>
