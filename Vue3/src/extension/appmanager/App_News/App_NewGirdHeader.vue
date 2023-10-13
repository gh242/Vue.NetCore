<template>

  <VolBox
    v-model="model"
    title="封面圖片設置"
    :height="230"
    :width="520"
    :lazy="true"
    :padding="15"
  >
    <VolUpload
        style="text-align: center; border: 1px dotted #FF9800;padding: 20px;"
        :autoUpload="false"
        :multiple="true"
        :url="url"
        :max-file="3"
        :img="true"
        :fileInfo="fileInfo"
        :upload-after="uploadAfter"
      >
        <div>可選擇1-3張圖片設置為封面圖</div>
      </VolUpload>
  </VolBox>
</template>
<script>
import VolBox from "@/components/basic/VolBox.vue";
import {defineComponent,defineAsyncComponent,ref} from "vue"
export default defineComponent({
  data() {
    return {
      url: "api/app_news/upload",
      fileInfo: [],
      model: false,
    };
  },
  components: {
    VolBox: VolBox,
    VolUpload:defineAsyncComponent( () => import("@/components/basic/VolUpload.vue"))
  },
  methods: {
    open(fileInfo) {
      this.model = true;
      this.fileInfo = fileInfo;
    },
    //封面圖片上傳完成後，將圖片的路徑更新到表中
    uploadAfter(result, files) {
      if (!result.status) {
        return true;
      }
      //上傳完成後，保存每個圖片所存储的文件路徑
      let fileUrls = files.map((x) => {
           //2021.09.25修復示例上傳路徑逻輯錯誤的問題
        if (x.path) {
          return x.path;
        }
        return result.data + x.name;
      });
      let data = { imageUrl: fileUrls.join(",") };
      //父組件vue對象(查詢介面)
      let $parentVue;
      //獲取父組件當前選中行的id用於後台更新
      this.$emit("parentCall", ($vue) => {
        $parentVue = $vue;
        data.Id = $vue.getSelectRows()[0].Id;
      });
      this.http
        .post("api/news/setCover", data, "正在設置圖片封面")
        .then((x) => {
          //刷新查詢介面table
          $parentVue.refresh();
          this.model = false;
        });
      return true;
    },
  }
});
</script>