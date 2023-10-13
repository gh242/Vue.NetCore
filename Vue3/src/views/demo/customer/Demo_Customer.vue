<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/demo/customer/Demo_Customer.js此處編寫
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
    import extend from "@/extension/demo/customer/Demo_Customer.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'CustomerId',
                footer: "Foots",
                cnName: '客戶',
                name: 'customer/Demo_Customer',
                url: "/Demo_Customer/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"Customer":"","PhoneNo":"","City":"","District":"","Address":"","Remark":""});
            const editFormOptions = ref([[{"title":"客戶","required":true,"field":"Customer"},
                               {"title":"手機","field":"PhoneNo"}],
                              [{"title":"城市","field":"City"},
                               {"title":"區","field":"District"}],
                              [{"title":"地址","field":"Address"}],
                              [{"title":"備註","field":"Remark","colSize":12}]]);
            const searchFormFields = ref({"Customer":"","PhoneNo":"","City":"","District":"","Address":"","Remark":"","Creator":"","CreateDate":[null,null]});
            const searchFormOptions = ref([[{"title":"客戶","field":"Customer","type":"like"},{"title":"手機","field":"PhoneNo","type":"like"},{"title":"城市","field":"City","type":"like"}],[{"title":"區","field":"District","type":"like"},{"title":"地址","field":"Address","type":"like"},{"title":"備註","field":"Remark","type":"like"}],[{"title":"創建人","field":"Creator","type":"like"},{"title":"創建時間","field":"CreateDate","type":"range"}]]);
            const columns = ref([{field:'CustomerId',title:'CustomerId',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Customer',title:'客戶',type:'string',link:true,width:120,require:true,align:'left',sort:true},
                       {field:'PhoneNo',title:'手機',type:'string',width:110,align:'left'},
                       {field:'City',title:'城市',type:'string',width:110,align:'left'},
                       {field:'District',title:'區',type:'string',width:110,align:'left'},
                       {field:'Address',title:'地址',type:'string',width:120,align:'left'},
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
