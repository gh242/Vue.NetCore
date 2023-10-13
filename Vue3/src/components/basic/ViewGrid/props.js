let props = {
  columns: {//當前表的配置信息
    type: Array,
    default: () => {
      return [];
    }
  },
  detail: {//從表明細配置
    type: Object,
    default: () => {
      return {
        columns: [],//從表列
        sortName: ""//從表排序字段
      };
    }
  },
  editFormFields: {//新建、編輯字段(key/value)
    type: Object,
    default: () => {
      return {};
    }
  },
  editFormOptions: {//新建、編輯配置信息
    type: Array,
    default: () => {
      return [];
    }
  },
  searchFormFields: {//查詢字段(key/value)
    type: Object,
    default: () => {
      return {};
    }
  },
  searchFormOptions: {//查詢配置信息(key/value)
    type: Array,
    default: () => {
      return [];
    }
  },
  table: {//表的配置信息：主鍵、排序等
    type: Object,
    default: () => {
      return {};
    }
  },
  extend: {//表的擴展方法與4組件都合併到此屬性中
    type: Object,
    default: () => {
      return {};
    }
  }
}

export default props;