<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/goods/Demo_Goods.js此處編寫
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
    import extend from "@/extension/demo/goods/Demo_Goods.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'GoodsId',
                footer: "Foots",
                cnName: '商品信息',
                name: 'goods/Demo_Goods',
                url: "/Demo_Goods/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"GoodsName":"","CatalogId":"","GoodsCode":"","Specs":"","Price":"","Enable":"","Remark":"","Img":""});
            const editFormOptions = ref([[{"title":"所屬分類","field":"CatalogId"},
                               {"title":"商品編號","required":true,"field":"GoodsCode"},
                               {"title":"商品名稱","required":true,"field":"GoodsName"}],
                              [{"title":"商品規格","field":"Specs"},
                               {"title":"單價","required":true,"field":"Price","type":"decimal"},
                               {"title":"是否可用","field":"Enable","type":"number"}],
                              [{"title":"備註","field":"Remark","colSize":12}],
                              [{"title":"商品圖片","field":"Img","type":"img"}]]);
            const searchFormFields = ref({"GoodsName":"","CatalogId":"","GoodsCode":"","Img":"","Specs":"","Price":"","Enable":"","Remark":""});
            const searchFormOptions = ref([[{"title":"所屬分類","field":"CatalogId"},{"title":"商品編號","field":"GoodsCode"},{"title":"商品名稱","field":"GoodsName"}],[{"title":"商品規格","field":"Specs"},{"title":"單價","field":"Price","type":"decimal"},{"title":"是否可用","field":"Enable","type":"number"}],[{"title":"商品圖片","field":"Img"}],[{"title":"備註","field":"Remark"}]]);
            const columns = ref([{field:'GoodsId',title:'GoodsId',type:'guid',width:170,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'GoodsName',title:'商品名稱',type:'string',link:true,width:120,require:true,align:'left',sort:true},
                       {field:'CatalogId',title:'所屬分類',type:'guid',width:110,align:'left'},
                       {field:'GoodsCode',title:'商品編號',type:'string',width:120,require:true,align:'left'},
                       {field:'Img',title:'商品圖片',type:'img',width:80,align:'left'},
                       {field:'Specs',title:'商品規格',type:'string',width:110,align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:80,require:true,align:'left'},
                       {field:'Enable',title:'是否可用',type:'int',width:80,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:180,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:150,align:'left',sort:true},
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
