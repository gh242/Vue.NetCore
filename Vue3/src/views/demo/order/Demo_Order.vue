<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/order/Demo_Order.js此處編寫
 -->
<template>
    <view-grid ref="grid"
               :columns="columns"
               :detail="detail"
               :editFormFields="editFormFields"
               :editFormOptions="editFormOptions"
               :searchFormFields="searchFormFields"
               :searchFormOptions="searchFormOptions"
               :table="table"
               :extend="extend">
    </view-grid>
</template>
<script>
    import extend from "@/extension/demo/order/Demo_Order.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Order_Id',
                footer: "Foots",
                cnName: '訂單管理2',
                name: 'order/Demo_Order',
                url: "/Demo_Order/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"OrderNo":"","OrderType":"","TotalPrice":"","TotalQty":"","OrderDate":"","OrderStatus":"","Customer":"","PhoneNo":"","Remark":""});
            const editFormOptions = ref([[{"title":"訂單編號","required":true,"field":"OrderNo"},
                               {"dataKey":"訂單類型","data":[],"title":"訂單類型","required":true,"field":"OrderType","type":"select"},
                               {"title":"總價","field":"TotalPrice","type":"decimal"}],
                              [{"title":"總數量","field":"TotalQty","type":"number"},
                               {"title":"訂單日期","required":true,"field":"OrderDate","type":"datetime"},
                               {"dataKey":"訂單狀態","data":[],"title":"訂單狀態","required":true,"field":"OrderStatus","type":"select"}],
                              [{"dataKey":"客戶","data":[],"title":"客戶","field":"Customer","disabled":true,"type":"select"},
                               {"title":"手機","field":"PhoneNo","type":"phone"},
                               {"title":"備註","field":"Remark"}]]);
            const searchFormFields = ref({"OrderNo":"","OrderType":"","TotalPrice":[null,null],"OrderDate":"","OrderStatus":"","CreateDate":""});
            const searchFormOptions = ref([[{"title":"訂單編號","field":"OrderNo","type":"like"},{"dataKey":"訂單類型","data":[],"title":"訂單類型","field":"OrderType","type":"select"},{"title":"總價","field":"TotalPrice","type":"range"}],[{"title":"訂單日期","field":"OrderDate","type":"datetime"},{"dataKey":"訂單狀態","data":[],"title":"訂單狀態","field":"OrderStatus","type":"select"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]]);
            const columns = ref([{field:'Order_Id',title:'Order_Id',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderNo',title:'訂單編號',type:'string',link:true,width:130,require:true,align:'left',sort:true},
                       {field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'訂單類型',data:[]},width:110,require:true,align:'left'},
                       {field:'TotalPrice',title:'總價',type:'decimal',width:80,align:'left'},
                       {field:'TotalQty',title:'總數量',type:'int',width:80,align:'left'},
                       {field:'OrderDate',title:'訂單日期',type:'datetime',width:150,require:true,align:'left',sort:true},
                       {field:'CustomerId',title:'客戶',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'Customer',title:'客戶',type:'string',bind:{ key:'客戶',data:[]},width:120,readonly:true,align:'left'},
                       {field:'PhoneNo',title:'手機',type:'string',width:110,align:'left'},
                       {field:'OrderStatus',title:'訂單狀態',type:'int',bind:{ key:'訂單狀態',data:[]},width:110,require:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:120,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:150,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left',sort:true}]);
            const detail = ref({
                cnName: "訂單明細",
                table: "Demo_OrderList",
                columns: [{field:'OrderList_Id',title:'OrderList_Id',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Order_Id',title:'Order_Id',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'GoodsId',title:'商品Id',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'GoodsCode',title:'商品編號',type:'string',width:120,readonly:true,edit:{type:''},require:true,align:'left',sort:true},
                       {field:'GoodsName',title:'商品名稱',type:'string',width:120,readonly:true,edit:{type:''},require:true,align:'left'},
                       {field:'Img',title:'商品圖片',type:'img',width:100,readonly:true,align:'left'},
                       {field:'Specs',title:'商品規格',type:'string',bind:{ key:'商品規格',data:[]},width:120,readonly:true,edit:{type:'select'},align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:110,readonly:true,edit:{type:''},require:true,align:'left'},
                       {field:'Qty',title:'數量',type:'int',width:110,edit:{type:''},require:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,edit:{type:''},align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:145,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left',sort:true}],
                sortName: "CreateDate",
                key: "OrderList_Id"
            });
            return {
                table,
                extend,
                editFormFields,
                editFormOptions,
                searchFormFields,
                searchFormOptions,
                columns,
                detail,
            };
        },
    });
</script>
