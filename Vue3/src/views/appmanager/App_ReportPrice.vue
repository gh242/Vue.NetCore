<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/appmanager/App_ReportPrice.js此處編寫
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
    import extend from "@/extension/appmanager/App_ReportPrice.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Id',
                footer: "Foots",
                cnName: '自定義實現一對多',
                name: 'App_ReportPrice',
                url: "/App_ReportPrice/",
                sortName: "Id"
            });
            const editFormFields = ref({"Variety":"","Age":"","City":"","Price":"","Creator":"","Enable":"","CreateDate":"","AuditDate":"","ModifyDate":""});
            const editFormOptions = ref([[{"dataKey":"pz","data":[],"title":"分類","required":true,"field":"Variety","type":"select"},
                               {"dataKey":"age","data":[],"title":"年齡","required":true,"field":"Age","type":"select"},
                               {"dataKey":"city","data":[],"title":"城市","required":true,"field":"City","type":"select"}],
                              [{"title":"價格","required":true,"field":"Price","type":"decimal"},
                               {"title":"創建人","field":"Creator","disabled":true},
                               {"dataKey":"enable","data":[],"title":"是否啟用","field":"Enable","disabled":true,"type":"switch"}],
                              [{"title":"創建時間","field":"CreateDate","disabled":true,"type":"datetime"},
                               {"title":"審核時間","field":"AuditDate","disabled":true},
                               {"title":"修改時間","field":"ModifyDate","disabled":true}]]);
            const searchFormFields = ref({"Variety":"","Age":"","City":"","CreateDate":"","AuditStatus":""});
            const searchFormOptions = ref([[{"dataKey":"pz","data":[],"title":"分類","field":"Variety","type":"select"},{"dataKey":"age","data":[],"title":"年齡","field":"Age","type":"select"},{"dataKey":"city","data":[],"title":"城市","field":"City","type":"select"}],[{"title":"創建時間","field":"CreateDate","type":"datetime"},{"dataKey":"audit","data":[],"title":"審核狀態","field":"AuditStatus","type":"select"}]]);
            const columns = ref([{field:'Id',title:'主鍵ID',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Variety',title:'分類',type:'string',bind:{ key:'pz',data:[]},width:80,require:true,align:'left',sort:true},
                       {field:'Age',title:'年齡',type:'string',bind:{ key:'age',data:[]},width:80,require:true,align:'left'},
                       {field:'City',title:'城市',type:'string',bind:{ key:'city',data:[]},width:90,require:true,align:'left'},
                       {field:'Price',title:'價格',type:'decimal',link:true,width:90,require:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,hidden:true,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:150,hidden:true,readonly:true,align:'left',sort:true},
                       {field:'AuditStatus',title:'審核狀態',type:'int',bind:{ key:'audit',data:[]},width:90,align:'left'},
                       {field:'AuditId',title:'審核人Id',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Auditor',title:'審核人',type:'string',width:90,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'byte',bind:{ key:'enable',data:[]},width:90,readonly:true,align:'left'},
                       {field:'CreateID',title:'創建人Id',type:'int',width:90,hidden:true,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'測試',type:'string',width:100,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,readonly:true,align:'left',sort:true}]);
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
