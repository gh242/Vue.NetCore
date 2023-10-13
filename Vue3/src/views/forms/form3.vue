<template>
  <div class="container">
    <VolHeader icon="el-icon-warning-outline" text="表單配置" style="margin: 0 0 20px 50px">
      <template #content>
        <div style="color: #909090; font-size: 13px">
          表單集成了element多種組件,只需要簡單配置,功能包括：表單只讀、自定義驗證、非空驗證、下拉框自動綁定、render動態渲染等.<a href="http://v2.volcore.xyz/document/api"
            target="_blank">查看文檔, <a href="https://api.volcore.xyz/vol.doc/form.html"   target="_blank" style="margin-left: 20px;"> 查看示例代碼</a>
          </a>
        </div>
      </template>
    </VolHeader>
    <VolForm ref="myform" :loadKey="loadKey" :formFields="formFields" :formRules="formRules">
      <div style="text-align: center; margin-top: -30px">
        <el-button type="primary" size="small" @click="getForm">獲取表單</el-button>
        <el-button type="success" size="small" @click="reset">重置表單</el-button>
        <el-button type="info" size="small" @click="setReadonlyStaus(true)">全部只讀</el-button>
        <el-button type="waring" size="small" @click="setReadonlyStaus(false)">取消只讀</el-button>
      </div>
    </VolForm>
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
export default {
  props: {
    showBtn: { type: Boolean, default: true },
  },
  components: {
    VolForm,
    VolHeader,
  },
  methods: {
    setReadonlyStaus(status) {
      this.formRules.forEach((rules) => {
        rules.forEach((option) => {
          option.readonly = status;
        });
      });
    },
    getForm() {
      this.$refs.myform.validate(() => {
        this.$message.error(JSON.stringify(this.formFields));
      })
    },
    reset() {
      //重置表單，重置時可指定重置的值，如果沒有指定重置的值，默認全部清空
      let data = { Variety: "1", AvgPrice: 888 };
      this.$refs.myform.reset(data);
      this.$message.error("表單已重置");
    },
    popover() {
      this.$message.success("點擊了提示");
    }
  },
  data() {
    return {
      model: false,
      loadKey: true,
      formFields: {
        Variety: "",
        AgeRange: "",
        DateRange: ["", ""],
        City: [], //注意多選這裡默認是數組
        AvgPrice: 8.88, //input標籤如果是數字，此處注意區分不要寫成字符串了
        Date: "",
        IsTop: 0,
        Address: "北京市海淀區",
        Source: [],
        Source1: "5",
        Remark: "",
        cascader: [], //注意級聯這裡默認是數組
        treeValue:[],//樹形級聯，注意，如果下面配置設置是單選，這裡應該null或者字符串
        monthValue: this.base.getDate().substr(0, 7), //'2022-10',
        phone: "",
        email: "",
        extra2: "",
        userVali: "",
        uploadFile: [
          {
            name: "exceltest.xlsx",
            path: "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/exceltest.xlsx",
          },
        ],
        img: [
          {
            name: "060222.jpg",
            path: "http://api.volcore.xyz/Upload/Tables/App_Expert/202103061753415708/060222.jpg",
          },
        ],
      },
      formRules: [
        //兩列的表單，formRules數據格式為:[[{},{}]]
        [
          {
            dataKey: "age", //後台下拉框對應的數據字典編號
            data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
            title: "下拉框",
            filter: true,
            required: true, //設置為必選項
            field: "AgeRange",
            type: "select",
            extra: {
              render: (h) => {
                return (
                  <el-popover
                    placement="top-start"
                    title="Title"
                    width="200"
                    trigger="hover"
                    content="this is content, this is content, this is content"
                  >
                    {{
                      reference: (
                        <i onClick={() => { this.popover() }}
                          class="el-icon-warning-outline"
                        ></i>
                      ),
                    }}
                  </el-popover>
                );
              },
            },
          },
          {
            title: "自動完成",
            autocomplete: true, //設置為自動完成
            dataKey: "pz",
            placeholder: "不存的數據自動填充",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [
              { key: "北京市", value: "北京市" },
              { key: "上海市", value: "上海市" },
            ],
            required: false,
            field: "Variety",
            type: "select",
          },
          {
            dataKey: "city",
            title: "多選",
            required: true,
            data: [],
            field: "City",
            type: "selectList", //selectList屬性為多選
          },
          {
            type: "decimal",
            title: "數字",
            required: true,
            placeholder: "你可以自己定義placeholder顯示的文字",
            field: "AvgPrice",
          },
        ],
        [
          {
            title: "日期",
            required: true,
            placeholder: "限制時間範圍",
            field: "Date",
            type: "datetime", //設置日期選擇範圍
            min: "2021-07-01",
            max: Date.now(),
            onChange: (val) => {
              console.log("選擇日期:" + val);
            },
          },
          {
            title: "日期2",
            range: true, //設置為true可以選擇開始與4結束日期
            placeholder: "日期",
            field: "DateRange",
            type: "date",
            //  colSize:6,//如果寬度不够，可以指定colSize,可選值,4,6,8,12
            onChange: (val) => {
              console.log("日期:" + val);
            },
          },
          {
            type: "text",
            title: "地址",
            required: false,
            field: "Address",
          },
          {
            title: "額外標籤",
            field: "extra2",
            type: "text",
            extra: {
              style: "color:red;cursor: pointer;",
              icon: "el-icon-search", //顯示圖標
              text: "點擊", //顯示文本
              //觸發事件
              click: (item) => {
                this.$message.error("點擊標籤觸發的事件");
              },
            },
          },
        ],
        [
          {
            title: "手機",
            required: true,
            field: "phoneNo",
            type: "phone",
          },
          {
            title: "郵箱",
            required: true,
            field: "email",
            type: "mail",
          },
          {
            title: "自定義驗證",
            required: true,
            field: "userVali",
            placeholder: "只能輸入值：123",
            type: "text",
            validator: (rule, val) => {
              if (val != "123") {
                return "自定設置必須輸入123";
              }
              return "";
            },
          },
          {
            dataKey: "top",
            title: "是否",
            required: true,
            field: "IsTop",
            data: [],
            type: "switch",
          },
        ],
        [
          {
            title: "年月",
            field: "monthValue",
            placeholder: "年月",
            type: "month",
          },
          {
            title: "級聯",
            field: "cascader", //注意上面formFields屬性cascader是數組
            placeholder: "配置數據源後自動綁定級聯",
            checkStrictly: true, //是否可以選擇任意一級
            type: "cascader",
            data: [],
            dataKey: "tree_roles", //配置數據源(見菜單下拉框綁定設置中的級聯角色自定義sql)

          },
          {
            title: "樹形級聯",
            dataKey: "組織機構",
            // 如果這裡綁定了data數據，後台不會加載此數據源
            data: [],
            field: "treeValue",
            multiple:true,//設置為多選
            readonly: true,
            type: "treeSelect",
            colSize: 6, //設置寬度50%
          }
        ],
        [
          {
            title: "備註",
            required: true,
            field: "Remark",
            placeholder: "你可以設置colSize屬性决定標籤的長度，可選值12/8/6/4",
            min: 1,
            max: 3,
            type: "textarea",
            colSize: 12, //設置寬度100%
          },
        ],
        [
          {
            colSize: 12,
            render: (h) => {
              return h(
                "div",
                { style: { "padding-left": "48px", "margin-bottom": "-14px" } },
                [
                  h(
                    "div",
                    {
                      style: {
                        "border-left": "10px solid #3095ff",
                        "line-height": "17px",
                        "padding-left": "4px",
                        "font-weight": "bold",
                        "font-size": "15px",
                      },
                    },
                    "其他基礎信息(通過render方法數據分欄)"
                  ),
                ]
              );
            },
          },
        ],
        [
          {
            dataKey: "pz",
            title: "多選",
            required: true,
            data: [],
            min: 1,
            max: 4,
            field: "Source",
            type: "checkbox",
            colSize: 12, //設置寬度100%
          },
        ],
        [
          {
            dataKey: "pz",
            title: "Radio",
            required: true,
            data: [],
            min: 2,
            max: 4,
            field: "Source1",
            type: "radio",
            colSize: 12, //設置寬度100%
          },
        ],
        [
          {
            title: "只讀",
            field: "Address",
            readonly: true,
          },
          {
            title: "只讀文本",
            field: "Address",
            type: "label",
            inputStyle: { color: "red" }, //自定義樣式,其他的input也生效
          },
          {
            title: "圖片",
            required: true,
            field: "img",
            type: "img",
            multiple: true,
            maxFile: 2,
            maxSize: 5,
            url: "api/App_Appointment/Upload",
            //  colSize: 6, //設置寬度50%
          },
          {
            title: "上傳",
            required: true,
            field: "uploadFile",
            type: "excel", //指定上傳類型excel/img/file
            multiple: true,
            maxFile: 5,
            maxSize: 3,
            url: "api/App_Appointment/Upload",
            // colSize: 6, //設置寬度50%
          },
        ],
      ],
    };
  },
};
</script>
<style lang="less" scoped>
.container{
  padding: 10px 20px 0px 2px;
}
</style>