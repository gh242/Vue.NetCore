
import gridHeader from './App_Expert/App_ExpertGridHeader'

//自定義選擇數據源頁面
import App_ExpertModelBody from './App_Expert/App_ExpertModelBody'
import { h, resolveComponent } from 'vue';
//聲明vue對象
let $this;
let extension = {
  components: {
    gridHeader: gridHeader,
    gridBody: {
      render() {
        return h(resolveComponent('ElAlert'), {
          style: { 'margin-bottom': '12px' },
          'show-icon': true,
          closable: false,
        }, [h('p', {}, '1、多個彈出框：手動創建一個頁面,接著在表對應的js文件中引入頁面放到gridHeader中'),
        h('p', {}, ' 2、在手動創建的頁面中，可以寫任意代碼,在js中通過this.$refs.gridHeader.xx即可訪問頁面的内容,具體見:App_Expert.js、App_ExpertGridHeader.vue')]);
      }
    },
    gridFooter: '',
    modelHeader: '',
    modelBody: App_ExpertModelBody,  //將選擇數據源頁面App_ExpertModelBody注冊到彈出框中
    modelFooter: ''
  }, //動態擴充組件或組件路徑
  buttons: {
    view: [{
      name: "彈出框1",
      icon: 'md-add',
      index: 1,//添加到第一個按鈕後面
      type: 'primary',
      onClick: function () {
        $this.$refs.gridHeader.open1()
      }
    }]
  },

  methods: { //事件擴展
    click1(){
      this.$message.success('點擊了提示')
    },
    onInited() {
      $this = this;
      //手動調度彈出框的高度
      this.boxOptions.height = this.boxOptions.height + 90;
      //手動設置表高度自動适應
      this.height = this.height - 100;
    },
    onInit() {

      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == 'HeadImageUrl') {
           
            //選擇文件時
            item.onChange=(files)=>{
               console.log('選擇文件事件')
               //此處不返回true，會中斷代碼執行
               return true;
            }
            //上傳前
            item.uploadBefore=(files)=>{
              console.log('上傳前')
              return true;
            }
             //上傳後
             item.uploadAfter=(files)=>{
              console.log('上傳後')
              return true;
            }
          }
        })
      })

      //選擇數據源功能
      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == 'ReallyName') {
            //給編輯表單設置[選擇數據]操作,extra具體配置見volform組件api
            item.extra = {
              icon: "el-icon-zoom-out",
              text: "選擇數據",
              style: "color:#2196F3;font-size: 12px;cursor: pointer;",
              click: item => {
                this.$refs.modelBody.openDemo();
              }
            }
          }
        })
      })


      //設置保存後繼續添加 ，不關閉當前窗口
      //2021.04.11需要更新methods.js,ViewGrid.vue
      this.continueAdd = true;
      this.continueAddName = "連續添加";

      //將編輯表單第一行第一列【名稱】字段添加一個額外提示屬性
      this.editFormOptions[0][0].extra =
      {
        render: (h) => {
          return <el-popover
            placement="top-start"
            title="Title"
            width="200"
            trigger="hover"
            content="this is content, this is content, this is content"
          >
            {/* 這裡對應下面的#reference數據槽 */}
            {{ reference: ()=>{return <i onClick={()=>{this.click1()}} class='el-icon-warning-outline'></i>} }}
          </el-popover>
        }
      }

      //設置介面上最多可顯示的按鈕數量 
      this.maxBtnLength = 6;

      // 第2個彈出框操作
      this.buttons.splice(2, 0, ...[{
        name: "彈出框2",
        icon: 'md-add',
        type: 'success',
        onClick: function () {
          $this.$refs.gridHeader.open2()
        }
      },
      {
        name: "獲取子組件對象",
        icon: 'md-add',
        type: 'warning',
        onClick: function () {
          this.$Message.info(this.$refs.gridHeader.getTestData())
        }
      }])


      // 第3個彈出框操作
      this.columns.forEach(x => {
        if (x.field == "Resume") {
          x.formatter = (row, column, event) => {
            return '<a style="color:#2a92ff;cursor: pointer;">(彈出框3)' + row.Resume + '</a>'
          };
          //綁定點擊事件
          x.click = (row, column, event) => {
            $this.$refs.gridHeader.open3(row)
          };
        }
      })

      //啟用多圖上傳,其他上傳參數，參照volupload組件api
      this.editFormOptions.forEach(x => {
        x.forEach(item => {
          if (item.field == 'HeadImageUrl') {
            //item.type = 'file';
            //設置成100%寬度
            item.colSize = 12;
            item.multiple = true;
            //最多可以上傳3張照片
            item.maxFile = 3;
            //限制圖片大小，默認3M
            item.maxSize = 3;
            // item.append = true;
          }
        })
      })

      // //動態添加一行表單配置
      // this.editFormOptions[2].splice(0, 1, ...[
      //   {
      //     colSize: 12,
      //     render: (h) => {
      //       return <el-alert title="這裡是自定的提示信息" style={{ padding:0 }} type="success" >
      //       </el-alert>
      //     }
      //   }
      // ])

    },
  }
};
export default extension;