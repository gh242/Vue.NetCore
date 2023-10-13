<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆蓋
 *業務請在@/extension/system/App_News.js此處編寫
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
    import extend from "@/extension/appmanager/App_News.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Id',
                footer: "Foots",
                cnName: '新聞列表',
                name: 'App_News',
                url: "/App_News/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"Title":"","NewsType":"","DailyRecommend":"","Enable":"","Content":""});
            const editFormOptions = ref([[{"title":"標題","required":true,"field":"Title","colSize":12}],
                              [{"dataKey":"enable","data":[],"title":"測試","required":true,"field":"DailyRecommend","type":"radio"},
                               {"dataKey":"enable","data":[],"title":"是否啟用","field":"Enable","type":"radio"},
                               {"dataKey":"news","data":[],"title":"新聞類型","required":true,"field":"NewsType","type":"select"}],
                              [{"title":"新聞内容","field":"Content","colSize":12,"type":"editor"}]]);
            const searchFormFields = ref({"Title":"","ReleaseDate":"","NewsType":"","DailyRecommend":"","Enable":"","Creator":""});
            const searchFormOptions = ref([[{"title":"標題","field":"Title"},{"dataKey":"news","data":[],"title":"新聞類型","field":"NewsType","type":"select"},{"dataKey":"enable","data":[],"title":"測試","field":"DailyRecommend","type":"select"}],[{"title":"發佈時間","field":"ReleaseDate","type":"datetime"},{"dataKey":"enable","data":[],"title":"是否啟用","field":"Enable","type":"select"},{"title":"發佈人","field":"Creator"}]]);
            const columns = ref([{field:'Id',title:'主鍵ID',type:'int',width:80,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Title',title:'標題',type:'string',link:true,width:200,require:true,align:'left',sortable:true},
                       {field:'Content',title:'新聞内容',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Author',title:'發佈人',type:'string',width:120,hidden:true,readonly:true,align:'left'},
                       {field:'ReleaseDate',title:'發佈時間',type:'datetime',width:140,hidden:true,readonly:true,align:'left',sortable:true},
                       {field:'ImageUrl',title:'封面圖片',type:'img',width:120,align:'left'},
                       {field:'BigImageUrls',title:'圖片(大圖)',type:'string',width:100,hidden:true,align:'left'},
                       {field:'DetailUrl',title:'新聞地址',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ViewCount',title:'瀏覽次數',type:'int',width:110,hidden:true,align:'left'},
                       {field:'NewsType',title:'新聞類型',type:'int',bind:{ key:'news',data:[]},width:110,require:true,align:'left'},
                       {field:'DailyRecommend',title:'測試',type:'int',bind:{ key:'enable',data:[]},width:120,hidden:true,require:true,align:'left'},
                       {field:'OrderNo',title:'推薦排序',type:'int',bind:{ key:'enable',data:[]},width:90,hidden:true,align:'left'},
                       {field:'Enable',title:'是否啟用',type:'int',bind:{ key:'enable',data:[]},width:100,align:'left'},
                       {field:'CreateID',title:'創建人Id',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'發佈人',type:'string',width:110,readonly:true,align:'left'},
                       {field:'CreateDate',title:'發佈時間',type:'datetime',width:140,readonly:true,align:'left',sortable:true},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:140,readonly:true,align:'left',sortable:true}]);
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
