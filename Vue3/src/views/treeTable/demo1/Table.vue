<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/system/Sys_User.js此處編寫
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
    import extend from "./table.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'User_Id',
                footer: "Foots",
                cnName: '用戶管理',
                name: 'Sys_User',
                url: "/Sys_User/",
                sortName: "User_Id"
            });
            const editFormFields = ref({"UserName":"","UserTrueName":"","Role_Id":"","IsRegregisterPhone":"","Gender":"","Enable":"","CreateDate":"","Creator":"","Remark":"","HeadImageUrl":""});
            const editFormOptions = ref([[{"title":"帳號","required":true,"field":"UserName","disabled":true},
                               {"title":"真實姓名","required":true,"field":"UserTrueName","type":"text"}],
                              [{"dataKey":"roles","data":[],"title":"角色","required":true,"field":"Role_Id","type":"select"},
                               {"dataKey":"isphone","data":[],"title":"手機用戶","required":true,"field":"IsRegregisterPhone","type":"select"}],
                              [{"dataKey":"gender","data":[],"title":"性別","field":"Gender","type":"select"},
                               {"dataKey":"enable","data":[],"title":"是否可用","required":true,"field":"Enable","type":"select"}],

                              [{"title":"備註","field":"Remark","colSize":12,"type":"textarea"}],
                              [{"title":"頭像","required":true,"field":"HeadImageUrl","type":"img"}]]);
            const searchFormFields = ref({"UserName":"","Gender":"","DeptName":"","Role_Id":"","Token":"","AppType":[],"UserTrueName":"","CreateDate":"","IsRegregisterPhone":"","PhoneNo":"","Enable":"","LastLoginDate":"","Address":"","Email":""});
            const searchFormOptions = ref([[{"title":"帳號","field":"UserName"},{"dataKey":"gender","data":[],"title":"性別","field":"Gender","type":"select"},{"title":"真實姓名","field":"UserTrueName"}],[{"title":"部門","field":"DeptName"},{"dataKey":"roles","data":[],"title":"角色","field":"Role_Id","type":"select"},{"title":"Token","field":"Token"}],[{"dataKey":"ut","data":[],"title":"登入設備類型","field":"AppType","type":"selectList"},{"dataKey":"isphone","data":[],"title":"手機用戶","field":"IsRegregisterPhone","type":"select"},{"title":"手機號","field":"PhoneNo"}],[{"dataKey":"enable","data":[],"title":"是否可用","field":"Enable","type":"select"},{"title":"地址","field":"Address"},{"title":"Email","field":"Email"}],[{"title":"注冊時間","field":"CreateDate","type":"datetime"},{"title":"最後登入時間","field":"LastLoginDate","type":"datetime"}]]);
            const columns = ref([{field:'UserName',title:'帳號',type:'string',link:true,width:110,readonly:true,require:true,align:'left',sortable:true},
                       {field:'User_Id',title:'User_Id',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Gender',title:'性別',type:'int',bind:{ key:'gender',data:[]},width:100,align:'left'},
                       {field:'HeadImageUrl',title:'頭像',type:'img',width:90,require:true,align:'left'},
                       {field:'Dept_Id',title:'Dept_Id',type:'int',width:90,hidden:true,align:'left'},
                       {field:'DeptName',title:'部門',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Role_Id',title:'角色',type:'int',bind:{ key:'roles',data:[]},width:100,require:true,align:'left'},
                       {field:'RoleName',title:'RoleName',type:'string',width:90,hidden:true,require:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'Enable',title:'是否可用',type:'byte',bind:{ key:'enable',data:[]},width:90,require:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,hidden:true,readonly:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true}]);
            const detail = ref({
                cnName: "#detailCnName",
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
