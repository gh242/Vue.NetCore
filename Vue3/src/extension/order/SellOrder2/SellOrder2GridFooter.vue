<template>
  <div style="padding: 0 4px;border-top: 10px solid #eee;">
    <h3 >
      <i class="ivu-icon ivu-icon-ios-information-circle-outline"></i>訂單明細
    </h3>
    <div style="padding:10px;background: white;padding-top: 0;">
      <vol-table ref="tableList"
                 :loadKey="true"
                 :columns="columns"
                 :pagination-hide="true"
                 :height="210"
                 :defaultLoadPage="false"
                 @loadBefore="loadBefore"
                 url="/api/SellOrder/getDetailPage"
                 :row-index="true"
                 :index="false"></vol-table>
    </div>
  </div>
</template>

<script>
import VolTable from "@/components/basic/VolTable.vue";
export default {
  components: {
    VolTable
  },
  methods: {
    loadBefore (params, callback) {
      return callback(true);
    }
  },
  data () {
    return {
      tableData: [],
      //從生成的代碼sellorder2.vue裡面把明細配置複製過來就能用
      columns:  [{field:'OrderList_Id',title:'OrderList_Id',type:'string',width:90,hidden:true,require:true,align:'left'},
                {field:'Order_Id',title:'訂單Id',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                {field:'ProductName',title:'商品名稱',type:'string',width:150,require:true,align:'left',sortable:true},
                {field:'MO',title:'批次',type:'string',width:100,require:true,align:'left'},
                {field:'Qty',title:'數量',type:'int',width:90,require:true,align:'left'},
                {field:'Weight',title:'重量',type:'decimal',width:90,align:'left'},
                {field:'Remark',title:'備註',type:'string',width:120,align:'left'},
                {field:'Creator',title:'創建人',type:'string',width:130,align:'left'},
                {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                {field:'Modifier',title:'修改人',type:'string',width:100,align:'left'},
                {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,align:'left'}]
    }
  }
}
</script>
<style scoped>
 h3{
     font-weight: 500;
    padding-left: 10px;
    background: white;
    margin-top: 8px;
    padding-bottom: 5px;
 }
</style>