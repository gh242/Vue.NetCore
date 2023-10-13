<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/order/View_OrderInfo.js此處編寫
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
    import extend from "@/extension/demo/order/View_OrderInfo.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Order_Id',
                footer: "Foots",
                cnName: '訂單詳情',
                name: 'order/View_OrderInfo',
                url: "/View_OrderInfo/",
                sortName: "OrderDate"
            });
            const editFormFields = ref({});
            const editFormOptions = ref([]);
            const searchFormFields = ref({"OrderNo":"","OrderType":"","TotalQty":[null,null],"TotalPrice":[null,null],"GoodsCode":"","GoodsName":"","Specs":"","Price":[null,null],"Qty":[null,null],"OrderDate":"","Creator":""});
            const searchFormOptions = ref([[{"title":"訂單編號","field":"OrderNo","type":"like"},{"dataKey":"訂單類型","data":[],"title":"訂單類型","field":"OrderType","type":"select"},{"title":"訂單數量","field":"TotalQty","type":"range"},{"title":"訂單金額","field":"TotalPrice","type":"range"}],[{"title":"商品編號","field":"GoodsCode","type":"like"},{"title":"商品名稱","field":"GoodsName","type":"like"},{"dataKey":"商品規格","data":[],"title":"商品規格","field":"Specs","type":"select"},{"title":"單價","field":"Price","type":"range"}],[{"title":"數量","field":"Qty","type":"range"},{"title":"訂單日期","field":"OrderDate","type":"datetime"},{"title":"創建人","field":"Creator","type":"like"}]]);
            const columns = ref([{field:'OrderNo',title:'訂單編號',type:'string',sort:true,width:120,align:'left',sort:true},
                       {field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'訂單類型',data:[]},width:90,require:true,align:'left'},
                       {field:'TotalQty',title:'訂單數量',type:'int',sort:true,width:80,align:'left'},
                       {field:'TotalPrice',title:'訂單金額',type:'decimal',sort:true,width:80,align:'left'},
                       {field:'GoodsId',title:'GoodsId',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'GoodsCode',title:'商品編號',type:'string',sort:true,width:100,hidden:true,align:'left'},
                       {field:'GoodsName',title:'商品名稱',type:'string',sort:true,width:100,align:'left'},
                       {field:'Specs',title:'商品規格',type:'string',bind:{ key:'商品規格',data:[]},sort:true,width:100,align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:80,align:'left'},
                       {field:'Qty',title:'數量',type:'int',width:80,align:'left'},
                       {field:'Img',title:'商品圖片',type:'string',width:90,align:'left'},
                       {field:'OrderDate',title:'訂單日期',type:'datetime',width:150,require:true,align:'left',sort:true},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'}]);
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
