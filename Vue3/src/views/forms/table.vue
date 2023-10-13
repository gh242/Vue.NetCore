<template>
  <div class="container">
    
   <div style=" padding-right:20px;">
     <h2 style="font-weight:500;font-weight: 500;
    padding: 10px 20px;">表單數據</h2>
         <VolForm
      ref="myform"
      :loadKey="true"
      :formFields="formFields"
      :formRules="formOptions"
    ></VolForm>
   </div>
    <!-- 查詢配置 -->
       <Alert type="success" style="    margin: 0 20px;" show-icon>
      table從api加載數據
      <div slot="desc">
       <p> 根據組件api文檔中voltable配置table顯示與4數據加載，更多功能參數配置見組件api->voltable</p>
       <p>如果需要更詳情的table編輯信息及其他操作見：組件api->VolTable組件</p>
      </div>
    </Alert>
    <div style="padding: 0px 20px">
      <VolHeader
        icon="md-apps"
        text="從api加載數據"
        style="margin-bottom: 10px; border: 0px; margin-top: 15px"
      >
        <div slot="content"></div>
        <slot>
          <div style="text-align: right">
            <Input
              style="width: 200px; margin-right: 10px"
              v-model.number="searchFields.UserName"
              placeholder="申請人"
            />
            <DatePicker
              style="width: 200px"
              type="datetime"
              placeholder="申請時間"
              v-model="searchFields.CreateDate1"
            ></DatePicker>
            -
            <DatePicker
              style="width: 200px"
              type="datetime"
              placeholder="申請時間"
              v-model="searchFields.CreateDate2"
            ></DatePicker>
            <Button type="info" ghost @click="load" style="margin-left: 20px"
              >查詢</Button
            >
            <Button type="info" ghost @click="del">刪除行</Button>
            <Button type="info" ghost @click="getRows">獲取選中的行</Button>
          </div>
        </slot>
      </VolHeader>

      <!-- table數據 -->
      <!-- :max-height="450" -->
      <vol-table
        ref="table"
        :loadKey="true"
        :columns="columns"
        :pagination-hide="false"
        :height="200"
        :url="url"
        :index="true"
        @loadBefore="loadTableBefore"
        @loadAfter="loadTableAfter"
      ></vol-table>
    </div>
  </div>
</template>
<script>
import VolTable from "@/components/basic/VolTable.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
import VolForm from "@/components/basic/VolForm.vue";
export default {
  components: { VolTable, VolHeader, VolForm },
  created() {},
  data() {
    return {
      //查詢條件字段
      searchFields: {
        CreateDate1: "",
        CreateDate2: "",
        UserName: "",
      },
      viewModel: false, //點擊單元格時彈出框
      url: "api/App_Expert/getPageData", //後從加載數據的url
      formFields: { price: null, age: null, variety: null,city:['北京','天壇'] },
      formOptions: [
        [
          {
            type: "decimal",
            title: "價格",
            required: true,
            placeholder: "你可以自己定義placeholder顯示的文字", //顯示自定義的信息
            field: "price",
          },
          {
            dataKey: "age", //後台下拉框對應的數據字典編號
            data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
            //data是須的參數，可以默認一個[]
            title: "年齡",
            required: false, //設置為必選項
            field: "age",
            placeholder: "可觸發事件的下拉框",
            type: "select",
            onChange(value, param) {
              //設置選擇數據時觸發的事件
              this.$message("當前選中的值為[" + value + "]");
            },
          },
          {
            title: "分類",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [
              { key: "1", value: "1" },
              { key: "2", value: "2" },
            ],
            required: true,
            field: "Variety",
            dataKey: "pz",
            data: [],
            type: "select",
          },

          {
            dataKey: "city",
            title: "級聯",
            required: true,
            field: "city",
            data: [
              {
                value: "北京",
                label: "北京",
                children: [
                  { value: "天壇", label: "天壇" },
                  { value: "王府井", label: "王府井" },
                ],
              },
            ],
            type: "cascader",
          },
        ],
      ],
      columns: [
        //表配置
        {
          field: "ExpertId", //數據庫表字段,必須和數據庫一樣，並且大小寫一樣
          title: "主鍵ID", //表頭顯示的名稱
          type: "int", //數據類型
          isKey: true, //是否為主鍵字段
          hidden: true, //是否顯示
          align: "left", //文字顯示位置
        },
        {
          field: "HeadImageUrl",
          title: "頭像",
          type: "img",
          width: 160,
        },
        {
          field: "UserName",
          title: "申請人帳號",
          width: 120,
          sort: true,
        },
        {
          field: "UserTrueName",
          title: "申請人",
          sort: true,
          width: 120,
        },
        {
          field: "Enable",
          title: "是否啟用",
          sort: true,
          bind: { key: "enable", data: [] }, //此處值為data空數據，自行從後台字典數據源加載
          width: 80,
        },
        {
          field: "ReallyName",
          title: "真實姓名",
          sort: true,
          width: 120,
          click: (row, column) => {
            //單元格點擊事亻
            let msg =
              "此處可以自己自定格式顯示内容,此單元格原始值是:【" +
              row.ReallyName +
              "】";
            this.$message.error(msg);
          },
          formatter: () => {
            //對單元格的數據格式化處理
            return "<a>點我</a>";
          },
        },
        {
          field: "CreateDate",
          title: "申請時間",
          sort: true,
          width: 150,
        },
      ],
    };
  },
  methods: {
    //點擊查詢時生成查詢條件
    loadTableBefore(param, callBack) {
      //此處是從後台加數據前的處理，自己在此處自定義查詢條件,查詢數據格式自己定義或參考代碼生成器查詢頁面請求的數據格式
      console.log("加載數據前" + param);
      //生成查詢條件
      param.wheres = [
        //設置為like模糊查詢
        {
          name: "UserName",
          value: this.searchFields.UserName,
          displayType: "like",
        },
        //設置日期查詢>=、<=
        {
          name: "CreateDate",
          value: this.searchFields.CreateDate1,
          displayType: "thanorequal",
        },
        {
          name: "CreateDate",
          value: this.searchFields.CreateDate2,
          displayType: "lessorequal",
        },
      ];
      callBack(true); //此處必須進行回調，返回處理結果，如果是false，則不會執行後台查詢
    },
    loadTableAfter(data, callBack) {
      //此處是從後台加數據後，你可以在渲染表格前，預先處理返回的數據
      console.log("加載數據後" + data);
      callBack(true); //同上
    },
    load() {
      //此處可以自定義查詢條件,如果不調用的框架的查詢，格式需要自己定義，
      //如果查詢的是框架getPageData方法,需要指定格式,如:
      // let where={wheres:[{"name":"UserTrueName","value":"教兽",displayType:"text"}]};
      let where = {};
      this.$refs.table.load(where);
    },
    del() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行");
      }
      this.$refs.table.delRow();
      //此處可以接著寫刪除後台的代碼
    },
    getRows() {
      let rows = this.$refs.table.getSelected();
      if (rows.length == 0) {
        return this.$message.error("請先選中行1");
      }
      let text = "當前選中的行數據：" + JSON.stringify(rows);

      this.$Message.info(text);
    },
  },
};
</script>