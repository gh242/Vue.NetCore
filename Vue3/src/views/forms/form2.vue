<template>
  <div
    style="
      width: 800px;
      position: absolute;
      left: 0;
      right: 0;
      margin: auto;
      top: 100px;
    "
  >
    <h4 style="padding-left: 63px; padding-bottom: 30px; font-weight: 500">
      兩列表單(數據源可後台自動綁定/也可手動綁定)
    </h4>
    <VolForm
      ref="myform"
      :loadKey="loadKey"
      :formFields="formFields"
      :formRules="formRules"
    >
      <div style="text-align: center">
        <el-button type="info" @click="getForm">獲取表單</el-button>
        <el-button type="error" @click="reset">重置表單</el-button>
      </div></VolForm
    >
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
export default {
  components: { VolForm },
  methods: {
    getForm() {
      this.$refs.myform.validate(() => {
        this.$message.error(JSON.stringify(this.formFields));
      });
    },
    reset() {
      //重置表單，重置時可指定重置的值，如果沒有指定重置的值，默認全部清空
      let data = { Variety: "1", AvgPrice: 888 };
      this.$refs.myform.reset(data);
      this.$message.error("表單已重置");
    },
  },
  data() {
    return {
      loadKey: true,
      formFields: {
        Variety: "",
        AgeRange: "",
        DateRange: [],
        City: "",
        AvgPrice: 8.88,
        Date: "",
        IsTop: "1",
      },
      formRules: [
        //兩列的表單，formRules數據格式為:[[{},{}]]
        [
          {
            dataKey: "age", //後台下拉框對應的數據字典編號
            data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
            title: "年齡",
            required: true, //設置為必選項
            field: "AgeRange",
            type: "select",
          },
          {
            title: "分類",
            dataKey: "age",
            placeholder: "此處數據源為手動綁定",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [
              { key: "1", value: "1x" },
              { key: "2", value: "2x" },
            ],
            required: false,
            field: "Variety",
            type: "select",
          },
        ],
        [
          {
            dataKey: "city",
            title: "城市",
            required: true,
            field: "City",
            data: [],
            type: "select",
          },
          {
            type: "decimal",
            title: "價格",
            required: true,
            placeholder: "你可以自己定義placeholder顯示的文字",
            field: "AvgPrice",
          },
        ],
        [
          {
            title: "寬度",
            required: true,
            field: "Date",
            placeholder: "你可以設置colSize屬性决定標籤的長度，可選值12/8/6/4",
            type: "datetime",
          },
          {
            title: "日期",
            range: true, //設置為true可以選擇開始與4結束日期
            required: false,
            field: "DateRange",
            type: "date",
          },
        ],
        [
          {
            dataKey: "top",
            title: "textarea",
            required: true,
            field: "IsTop",
            colSize: 12, //設置12，此列占100%寬度
            type: "textarea",
          },
        ],
      ],
    };
  },
};
</script>