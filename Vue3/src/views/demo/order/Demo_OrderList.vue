<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/order/Demo_OrderList.js此處編寫
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
    import extend from "@/extension/demo/order/Demo_OrderList.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'OrderList_Id',
                footer: "Foots",
                cnName: '訂單明細',
                name: 'order/Demo_OrderList',
                url: "/Demo_OrderList/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"GoodsCode":"","GoodsName":"","Specs":"","Price":"","Qty":"","Remark":""});
            const editFormOptions = ref([[{"dataKey":"商品規格","data":[],"title":"商品規格","field":"Specs","disabled":true,"type":"select"},
                               {"title":"備註","field":"Remark"},
                               {"title":"商品編號","required":true,"field":"GoodsCode","disabled":true},
                               {"title":"商品名稱","required":true,"field":"GoodsName","disabled":true},
                               {"title":"單價","required":true,"field":"Price","disabled":true,"type":"decimal"},
                               {"title":"數量","required":true,"field":"Qty","type":"number"}]]);
            const searchFormFields = ref({});
            const searchFormOptions = ref([]);
            const columns = ref([{field:'OrderList_Id',title:'OrderList_Id',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Order_Id',title:'Order_Id',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'GoodsId',title:'商品Id',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'GoodsCode',title:'商品編號',type:'string',width:120,readonly:true,require:true,align:'left',sort:true},
                       {field:'GoodsName',title:'商品名稱',type:'string',width:120,readonly:true,require:true,align:'left'},
                       {field:'Img',title:'商品圖片',type:'img',width:100,readonly:true,align:'left'},
                       {field:'Specs',title:'商品規格',type:'string',bind:{ key:'商品規格',data:[]},width:120,readonly:true,align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:110,readonly:true,require:true,align:'left'},
                       {field:'Qty',title:'數量',type:'int',width:110,require:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:145,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left',sort:true}]);
            const detail = ref({
                cnName: "#detailCnName",
                table: "#detailTable",
                columns: [],
                sortName: "",
                key: ""
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
