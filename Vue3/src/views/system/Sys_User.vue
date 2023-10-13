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
    import extend from "@/extension/system/Sys_User.js";
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
            const editFormFields = ref({"UserName":"","UserTrueName":"","Role_Id":[],"DeptIds":"","Enable":"","Gender":"","Remark":"","HeadImageUrl":""});
            const editFormOptions = ref([[{"title":"帳號","required":true,"field":"UserName","disabled":true}],
                              [{"title":"姓名","required":true,"field":"UserTrueName","type":"text"}],
                              [{"dataKey":"tree_roles","data":[],"title":"角色","required":true,"field":"Role_Id","type":"cascader"}],
                              [{"dataKey":"組織機構","data":[],"title":"組織構架","field":"DeptIds","colSize":12,"type":"treeSelect"}],
                              [{"dataKey":"enable","data":[],"title":"是否可用","required":true,"field":"Enable","type":"select"}],
                              [{"dataKey":"gender","data":[],"title":"性別","field":"Gender","type":"select"}],
                              [{"title":"備註","field":"Remark","colSize":12,"type":"textarea"}],
                              [{"title":"頭像","field":"HeadImageUrl","type":"img"}]]);
            const searchFormFields = ref({"UserName":"","UserTrueName":"","Gender":"","DeptName":"","Role_Id":[],"Token":"","AppType":[],"CreateDate":"","IsRegregisterPhone":"","PhoneNo":"","Enable":"","LastLoginDate":"","Address":"","Email":""});
            const searchFormOptions = ref([[{"title":"帳號","field":"UserName"},{"title":"姓名","field":"UserTrueName"},{"dataKey":"gender","data":[],"title":"性別","field":"Gender","type":"select"}],[{"title":"不用","field":"DeptName"},{"dataKey":"tree_roles","data":[],"title":"角色","field":"Role_Id","type":"select"},{"title":"Token","field":"Token"}],[{"dataKey":"ut","data":[],"title":"類型","field":"AppType","type":"selectList"},{"dataKey":"isphone","data":[],"title":"手機用戶","field":"IsRegregisterPhone","type":"select"},{"title":"手機號","field":"PhoneNo"}],[{"dataKey":"enable","data":[],"title":"是否可用","field":"Enable","type":"select"},{"title":"地址","field":"Address"},{"title":"Email","field":"Email"}],[{"title":"注冊時間","field":"CreateDate","type":"datetime"},{"title":"最後登入時間","field":"LastLoginDate","type":"datetime"}]]);
            const columns = ref([{field:'User_Id',title:'User_Id',type:'int',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'UserName',title:'帳號',type:'string',link:true,width:120,readonly:true,require:true,align:'left',sort:true},
                       {field:'UserTrueName',title:'姓名',type:'string',width:120,require:true,align:'left'},
                       {field:'Gender',title:'性別',type:'int',bind:{ key:'gender',data:[]},width:80,align:'left'},
                       {field:'HeadImageUrl',title:'頭像',type:'img',width:90,align:'left'},
                       {field:'Dept_Id',title:'不用',type:'int',width:90,hidden:true,align:'left'},
                       {field:'DeptName',title:'不用',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Role_Id',title:'角色',type:'int',bind:{ key:'tree_roles',data:[]},width:130,require:true,align:'left'},
                       {field:'RoleName',title:'不用',type:'string',width:90,hidden:true,align:'left'},
                       {field:'Token',title:'Token',type:'string',width:180,hidden:true,align:'left'},
                       {field:'AppType',title:'類型',type:'int',bind:{ key:'ut',data:[]},width:150,hidden:true,align:'left'},
                       {field:'DeptIds',title:'組織構架',type:'string',bind:{ key:'組織機構',data:[]},width:140,hidden:true,align:'left'},
                       {field:'CreateDate',title:'注冊時間',type:'datetime',width:150,readonly:true,align:'left',sort:true},
                       {field:'IsRegregisterPhone',title:'手機用戶',type:'int',bind:{ key:'isphone',data:[]},width:120,hidden:true,align:'left'},
                       {field:'PhoneNo',title:'手機號',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Tel',title:'Tel',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'Enable',title:'是否可用',type:'byte',bind:{ key:'enable',data:[]},width:90,require:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,hidden:true,readonly:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,readonly:true,align:'left',sort:true},
                       {field:'AuditStatus',title:'審核狀態',type:'int',bind:{ key:'audit',data:[]},width:90,hidden:true,align:'left'},
                       {field:'Auditor',title:'審核人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'AuditDate',title:'審核時間',type:'datetime',width:150,hidden:true,align:'left',sort:true},
                       {field:'LastLoginDate',title:'最後登入時間',type:'datetime',width:150,hidden:true,align:'left',sort:true},
                       {field:'LastModifyPwdDate',title:'最後密碼修改時間',type:'datetime',width:150,hidden:true,align:'left',sort:true},
                       {field:'Address',title:'地址',type:'string',width:190,hidden:true,align:'left'},
                       {field:'Mobile',title:'電話',type:'string',width:140,hidden:true,align:'left'},
                       {field:'Email',title:'Email',type:'string',width:140,hidden:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:180,hidden:true,align:'left'},
                       {field:'OrderNo',title:'排序號',type:'int',width:90,hidden:true,align:'left'}]);
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
