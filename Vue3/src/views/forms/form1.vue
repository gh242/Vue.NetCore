<template>
  <div class="single-form">
    <VolForm ref="myform" :formFields="formFields" :formRules="formRules">
      <div style="text-align: center; margin-top: 10px">
        <el-button type="primary" size="small" @click="getForm"
          >獲取表單</el-button
        >
        <el-button type="success" size="small" @click="reset"
          >重置表單</el-button
        >
      </div>
    </VolForm>
    <el-alert
      title="關於VolForm表單"
      type="success"
      style="margin-left: 30px; align-items: flex-start; padding-top: 25px"
      show-icon
      :closable="false"
    >
      <p>1、VolForm基於Element Plus常用組件封裝的表單。</p>
      <p>
        2、組件包括：select、select聯動、switch、radio、checkbox、多選、日期、圖片顯示與4預覽、文件上傳下載、input、render動態渲染等組件。
      </p>
      <p>
        3、封裝後的組件功能包括：自動綁定下拉框數據源、及表單驗證等常用功能。
      </p>
      <p>4、 只需要照著文檔配置json即可完成各種表單。</p>

      <p>
        5、具體使用見：框架文檔->組件api->VolForm。<a
          href="http://v2.volcore.xyz/document/api"
          target="_blank"
          >點擊查看文檔
        </a>
      </p>
    </el-alert>
  </div>
</template>
<script>
let $vue;
import VolForm from '@/components/basic/VolForm.vue';
import { readonly } from '@vue/reactivity';
export default {
  components: { VolForm },
  created() {
    $vue = this;
  },
  methods: {
    add() {
      //動態添加列
      this.formFields = {};
      this.index += 1;
      this.formFields['test' + this.index] = '';
      this.required = !this.required;
      this.formRules.splice(0);
      //如果有對列刪除，必須重置表單，否則驗證提示的標籤顯示位置會錯措
      this.reset();
      this.formRules.push(
        ...[
          [
            {
              dataKey: 'pz', //已經設置loadKey自動綁定數據源
              data: [],
              type: 'datetime',
              title: 'test' + this.index,
              required: this.required,
              field: 'test' + this.index,
              colSize: 12
            }
          ]
        ]
      );
    },
    getForm() {
      if (!this.$refs.myform.validate()) {
        return;
      }
      this.$message.error(JSON.stringify(this.formFields));
    },
    reset() {
      //重置表單，重置時可指定重置的值，如果沒有指定重置的值，默認全部清空
      let data = { Variety: '1', AvgPrice: 888 };
      this.$refs.myform.reset(data);
      this.$message.error('表單已重置');
    }
  },
  data() {
    return {
      loadKey: true,
      index: 1,
      required: false,
      formFields: {
        Variety: '',
        AgeRange: '', // [50, 100],
        DateRange: [],
        ParentId: [],
        AvgPrice: null,
        Date: '',
        date3: '',
        InpuText: '',
        InpuText1: ''
      },
      formRules: [
        [
          {
            type: 'range',
            title: '區間輸入',
            required: true,
            placeholder: '你可以自己定義placeholder顯示的文字', //顯示自定義的信息
            field: 'AvgPrice'
          }
        ],
        [
          {
            dataKey: 'age', //後台下拉框對應的數據字典編號
            data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
            //data是須的參數，可以默認一個[]
            title: '選擇事件',
            required: false, //設置為必選項
            field: 'AgeRange',
            placeholder: '可觸發事件的下拉框',
            type: 'select',
            onChange(value, param) {
              //設置選擇數據時觸發的事件
              $vue.formFields.AvgPrice =
                (Math.random(1, 1000) * 100).toFixed(2) * 1;
              $vue.$message.info(
                '當前選中的值為[' +
                  value +
                  '],選中後給成交均價賦一個随機值[' +
                  $vue.formFields.AvgPrice +
                  ']'
              );
            }
          }
        ],
        [
          {
            title: '自動綁定',
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [
              { key: '1', value: '1x' },
              { key: '2', value: '2x' }
            ],
            required: true,
            field: 'Variety',
            dataKey: 'pz',
            data: [],
            type: 'select'
          }
        ],
        [
          {
            title: '輸入事件',
            required: true,
            field: 'InpuText',
            type: 'text',
            onKeyPress: ($event) => {
              this.formFields.InpuText1 = $event.key;
            },
            extra: {
              style: 'color:red;cursor: pointer;',
              icon: 'el-icon-edit', //顯示圖標
              text: '點擊', //顯示文本
              //觸發事件
              click: (item) => {
                this.$message.error('點擊標籤觸發的事件');
              }
            }
          }
        ],
        [
          {
            title: '輸入事件',
            required: true,
            readonly: true,
            field: 'InpuText1',
            placeholder: '上面添加onKeyPress事件',
            type: 'text',
            readonly: true
          }
        ],
        [
          {
            dataKey: 'tree_roles',
            title: '級聯選擇',
            required: true,
            field: 'ParentId',
            checkStrictly: true, //設置可以選擇任意一級
            data: [],
            // data: [
            //   {
            //     value: "北京",
            //     label: "北京",
            //     children: [
            //       { value: "天壇", label: "天壇" },
            //       { value: "王府井", label: "王府井" },
            //     ],
            //   },
            // ],
            type: 'cascader'
          }
        ],
        // [
        //   {
        //     title: "日期單選",
        //     required: true,
        //     field: "Date",
        //     type: "datetime",
        //   },
        // ],
        [
          {
            title: '多選日期',

            range: true, //設置為true可以選擇開始與4結束日期
            required: true,
            field: 'DateRange',
            type: 'date'
          }
        ],
        [
          {
            title: '可選日期',
            min: '2021-07-01 00:00:000', //設置只能選2021-07-01到今天的數據(注意後面的小時00:00:000)
            max: Date.now(), // "2021-07-10",
            required: true,
            field: 'date3',
            placeholder: '設置日期選擇範圍(可選與4不可選)',
            type: 'date'
          }
        ]
      ]
    };
  }
};
</script>
<style scoped>
.single-form {
  position: relative;
  max-width: 1200px;
  padding: 30px 45px;
  left: 0;
  display: flex;
  margin: auto;
}
</style>
