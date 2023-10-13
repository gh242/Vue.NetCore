<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/catalog/Demo_Catalog.js此處編寫
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
    import extend from "@/extension/demo/catalog/Demo_Catalog.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'CatalogId',
                footer: "Foots",
                cnName: '商品分類',
                name: 'catalog/Demo_Catalog',
                url: "/Demo_Catalog/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"CatalogName":"","CatalogCode":"","ParentId":[],"Enable":"","Remark":"","Img":""});
            const editFormOptions = ref([[{"title":"分類編號","required":true,"field":"CatalogCode"},
                               {"title":"分類名稱","required":true,"field":"CatalogName"}],
                              [{"dataKey":"分類級聯","data":[],"title":"上級分類","field":"ParentId","type":"cascader"},
                               {"dataKey":"商品分類可用","data":[],"title":"是否可用","field":"Enable","type":"radio"}],
                              [{"title":"備註","field":"Remark","colSize":12,"type":"textarea"}],
                              [{"title":"分類圖片","field":"Img","type":"img"}]]);
            const searchFormFields = ref({"CatalogName":"","CatalogCode":"","Enable":[],"CreateDate":"","ModifyDate":""});
            const searchFormOptions = ref([[{"title":"分類編號","field":"CatalogCode","type":"like"},{"title":"分類名稱","field":"CatalogName","type":"like"},{"dataKey":"商品分類可用","data":[],"title":"是否可用","field":"Enable","type":"selectList"}],[{"title":"創建時間","field":"CreateDate","type":"datetime"},{"title":"修改時間","field":"ModifyDate","type":"datetime"}]]);
            const columns = ref([{field:'CatalogId',title:'商品分類',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'CatalogName',title:'分類名稱',type:'string',link:true,width:150,require:true,align:'left',sort:true},
                       {field:'CatalogCode',title:'分類編號',type:'string',width:60,require:true,align:'left'},
                       {field:'ParentId',title:'上級分類',type:'guid',bind:{ key:'分類級聯',data:[]},width:110,align:'left'},
                       {field:'Enable',title:'是否可用',type:'int',bind:{ key:'商品分類可用',data:[]},width:80,align:'left'},
                       {field:'Img',title:'分類圖片',type:'img',width:100,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:120,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:110,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:150,align:'left',sort:true}]);
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
