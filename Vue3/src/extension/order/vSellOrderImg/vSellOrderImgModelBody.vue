<template>
  <VolBox
    v-model="model"
    title="選擇圖片"
    :height="228"
    :width="500"
    :padding="15"
  >
    <!-- 上傳圖片、excel或其他文件、文件數量、大小限制都可以，參照volupload組件api -->
    <div style="height: 150px;">
      <VolUpload
      style="text-align: center; border: 1px dotted #ff9800; padding: 20px"
      :autoUpload="false"
      :multiple="true"
      :url="url"
      :max-file="2"
      :img="true"
      :fileInfo="fileInfo"
      :upload-after="uploadAfter"
    >
      <div>選擇圖片,只能上傳3M以内的照片</div>
    </VolUpload>
    </div>
  </VolBox>
</template>
<script>
import { h, resolveComponent, defineAsyncComponent } from "vue";
export default {
  components: {
    VolUpload: defineAsyncComponent(() =>
      import("@/components/basic/VolUpload.vue")
    ),
    VolBox: defineAsyncComponent(() => import("@/components/basic/VolBox.vue")),
  },
  data() {
    return {
      //設置保存圖片的路徑,此處選擇的是框架自帶每個表的上傳的路徑
      //也可以自定義設置上傳文件的接口，如果需要上傳到第三方請實現uploadBefore方法
      url: "api/Sys_Dictionary/upload",
      fileInfo: [], //初始化一個空對象，用來存储編輯上傳圖片時的原始圖片
      model: false,
      row: {}, //當前操作的明細表行
    };
  },
  methods: {
    open(fileInfo, row) {
      this.row=row;
      this.fileInfo = fileInfo;
      this.model = true;
    },
    //上傳完成，將圖片寫入明細表數據
    uploadAfter(result, files) {
      if (!result.status) return true;
      //生成圖片保存後返回的路徑
      let imgs = files.map((x) => {
        //2021.09.25修復示例上傳路徑逻輯錯誤的問題
        if (x.path) {
          return x.path;
        }
        return result.data + x.name;
      });
      // //獲取vue父組件(查詢介面)
      // this.$emit('parentCall',($parent)=>{
      // })
      //將圖片通過逗號隔開，重新寫入明細表的行數據中
      this.row.Remark = imgs.join(",");
      this.model = false;
      return true;
    },
  },
};
</script>
