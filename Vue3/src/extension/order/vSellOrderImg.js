
import { h, resolveComponent } from 'vue';
import modelBody from "./vSellOrderImg/vSellOrderImgModelBody.vue"
let extension = {
  components: {//動態擴充組件或組件路徑
    gridHeader: '',
    gridBody: {
      render() {
        return h(resolveComponent('el-alert'), {
          type: "success",
          style: { 'margin-bottom': '12px' },
          'show-icon': false,
          closable: false,
        }, '點擊新建、編輯彈出框，可以對明細表進行上傳文件或者圖片操作，見vSellOrderImg.js')
      }
    },
    gridFooter: '',
    modelHeader: "",
    //點擊上傳圖片的彈出框
    modelBody: modelBody,
    modelFooter: "",
  },
  text: "一對一從表編輯上傳圖片",
  methods: {
    onInit() {
    },
    onInited() {
      this.height = this.height - 56;
      //獲取圖片位置，在圖片後面加一個上傳按鈕,這裡只是演示随便找的一個字段存圖片
      let _index = this.detailOptions.columns.findIndex(x => { return x.field == 'Remark' });

      //這裡只是演示，實際操作在代碼生成器table顯示類型設置為圖片後這裡就不用操作了
      //代碼生成器中編輯行號設置為0，不要設置為大於0的數據
      this.detailOptions.columns[_index].edit = null;
      this.detailOptions.columns[_index].type = 'img';
      this.detailOptions.columns[_index].title = '圖片';

      //從表動態添加一列(上傳圖片列),生成上傳圖片、與4刪除圖片操作
      this.detailOptions.columns.splice(_index, 0, {
        field: "随便寫",
        title: "上傳圖片",
        width: 150,
        align: "center",
        render: (h, { row, column, index }) => {
          //下面所有需要顯示的信息都從row裡面取出來
          return h(
            "div", { style: { color: '#0c83ff', 'font-size': '13px', cursor: 'pointer' } },
            [
              h(
                "i", {
                style: { 'margin-right': '10px' },
                class:['el-icon-upload'],
                onClick: (e) => {
                  e.stopPropagation();
                  //記住當前操作的明細表行數據
                  //如果原來有圖片,上傳圖片介面把原來的圖片也顯示出來
                  let fileInfo = (row.Remark || '').split(",").filter(x => { return x }).map(img => {
                    //(生成文件格式) fileInfo格式參數，見volupload組件
                    return { path: img, name: "" };
                  })
                  this.$refs.modelBody.open(fileInfo, row)
                }
              }, [], '上傳圖片'
              ),
              h('i', {
                class: ['el-icon-delete'], onClick: (e) => {
                  e.stopPropagation(); row.Remark = ''
                }
              }, '刪除圖片')
            ])
        },
      })
    }
  }
};
export default extension;
