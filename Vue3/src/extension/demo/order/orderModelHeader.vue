<template>
  <!-- <el-button type="default" size="mini" @click="model = true">
    打開彈出框
  </el-button> -->
  <vol-box :lazy="true" 
    v-model="model" 
    title="選擇客戶" 
    :height="550" 
    :width="1200" 
    :padding="0" 
    :onModelClose="onModelClose"
    >
    <div>
      <customer ref="customer"></customer>
    </div>
    <!-- @click="$Message.error('點擊確認')" -->
    <template #footer>
      <div>
        <el-button 
          type="primary" 
          size="small" 
          @click="onSelect"
        >確認</el-button>
        <!-- <el-button type="default" size="small" @click="model = false" >點擊關閉彈出框</el-button > -->
      </div>
    </template>
  </vol-box>


  <!-- 批量選擇明細表數據 -->
  <!-- :onModelClose="onModelClose" -->
  <vol-box :lazy="true" 
    v-model="detailModel" 
    title="批量選擇明細表數據" 
    :height="650" 
    :width="1200" 
    :padding="0" 
    >
    <div>
      <!-- <customer ref="customer"></customer> -->
      <!-- :max-height="550" -->
      <!-- <div class="search-form">
        <label>商品名稱</label>
        <el-input style="width:200px;" v-mdoel="GoodsName"></el-input>
        <el-button 
          size="small"
          type="primary" 
          icon="el-icon-search" 
          @click="search"
        >搜索</el-button>
      </div> -->
      <vol-table
        ref="detailTable"
        :columns="columns"
        :pagination-hide="false"
        :height="450"
        :url="url"
      ></vol-table>
    </div>
    <!-- @click="$Message.error('點擊確認')" -->
    <!-- @click="onSelect" -->
    <template #footer>
      <div>
        <el-button 
          type="primary" 
          size="small"
          @click="detailSelectClick"
        >選擇數據</el-button>
        <!-- <el-button type="default" size="small" @click="model = false" >點擊關閉彈出框</el-button > -->
      </div>
    </template>
  </vol-box>
</template>
<script>
import VolBox from '@/components/basic/VolBox.vue';
import customer from '@/views/demo/customer/Demo_Customer';
import VolTable from "@/components/basic/VolTable.vue";
//這裡使用的vue2語法，也可以寫成vue3語法
export default {
  components: { 
    'vol-box': VolBox ,
    customer,
    VolTable
  },
  methods: {},
  data() {
    return {
      model: false, // 彈出框
      detailModel: false, // 批量選擇明細表數據model彈出框
      url:"api/Demo_Order/getGoods",
      GoodsName: '', //商品名稱
      columns:[{field:'GoodsId',title:'GoodsId',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
               {field:'GoodsName',title:'商品名稱',type:'string',link:true,width:120,require:true,align:'left',sort:true},
               {field:'CatalogId',title:'所屬分類',type:'guid',width:110,align:'left'},
               {field:'GoodsCode',title:'商品編號',type:'string',width:120,require:true,align:'left',sort:true},
               {field:'Img',title:'商品圖片',type:'img',width:80,align:'left'},
               {field:'Specs',title:'商品規格',type:'string',width:110,align:'left',sort:true},
               {field:'Price',title:'單價',type:'decimal',width:90,require:true,align:'left'},
               {field:'Enable',title:'是否可用',type:'int',width:80,align:'left'},
               {field:'Remark',title:'備註',type:'string',width:180,align:'left'},
               {field:'CreateID',title:'CreateID',type:'int',width:100,hidden:true,align:'left'},
               {field:'Creator',title:'創建人',type:'string',width:100,align:'left',sort:true},
               {field:'CreateDate',title:'創建時間',type:'datetime',width:150,align:'left',sort:true}]
    };
  },
  methods: {
    //   onModelClose(){
    //        alert('彈出框右上角點擊x關閉事件')
    //   }
    open(){
      this.model = true;
    },
    openDetail(){
      this.detailModel = true;
    },
    onSelect(){
      console.log('this.$refs.customer.$refs.grid=', this.$refs.customer.$refs.grid);
      
      let rows = this.$refs.customer.$refs.grid.getSelectRows();
      if(!rows.length){
        return this.$message.error('請選擇數據')
      }
      //如：調用頁面查詢 $parent.search() })可以訪問父組件ViewGird中的任何屬性、對象、方法
      this.$emit('parentCall', $parent => { 
        $parent.editFormFields.Customer = rows[0].Customer;
        $parent.editFormFields.CustomerId = rows[0].CustomerId;
        $parent.editFormFields.PhoneNo = rows[0].PhoneNo;
        this.model = false;
      })
      // return this.$message.error(JSON.stringify(rows))
    },
    detailSelectClick(){
      let rows = this.$refs.detailTable.getSelected();
      if(!rows.length){
        return this.$message.error('請選擇數據');
      }

      // GoodsId,GoodsCode,GoodsName,Img,Specs,Price
      let _rows = rows.map(rows =>{
        return {
          GoodsId: rows.GoodsId,
          GoodsCode: rows.GoodsCode,
          GoodsName: rows.GoodsName,
          Img: rows.Img,
          Specs: rows.Specs,
          Price: rows.Price
        }
      })

      this.$emit('parentCall', $parent => { 
        $parent.$refs.detail.rowData.unshift(..._rows);

        this.detailModel = false;
      })
    },
    search(){

    }
  }
};
</script>
<style lang="less" scoped>
.search-form{
  display: flex;
  button{
    margin-left: 10px;
  }
}
</style>