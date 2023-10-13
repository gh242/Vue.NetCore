

import { h, resolveComponent } from 'vue';
let extension = {
  components: {
    gridHeader: ``,
    gridBody: {
      render() {
        return [
          h(resolveComponent('el-alert'), {
            style: { 'margin-bottom': '12px' },
            'show-icon': true, type: 'warning',
            closable: false, title: '複雜table内容，适用於table列較多的情況可以將列合併在一起顯示(代碼生成後，在對應js文件中設置合併顯示信息),只需幾行代碼即可實現,見:App_Expert2.js'
          }, ''),
        ]
      }
    },
    gridFooter: '',
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  }, //動態擴充組件或組件路徑
  buttons: {
    view: []
  },
  methods: { //事件擴展
    onInit() {

      this.buttons.push({
        name: '查看代碼',
        onClick: () => {
          window.open('http://api.volcore.xyz/vol.doc/appexpert2.html', 'blank');
        }
      });


      //設置單單元格合併
      this.initFirstColumn();
      //動態添加【狀態】列
      this.initColor();
      //在表最後添加操作列
      this.initColumnButton();
    },
    onInited() {
      //手動調整介面表高度
      this.height = this.height - 50;
      this.boxOptions.height = this.boxOptions.height + 40;
    },
    initColor() {
      var _index = this.columns.findIndex(x => { return x.field == "Enable" });
      if (_index == -1) {
        return;
      }
      //動態添加一列【狀態】
      this.columns.splice(_index, 1, {
        title: "狀態",
        field: "狀態",
        with: 60,
        formatter: (row) => {
          var _color = row.Enable == 1 ? '#ff1212' : '#04c966';
          return "<span style='background:" + _color + ";width: 8px;  height: 8px; position: absolute; border-radius: 50%; margin-left: 7px;'>&nbsp;</span>"
        }
      })
    },
    initFirstColumn() {
      //在第一行後面動態添加一行
      this.columns.splice(1, 0, {
        title: "用戶信息",
        field: "用戶信息",
        width: 220,
        render: (h, { row, column, index }) => {
          //下面所有需要顯示的信息都從row裡面取出來
          return h(
            "div",
            {
              style: { display: "flex", cursor: 'pointer', },
            },
            [
              h(
                "img",
                {
                  src: this.http.ipAddress + row.HeadImageUrl,
                  style: { "width": "70px", height: "90px", "object-fit": "cover" },
                },
              ),
              h(
                "div",
                {
                  props: {},
                  style: { "margin-left": "15px" },
                },
                [
                  h("div", { style: { "font-size": "12px", "color": "#459fff", "margin-bottom": "8px", "font-weight": "bold" } },
                    [h("span", {}, row.ReallyName),
                    h("span", { style: { "margin-left": "20px" } }, row.Enable == "1" ? '在線' : "離線")]
                  ),
                  h("div", { style: { "font-size": "12px", "color": "#459fff", 'line-height': '20px' } }, row.IDNumber || '無身份證信息'),
                  h("div", { style: { "font-size": "12px", "color": "#459fff", 'line-height': '20px' } }, "電話：" + row.PhoneNo),
                  h("div", { style: { "font-size": "12px", "color": "#459fff", 'line-height': '20px' } }, "城市：" + row.City),
                ]
              )])
        }
      })
    },
    initColumnButton() {
      //在表最後添加操作列
      this.columns.push({
        title: '操作',
        fixed: 'right',
        align: "center",
        width: 130,
        render: (h, { row, column, index }) => {
          return h(
            "div", { style: { cursor: 'pointer' } }, [
            h(
              resolveComponent("el-button"), {
              style: { padding: '0 7px' },
              type: "danger",
              plain: true,
              size: 'mini',
              onClick: (e) => {
                e.stopPropagation()
                this.del([row]);
              },
            },
              [h('i', { class: 'el-icon-delete' }, '刪除')]
            ),
            h(
              resolveComponent("el-button"), {
              style: { padding: '0 7px' },
              type: "primary",
              plain: true,
              size: 'mini',
              onClick: (e) => {
                e.stopPropagation()
                this.linkData(row);
              },
            },
              [h('i', { class: 'el-icon-edit' }, '修改')]
            ),
          ])
        }
      })
    }

  }
};
export default extension;