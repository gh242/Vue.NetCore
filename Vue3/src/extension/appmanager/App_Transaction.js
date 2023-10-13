import { h, resolveComponent } from 'vue';
let extension = {
  components: {
    //動態擴充組件或組件路徑
    //表單header、content、footer對應位置擴充的組件
    gridHeader: '', //{ template: "<div>擴展組xx件</div>" },
    // gridBody: {
    //   template: '<Alert  type="success" show-icon>\
    //     查詢默認/顯示所有查詢條件<template slot="desc"><p>1、 onInit(){ this.setFiexdSearchForm(true);  //設置固定顯示所有查詢條件}</p>\
    //     2、 設置默認查詢值(下拉框、默認日期)、查詢介面表高度等,具體見App_Transaction.js配置與4說明</template>\
    //    </Alert>' },
    gridBody: {
      render() {
        return h(
          resolveComponent('ElAlert'),
          {
            style: { 'margin-bottom': '12px' },
            'show-icon': true,
            type: 'success',
            closable: false
          },
          [
            h(
              'p',
              {},
              '1、默認介面上顯示的列都可以導入，也可以在後台指定導入與4導出的列，見後台開發文檔->後台基礎代碼擴展實現導入方法說明'
            ),
            h(
              'p',
              {},
              ' 2、添加switch、checkbox組件，設置默認查詢值(下拉框、默認日期)、查詢介面表高度等,App_Transaction.js配置與4說明'
            )
          ]
        );
      }
    },
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  tableAction: 'App_Appointment', //指定菜單權限，其他任何頁面引用時都會走對應權限
  text:
    '目前導入導出的字段為代碼生成器中配置[是否顯示與4編輯列],可自行添加配置字段處理',
  buttons: [], //擴展的按鈕
  methods: {
    //事件擴展

    //事件擴展
    onInit() {

      this.buttons.push({
        name: '查看代碼',
        onClick: () => {
          window.open('http://api.volcore.xyz/vol.doc/apptransaction.html', 'blank');
        }
      });


      //設置查詢條件
      // this.searchFormFields.Name = "林";
      //設置下拉框的字段(是否買入)的默認值(注意下拉框key如要是數字同樣也要設置為字符類型數字,如果是自定sql數據源的key則需要設置成數字類型)
      //  this.searchFormFields.TransactionType = '1';
      //注意日期設置默認值的時候 ，如果查詢類型選擇的是datetime後面一定要寫上,00:00:00
      //如果查詢類型是date直接寫年月日即可
      this.searchFormFields.CreateDate = [
        '2015-01-01 00:00:00',
        '2030-12-31 00:00:00'
      ];
      //設置頁面上顯示的按鈕個數(不是必須的)
      this.maxBtnLength = 6;
      //設置顯示所有查詢條件
      this.setFiexdSearchForm(true);

      this.columns.forEach(col=>{
          if (col.field=='Describe') {
            col.hidden=true;
          }
      })

      this.columns.push({
        title: 'switch',
        field: 'switch',
        width: 80,
        fixed: 'right',
        align: 'center',
        render: (h, { row, column, index }) => {
          //這裡必須將要操作的字段變更為bool類型
          row.Enable=row.Enable||false;
          return (
            <div>
              <el-switch
                v-model={row.Enable}
                onChange={(val) => {
                  this.switchChange(row);
                }}
              ></el-switch>
            </div>
          );
        }
      });

      this.columns.push({
        title: 'checkbox',
        field: 'checkbox',
        width: 80,
        fixed: 'right',
        align: 'center',
        render: (h, { row, column, index }) => {
          //這裡必須將要操作的字段變更為bool類型
          row.Enable=row.Enable||false;
          return (
            <div>
              <el-checkbox
                v-model={row.Enable}
                onChange={(val) => {
                  this.switchChange(row);
                }}
              ></el-checkbox>
            </div>
          );
        }
      });
    },
    switchChange(row) {
      this.$message.success(row.Enable + '');
    },
    onInited() {
      //設置查詢介面表高度
      this.height = this.height-215;
    }
  }
};
export default extension;
