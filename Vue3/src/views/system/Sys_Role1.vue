<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/system/Sys_Role.js此處編寫
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
    import extend from "@/extension/system/Sys_Role1.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Role_Id',
                footer: "Foots",
                cnName: '角色管理',
                name: 'Sys_Role',
                url: "/Sys_Role/",
                sortName: "Role_Id"
            });
            const editFormFields = ref({"ParentId":[],"RoleName":"","DeptName":"","Enable":"","Creator":"","CreateDate":"","Modifier":"","ModifyDate":""});
            const editFormOptions = ref([[{"dataKey":"tree_roles","data":[],"title":"父級ID","required":true,"field":"ParentId","type":"cascader"},
                               {"title":"角色名稱","required":true,"field":"RoleName"}],
                              [{"title":"部門名稱","field":"DeptName"},
                               {"dataKey":"enable","data":[],"title":"是否啟用","field":"Enable","type":"switch"}],
                              [{"title":"創建人","field":"Creator","disabled":true},
                               {"title":"創建時間","field":"CreateDate","disabled":true,"type":"datetime"}],
                              [{"title":"修改人","field":"Modifier","disabled":true},
                               {"title":"修改時間","field":"ModifyDate","disabled":true}]]);
            const searchFormFields = ref({"RoleName":"","DeptName":"","Enable":"","CreateDate":"","ModifyDate":""});
            const searchFormOptions = ref([[{"title":"角色名稱","field":"RoleName","type":"text"},{"title":"部門名稱","field":"DeptName","type":"text"},{"dataKey":"enable","data":[],"title":"是否啟用","field":"Enable","type":"select"}],[{"title":"創建時間","field":"CreateDate","type":"datetime"},{"title":"修改時間","field":"ModifyDate","type":"datetime"}]]);
            const columns = ref([{field:'Role_Id',title:'角色ID',type:'int',width:70,readonly:true,require:true,align:'left',sortable:true},
                      // {field:'ParentId',title:'父級ID',type:'int',bind:{ key:'tree_roles',data:[]},width:70,require:true,align:'left'},
                       {field:'RoleName',title:'角色名稱',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'Dept_Id',title:'部門ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'DeptName',title:'部門名稱',type:'string',width:90,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'byte',bind:{ key:'enable',data:[]},width:90,align:'left'},
                       {field:'OrderNo',title:'排序',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true},
                       {field:'Modifier',title:'修改人',type:'string',width:130,readonly:true,align:'left'},
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
