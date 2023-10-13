<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/appmanager/App_Expert.js此處編寫
 -->
<template>
    <div>
        <view-grid
                   ref="grid"
                   :columns="columns"
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
    import extend from "@/extension/appmanager/App_Expert2.js";
    var vueParam = {
        data() {
            return {
                table: {
                    key: 'ExpertId',
                    footer: "Foots",
                    cnName: '複雜單元格内容',
                    name: 'App_Expert',
                    url: "/App_Expert/",
                    sortName: "ExpertId"
                },
                extend: extend,
                editFormFields: {"ExpertName":"","Enable":"","City":"","IDNumber":"","PhoneNo":"","CreateDate":"","Resume":"","HeadImageUrl":""},
                editFormOptions: [[{"title":"名稱","field":"ExpertName"},
                               {"dataKey":"enable","data":[],"title":"是否啟用","required":true,"field":"Enable","type":"select"}],
                              [{"dataKey":"city","data":[],"title":"地區","field":"City","type":"select"},
                                 {"title":"電話","field":"PhoneNo"} ],
                                     [{"title":"身份證號","field":"CiIDNumberty","type":"text"},
                               {"title":"申請時間","field":"CreateDate","disabled":true}],
                              [{"title":"個人簡介","field":"Resume","colSize":12,"type":"textarea"}],
                              [{"title":"頭像","field":"HeadImageUrl","type":"img"}]],
                searchFormFields: {"ExpertName":"","AuditStatus":[],"ReallyName":"","IDNumber":"","Education":"","Professional":"","Company":"","City":[],"AuditDate":""},
                searchFormOptions: [[{"title":"名稱","field":"ExpertName","type":"like"},{"title":"真實姓名","field":"ReallyName"},{"title":"身份證號","field":"IDNumber"}],[{"title":"學歷","field":"Education","type":"text"},{"title":"職業","field":"Professional"},{"title":"所在公司","field":"Company"}],[{"dataKey":"audit","data":[],"title":"審核狀態","field":"AuditStatus","type":"selectList"},{"dataKey":"city","data":[],"title":"地區","field":"City","type":"selectList"},{"title":"審核時間","field":"AuditDate","type":"datetime"}]],
                columns: [{field:'ExpertId',title:'主鍵ID',type:'int',width:80,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'User_Id',title:'申請人帳號Id',type:'int',width:90,hidden:true,align:'left'},
                       {field:'ExpertName',title:'名稱',type:'string',width:120,align:'left',hidden:true,sort:true},
                       {field:'HeadImageUrl',title:'頭像',type:'img',width:150,hidden:true,align:'left'},
                       {field:'UserName',title:'帳號',type:'string',width:120,hidden:true,align:'left'},
                       {field:'UserTrueName',title:'申請人',type:'string',width:100,hidden:true,align:'left'},
                       {field:'AuditStatus',title:'審核狀態',type:'int',bind:{ key:'audit',data:[]},width:100,require:true,align:'left'},
                       {field:'Auditor',title:'審核人',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'byte',bind:{ key:'enable',data:[]},width:90,require:true,align:'left'},
                       {field:'ReallyName',title:'真實姓名',type:'string',width:150,hidden:true,align:'left'},
                       {field:'IDNumber',title:'身份證號',type:'string',width:200,hidden:true,align:'left'},
                       {field:'PhoneNo',title:'電話',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Professional',title:'職業',type:'string',width:120,hidden:true,align:'left'},
                       {field:'Company',title:'所在公司',type:'string',width:150,hidden:true,align:'left'},
                       {field:'City',title:'地區',hidden:true,type:'string',bind:{ key:'city',data:[]},width:100,align:'left'},
                       {field:'Resume',title:'個人簡介',link:true,type:'string',width:150,align:'left'},
                       {field:'AuditId',title:'審核人Id',type:'int',width:80,hidden:true,align:'left'},
                       {field:'AuditDate',title:'審核時間',type:'datetime',width:150,hidden:true,align:'left',sort:true},
                       {field:'CreateID',title:'創建人Id',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:100,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:130,align:'left',sort:true}],
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
