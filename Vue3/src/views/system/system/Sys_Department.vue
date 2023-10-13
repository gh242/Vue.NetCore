<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/system/system/Sys_Department.js此處編寫
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
    import extend from "@/extension/system/system/Sys_Department.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'DepartmentId',
                footer: "Foots",
                cnName: '組織架構',
                name: 'system/Sys_Department',
                url: "/Sys_Department/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"DepartmentName":"","DepartmentCode":"","ParentId":[],"Remark":""});
            const editFormOptions = ref([[{"title":"組織名稱","required":true,"field":"DepartmentName"}],
                              [{"title":"組織編號","field":"DepartmentCode"}],
                              [{"dataKey":"組織機構","data":[],"title":"上級組織","field":"ParentId","type":"cascader"}],
                              [{"title":"備註","field":"Remark","type":"textarea"}]]);
            const searchFormFields = ref({"DepartmentName":"","DepartmentCode":"","Creator":"","CreateDate":""});
            const searchFormOptions = ref([[{"title":"組織名稱","field":"DepartmentName","type":"like"},{"title":"組織編號","field":"DepartmentCode"},{"title":"創建人","field":"Creator"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]]);
            const columns = ref([{field:'DepartmentId',title:'DepartmentId',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'DepartmentName',title:'組織名稱',type:'string',link:true,width:180,require:true,align:'left',sort:true},
                       {field:'DepartmentCode',title:'組織編號',type:'string',width:90,align:'left'},
                       {field:'ParentId',title:'上級組織',type:'guid',bind:{ key:'組織機構',data:[]},width:110,hidden:true,align:'left'},
                       {field:'DepartmentType',title:'組織類型',type:'string',width:110,hidden:true,align:'left'},
                       {field:'Enable',title:'是否可用',type:'int',width:110,hidden:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:145,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:140,align:'left',sort:true}]);
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
