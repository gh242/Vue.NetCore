<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/order/App_Appointment.js此處編寫
 -->
<template>
    <div>
        <view-grid :columns="columns"
                   :detail="detail"
                   :editFormFields="editFormFields"
                   :editFormOptions="editFormOptions"
                   :searchFormFields="searchFormFields"
                   :searchFormOptions="searchFormOptions"
                   :table="table"
                   :extend="extend">
        </view-grid>
    </div>
</template>

<script>
    import extend from "../formsMulti/multil2Extension/multi2.js"
    var vueParam = {
        data() {
            return {
                table: {
                    key: 'Id',
                    footer: "Foots",
                    cnName: '混合表單一對多',
                    name: 'App_Appointment',
                    url: "/App_Appointment/",
                    sortName: "CreateDate"
                },
                extend: extend,
                editFormFields: {"Name":"","Id":"","Describe":"","PhoneNo":"","CreateDate":"","Creator":"","Modifier":"","ModifyDate":""},
                editFormOptions: [[{"title":"姓名","required":true,"field":"Name","disabled":true},
                               {"title":"","required":true,"field":"Id","disabled":true}],
                              [{"title":"描述","required":true,"field":"Describe","disabled":true},
                               {"title":"電話","required":true,type:"phone","field":"PhoneNo","disabled":true}],
                              [{"type":"datetime","title":"創建時間","field":"CreateDate","disabled":true},
                               {"title":"創建人","field":"Creator","disabled":true}],
                              [{"title":"修改人","field":"Modifier","disabled":true},
                               {"type":"datetime","title":"修改時間","field":"ModifyDate","disabled":true}]],
                searchFormFields: {"Name":"","PhoneNo":"","CreateDate":"","Creator":""},
                searchFormOptions: [[{"title":"姓名","field":"Name"},{"title":"電話",type:"phone","field":"PhoneNo"},{"type":"datetime","title":"創建時間","field":"CreateDate","type":"datetime"},{"title":"創建人","field":"Creator"}]],
                columns: [{field:'Name',title:'姓名',type:'string',link:true,width:100,readonly:true,require:true,align:'left',sortable:true},
                       {field:'Describe',title:'描述',type:'string',width:180,readonly:true,require:true,align:'left'},
                       {field:'PhoneNo',title:'電話',type:'string',width:130,readonly:true,require:true,align:'left'},
                       {field:'Id',title:'Id',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:120,readonly:true,align:'left',sortable:true},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,readonly:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,readonly:true,align:'left',sortable:true},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                detail: {
                    cnName:"#detailCnName",
                    columns: [],
                    sortName: "",
                    key:""
                }
            };
        }
    };
    export default vueParam;
</script>
