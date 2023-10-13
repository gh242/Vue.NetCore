<template>
  <div class="layout-container">
    <a :href="exportHref" ref="export"></a>
    <!--開啟懶加載2020.12.06 -->
    <vol-box
      :on-model-close="closeCustomModel"
      v-model="viewModel"
      :height="520"
      :width="500"
      :padding="0"
      :lazy="true"
      title="設置"
    >
      <template #content>
        <custom-column :view-columns="viewColumns"></custom-column>
      </template>
      <template #footer>
        <div style="text-align: center">
          <el-button type="default" size="small" @click="closeCustomModel"
            ><i class="el-icon-close"></i>取消</el-button
          >
          <el-button type="success" size="small" @click="initViewColumns(true)"
            ><i class="el-icon-refresh"></i>重置</el-button
          >
          <el-button type="primary" size="small" @click="saveColumnConfig"
            ><i class="el-icon-check"></i>確定</el-button
          >
        </div>
      </template>
    </vol-box>
    <ViewGridAudit @auditClick="saveAudit" :option="table" ref="audit">

    </ViewGridAudit>
 
    <!--導入excel功能-->
    <!--2020.10.31添加導入前的方法-->
    <!--開啟懶加載2020.12.06 -->
    <!-- 2022.01.08增加明細表導入判斷 -->
    <vol-box
      v-if="upload.url"
      v-model="upload.excel"
      :height="350"
      :width="600"
      :lazy="true"
      :title="(boxModel ? detailOptions.cnName : table.cnName) + '-導入'"
    >
      <UploadExcel
        ref="upload_excel"
        @importExcelAfter="importExcelAfter"
        :importExcelBefore="importExcelBefore"
        :url="upload.url"
        :template="upload.template"
      ></UploadExcel>
    </vol-box>
    <!--頭部自定義組件-->
    <component
      :is="dynamicComponent.gridHeader"
      ref="gridHeader"
      @parentCall="parentCall"
    ></component>
    <!--主介面查詢與4table表單布局-->
    <div class="view-container">
      <!-- 2020.09.11增加固定查詢表單 -->
      <!--查詢條件-->
      <div class="grid-search">
        <div
          :class="[fiexdSearchForm ? 'fiexd-search-box' : 'search-box']"
          v-show="searchBoxShow"
        >
          <!-- 2020.09.13增加formFileds拼寫錯誤兼容處理 -->
          <vol-form
            ref="searchForm"
            :load-key="false"
            style="padding: 0 15px"
            :label-width="labelWidth"
            :formRules="searchFormOptions"
            :formFields="searchFormFields"
            :select2Count="select2Count"
          >
            <template #footer>
              <div v-if="!fiexdSearchForm" class="form-closex">
                <el-button size="small" type="primary" plain @click="search">
                  <i class="el-icon-search" />查詢
                </el-button>

                <el-button
                  size="small"
                  type="success"
                  plain
                  @click="resetSearch"
                >
                  <i class="el-icon-refresh-right" />重置
                </el-button>
                <el-button
                  size="small"
                  plain
                  @click="searchBoxShow = !searchBoxShow"
                >
                  <i class="el-icon-switch-button" />關閉
                </el-button>
              </div>
            </template>
          </vol-form>
          <div v-if="fiexdSearchForm" class="fs-line"></div>
        </div>
        <div class="view-header">
          <div class="desc-text">
            <i class="el-icon-s-grid" />
            <span>{{ table.cnName }}</span>
          </div>
          <div class="notice">
            <a class="text" :title="extend.text">{{ extend.text }}</a>
          </div>
          <!--快速查詢字段-->
          <div class="search-line" v-if="!fiexdSearchForm">
            <QuickSearch
              v-if="singleSearch"
              :singleSearch="singleSearch"
              :searchFormFields="searchFormFields"
              :tiggerPress="quickSearchKeyPress"
            ></QuickSearch>
          </div>
          <!--操作按鈕組-->
          <!-- 2020.11.29增加查詢介面hidden屬性 -->

          <div class="btn-group">
            <template
              :key="bIndex"
              v-for="(btn, bIndex) in buttons.slice(0, maxBtnLength)"
            >
              <el-dropdown size="small" v-if="btn.data" :split-button="false">
                <el-button
                  :color="btn.color"
                  :dark="false"
                  :type="btn.type"
                  :plain="btn.plain"
                >
                  {{ btn.name }}<i class="el-icon-arrow-down el-icon--right"></i
                ></el-button>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item
                      v-for="(item, index) in btn.data"
                      :key="index"
                    >
                      <div @click="onClick(item.onClick)">
                        <i :class="item.icon"></i>
                        {{ item.name }}
                      </div>
                    </el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
              <el-button
                v-else
                :type="btn.type"
                size="small"
                :color="btn.color"
                :dark="false"
                :class="btn.class"
                :plain="btn.plain"
                v-show="!btn.hidden"
                @click="onClick(btn.onClick)"
              >
                <i :class="btn.icon"></i> {{ btn.name }}
              </el-button>
            </template>
            <el-button
              type="default"
              style="padding: 0px 10px"
              size="small"
              :plain="true"
              v-if="showCustom"
              @click="showCustomModel"
            >
              <i class="el-icon-s-grid"></i>
            </el-button>
            <el-dropdown
              size="small"
              @click="changeDropdown"
              v-if="buttons.length > maxBtnLength"
            >
              <el-button type="primary" plain size="small">
                更多<i class="el-icon-arrow-down el-icon--right"></i>
              </el-button>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item
                    @click="changeDropdown(item.name)"
                    :name="item.name"
                    v-show="!item.hidden"
                    v-for="(item, dIndex) in buttons.slice(
                      maxBtnLength,
                      buttons.length
                    )"
                    :key="dIndex"
                  >
                    <i :class="item.icon"></i> {{ item.name }}</el-dropdown-item
                  >
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </div>
        </div>

        <!-- 分割位置 -->
        <vol-box
          v-if="boxInit"
          v-model="boxModel"
          :title="boxOptions.title"
          :width="boxOptions.width"
          :height="boxOptions.height"
          :modal="boxOptions.modal"
          :draggable="boxOptions.draggable"
          :padding="0"
          :on-model-close="onGridModelClose"
          @fullscreen="fullscreen"
        >
          <!--明細頭部自定義組件-->
          <template #content>
            <div class="box-com">
              <component
                :is="dynamicComponent.modelHeader"
                ref="modelHeader"
                @parentCall="parentCall"
              ></component>
              <!-- <div v-show="isBoxAudit" class="flow-step">
                <div v-for="(item, index) in workFlowSteps" :key="index">
                  {{ item.stepName }}
                </div>
              </div> -->
              <div class="item form-item">
                <vol-form
                  ref="form"
                  :editor="editor"
                  :load-key="false"
                  :label-width="boxOptions.labelWidth"
                  :formRules="editFormOptions"
                  :formFields="editFormFields"
                  :select2Count="select2Count"
                ></vol-form>
              </div>
              <!--明細body自定義組件-->
              <component
                :is="dynamicComponent.modelBody"
                ref="modelBody"
                @parentCall="parentCall"
              ></component>
              <div
                v-show="hasDetail"
                v-if="detail.columns && detail.columns.length > 0"
                class="grid-detail table-item item"
              >
                <div class="toolbar">
                  <div class="title form-text">
                    <span>
                      <i class="el-icon-menu" />
                      {{ detail.cnName }}
                    </span>
                  </div>
                  <!--明細表格按鈕-->
                  <div class="btns" v-show="!isBoxAudit">
                    <el-button
                      v-for="(btn, bIndex) in detailOptions.buttons"
                      :key="bIndex"
                      :plain="btn.plain"
                      v-show="!(typeof btn.hidden == 'boolean' && btn.hidden)"
                      @click="onClick(btn.onClick)"
                      size="small"
                      ><i :class="btn.icon"></i>{{ btn.name }}</el-button
                    >
                  </div>
                </div>
                <vol-table
                  ref="detail"
                  @loadBefore="loadInternalDetailTableBefore"
                  @loadAfter="loadDetailTableAfter"
                  @rowChange="detailRowOnChange"
                  @rowClick="detailRowOnClick"
                  :url="detailOptions.url"
                  :load-key="false"
                  :index="true"
                  :tableData="detailOptions.data"
                  :columns="detailOptions.columns"
                  :pagination="detailOptions.pagination"
                  :height="detailOptions.height"
                  :single="detailOptions.single"
                  :pagination-hide="false"
                  :defaultLoadPage="detailOptions.load"
                  :beginEdit="detailOptions.beginEdit"
                  :endEditBefore="detailOptions.endEditBefore"
                  :endEditAfter="detailOptions.endEditAfter"
                  :summary="detailOptions.summary"
                  :click-edit="detailOptions.clickEdit"
                  :double-edit="detailOptions.doubleEdit"
                  :column-index="detailOptions.columnIndex"
                  :ck="detailOptions.ck"
                  :text-inline="detailOptions.textInline"
                  :select2Count="select2Count"
                  :selectable="detailSelectable"
                  :spanMethod="detailSpanMethod"
                ></vol-table>
              </div>
              <!--明細footer自定義組件-->
              <component
                :is="dynamicComponent.modelFooter"
                ref="modelFooter"
                @parentCall="parentCall"
              ></component>
            </div>
          </template>
          <template #footer>
            <div style="text-align: center;" v-show="isBoxAudit">
              <el-button
                size="small"
                type="primary"
                plain
                @click="onGridModelClose(false)"
              >
                <i class="el-icon-close">關閉</i>
              </el-button>
              <el-button
                size="small"
                type="primary"
                v-show="auditParam.showViewButton"
                @click="auditParam.model = true"
              >
                <i class="el-icon-view">審批</i>
              </el-button>
            </div>
            <div v-show="!isBoxAudit">
              <el-button
                v-for="(btn, bIndex) in boxButtons"
                :key="bIndex"
                :type="btn.type"
                size="small"
                :plain="btn.plain"
                v-show="!(typeof btn.hidden == 'boolean' && btn.hidden)"
                :disabled="btn.hasOwnProperty('disabled') && !!btn.disabled"
                @click="onClick(btn.onClick)"
              >
                <i :class="btn.icon"></i>{{ btn.name }}
              </el-button>
              <el-button
                size="small"
                type="primary"
                plain
                @click="onGridModelClose(false)"
              >
                <i class="el-icon-close">關閉</i>
              </el-button>
            </div>
          </template>
        </vol-box>
      </div>
      <!--body自定義組件-->
      <div class="grid-body">
        <component
          :is="dynamicComponent.gridBody"
          ref="gridBody"
          @parentCall="parentCall"
        ></component>
      </div>

      <!--table表格-->
      <div class="grid-container">
        <!-- 2021.05.02增加樹形結構 rowKey -->
        <vol-table
          ref="table"
          :single="single"
          :rowKey="rowKey"
          :loadTreeChildren="loadTreeTableChildren"
          @loadBefore="loadTableBefore"
          @loadAfter="loadTableAfter"
          @rowChange="rowOnChange"
          @rowClick="rowOnClick"
          @rowDbClick="rowOnDbClick"
          :tableData="[]"
          :linkView="linkData"
          :columns="columns"
          :pagination="pagination"
          :height="height"
          :max-height="tableMaxHeight"
          :pagination-hide="false"
          :url="url"
          :load-key="false"
          :defaultLoadPage="load"
          :summary="summary"
          :double-edit="doubleEdit"
          :index="doubleEdit"
          :beginEdit="tableBeginEdit"
          :endEditBefore="tableEndEditBefore"
          :click-edit="true"
          :column-index="columnIndex"
          :text-inline="textInline"
          :ck="ck"
          :select2Count="select2Count"
          :selectable="selectable"
          :spanMethod="spanMethod"
        ></vol-table>
      </div>
    </div>

    <!--footer自定義組件-->
    <component
      :is="dynamicComponent.gridFooter"
      ref="gridFooter"
      @parentCall="parentCall"
    ></component>
  </div>
</template>

<script>
const _const = {
  EDIT: 'update',
  ADD: 'Add',
  VIEW: 'view',
  PAGE: 'getPageData',
  AUDIT: 'audit',
  DEL: 'del',
  EXPORT: 'Export', //導出操作返回加密後的路徑
  DOWNLOAD: 'DownLoadFile', //導出文件
  DOWNLOADTEMPLATE: 'DownLoadTemplate', //下載導入模板
  IMPORT: 'Import', //導入(導入表的Excel功能)
  UPLOAD: 'Upload' //上傳文件
};
import Empty from '@/components/basic/Empty.vue';

import VolTable from '@/components/basic/VolTable.vue';
import VolForm from '@/components/basic/VolForm.vue';
import {
  defineAsyncComponent,
  defineComponent,
  ref,
  shallowRef,
  toRaw
} from 'vue';
var vueParam = {
  components: {
    'vol-form': VolForm,
    'vol-table': VolTable,
    VolBox: defineAsyncComponent(() => import('@/components/basic/VolBox.vue')),
    QuickSearch: defineAsyncComponent(() =>
      import('@/components/basic/QuickSearch.vue')
    ),
    Audit: defineAsyncComponent(() => import('@/components/basic/Audit.vue')),
    UploadExcel: defineAsyncComponent(() =>
      import('@/components/basic/UploadExcel.vue')
    ),
    'custom-column': defineAsyncComponent(() =>
      import('./ViewGridCustomColumn.vue')
    ),
    'vol-header': defineAsyncComponent(() => import('./../VolHeader.vue')),
     ViewGridAudit: defineAsyncComponent(() => import('./ViewGridAudit.vue'))
  },
  props: {},
  setup(props) {
    //2021.07.17調整擴展組件組件
    const dynamicCom = {
      gridHeader: Empty,
      gridBody: Empty,
      gridFooter: Empty,
      modelHeader: Empty,
      modelBody: Empty,
      modelFooter: Empty
    };
    //合併擴展組件
    if (props.extend.components) {
      for (const key in props.extend.components) {
        if (props.extend.components[key]) {
          dynamicCom[key] = toRaw(props.extend.components[key]);
        }
      }
    }
    const dynamicComponent = shallowRef(dynamicCom);
    return { dynamicComponent };
  },
  data() {
    return {
      isBoxAudit: false,
      formFieldsType: [],
      workFlowSteps: [],
      //樹形結構的主鍵字段，如果設置值默認會開啟樹形table；注意rowKey字段的值必須是唯一（2021.05.02）
      rowKey: undefined,
      fiexdSearchForm: false, //2020.09.011是否固定查詢表單，true查詢表單將固定顯示在表單的最上面
      _inited: false,
      doubleEdit: false, //2021.03.19是否開啟查詢介面表格雙擊編輯
      single: false, //表是否單選
      const: _const, //增刪改查導入導出等對應的action
      boxInit: false, //新建或編輯的彈出框初化狀態，默認不做初始化，點擊新建或編輯才初始化彈出框
      searchBoxShow: false, //高級查詢(介面查詢後的下拉框點擊觸發)
      singleSearch: {}, //快速查詢字段
      exportHref: '',
      currentAction: _const.ADD, //當新建或編輯時，記錄當前的狀態:如當前操作是新建
      currentRow: {}, //當前編輯或查看數據的行
      closable: false,
      boxModel: false, //彈出新建、編輯框
      width: 700, //彈出框查看表數據結構
      labelWidth: 100, //高級查詢的標籤寬度
      viewModel: false, //查看表結構的彈出框
      viewColumns: [], //查看表結構的列數據
      viewColumnsClone: [],
      showCustom: true, //是否顯示自定義配置列按鈕2022.05.27
      // viewData: [], //查看表結構信息
      maxBtnLength: 8, //介面按鈕最多顯示的個數，超過的數量都顯示在更多中
      buttons: [], //查詢介面按鈕  如需要其他操作按鈕，可在表對應的.js中添加(如:Sys_User.js中buttons添加其他按鈕)
      splitButtons: [],
      uploadfiled: [], //上傳文件圖片的字段
      boxButtons: [], //彈出框按鈕 如需要其他操作按鈕，可在表對應的.js中添加
      dicKeys: [], //當前介面所有的下拉框字典編號及數據源
      hasKeyField: [], //有字典數據源的字段
      keyValueType: { _dinit: false },
      url: '', //介面表查詢的數據源的url
      hasDetail: false, //是否有從表(明細)表格數據
      initActivated: false,
      load: true, //是否默認加載表數據
      activatedLoad: false, //頁面觸發actived時是否刷新頁面數據
      summary: false, //查詢介面table是否顯示合計
      //需要從遠程綁定數據源的字典編號,如果字典數據源的查詢結果較多，請在onInit中將字典編號添加進來
      //只對自定sql有效
      remoteKeys: [],
      columnIndex: false, //2020.11.01是否顯示行號
      ck: true, //2020.11.01是否顯示checkbox
      continueAdd: false, //2021.04.11新建時是否可以連續新建操作
      continueAddName: '保存後繼續添加', //2021.04.11按鈕名稱
      // detailUrl: "",
      detailOptions: {
        //彈出框從表(明細)對象
        //從表配置
        buttons: [], //彈出框從表表格操作按鈕,目前有刪除行，添加行，刷新操作，如需要其他操作按鈕，可在表對應的.js中添加
        cnName: '', //從表名稱
        key: '', //從表主鍵名
        data: [], //數據源
        columns: [], //從表列信息
        edit: true, //明細是否可以編輯
        single: false, //明細表是否單選
        load: false, //
        delKeys: [], //當編輯時刪除當前明細的行主鍵值
        url: '', //從表加載數據的url
        pagination: { total: 0, size: 100, sortName: '' }, //從表分頁配置數據
        height: 0, //默認從表高度
        textInline: true, //明細表行内容顯示在一行上，如果需要換行顯示，請設置為false
        doubleEdit: true, //使用雙擊編輯
        clickEdit: false, //是否開啟點擊單元格編輯，點擊其他行時結束編輯
        currentReadonly: false, //當前用戶沒有編輯或新建權限時，表單只讀(可用於判斷用戶是否有編輯或新建權限)
        //開啟編輯時
        beginEdit: (row, column, index) => {
          return true;
        },
        //結束編輯前
        endEditBefore: (row, column, index) => {
          return true;
        },
        //結束編輯後
        endEditAfter: (row, column, index) => {
          return true;
        },
        columnIndex: false, //2020.11.01明細是否顯示行號
        ck: true //2020.11.01明細是否顯示checkbox
      },
      auditParam: {
        //審核對象
        rows: 0, //當前選中審核的行數
        model: false, //審核彈出框
        value: -1, //審核結果
        status: -1,
        reason: '', //審核原因
        height: 500,
        showViewButton: true,
        auditHis: [],
        showAction: false, //是否顯示審批操作(當前節點為用戶審批時顯示)
        //審核選項(可自行再添加)
        data: [
          { text: '通過', value: 1 },
          { text: '拒絕', value: 2 },
          { text: '駁回', value: 3 }
        ]
      },
      upload: {
        //導入上傳excel對象
        excel: false, //導入的彈出框是否顯示
        url: '', //導入的路徑,如果沒有值，則不渲染導入功能
        template: {
          //下載模板對象
          url: '', //下載模板路徑
          fileName: '' //模板下載的中文名
        },
        init: false //是否有導入權限，有才渲染導入組件
      },
      height: 0, //表高度
      tableHeight: 0, //查詢頁面table的高度
      tableMaxHeight: 0, //查詢頁面table的最大高度
      textInline: true, //table内容超出後是否不換行2020.01.16
      pagination: { total: 0, size: 30, sortName: '' }, //從分頁配置數據
      boxOptions: {
        title: '', //彈出框顯示的標題2022.08.01
        saveClose: true,
        labelWidth: 100,
        height: 0,
        width: 0,
        summary: false, //彈出框明細table是否顯示合計
        draggable: false, //2022.09.12彈出框拖動功能
        modal: true //2022.09.12彈出框背景遮罩層
      }, //saveClose新建或編輯成功後是否關閉彈出框//彈出框的標籤寬度labelWidth
      editor: {
        uploadImgUrl: '', //上傳路徑
        upload: null //上傳方法
      },
      numberFields: [],
      //2022.09.26增加自定義導出文件名
      downloadFileName: null,
      select2Count: 500 //超出500數量顯示select2組件
    };
  },
  methods: {},
  activated() {
     this.initFlowQuery();
    //2020.06.25增加activated方法
    this.onActivated && this.onActivated();
    if (!this._inited) {
      this._inited = true;
      return;
    }
    if (this.activatedLoad) {
      this.refresh();
    }
  },
  mounted() {
    this.mounted();
    // this.$refs.searchForm.forEach()
  },
  unmounted() {
    this.destroyed();
  },
  created: function() {
    //合併自定義業務擴展方法
    Object.assign(this, this.extend.methods);
    //如果沒有指定排序字段，則用主鍵作為默認排序字段
    this.pagination.sortName = this.table.sortName || this.table.key;
    this.initBoxButtons(); //初始化彈出框與4明細表格按鈕
    this.initAuditColumn();
    this.onInit(); //初始化前，如果需要做其他處理在擴展方法中覆蓋此方法
    this.getButtons();
    //初始化自定義表格列
    this.initViewColumns();
    //初始編輯框等數據
    this.initBoxHeightWidth();
    this.initDicKeys(); //初始下框數據源
    this.onInited(); //初始化後，如果需要做其他處理在擴展方法中覆蓋此方法
  },
  beforeUpdate: function() {},
  updated: function() {}
};

import props from './props.js';
import methods from './methods.js';

//合併屬性
vueParam.props = Object.assign(vueParam.props, props);
//合併方法
vueParam.methods = Object.assign(
  vueParam.methods,
  methods,
  props.extend.methods
);
export default defineComponent(vueParam);
</script>
<style lang="less" scoped>
@import './ViewGrid.less';
</style>
<style lang="less" scoped>
.btn-group ::v-deep(.ivu-select-dropdown) {
  padding: 0px !important;
  right: 3px;
}

.btn-group ::v-deep(.ivu-select-dropdown .ivu-dropdown-menu) {
  min-width: 100px;
  right: -2px;
  position: absolute;
  background: white;
  width: 130px;
  border-radius: 5px;
  border: 1px solid #e7e5e5;
}

.vertical-center-modal ::v-deep(.srcoll-content) {
  padding: 0;
}

.view-model-content {
  background: #eee;
}

.grid-detail ::v-deep(.v-table .el-table__header th) {
  height: 44px;
}
</style>
<style lang="less" scoped>
.grid-search {
  position: relative;

  .search-box {
    background: #fefefe;
    margin-top: 33px;
    border: 1px solid #eae8e8;
    position: absolute;
    z-index: 999;
    left: 15px;
    right: 15px;
    padding: 25px 20px;
    padding-bottom: 0;
    border-top: 0;
    box-shadow: 0 7px 18px -12px #bdc0bb;
  }
}
</style>
