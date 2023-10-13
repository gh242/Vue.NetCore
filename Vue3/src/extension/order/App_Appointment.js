import { h, resolveComponent } from 'vue';
let extension = {
  components: {
    //動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '',
    gridBody: {
      render(h) {
        return (
          <el-alert
            style={{ 'margin-bottom': '10px' }}
            type="success"
            show-icon
          >
            <p v-html="1、 onInit(){ this.setFiexdSearchForm(true){設置固定顯示所有查詢條件}">
            </p>
            <p v-html="2、自定義表格按鈕、彈出框提示、彈出框分組信息,見示例：App_Appointment.js文件">
            </p>
            <p v-html="3、2023.08.19集成表格行、列合併功能,更新文件見:http://v2.volcore.xyz/document/log">
            </p>
          </el-alert>
        );
      }
    },
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  buttons: [], //擴展的按鈕
  tableAction: 'App_Appointment', //指定菜單權限，其他任何頁面引用時都會走對應權限
  text:
    '代碼生成器中設置[是否只讀]或如果沒有編輯或新建權限彈出框都是只讀的(點擊用戶姓名列可查看表單分組)',
  methods: {
    btn1Click(row, $e) {
      $e.stopPropagation();
      this.edit(row);
    },
    btn2Click(row, $e) {
      $e.stopPropagation();
      this.$message.success('點擊了按鈕');
    },
    dropdownClick(value, row, column) {
      console.log(row);
      this.$message.success(value);
    },
    //事件擴展
    onInit() {
      this.buttons.push({
        name: '查看代碼',
        onClick: () => {
          window.open('http://api.volcore.xyz/vol.doc/appointment.html', 'blank');
        }
      });

      //設置顯示所有查詢條件
      this.setFiexdSearchForm(true);

      //表格上添加自定義按鈕
      this.columns.push({
        title: '操作',
        field: '操作',
        width: 150,
        align: 'left',// 'center',
        render: (h, { row, column, index }) => {
          return (
            <div>
              <el-button
                onClick={($e) => {
                  this.btn1Click(row, $e);
                }}
                type="primary"
                plain
                style="height:26px; padding: 10px !important;"
              >
                查看
              </el-button>

              {/* 通過條件判斷,要顯示的按鈕 */}
              {
                /*  {
                      index % 2 === 1 
                      ?<el-button>修改</el-button>
                      : <el-button>設置</el-button>
                  } */
              }


              {/* 通過v-show控制按鈕隱藏與4顯示
                  下面的index % 2 === 1換成：row.字段==值 */
              }
              <el-button
                onClick={($e) => {
                  this.btn2Click(row, $e);
                }}
                v-show={index % 2 === 1}
                type="success"
                plain
                style="height:26px;padding: 10px !important;"
              >
                修改
              </el-button>

              <el-button
                onClick={($e) => {
                  this.btn2Click(row, $e);
                }}
                v-show={index % 2 === 0}
                type="warning"
                plain
                style="height:26px;padding: 10px !important;"
              >
                設置
              </el-button>

              <el-dropdown
                onClick={(value) => {
                  this.dropdownClick(value);
                }}
                trigger="click"
                v-slots={{
                  dropdown: () => (
                    <el-dropdown-menu>
                      <el-dropdown-item>
                        <div
                          onClick={() => {
                            this.dropdownClick('京醬肉絲', row, column);
                          }}
                        >
                          京醬肉絲
                        </div>
                      </el-dropdown-item>
                      <el-dropdown-item>
                        <div
                          onClick={() => {
                            this.dropdownClick('驢肉火烧', row, column);
                          }}
                        >
                          驢肉火烧
                        </div>
                      </el-dropdown-item>
                    </el-dropdown-menu>
                  )
                }}
              >
                <span
                  style="font-size: 13px;color: #409eff;margin: 5px 0 0 10px;"
                  class="el-dropdown-link"
                >
                  更多<i class="el-icon-arrow-right"></i>
                </span>
              </el-dropdown>
            </div>
          );
        }
      });

      //表格顯示Tooltip提示
      this.columns.forEach((col) => {
        if (col.field == 'Describe') {
          col.title = '單元格Tooltip(鼠標放上來看效果)';
          col.showOverflowTooltip = true;
          //或者用下面這的個方法自定義提示
          // col.render = (h, { row, column, index }) => {
          //   return (
          //     <el-popover
          //       placement="top-start"
          //       title="提示信息"
          //       width={350}
          //       trigger="hover"
          //       content={
          //         row.Describe +
          //         ',這裡是用render渲染的表格提示,示例見:App_Appointment.js'
          //       }
          //     >
          //       {{ reference: <span>{row.Describe}</span> }}
          //     </el-popover>
          //   );
          // };
        }
      });

      //增加彈出框提示信息
      //https://cn.vuejs.org/guide/extras/render-function.html#passing-slots
      //自定義提示
      this.editFormOptions[0][0].extra = {
        render: (h) => {
          return (
            <el-popover
              placement="top-start"
              title="Title"
              width="200"
              trigger="hover"
              content="this is content, this is content, this is content"
            >
              {/* 這裡對應下面的#reference數據槽 */}
              {{ reference: <i class="el-icon-warning-outline"></i> }}
            </el-popover>
          );
        }
      };

      //   <el-popover
      //   placement="top-start"
      //   title="Title"
      //   :width="200"
      //   trigger="hover"
      //   content="this is content, this is content, this is content"
      // >
      //   <template #reference>
      //     <el-button>Hover to activate</el-button>
      //   </template>
      // </el-popover>

      //格式化單格顏色
      this.columns.forEach((x) => {
        if (x.field == 'PhoneNo') {
          x.title='設置背景顏色'
          x.cellStyle = (row, rowIndex, columnIndex) => {
            if (row.PhoneNo == '138888887698') { //&& rowIndex == 0
              return { background: '#ddecfd' };
            }else  if (row.PhoneNo == '138888887692') { //&& rowIndex == 0
              return { background: '#a9d6f9' };
            }
          };
        }
        if (x.field == 'Creator') {
          x.title='合併列'
          x.cellStyle = (row, rowIndex, columnIndex) => {
            if (row.Creator == '超級管理員') {
              return { background: 'rgb(45 140 240)', color: '#ffff' };
            }
          };
        }

        //設置進度條
        //進度條組件見:https://element-plus.gitee.io/zh-CN/component/progress.html#%E8%BF%9B%E5%BA%A6%E6%9D%A1%E5%86%85%E6%98%BE%E7%A4%BA%E7%99%BE%E5%88%86%E6%AF%94%E6%A0%87%E8%AF%86
        if (x.field == 'CreateDate') {
          x.title = '進度條'
          x.render = (h, { row, column, index }) => {
            if (index % 2 === 1) {
              //90改為row.字段
              return <el-progress stroke-width={4} percentage={90} />
            }
            //10改為row.字段
            return <el-progress stroke-width={4} color="#67c23a" show-text={true} percentage={10} />
          }
        }

        if (x.field == 'Name') {
          x.title = '點擊查看(合併行)';
        }
      });

      //設置表單分組
      this.editFormOptions.splice(0, 0, [
        {
          colSize: 12,
          render: (h) => {
            return (
              <div
                style={{
                  display: 'flex',
                  'margin-bottom': '-4px',
                  'line-height': '20px',
                  'margin-top': '5px',
                  'padding-bottom': '5px',
                  'border-bottom': '1px solid rgb(238, 238, 238)'
                }}
              >
                <div style="height: 19px; background: rgb(45, 206, 217); width: 9px; border-radius: 10px;"></div>
                <div style="padding-left: 6px; font-weight: bold; font-size: 13px;">
                  基本信息
                </div>
              </div>
            );
          }
        }
      ]);

      //設置表單分組

      this.editFormOptions.splice(2, 0, [
        {
          colSize: 12,
          render: (h) => {
            return (
              <div
                style={{
                  display: 'flex',
                  'margin-bottom': '-4px',
                  'line-height': '20px',
                  'margin-top': '5px',
                  'padding-bottom': '5px',
                  'border-bottom': '1px solid rgb(238, 238, 238)'
                }}
              >
                <div style="height: 19px; background: rgb(45, 206, 217); width: 9px; border-radius: 10px;"></div>
                <div style="padding-left: 6px; font-weight: bold; font-size: 13px;">
                  基本信息
                </div>
              </div>
            );
          }
        }
      ]);

      //彈出框增加分段alert提示
      this.editFormOptions.splice(3, 0, [
        {
          colSize: 12,
          render: (h) => {
            return (
              <el-alert
                title="這裡是自定的提示信息"
                style={{ padding: 0 }}
                type="success"
              ></el-alert>
            );
          }
        }
      ]);

      //查詢介面按鈕組
      //按鈕圖標這裡找https://element.eleme.cn/#/zh-CN/component/icon
      //第二個按鈕後面添加按鈕組
      this.buttons.splice(2, 1, {
        name: '按鈕組',
        type: 'primary',
        plain: true,
        color: '#009688',
        data: [
          {
            name: '按鈕一',
            icon: 'el-icon-plus',
            onClick: () => {
              this.$message.info('按鈕一');
            }
          },
          {
            name: '按鈕二',
            icon: 'el-icon-zoom-out',
            onClick: () => {
              this.$message.info('按鈕二');
            }
          }
        ]
      });
    },

    onInited() {
      //多頁籤菜單打開時，重新設置表格的最大高度
      if (this.$route.path == '/tabsTable') {
        this.tableMaxHeight = document.body.clientHeight - 415;
      } else {
        //設置表的最大高度
        this.tableMaxHeight = this.height - 125; //400
      }
      //移除快捷查詢
      this.singleSearch = null;
    },
    spanMethod({ row, column, rowIndex, columnIndex }) { //2023.08.19增加行、列合併功能,el合併文檔見https://element-plus.gitee.io/zh-CN/component/table.html#%E5%90%88%E5%B9%B6%E8%A1%8C%E6%88%96%E5%88%97
      if (rowIndex % 2 === 0) {
        if (columnIndex === 6) {
          return [1, 2]
        } 
        else if (columnIndex === 5) {
           return [0, 0]
        }
      }
      if (columnIndex === 1) {
        if (rowIndex % 2 === 0) {
          return {
            rowspan: 2,
            colspan: 1,
          }
        } else {
          return {
            rowspan: 0,
            colspan: 0,
          }
        }
      }
    }
  }
};
export default extension;
