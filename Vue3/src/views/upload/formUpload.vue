<template>
  <div class="upload-container">
    <div>
       <h4 style=" text-align: center;margin-bottom: 20px;">表單與4文件上傳下載(可自定義上傳、圖片預覽與4點擊下載文件)</h4>
      <VolForm
        ref="myform"
        :label-width="180"
        :load-key="true"
        :form-fields="formFields"
        :form-rules="formRules"
      ></VolForm>
    </div>
  </div>
</template>
<script>
import VolForm from "@/components/basic/VolForm.vue";
import VolUpload from "@/components/basic/VolUpload.vue";
import VolHeader from "@/components/basic/VolHeader.vue";
export default {
  components: {
    VolUpload,
    VolForm,
    VolHeader
  },
  data() {
    //表單驗證時根據remove判斷是否有圖片
    return {
      formFields: {
        OrderNo: "BJ2019000001",
        OrderDate: new Date(),
        City: "北京市",
        Price: 277.6,
        ProductCode: "2019FX001",
        ProductName: "無線充電寶",
        file1: "",
        file2: "",
        file3: [
          //對已有文件顯示，並可重新上傳
          {
            name: "x1.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x1.jpg"
          },
          {
            name: "x2.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x2.jpg"
          },
          {
            name: "x3.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x3.jpg"
          },
          {
            name: "xx.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/xx.jpg"
          },
          {
            name: "tj01.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/tj01.jpg"
          }
        ],
        file4: "",
        file5: [
          {
            name: "測試現有文件可下載1.xlsx",
            path:
              "https://files-1256993465.cos.ap-chengdu.myqcloud.com/測試現有文件可下載1.xlsx"
          },
          {
            name: "測試現有文件可下載2.xlsx",
            path:
              "https://files-1256993465.cos.ap-chengdu.myqcloud.com/測試現有文件可下載2.xlsx"
          }
        ],
        //只讀圖片
        //也可以直接用逗號拼接 "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x3.jpg,https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x3.jpg",
        file6: [
          {
            name: "x2.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x2.jpg"
          },
          {
            name: "x3.jpg",
            path:
              "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/h5pic/x3.jpg"
          }
        ], //只讀可下載的文件，格式與4上面只讀圖片相同
        file7: [
          {
            name: "(api後台文件)基礎表單+編輯只讀 (4).xlsx",
            path:
              "Upload/Tables/App_Appointment/201911241454458938/基礎表單+編輯只讀 (4).xlsx"
          },
          {
            name: "(第三方服務器文件).xlsx",
            path:
              "https://files-1256993465.cos.ap-chengdu.myqcloud.com/測試現有文件可下載1.xlsx"
          }
        ],
        file8: ""
      },
      formRules: [
        //兩列的表單，formRules數據格式為:[[{},{}]]
        [
          {
            title: "商品編號",
            required: true, //設置為必選項
            field: "ProductCode",
            type: "text"
          },
          {
            title: "商品名稱",
            placeholder: "此處數據源為手動綁定",
            required: false,
            field: "ProductName",
            type: "text"
          },
          {
            type: "decimal",
            title: "商品價格",
            required: true,
            placeholder: "你可以自己定義placeholder顯示的文字",
            field: "Price"
          }],
          [{
            dataKey: "city",
            title: "收貨城市",
            required: true,
            field: "City",
            data: [],
            type: "select"
          }
      ,
          {
            title: "發貨單號",
            field: "OrderNo",
            type: "text"
          },
          {
            title: "發貨日期",
            field: "OrderDate",
            type: "datetime"
          }
        ],
        [
          {
            title: "自動上傳",
            field: "file1",
            required: true,
            maxSize: 1, //最大1M文件
            url: "/api/App_Appointment/Upload",
            type: "file"
          },
          {
            title: "上傳excel",
            field: "file2",
            maxSize: 0.5, //最大0.5M文件
            multiple: true, //啟用多文件
            maxFile: 2, //最多兩個文件
            url: "/api/App_Appointment/Upload",
            type: "excel"
          },
              {
            title: "文件只讀",
            readonly: true,
            field: "file7",
            downLoad: true,
            maxSize: 0.5, //最大0.5M文件
            multiple: true, //啟用多文件
            maxFile: 2, //最多兩個文件
            type: "excel"
          }
        ],
        [
          {
            title: "上傳下載",
            autoUpload: false,
            field: "file5",
            maxSize: 1, //最大1M文件
            multiple: true, //啟用多文件
            maxFile: 3, //最多3個文件
            downLoad: true,
            url: "/api/App_Appointment/Upload",
            type: "file",
            fileClick(index, file, files) {
              this.$Message.error(file.name);
              return true;
            }
          },
            {
            title: "圖片只讀",
            field: "file6",
            readonly: true,
            type: "img"
          },
          {
            title: "手動上傳",
            field: "file4",
            maxSize: 1, //最大1M文件
            multiple: true, //啟用多文件
            maxFile: 3, //最多兩個文件
            url: "/api/App_Appointment/Upload",
            type: "img"
          }
        ],
        [
          {
            title: "上傳圖片",
            autoUpload: false,
            field: "file3",
            maxSize: 1, //最大1M文件
            colSize: 12,
            url: "/api/App_Appointment/Upload",
            type: "img"
          }
        ]
      ]
    };
  }
};
</script>
<style scoped>
.upload-container {
  padding: 10px 15px;
}
.upload-container >>> .img-item,
.upload-container >>> .img-item img{
  width: 70px  !important;
  height: 70px  !important;
}
</style>