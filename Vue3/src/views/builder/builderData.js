let columnType = [{ "key": 1, "value": "img" },
{ "key": 2, "value": "excel" },
{ "key": 3, "value": "file" },
//2021.07.27增加table列顯示類型date(自動格式化)
{ "key": 4, "value": "date" }
]

let dataType = [
  { "key": "text", "value": "input" },
  { "key": "textarea", "value": "textarea" },
  { "key": "switch", "value": "switch" },
  // { "key": "dropList", "value": "dropList" },
  { "key": "select", "value": "select" },
  { "key": "selectList", "value": "select多選" },
  { "key": "date", "value": "date" },
  { "key": "datetime", "value": "datetime" },
  { "key": "month", "value": "年月日" },
  { "key": "rate", "value": "rate評分" },
  { "key": "time", "value": "time" },
  { "key": "checkbox", "value": "checkbox" },
  // 2021.05.16集成iview radio組件
  { "key": "radio", "value": "radio" },
  { "key": "cascader", "value": "級聯" },//2020.11.01增加級聯選擇
  { "key": "treeSelect", "value": "樹形級聯tree-select" },//2020.11.01增加級聯選擇
  { "key": "editor", "value": "富文本編輯器" },
  { "key": "mail", "value": "mail" },
  { "key": "number", "value": "number" },
  { "key": "decimal", "value": "decimal" },
  { "key": "phone", "value": "phone" },
  { "key": "img", "value": "img" },
  { "key": "excel", "value": "excel" },
  { "key": "file", "value": "file" }
];

let searchDataType = [
  { "key": "text", "value": "input" },
  { "key": "like", "value": "模糊查詢" },
  { "key": "textarea", "value": "textarea" },
  { "key": "switch", "value": "switch" },
  { "key": "select", "value": "select" },
  { "key": "selectList", "value": "select多選" },
  { "key": "date", "value": "date" },
  { "key": "datetime", "value": "datetime" },
  { "key": "month", "value": "year_month" },
  { "key": "time", "value": "time" },
  { "key": "cascader", "value": "級聯" },//2020.11.01增加級聯選擇
  { "key": "checkbox", "value": "checkbox" },
  // 2021.05.16集成iview radio組件
  { "key": "radio", "value": "radio" },
  { "key": "range", "value": "區間查詢" },
  { "key": "mail", "value": "mail" },
  { "key": "number", "value": "number" },
  { "key": "decimal", "value": "decimal" },
  { "key": "phone", "value": "phone" }
];
let data = {
  form: {
    fields: {
      table_Id: '',
      parentId: null,
      namespace: '',
      columnCNName: '',
      tableName: '',
      tableTrueName: '',
      folderName: '',
      detailCnName: '',
      detailName: '',
      expressField: '',
      sortName: '',
      richtitle: '',
      uploadField: '',
      uploadMaxCount: '',
      enable: 0,
      vuePath: '',
      appPath: "",
      userPermissionDesc: '開啟後當前用戶只能操作自己(與4下級角色)創建的數據,如:查詢、刪除、修改等操作'
    },
    addOptions: [
      [{ "title": "父 級 ID", min: 0, "field": "parentId", "required": true, type: 'number', placeholder: '放在【代碼生成配置】列表的文件夾ID下,如果填入【0】就是一級目錄' }],
      [{
        "title": "項目類庫",
        "field": "namespace",
        "placeholder": "代碼生成後的所在類庫(可以自己提前在後台項目中創建一個.netcore類庫)",
        "type": "select",
        "required": true,
        data: []
      }],
      [{ "title": "表中文名", "field": "columnCNName", "required": true, placeholder: "表對應的中文名字,介面上顯示會用到" }],
      [{ "title": "實際表名", "field": "tableName", "required": true, placeholder: "數據庫實際表名或者視圖名(多表關聯請創建視圖再生成代碼)" }],
      [{ "title": "文件夾名", placeholder: "生成文件所在類庫中的文件夾名(文件夾可以不存在);注意只需要填寫文件夾名，不是路徑", "field": "folderName", "required": true }]
    ],
    options: [
      [
        { "title": "主 鍵 ID", "field": "table_Id", "dataSource": [], readonly: true, disabled: true, columnType: 'int' },
        { "title": "父 級 ID", "field": "parentId", min: 0, "required": true, type: 'number' },
        {
          "title": "項目類庫",
          "placeholder": "代碼生成存放的位置",
          "field": "namespace",
          "type": "select",
          "required": true,
          data: []
        }
      ],
      [
        { "title": "表中文名", "field": "columnCNName", "dataSource": [], "required": true },
        { "title": "表 別 名", placeholder: "默認與4實際表名相同", "field": "tableName", "required": true },
        { "title": "實際表名", "field": "tableTrueName" },

      ],
      [
        { "title": "文件夾名", placeholder: "生成文件所在類庫中的文件夾名(文件夾可以不存在)", "field": "folderName", "required": true },
        { "title": "明細表名", "field": "detailCnName", placeholder: "明細表中文名字" },
        { "title": "明 細 表", "field": "detailName", placeholder: "數據庫的表名" },

      ],
      [
        { "title": "快捷編輯", "field": "expressField", placeholder: "快捷編輯字段" },
        { "title": "排序字段", "field": "sortName", "placeholder": "多個排序字段逗號隔開(默認降序排序),如：Name,Age", colSize: 8 },

        // { "title": "還沒想好", "field": "richtitle" }
      ],
      [{ "title": "Vue路徑", "field": "vuePath", type: "text", placeholder: 'Vue項目所在絕對路徑,到views文件夾,如：E:/app/src/views', colSize: 6 },
      { "title": "app路徑", "field": "appPath", type: "text", placeholder: 'uniapp項目所在絕對路徑,到pages文件夾,如：E:/uniapp/pages', colSize: 6 }]
      // [ //待完
      //     { "title": "開啟用戶權限數據", "field": "enable", bind: { data: [{ key: 1, value: '是', key: 0, value: '否' }] }, type: 'switch', colSize: 2 },
      //     { "title": "提示", "required": true, "field": "userPermissionDesc", colSize: 10, "placeholder": "非自增主鍵需要輸入排序字段",readonly:true }
      // ],
      // [

      // ],
      // [
      //     { "title": "富文本編輯字段", "field": "richtitle", "displayType": "title" },
      //     { "title": "文件上傳字段", "field": "uploadField", "displayType": "title" },
      //     { "title": "文件上傳數量限制", "field": "uploadMaxCount", "displayType": "title", columnType: 'int' }
      // ],
      // [
      //     { "title": "Vue視圖絕對路徑", "field": "vuePath", "displayType": "title", colSize: 12, placeholder: 'Vue項目所在絕對路徑,到views文件夾,如：E:/app/src/views' },
      // ]
    ]
  },
  //2021.01.09增加代碼生成器設置table排序功能
  columns: [
    { field: 'columnId', title: 'ColumnId', width: 120, align: 'left', edit: { type: "text" }, hidden: true },
    { field: 'table_Id', title: 'Table_Id', width: 120, align: 'left', editor: 'text', hidden: true },
    { field: 'columnCnName', title: '列顯示名稱', fixed: true, width: 120, align: 'left', edit: { type: "text" } },
    { field: 'columnName', title: '列名', fixed: true, width: 120, align: 'left', edit: { type: "text" } },
    { field: 'isKey', title: '主鍵', width: 90, align: 'left', edit: { type: "switch" } },
    { field: 'sortable', title: '是否排序', width: 90, align: 'left', edit: { type: "switch", keep: true } },
    {
      field: 'enable', title: 'app列', width: 140, align: 'left', edit: { type: "select" },
      bind: {
        data: [
          { key: 1, value: "顯示/查詢/編輯" },
          { key: 2, value: "顯示/編輯" },
          { key: 3, value: "顯示/查詢" },
          { key: 4, value: "顯示" },
          { key: 5, value: "查詢/編輯" },
          { key: 6, value: "查詢" },
          { key: 7, value: "編輯" },
        ]
      }
    },
    { field: 'searchRowNo', title: '查詢行', width: 90, align: 'left', edit: { type: "text" } },
    { field: 'searchColNo', title: '查詢列', width: 90, align: 'left', edit: { type: "text" } },
    { field: 'searchType', title: '查詢類型', width: 150, align: 'left', edit: { type: "select" }, bind: { data: searchDataType } },
    { field: 'editRowNo', title: '編輯行', width: 90, align: 'numberbox', edit: { type: "text" } },
    { field: 'editColNo', title: '編輯列', width: 90, align: 'numberbox', edit: { type: "text" } },
    { field: 'editType', title: '編輯類型', width: 150, align: 'left', edit: { type: "select" }, bind: { data: dataType } },
    { field: 'dropNo', title: '數據源', width: 120, align: 'left', bind: { data: [] }, edit: { type: "select", data: [] } },
    { field: 'isImage', title: 'table列顯示類型', hidden: false, width: 130, align: 'left', edit: { type: "select" }, bind: { data: columnType } },
    { field: 'orderNo', title: '列顯示順序', width: 120, align: 'left', edit: { type: "text" } },
    { field: 'maxlength', title: '字段最大長度', width: 130, align: 'left', edit: { type: "text" } },
    { field: 'columnType', title: '數據類型', width: 120, align: 'left', edit: { type: "text" } },
    { field: 'isNull', title: '可為空', width: 120, align: 'left', edit: { type: "switch", keep: true } },
    { field: 'isReadDataset', title: '是否只讀', width: 120, align: 'left', edit: { type: "switch", keep: true } },
    { field: 'isColumnData', title: '數據列', width: 120, align: 'left', edit: { type: "switch", keep: true } },
    { field: 'isDisplay', title: '是否顯示', width: 120, align: 'left', edit: { type: "switch", keep: true } },
    { field: 'columnWidth', title: 'table列寬度', width: 120, align: 'left', edit: { type: "text" } },
    { field: 'colSize', title: '編輯列標籤寬度colSize', width: 180, align: 'left', edit: { type: "text" } },
    // { field: 'import', title: '導入列', hidden: true, width: 100, align: 'left', edit: { type: "switch" } },
    // { field: 'apiInPut', title: 'Api輸入列(待實現)', width: 100, align: 'left', edit: { type: "switch" } },
    // { field: 'apiIsNull', title: 'Api輸入列可為空(待實現)', width: 130, align: 'left', edit: { type: "switch" } },
    // { field: 'apiOutPut', title: 'Api輸出列(待實現)', width: 100, align: 'left', edit: { type: "switch" } },
    // { field: 'columnformat', title: '顯示格式', width: 120, align: 'left', editor: 'text', editor: 'textarea' },
    // { field: 'script', title: '脚本', width: 120, align: 'left', editor: 'textarea' },
    // { field: 'creator', title: '創建人', width: 120, align: 'left' },
    { field: 'createDate', title: '創建時間', width: 120, align: 'left' },
    // { field: 'modifier', title: '修改人', width: 120, align: 'left' },
    // { field: 'modifyDate', title: '修改時間', width: 120, align: 'left' }
  ]
}

export default data