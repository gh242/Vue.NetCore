<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/system/quartz/Sys_QuartzLog.js此處編寫
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
    import extend from "@/extension/system/quartz/Sys_QuartzLog.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'LogId',
                footer: "Foots",
                cnName: '執行記錄',
                name: 'quartz/Sys_QuartzLog',
                url: "/Sys_QuartzLog/",
                sortName: "StratDate"
            });
            const editFormFields = ref({});
            const editFormOptions = ref([]);
            const searchFormFields = ref({"TaskName":"","ElapsedTime":[null,null],"StratDate":"","Result":""});
            const searchFormOptions = ref([[{"title":"任務名稱","field":"TaskName","type":"like"},{"title":"耗時(秒)","field":"ElapsedTime","type":"range"},{"title":"開始時間","field":"StratDate","type":"datetime"},{"dataKey":"enable","data":[],"title":"是否成功","field":"Result","type":"select"}]]);
            const columns = ref([{field:'LogId',title:'LogId',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Id',title:'任務id',type:'guid',width:110,hidden:true,align:'left'},
                       {field:'TaskName',title:'任務名稱',type:'string',width:120,align:'left',sort:true},
                       {field:'ElapsedTime',title:'耗時(秒)',type:'int',width:90,align:'left'},
                       {field:'StratDate',title:'開始時間',type:'datetime',width:150,align:'left',sort:true},
                       {field:'EndDate',title:'結束時間',type:'datetime',width:150,align:'left',sort:true},
                       {field:'Result',title:'是否成功',type:'int',bind:{ key:'enable',data:[]},width:100,align:'left'},
                       {field:'ResponseContent',title:'返回内容',type:'string',width:250,align:'left'},
                       {field:'ErrorMsg',title:'異常信息',type:'string',width:150,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:130,hidden:true,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:110,hidden:true,align:'left',sort:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:110,hidden:true,align:'left',sort:true}]);
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
