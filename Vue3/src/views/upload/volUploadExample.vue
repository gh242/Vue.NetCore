<template>
  <div>
    <!-- <img src="http://localhost:9991/Upload/Tables/App_Appointment/201911240828293464/h52.jpg"> -->
    <div class="upload-container">
      <div class="upload-item">
        <VolUpload
          :url="url"
          :upload-before="uploadBefore"
          :upload-after="uploadAfter"
          :fileInfo="fileInfo5"
          :on-change="onChange"
        >
          <div>單文件自動上傳</div>
        </VolUpload>
      </div>
      <div class="upload-item">
        <VolUpload
          :autoUpload="false"
          :multiple="true"
          :max-file="2"
          :excel="true"
          :fileInfo="fileInfo4"
          :url="url"
          :upload-before="uploadBefore"
          :upload-after="uploadAfter"
          :on-change="onChange"
        >
          <div>多文件手動上傳,最多2個excel文件</div>
        </VolUpload>
      </div>
      <div class="upload-item">
        <a ref="downFile"></a>
        <VolUpload
          :multiple="true"
          :url="url"
          :upload-before="uploadBefore1"
          :upload-after="uploadAfter1"
          :on-change="onChange"
          :fileInfo="fileInfo"
        >
          <div>下載已經上傳過的文件</div>
        </VolUpload>
      </div>
    </div>

    <div class="upload-container">
      <div class="upload-item">
        <VolUpload
          :url="url"
          :img="true"
          :multiple="true"
          :max-size="null"
          :fileInfo="fileInfo2"
          :upload-before="uploadBefore"
          :upload-after="uploadAfter"
          :on-change="onChange"
        >
          <div>多圖片自動上傳,圖片最大1M</div>
        </VolUpload>
      </div>
    </div>
    <div class="upload-container">
      <div class="upload-item">
        <VolUpload
          :autoUpload="false"
          :url="url"
          :img="true"
          :upload-before="uploadBefore"
          :upload-after="uploadAfter"
          :fileInfo="fileInfo3"
          :on-change="onChange"
        >
          <div>單文件手動上傳,只能上傳圖片</div>
        </VolUpload>
      </div>
    </div>
  </div>
</template>
<script>
import VolUpload from "@/components/basic/VolUpload.vue";
export default {
  components: { VolUpload },
  data() {
    return {
      url: "/api/App_Appointment/Upload",
      model: true,
      fileInfo: [
        {
          name: "exceltest.xlsx",
          path:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/exceltest.xlsx"
        },
        {
          name: "wordtest.docx",
          path:
            "https://imgs-1256993465.cos.ap-chengdu.myqcloud.com/github/wordtest.docx"
        }
      ],
      fileInfo2:[],
      fileInfo3:[],
      fileInfo4:[],
      fileInfo5:[],
      loadingStatus: false
    };
  },
  methods: {
    removeFile(index) {
      this.fileInfo.splice(index, 1);
    },
    uploadBefore1(files) {
      this.fileInfo.splice(0);
      for (let index = 0; index < files.length; index++) {
        const element = files[index];
        this.fileInfo.push({ name: element.name });
      }
      return true;
    },
    uploadAfter1(result, files) {
      if (!result.status) return true;
      // this.fileInfo.forEach(x => {
      //   x.path = result.data;
      // });
      return true;
    },
    fileClick(index, file) {
      if (
        !file.path &&
        file.name.indexOf("/") == -1 &&
        file.name.indexOf("\\") == -1
      ) {
        return this.$Message.error({ duration: 5, content: "請先上傳此文件" });
      }
      //從api服務器下載
      if (!this.base.checkUrl(file.path)) {
        this.dowloadFile(
          this.http.ipAddress +
            "api/App_Appointment/DownLoadFile?path=" +
            file.path +
            file.name,
          file.name,
          {"":this.http}
        );
        return;
      }
      //從遠程下載
      this.$refs.downFile.href = file.path || file.name;
      this.$refs.downFile.setAttribute("download", "download");
      this.$refs.downFile.click();
    },
    getFileNames(files) {
      let arr = [];
      for (let index = 0; index < files.length; index++) {
        const element = files[index];
        arr.push(element.name);
      }
      return arr.join(",");
    },
    uploadBefore(files) {
      console.log("上傳前的文件:" + this.getFileNames(files));
      return true;
    },
    uploadAfter(result, files) {
      console.log(
        "上傳結果" +
          JSON.stringify(result) +
          "上傳前的文件:" +
          this.getFileNames(files)
      );
      return true;
    },
    onChange(files) {
      console.log("選擇的文件:" + +this.getFileNames(files));
      return true;
    },
    dowloadFile(url, fileName) {
      this.base.dowloadFile(url, fileName, {
        Authorization: this.$store.getters.getToken()
      });
    }
  }
};
</script>
<style lang="less" scoped>
.upload-container {
  max-height: 160px;
  display: flex;
  .upload-item {
    flex: 1;
    padding: 20px;
  }
}
</style>
