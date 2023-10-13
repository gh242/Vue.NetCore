import detailMethods from './detailMethods.js';
//業務處理方法,全部可以由開發覆蓋
import serviceFilter from './serviceFilter.js';
let methods = {
  //當添加擴展組件gridHeader/gridBody/gridFooter及明細modelHeader/modelBody/modelFooter時，
  //如果要獲取父級Vue對象,請使用此方法進行回調
  parentCall(fun) {
    if (typeof fun != 'function') {
      return console.log('擴展組件需要傳入一個回調方法才能獲取父級Vue對象');
    }
    fun(this);
  },
  getCurrentAction() {
    if (this.currentReadonly) {
      return '';
    }
    return '--' + (this.currentAction == this.const.ADD ? '新增' : '編輯');
  },
  quickSearchKeyPress($event) {
    //查詢字段為input時，按回車查詢
    if ($event.keyCode == 13) {
      if (this.searchFormFields[this.singleSearch.field] != '') {
        this.search();
      }
    }
  },
  getButtons() {
    //生成ViewGrid介面的操作按鈕及更多選項
    let searchIndex = this.buttons.findIndex((x) => {
      return x.value == 'Search';
    });
    //添加高級查詢
    let hasOneFormItem =
      this.searchFormOptions.length == 1 &&
      this.searchFormOptions[0].length == 1;
    if (searchIndex != -1 && !hasOneFormItem) {
      this.buttons.splice(searchIndex + 1, 0, {
        icon: this.fiexdSearchForm ? 'el-icon-refresh-left' : 'el-icon-search',
        name: this.fiexdSearchForm ? '重置' : '高級查詢',
        plain: true,
        type: this.buttons[searchIndex].type,
        onClick: () => {
          if (this.fiexdSearchForm) {
            return this.resetSearch();
          }
          this.searchBoxShow = !this.searchBoxShow;
        }
      });
    }
    if (hasOneFormItem) {
      this.fiexdSearchForm = false;
    }
    this.maxBtnLength += searchIndex == -1 ? 0 : 1;
    // if (this.buttons.length <= this.maxBtnLength) {
    //   return this.buttons;
    // }
    // let btns = this.buttons.slice(0, this.maxBtnLength);
    // btns[this.maxBtnLength - 1].last = true;
    // return btns;
  },
  extendBtn(btns, source) {
    //btns權限按鈕，source為擴展按鈕
    if (!btns || !(source && source instanceof Array)) {
      return;
    }
    //source通過在表的擴展js文件中buttons對應按鈕的屬性index决定按鈕所放位置
    source.forEach((x) => {
      //通過按鈕的Index屬性，放到指定的位置
      btns.splice(x.index == undefined ? btns.length : x.index, 0, x);
    });
    // if (this.extend.buttons.view) {
    //     this.extend.buttons.view.forEach((x) => {
    //         //通過按鈕的Index屬性，放到指定的位置
    //         this.buttons.splice(x.index == undefined ? this.buttons.length : x.index, 0, x);
    //     })
    // }
  },
  initBoxButtons() {
    //初始化ViewGird與4彈出框/明細表按鈕
    let path = this.$route.path;
    //通過菜單獲取用戶所對應菜單需要顯示的按鈕
    let permissionButtons = this.permission.getButtons(
      path,
      null,
      this.extend.tableAction,
      this.table.name
    );
    if (permissionButtons) {
      //2020.03.31添加深拷貝按鈕組
      permissionButtons.forEach((p) => {
        let _obj = {};
        for (const key in p) {
          _obj[key] = p[key];
        }
        this.buttons.push(_obj);
      });
      // this.buttons.push(...permissionButtons);
    }
    if (!this.extend) {
      this.extend = {};
    }
    if (!this.extend.buttons) {
      this.extend.buttons = {};
    }
    //查詢介面擴展按鈕(擴展按鈕可自行通過設置按鈕的Index屬性顯示到具體位置)
    if (this.extend.buttons.view) {
      this.extendBtn(this.buttons, this.extend.buttons.view);
    }

    //彈出框按鈕
    let boxButtons = [];

    let saveBtn = this.buttons.some((x) => {
      if (
        x.value &&
        (x.value.toLowerCase() == this.const.ADD.toLowerCase() ||
          x.value.toLowerCase() == this.const.EDIT.toLowerCase())
      )
        return true;
    });
    this.currentReadonly = !saveBtn;
    //從表表格操作按鈕
    let detailGridButtons = {
      name: '刷新',
      type: 'info',
      icon: 'el-icon-refresh',
      onClick() {
        //如果明細表當前的狀態為新建時，禁止刷新
        if (this.currentAction == this.const.ADD) {
          return;
        }
        this.refreshRow();
      }
    };

    let importExcel = this.buttons.some((x) => {
      if (x.value == this.const.IMPORT) return true;
    });
    //如果有導入權限,則需要初始化導入組件
    if (importExcel) {
      this.upload.url = this.getUrl(this.const.IMPORT);
      //定義下載模板的文件名
      this.upload.template.fileName = this.table.cnName;
      //定義下載模板的Url路徑
      this.upload.template.url =
        this.http.ipAddress + this.getUrl(this.const.DOWNLOADTEMPLATE, true);
    }

    // disabled
    //如果當前角色沒有編輯或新建功能，查看明細時字段設置全部只讀
    //只有明細表，將明細表也設置為不可能編輯，並且不顯示添加行、刪除行
    if (!saveBtn) {
      this.editFormOptions.forEach((row) => {
        row.forEach((x) => {
          x.disabled = true;
        });
      });
      //沒有新增編輯權限的，彈出框都設置為只讀
      this.detail.columns.forEach((column) => {
        if (column.hasOwnProperty('edit')) {
          column.readonly = true;
          // row['edit'] = false;
        }
      });
      //彈出框擴展按鈕
      this.extendBtn(boxButtons, this.extend.buttons.box);
      //彈出彈框按鈕(2020.04.21),沒有編輯或新建權限時，也可以通過buttons屬性添加自定義彈出框按鈕
      this.boxButtons.push(...boxButtons);
      this.detailOptions.buttons.push(detailGridButtons);
      this.detailOptions.buttons.forEach((button) => {
        if (!button.hasOwnProperty('hidden')) {
          button.hidden = false;
        }
      });
      //彈出框擴展明細表按鈕
      this.extendBtn(this.detailOptions.buttons, this.extend.buttons.detail);

      return boxButtons;
    }

    this.detailOptions.edit = true;
    boxButtons.push(
      ...[
        {
          name: '保 存',
          icon: 'el-icon-check',
          type: 'danger',
          disabled: false,
          value: 'save',
          onClick() {
            this.save();
          }
        }
        // {
        //   name: '重 置',
        //   icon: 'el-icon-refresh-right',
        //   type: 'primary',
        //   disabled: false,
        //   onClick() {
        //     this.resetEdit();
        //   }
        // }
      ]
    );
    //從表表格操作按鈕
    this.detailOptions.buttons.push(
      ...[
        {
          name: '添加行',
          icon: 'el-icon-plus',
          type: 'primary',
          hidden: false,
          plain: true,
          onClick() {
            this.addRow();
          }
        },
        {
          type: 'danger',
          plain: true,
          name: '刪除行',
          hidden: false,
          icon: 'el-icon-delete',
          onClick() {
            this.delRow();
          }
        },
        //2022.01.08增加明細表導入導出功能
        //注意需要重寫後台明細表接口的導入與4下載模板、導出的權限,Sys_DictionaryListController.cs/SellOrderListController.cs
        {
          type: 'danger',
          plain: true,
          name: '導入',
          value: 'import',
          hidden: false,
          icon: 'el-icon-upload2',
          onClick() {
            this.upload.url = `${this.http.ipAddress}api/${this.detail.table}/${this.const.IMPORT}?table=1`;
            this.upload.template.url = `${this.http.ipAddress}api/${this.detail.table}/${this.const.DOWNLOADTEMPLATE}`;
            //定義下載模板的文件名
            this.upload.template.fileName = this.detail.cnName;
            this.upload.excel = true;
          }
        },
        {
          type: 'danger',
          plain: true,
          name: '導出',
          value: 'export',
          icon: 'el-icon-download',
          hidden: false,
          onClick() {
            this.export(true);
          }
        }
      ]
    );
    this.detailOptions.buttons.forEach((button) => {
      if (button.hasOwnProperty('hidden')) {
        button.hidden = false;
      }
    });
    //彈出框擴展按鈕
    this.extendBtn(boxButtons, this.extend.buttons.box);

    //彈出框擴展明細表按鈕
    this.detailOptions.buttons.push(detailGridButtons);
    this.extendBtn(this.detailOptions.buttons, this.extend.buttons.detail);

    //彈出彈框按鈕
    this.boxButtons.push(...boxButtons);
  },
  onClick(click) {
    click.apply(this);
  },
  changeDropdown(btnName, v1) {
    let button = this.buttons.filter((x) => {
      return x.name == btnName;
    });
    if (button && button.length > 0) {
      button[0].onClick.apply(this);
    }
  },
  emptyValue(value) {
    if (typeof value == 'string' && value.trim() === '') {
      return true;
    }
    if (value instanceof Array && !value.length) {
      return true;
    }
    return value === null || value === undefined || value === '';
  },
  getSearchParameters() {
    //獲取查詢參數
    // 2020.09.11增加固定查詢表單,如果設置固定了查詢表單，點擊查詢時，不再關閉
    if (!this.fiexdSearchForm) {
      this.searchBoxShow = false;
    }

    let query = { wheres: [] };
    for (const key in this.searchFormFields) {
      let value = this.searchFormFields[key];
      if (this.emptyValue(value)) continue;

      if (typeof value == 'number') {
        value = value + '';
      }
      let displayType = this.getSearchItem(key);

      //聯級只保留選中節點的最後一個值
      if (displayType == 'cascader') {
        //查詢下面所有的子節點，如：選中的是父節點，應該查詢下面所有的節點數據--待完
        value = value.length ? value[value.length - 1] + '' : '';
      }
      //2021.05.02增加區間查詢
      if (
        typeof value == 'string' ||
        ['date', 'datetime', 'range'].indexOf(displayType) == -1
      ) {
        query.wheres.push({
          name: key,
          value:
            typeof value == 'string' ? (value + '').trim() : value.join(','),
          displayType: displayType
        });
        continue;
      }
      for (let index = 0; index < value.length; index++) {
        if (!this.emptyValue(value[index])) {
          query.wheres.push({
            name: key,
            value: (value[index] + '').trim(),
            displayType: (() => {
              if (['date', 'datetime', 'range'].indexOf(displayType) != -1) {
                return index ? 'lessorequal' : 'thanorequal';
              }
              return displayType;
            })()
          });
        }
      }
    }
    return query;
  },
  search() {
    //查詢
    // let query = this.getSearchParameters();
    // this.$refs.table.load(query, true);
    this.$refs.table.load(null, true);
  },
  loadTableBefore(param, callBack) {
    //查詢前設置查詢條件及分頁信息
    let query = this.getSearchParameters();
    if (query) {
      param = Object.assign(param, query);
    }

    if (this.$route.query.viewflow && this.$route.query.id) {
      param.wheres.push({
        name: this.table.key,
        value: this.$route.query.id
      });
    }
    // if (this.isViewFlow() && data && data.length) {
    //   let query = JSON.parse(JSON.stringify(this.$route.query));
    //   query.viewflow = 0;
    //   this.$router.replace({ path: this.$route.path, query: query });
    //   this.$nextTick(() => {
    //     this.getWorkFlowSteps(data[0]);
    //   });
    // }
    let status = this.searchBefore(param);
    callBack(status);
  },

  loadTableAfter(data, callBack, result) {
    //查詢後
    //2020.10.30增加查詢後返回所有的查詢信息
    let status = this.searchAfter(data, result);
    callBack(status);
    //自動彈出框審批詳情
  },
  loadDetailTableBefore(param, callBack,data) {
    //明細查詢前
    //新建時禁止加載明細
    if (this.currentAction == this.const.ADD) {
      callBack(false);
      return false;
    }
    let status = this.searchDetailBefore(param,data);
    callBack(status);
  },
  loadDetailTableAfter(data, callBack,result) {
    //明細查詢後
    let status = this.searchDetailAfter(data,result);
    callBack(status);
  },
  getSearchItem(field) {
    //獲取查詢的參數
    let data;
    for (let index = 0; index < this.searchFormOptions.length; index++) {
      if (data) return data.type;
      const item = this.searchFormOptions[index];
      data = item.find((x) => {
        return x.field == field;
      });
    }

    return (data || {}).type;
  },
  resetSearch() {
    //重置查詢對象
    this.resetSearchForm();
    //2020.10.17增加重置後方法
    this.resetSearchFormAfter && this.resetSearchFormAfter();
  },
  resetEdit() {
    //重置編輯的數據
    let isEdit = this.currentAction != this.const.ADD;
    //重置之前
    if (!this[isEdit ? 'resetUpdateFormBefore' : 'resetAddFormBefore']()) {
      return;
    }
    let objKey = {};
    //編輯狀態下,不需要重置主鍵,創建時間創建人
    if (isEdit) {
      objKey[this.table.key] = this.editFormFields[this.table.key];
    }
    this.resetEditForm(objKey);
    //重置之後

    if (!this[isEdit ? 'resetUpdateFormAfter' : 'resetAddFormAfter']()) {
      return;
    }
  },
  resetSearchForm(sourceObj) {
    //重置查詢表
    this.resetForm('searchForm', sourceObj);
  },
  resetEditForm(sourceObj) {
    if (this.hasDetail && this.$refs.detail) {
      // this.$refs.detail.rowData.splice(0);
      this.$refs.detail.reset();
    }
    this.resetForm('form', sourceObj);
    if (this.$refs.form && this.$refs.form.$refs.volform) {
      setTimeout(() => {
        this.$refs.form.$refs.volform.clearValidate();
      }, 100);
    }
  },
  getKeyValueType(formData, isEditForm) {
    try {
      let keyLeft = (isEditForm ? 'e' : 's') + '_b_';
      formData.forEach((item) => {
        item.forEach((x) => {
          if (this.keyValueType.hasOwnProperty(keyLeft + x.field)) {
            return true;
          }
          let data;
          if (x.type == 'switch') {
            this.keyValueType[x.field] = 1;
          } else if (x.bind && x.bind.data) {
            data = x.bind.data;
          } else if (x.data) {
            if (x.data instanceof Array) {
              data = x.data;
            } else if (x.data.data && x.data.data instanceof Array) {
              data = x.data.data;
            }
          }
          if (
            data &&
            data.length > 0 &&
            !this.keyValueType.hasOwnProperty(x.field)
          ) {
            this.keyValueType[x.field] = data[0].key;
            this.keyValueType[keyLeft + x.field] = x.type;
          }
        });
      });
    } catch (error) {
      console.log(error.message);
    }
  },
  resetForm(formName, sourceObj) {
    //   return;
    //重置表單數據
    if (this.$refs[formName]) {
      this.$refs[formName].reset();
    }

    if (!sourceObj) return;
    let form, keyLeft;
    if (formName == 'searchForm') {
      form = this.searchFormFields;
      keyLeft = 's' + '_b_';
    } else {
      form = this.editFormFields;
      keyLeft = 'e' + '_b_';
    }
    //獲取數據源的data類型，否則如果數據源data的key是數字，重置的值是字符串就無法綁定值
    if (!this.keyValueType._dinit) {
      this.getKeyValueType(this.editFormOptions, true);
      this.getKeyValueType(this.searchFormOptions, false);
      this.keyValueType._dinit = true;
    }
    var _cascaderParentTree;
    for (const key in form) {
      if (sourceObj.hasOwnProperty(key)) {
        let newVal = sourceObj[key];
        let kv_type = this.keyValueType[keyLeft + key];

        if (
          kv_type == 'selectList' ||
          kv_type == 'checkbox' ||
          kv_type == 'cascader' ||
          kv_type == 'treeSelect'
        ) {
          // 2020.05.31增加iview組件Cascader
          // 2020.11.01增加iview組件Cascader表單重置時查詢所有的父節點
          if (kv_type == 'cascader' || kv_type == 'treeSelect') {
            var treeDic = this.dicKeys.find((dic) => {
              return dic.fileds && dic.fileds.indexOf(key) != -1;
            });

            if (treeDic && treeDic.orginData && treeDic.orginData.length) {
              let keyIsNum = typeof treeDic.orginData[0].id == 'number';

              if (kv_type == 'cascader') {
                newVal = keyIsNum ? newVal * 1 || 0 : newVal + '';
                if (kv_type == 'cascader') {
                  _cascaderParentTree = this.base.getTreeAllParent(
                    newVal,
                    treeDic.orginData
                  );
                  if (_cascaderParentTree) {
                    newVal = _cascaderParentTree.map((x) => {
                      return x.id;
                    });
                  }
                }
              } else {
                if (newVal === null || newVal === undefined) {
                  newVal = [];
                } else if (typeof newVal == 'string') {
                  newVal = newVal.split(',');
                }
                if (keyIsNum) {
                  if (Array.isArray(newVal)) {
                    newVal = newVal.map((x) => {
                      return x * 1 || 0;
                    });
                  }
                } else if (typeof newVal == 'number') {
                  newVal = [newVal + ''];
                }
              }
            } else {
              newVal = [newVal];
            }
          } else if (
            newVal != '' &&
            newVal != undefined &&
            typeof newVal == 'string'
          ) {
            newVal = newVal.split(',');
          } else if (kv_type == 'checkbox') {
            newVal = [];
          }
        } else if (
          this.keyValueType.hasOwnProperty(key) &&
          typeof this.keyValueType[key] == 'number' &&
          newVal * 1 == newVal
        ) {
          newVal = newVal * 1;
        } else {
          if (newVal == null || newVal == undefined) {
            newVal = '';
          } else if (this.numberFields.indexOf(key) != -1) {
            newVal = newVal * 1 || 0;
          } else {
            newVal += '';
          }
        }
        if (newVal instanceof Array) {
          if (form[key]) {
            form[key] = [];
          }
          form[key] = newVal;
        } else {
          form[key] = newVal;
        }
      } else {
        form[key] = form[key] instanceof Array ? [] : '';
      }
    }
  },
  onBtnClick(param) {
    this[param.method](param.data);
  },
  refresh() {
    //刷新
    this.search();
    // this.$refs.table.load();
  },
  saveBefore(formData) {
    return true;
  },
  saveAfter(formData, result) {
    return true;
  },
  save() {
    //新增或編輯時保存
    // if (!this.$refs.form.validate()) return;
    this.$refs.form.validate((result) => {
      if (result) {
        this.saveExecute();
      }
    });
  },
  async saveExecute() {
    let editFormFields = {};
    //上傳文件以逗號隔開
    for (const key in this.editFormFields) {
      if (
        this.uploadfiled &&
        this.uploadfiled.length > 0 &&
        this.uploadfiled.indexOf(key) != -1 &&
        this.editFormFields[key] instanceof Array
      ) {
        let allPath = this.editFormFields[key].map((x) => {
          return x.path;
        });
        editFormFields[key] = allPath.join(',');
      } else if (typeof this.editFormFields[key] == 'function') {
        try {
          editFormFields[key] = this.editFormFields[key]();
        } catch (error) { }
      } else {
        //2021.05.30修復下拉框清除數據後後台不能保存的問題
        if (
          this.editFormFields[key] === undefined &&
          this.dicKeys.some((x) => {
            return x.fileds && x.fileds.indexOf(key) != -1;
          })
        ) {
          editFormFields[key] = null;
        } else {
          editFormFields[key] = this.editFormFields[key];
        }
      }
    }
    //將數組轉換成string
    //2020.11.01增加級聯處理
    for (const key in editFormFields) {
      if (editFormFields[key] instanceof Array) {
        var iscascader = this.dicKeys.some((x) => {
          return (
            x.type == 'cascader'&&x.e_type!="treeSelect" && x.fileds && x.fileds.indexOf(key) != -1
          );
        });
        if (iscascader && editFormFields[key].length) {
          editFormFields[key] =
            editFormFields[key][editFormFields[key].length - 1];
        } else {
          editFormFields[key] = editFormFields[key].join(',');
        }
      }
    }

    let formData = {
      mainData: editFormFields,
      detailData: null,
      delKeys: null
    };

    //獲取明細數據(前台數據明細未做校驗，待完.後台已經校驗)
    if (this.hasDetail) {
      formData.detailData = this.$refs.detail.rowData;
      let _fields = this.detail.columns
        .filter((c) => {
          return (
            c.type == 'selectList' || (c.edit && c.edit.type == 'selectList')
          );
        })
        .map((c) => {
          return c.field;
        });
      //2022.06.20增加保存時對明細表下拉框多選的判斷
      if (_fields.length) {
        formData.detailData = JSON.parse(JSON.stringify(formData.detailData));
        formData.detailData.forEach((row) => {
          for (let index = 0; index < _fields.length; index++) {
            const _field = _fields[index];
            if (Array.isArray(row[_field])) {
              row[_field] = row[_field].join(',');
            }
          }
        });
      }
    }
    if (this.detailOptions.delKeys.length > 0) {
      formData.delKeys = this.detailOptions.delKeys;
    }
    //保存前攔截
    let _currentIsAdd = this.currentAction == this.const.ADD;
    if (_currentIsAdd) {
      //2020.12.06增加新建前異步處理方法
      //2021.08.16修復異步語法寫錯的問題
      if (!this.addBefore(formData) || !(await this.addBeforeAsync(formData)))
        return;
    } else {
      //2020.12.06增加修改前異步處理方法
      if (
        !this.updateBefore(formData) ||
        !(await this.updateBeforeAsync(formData))
      )
        return;
    }
    let url = this.getUrl(this.currentAction);
    this.http.post(url, formData, true).then((x) => {
      //保存後
      if (_currentIsAdd) {
        if (!this.addAfter(x)) return;
        //連續添加
        if (this.continueAdd && x.status) {
          this.$success(x.message);
          //新建
          this.currentAction = this.const.ADD;
          //2023.07.23增加連續添加後方法
          let _formFields;
          if (this.continueAddAfter) {
            _formFields=JSON.parse(JSON.stringify(this.editFormFields))
          }
          this.currentRow = {};
          this.resetAdd();
          this.refresh();
          //2023.07.23增加連續添加後方法
          this.continueAddAfter&&this.continueAddAfter(_formFields,formData,x);
          return;
        }
      } else {
        if (!this.updateAfter(x)) return;
      }
      if (!x.status) return this.$error(x.message);
      this.$success(x.message || '操作成功');
      //如果保存成功後需要關閉編輯框，直接返回不處理後面
      if (this.boxOptions.saveClose) {
        this.boxModel = false;
        //2020.12.27如果是編輯保存後不重置分頁頁數，刷新頁面時還是顯示當前頁的數據
        this.$refs.table.load(null, _currentIsAdd);
        //this.refresh();
        return;
      }
      let resultRow;
      if (typeof x.data == 'string' && x.data != '') {
        resultRow = JSON.parse(x.data);
      } else {
        resultRow = x.data;
      }

      if (this.currentAction == this.const.ADD) {
        //  this.currentRow=x.data;
        this.editFormFields[this.table.key] = '';
        this.currentAction = this.const.EDIT;
        this.currentRow = resultRow.data;
      }
      this.resetEditForm(resultRow.data);
      // console.log(resultRow);
      if (this.hasDetail) {
        this.detailOptions.delKeys = [];
        if (resultRow.list) {
          this.$refs.detail.rowData.push(...resultRow.list);
        }
      }
      this.$refs.table.load(null, _currentIsAdd);
      // this.refresh();
    });
  },
  del(rows) {
    if (rows) {
      if (!(rows instanceof Array)) {
        rows = [rows];
      }
    } else {
      rows = this.$refs.table.getSelected();
    }
    //刪除數據

    if (!rows || rows.length == 0) return this.$error('請選擇要刪除的行!');
    let delKeys = rows.map((x) => {
      return x[this.table.key];
    });
    if (!delKeys || delKeys.length == 0)
      return this.$error('沒有獲取要刪除的行數據!');
    //刪除前
    if (!this.delBefore(delKeys, rows)) {
      return;
    }
    let tigger = false;
    this.$confirm('確認要刪除選擇的數據嗎?', '警告', {
      confirmButtonText: '確定',
      cancelButtonText: '取消',
      type: 'warning',
      center: true
    }).then(() => {
      if (tigger) return;
      tigger = true;
      let url = this.getUrl(this.const.DEL);
      this.http.post(url, delKeys, '正在刪除數據....').then((x) => {
        if (!x.status) return this.$error(x.message);
        this.$success(x.message);
        //刪除後
        if (!this.delAfter(x)) {
          return;
        }
        this.refresh();
      });
    });
  },
  async modelOpenBeforeAsync(row) {
    return true;
  },
  async initBox() {
    //2022.01.08增加新建時隱藏明細表導出功能
    this.detailOptions.buttons.forEach((x) => {
      if (x.value == 'export') {
        x.hidden = this.currentAction == 'Add';
      }
    });
    //初始化新建、編輯的彈出框
    if (!(await this.modelOpenBeforeAsync(this.currentRow))) return false;
    this.modelOpenBefore(this.currentRow);
    if (!this.boxInit) {
      this.boxInit = true;
      this.boxModel = true;
      // this.detailUrl = this.url;
    }
    return true;
  },
  setEditForm(row) {
    // if (this.remoteColumns.length == 0 || !rows || rows.length == 0) return;
    let remoteColumns = this.$refs.table.remoteColumns;
    remoteColumns.forEach((column) => {
      this.editFormOptions.forEach((option) => {
        option.forEach((x) => {
          if (x.field == column.field) {
            x.data.data = Object.assign([], x.data, column.bind.data);
          }
        });
      });
    });
    this.editFormFields;
    //重置編輯表單數據
    this.editFormFields[this.table.key] = row[this.table.key];

    this.resetEditForm(row);
    this.currentAction = this.const.EDIT;
    this.boxModel = true;
  },
  async linkData(row, column) {
    this.boxOptions.title = this.table.cnName + '(編輯)';
    //點擊table單元格快捷連接顯示編輯數據
    this.currentAction = this.const.EDIT;
    this.currentRow = row;
    if (!(await this.initBox())) return;
    this.resetDetailTable(row);
    this.setEditForm(row);
    this.setContinueAdd(false);
    //設置遠程查詢表單的默認key/value
    this.getRemoteFormDefaultKeyValue();
    //點擊編輯按鈕彈出框後，可以在此處寫逻輯，如，從後台獲取數據
    this.modelOpenProcess(row);
  },
  setContinueAdd(isAdd) {
    if (!this.continueAdd) return;
    var _button = this.boxButtons.find((x) => {
      return x.value == 'save';
    });
    if (_button) {
      _button.name = isAdd ? this.continueAddName : '保 存';
    }
  },
  resetAdd() {
    if (this.hasDetail) {
      this.$refs.detail &&
        //  this.$refs.detail.rowData &&
        this.$refs.detail.reset();
    }
    let obj = {};
    //如果有switch標籤，默認都設置為否
    this.editFormOptions.forEach((x) => {
      x.forEach((item) => {
        if (item.type == 'switch') {
          obj[item.field] = 0;
        }
      });
    });
    this.resetEditForm(obj);
  },
  async add() {
    this.boxOptions.title = this.table.cnName + '(新建)';
    //新建
    this.currentAction = this.const.ADD;
    this.currentRow = {};
    if (!(await this.initBox())) return;

    this.resetAdd();
    this.setContinueAdd(true);
    //  this.resetEditForm();
    this.boxModel = true;
    //點擊新建按鈕彈出框後，可以在此處寫逻輯，如，從後台獲取數據
    this.modelOpenProcess();
    // this.modelOpenAfter();
  },
  async edit(rows) {
    this.boxOptions.title = '編輯';
    //編輯
    this.currentAction = this.const.EDIT;
    if (rows) {
      if (!(rows instanceof Array)) {
        rows = [rows];
      }
    } else {
      rows = this.$refs.table.getSelected();
    }
    if (rows.length == 0) {
      return this.$error('請選擇要編輯的行!');
    }
    if (rows.length != 1) {
      return this.$error('只能選擇一行數據進行編輯!');
    }
    //記錄當前編輯的行
    this.currentRow = rows[0];
    //初始化彈出框
    if (!(await this.initBox())) return;
    this.setContinueAdd(false);
    //重置表單
    this.resetDetailTable();

    //設置當前的數據到表單上
    this.setEditForm(rows[0]);
    //設置遠程查詢表單的默認key/value
    this.getRemoteFormDefaultKeyValue();
    //點擊編輯按鈕彈出框後，可以在此處寫逻輯，如，從後台獲取數據
    this.modelOpenProcess(rows[0]);
    // this.modelOpenAfter(rows[0]);
  },
  getRemoteFormDefaultKeyValue() {
    //設置表單遠程數據源的默認key.value
    if (this.currentAction != this.const.EDIT || this.remoteKeys.length == 0)
      return;
    this.editFormOptions.forEach((x, xIndex) => {
      x.forEach((item, yIndex) => {
        if (item.remote) {
          let column = this.columns.find((x) => {
            return x.bind && x.bind.key == item.dataKey;
          });
          if (!column) return;
          let key = this.currentRow[item.field];
          let obj = column.bind.data.find((x) => {
            return x.key == key;
          });
          // obj ? obj.value : key如果沒有查到數據源，直接使用原數據
          item.data = [{ key: key, value: obj ? obj.value : key }];
          this.editFormOptions[xIndex].splice(yIndex, 1, item);
          // this.$set(item, 'data', [{ key: key + '', value: obj.value }])
          //  item.data = [{ key: key + '', value: obj.value }];
        }
      });
    });
  },
  modelOpenProcess(row) {
    this.$nextTick(() => {
      this.modelOpenAfter(row);
    });
    return;
    // if (!this.$refs.form) {
    //     let timeOut = setTimeout(x => {
    //         this.modelOpenAfter(row);
    //     }, 500)
    //     return;
    // }
    // this.modelOpenAfter(row);
  },
  import() {
    //導入(上傳excel),彈出導入組件UploadExcel.vue
    this.upload.excel = true;
    this.$refs.upload_excel && this.$refs.upload_excel.reset();
  },
  download(url, fileName) {
    //下載導出的文件
    let xmlResquest = new XMLHttpRequest();
    xmlResquest.open('GET', url, true);
    xmlResquest.setRequestHeader('Content-type', 'application/json');
    xmlResquest.setRequestHeader(
      'Authorization',
      this.$store.getters.getToken()
    );
    let elink = this.$refs.export;
    xmlResquest.responseType = 'blob';
    xmlResquest.onload = function (oEvent) {
      if (xmlResquest.status != 200) {
        this.$error('下載文件出錯了..');
        return;
      }
      let content = xmlResquest.response;
      //  let elink = this.$refs.export;//document.createElement("a");
      elink.download = fileName; //+".xlsx";
      // elink.style.display = "none";
      let blob = new Blob([content]);
      elink.href = URL.createObjectURL(blob);
      //  document.body.appendChild(elink);
      elink.click();
      //  document.body.removeChild(elink);
    };
    xmlResquest.send();
  },
  getFileName(isDetail) {
    //2021.01.08增加導出excel時自定義文件名
    if (isDetail) {
      return this.detail.cnName + '.xlsx';
    }
    return this.table.cnName + '.xlsx';
  },
  export(isDetail) {
    //導出
    let url, query, param;
    if (isDetail) {
      //明細表導出時如果是新建狀態，禁止導出
      if (this.currentAction == 'Add') {
        return;
      }
      url = `api/${this.detail.table}/${this.const.EXPORT}`;
      param = {
        wheres: [
          { name: this.table.key, value: this.editFormFields[this.table.key] }
        ]
      };
    } else {
      //主表導出
      url = this.getUrl(this.const.EXPORT);
      query = this.getSearchParameters();
      param = { order: this.pagination.order, wheres: query.wheres || [] };
    }
    //2020.06.25增加導出前處理
    if (!isDetail && !this.exportBefore(param)) {
      return;
    }

    if (param.wheres && typeof param.wheres == 'object') {
      param.wheres = JSON.stringify(param.wheres);
    }
    let $http = this.http;
    //2022.09.26增加自定義導出文件名
    let fileName = this.downloadFileName || this.getFileName(isDetail);
    //2021.01.08優化導出功能
    $http
      .post(url, param, '正在導出數據....', { responseType: 'blob' })
      .then((content) => {
        const blob = new Blob([content]);
        if ('download' in document.createElement('a')) {
          // 非IE下載
          const elink = document.createElement('a');
          elink.download = fileName;
          elink.style.display = 'none';
          elink.href = URL.createObjectURL(blob);
          document.body.appendChild(elink);
          elink.click();
          URL.revokeObjectURL(elink.href);
          document.body.removeChild(elink);
        } else {
          // IE10+下載
          navigator.msSaveBlob(blob, fileName);
        }
      });
    //.then(result => {
    // if (!result.status) {
    //   return this.$error(result.message);
    // }
    // let path = this.getUrl(this.const.DOWNLOAD);
    // path = path[0] == "/" ? path.substring(1) : path;
    // this.download(
    //   $http.ipAddress + path + "?path=" + result.data,
    //   this.table.cnName + ".xlsx" // filePath
    // );
    ///  window.open($http.ipAddress + path + "?fileName=" + filePath, "_self");
    // });
  },
  getSelectRows() {
    //獲取選中的行
    return this.$refs.table.getSelected();
  },
  getDetailSelectRows() {
    //或獲取明細選中的行
    if (!this.$refs.detail) {
      return [];
    }
    return this.$refs.detail.getSelected();
  },
  audit() {
    //審核彈出框
    let rows = this.$refs.table.getSelected();
    if (rows.length == 0) return this.$error('請選擇要審核的行!');
    let auditStatus = Object.keys(rows[0]).find(x => { return x.toLowerCase() === 'auditstatus' });
    if (!auditStatus) {
      return this.$message.error(`表必須包括審核字段【AuditStatus】,並且是int類型`)
    }
    // let checkStatus = rows.every((x) => {
    //   return this.$global.audit.status.some(c => { return c === x[auditStatus] || !x[auditStatus] })
    // });
    // if (!checkStatus) return this.$error('只能選擇待審批或審核中的數據!');
    this.$refs.audit.open(rows);
  },
  saveAudit(params, rows, callback) {

    //保存審核
    let keys = rows.map(x => { return x[this.table.key] });
    if (!this.auditBefore(keys, rows)) {
      return;
    }
    let url = `${this.getUrl(this.const.AUDIT)}?auditReason=${params.reason}&auditStatus=${params.value}`
    this.http.post(url, keys, '審核中....').then((x) => {
      if (!this.auditAfter(x, keys)) {
        return;
      }
      if (!x.status) return this.$error(x.message);

      callback && callback(x);
      this.$success(x.message);
      this.refresh();
    });
  },
  viewModelCancel() {
    //查看表結構
    this.viewModel = false;
  },
  initFormOptions(formOptions, keys, formFields, isEdit) {
    //初始化查詢、編輯對象的下拉框數據源、圖片上傳連接地址
    //let defaultOption = { key: "", value: "請選擇" };
    //有上傳的字段
    //2020.05.03新增
    //編輯數據源的類型
    formOptions.forEach((item) => {
      item.forEach((d) => {
        if (d.type == 'number') {
          //2022.08.22優化表單類型為number時的默認值
          if (formFields[d.field] === '') {
            formFields[d.field] = undefined;
          }
          this.numberFields.push(d.field);
        }
        if (
          d.type == 'img' ||
          d.type == 'excel' ||
          d.type == 'file' ||
          d.columnType == 'img'
        ) {
          d.url = this.http.ipAddress + 'api' + this.table.url + 'Upload';
          this.uploadfiled.push(d.field);
        }
        if (!d.dataKey) return true;
        //2022.02.20强制開啟聯級可以選擇某個節點
        if (d.type == 'cascader' && !d.hasOwnProperty('changeOnSelect')) {
          //强制開啟聯級可以選擇某個節點
          d.changeOnSelect = true;
        }
        //開啟遠程搜索
        if (d.remote) {
          this.remoteKeys.push(d.dataKey);
          d.data = []; //{ dicNo: d.dataKey, data: [] };
          return true;
        }
        //2020.05.03增加編輯表單對checkbox的支持
        if (d.type == 'checkbox' && !(formFields[d.field] instanceof Array)) {
          formFields[d.field] = [];
        }
        if (keys.indexOf(d.dataKey) == -1) {
          //2020.05.03增加記錄編輯字段的數據源類型

          keys.push(d.dataKey);
          //2020.05.03修復查詢表單與4編輯表單type類型變成强一致性的問題
          //this.dicKeys.push({ dicNo: d.dataKey, data: [], type: d.type });
          //  2020.11.01增加iview組件Cascader數據源存储
          let _dic = {
            dicNo: d.dataKey,
            data: [],
            fileds: [d.field],
            orginData: []
          };
          if (d.type == 'cascader') {
            _dic.type = 'cascader';
          }
          if (isEdit) {
            _dic['e_type'] = d.type;
          }
          this.dicKeys.push(_dic);
        } else if (d.type == 'cascader') {
          this.dicKeys.forEach((x) => {
            if (x.dicNo == d.dataKey) {
              x.type = 'cascader';
              x.fileds.push(d.field);
            }
          });
        }
        if (d.type != 'cascader') {
          //2020.01.30移除内部表單formOptions數據源配置格式data.data，所有參數改為與4組件api格式相同
          Object.assign(
            d,
            this.dicKeys.filter((f) => {
              return f.dicNo == d.dataKey;
            })[0],
            { type: d.type }
          );
        }
      });
    });
  },
  //初始table與4明細表的數據源指向dicKeys對象，再去後台加載數據源
  initColumns(scoure, dicKeys, keys) {
    if (!scoure || !(scoure instanceof Array)) return;
    scoure.forEach((item) => {
      if (!item.bind || (item.bind.data && item.bind.data.length > 0))
        return true;
      let key = item.bind.key || item.bind.dicNo;
      if (this.remoteKeys.indexOf(key) != -1) {
        item.bind.remote = true;
        return true;
      }
      if (this.hasKeyField.indexOf(item.field) == -1) {
        this.hasKeyField.push(item.field);
      }
      var dic = dicKeys.filter((x) => {
        return x.dicNo == key;
      });
      if (!dic || dic.length == 0) {
        dicKeys.push({ dicNo: key, data: [] });
        dic = [dicKeys[dicKeys.length - 1]];
        keys.push(key);
      }
      //2020.11.01增加級聯處理
      if (dic[0].type == 'cascader' || dic[0].type == 'treeSelect') {
        item.bind = { data: dic[0].orginData, type: 'select', key: key };
      } else {
        item.bind = dic[0];
      }
      //2020.05.03優化table數據源checkbox與4select類型從編輯列中選取
      item.bind.type = item.bind.e_type || 'string';
    });
  },
  bindOptions(dic) {
    //綁定下拉框的數據源
    //綁定後台的字典數據
    dic.forEach((d) => {
      if (d.data.length >= (this.select2Count || 500)) {
        if (
          !this.dicKeys.some((x) => {
            return (
              x.dicNo == d.dicNo &&
              (x.type == 'cascader' || x.type == 'treeSelect')
            );
          })
        ) {
          d.data.forEach((item) => {
            item.label = item.value;
            item.value = item.key;
          });
        }
      }
      this.dicKeys.forEach((x) => {
        if (x.dicNo != d.dicNo) return true;
        //2020.10.26增加級聯數據源綁定處理
        if (x.type == 'cascader' || x.type == 'treeSelect') {
          // x.data=d.data;
          //生成tree結構
          let _data = JSON.parse(JSON.stringify(d.data));
          //2022.04.04增加級聯字典數據源刷新後table沒有變化的問題
          this.columns.forEach((column) => {
            if (column.bind && column.bind.key == d.dicNo) {
              column.bind.data = d.data;
            }
          });
          let arr = this.base.convertTree(_data, (node, data, isRoot) => {
            if (!node.inited) {
              node.inited = true;
              node.label = node.value;
              node.value = node.key;
            }
          });
          x.data.push(...arr);
          x.orginData.push(...d.data);
          //2021.10.17修復查詢級聯不能綁定數據源的問題
          this.searchFormOptions.forEach((searhcOption) => {
            searhcOption.forEach((_option) => {
              if (_option.type == 'cascader' && _option.dataKey == x.dicNo) {
                _option.data = arr;
                _option.orginData = d.data;
              }
            });
          });
          //2021.10.17修復級聯不能二級刷新的問題
          this.editFormOptions.forEach((editOption) => {
            editOption.forEach((_option) => {
              if (
                (_option.type == 'cascader' || _option.type == 'treeSelect') &&
                _option.dataKey == x.dicNo
              ) {
                _option.data = arr;
                _option.orginData = d.data;
              }
            });
          });
        } else if (d.data.length > 0 && !d.data[0].hasOwnProperty('key')) {
          let source = d.data,
            newSource = new Array(source.length);
          for (let index = 0; index < source.length; index++) {
            newSource[index] = {
              //默認從字典數據讀出來的key都是string類型,但如果數據從sql中查詢的可能為非string,否是async-validator需要重置設置格式
              key: source['key'] + '', //source[index][x.config.valueField] + "",
              value: source['value'] //source[index][x.config.textField]
            };
          }

          x.data.push(...newSource);
        } else {
          //2020.06.06，如果是selectList數據源使用的自定義sql並且key是數字，强制轉換成字符串
          if (
            x.e_type == 'selectList' &&
            d.data.length > 0 &&
            typeof d.data[0].key == 'number'
          ) {
            d.data.forEach((c) => {
              c.key = c.key + '';
            });
          }
          x.data.push(...d.data);
        }
        if (
          this.singleSearch &&
          this.singleSearch.dataKey &&
          this.singleSearch.dataKey == x.dicNo
        ) {
          this.singleSearch.data.splice(0, 1, ...x.data);
        }
      });
    });
  },
  getUrl(action, ingorPrefix) {
    //是否忽略前缀/  獲取操作的url
    return (!ingorPrefix ? '/' : '') + 'api' + this.table.url + action;
  },
  initDicKeys() {
    //初始化字典數據
    let keys = [];
    //2022.04.17優化重新加載數據源
    this.dicKeys.forEach((item) => {
      item.data.splice(0);
      item.orginData && item.orginData.splice(0);
    });
    //this.dicKeys.splice(0);
    //初始化編輯數據源,默認為一個空數組，如果要求必填設置type=number/decimal的最小值
    this.initFormOptions(this.editFormOptions, keys, this.editFormFields, true);
    //初始化查詢數據源,默認為一個空數組
    this.initFormOptions(
      this.searchFormOptions,
      keys,
      this.searchFormFields,
      false
    );
    //查詢日期設置為可選開始與4結果日期
    this.searchFormOptions.forEach((item) => {
      item.forEach((x) => {
        if (x.type == 'date' || x.type == 'datetime') x.range = true;
      });
    });
    //初始化datatable表數據源,默認為一個空數組,dicKeys為介面所有的數據字典編號
    this.initColumns(this.columns, this.dicKeys, keys);
    //2021.05.23默認開啟查詢頁面所有字段排序,如果不需要排序，在onInited遍歷columns設置sort=false
    //2021.09.25移除强制排序功能
    // this.columns.forEach(x => {
    //   x.sort = x.render ? false : true;
    // })
    if (this.detailOptions && this.detailOptions.columns) {
      this.initColumns(this.detailOptions.columns, this.dicKeys, keys);
    }
    //初始化快速查詢字段,默認使用代碼生成器配置的第一個查詢字段
    if (this.searchFormOptions.length > 0) {
      this.singleSearch = {
        dataKey: this.searchFormOptions[0][0].dataKey,
        dicNo: this.searchFormOptions[0][0].dicNo,
        field: this.searchFormOptions[0][0].field,
        title: this.searchFormOptions[0][0].title,
        type: this.searchFormOptions[0][0].type,
        data: []
      };
      // this.singleSearch = this.searchFormOptions[0][0];
    }
    if (keys.length == 0) return;
    let $this = this;
    this.http.post('/api/Sys_Dictionary/GetVueDictionary', keys).then((dic) => {
      $this.bindOptions(dic);
      //2022.04.04增加字典加載完成方法
      $this.dicInited && $this.dicInited(dic);
    });
  },
  setFiexdColumn(columns, containerWidth) {
    //計算整個table的寬度，根據寬度决定是否啟用第一行顯示的列為固定列
    //2021.09.21移除强制固定第一列
    // let columnsWidth = 0;
    // columns.forEach(x => {
    //   if (!x.hidden && x.width) {
    //     columnsWidth += x.width;
    //   }
    // });
    // //啟用第一列為固定列
    // if (columnsWidth > containerWidth) {
    //   let firstColumn = columns.find(x => !x.hidden);
    //   if (firstColumn) {
    //     firstColumn.fixed = true;
    //   }
    // }
  },
  initBoxHeightWidth() {
    //初始化彈出框的高度與4寬度
    let clientHeight = document.documentElement.clientHeight;
    //彈出框高度至少250px
    clientHeight = clientHeight < 250 ? 250 : clientHeight;
    let clientWidth = document.documentElement.clientWidth;
    if (
      this.editFormOptions.some((x) => {
        return x.some((item) => {
          return item.type == 'editor';
        });
      })
    ) {
      this.editor.uploadImgUrl = this.getUrl('upload');
      this.boxOptions.height = clientHeight * 0.8;
      this.boxOptions.width = clientWidth * 0.8;
    } else {
      if (this.boxOptions.height) {
        //如果高度與4寬度超過了獲取到的可見高寬度，則設為默認的90%高寬
        if (this.boxOptions.height > clientHeight * 0.8) {
          this.boxOptions.height = clientHeight * 0.8;
        }
      }
      if (this.boxOptions.width) {
        //如果高度與4寬度超過了獲取到的可見高寬度，則設為默認的90%高寬
        if (this.boxOptions.width > clientWidth * 0.8) {
          this.boxOptions.width = clientWidth * 0.8;
        }
      }
    }
    //計算整個table的寬度，根據寬度决定是否啟用第一行顯示的列為固定列
    let maxTableWidth = clientWidth - 270;
    this.setFiexdColumn(this.columns, maxTableWidth);

    this.height = this.tableHeight || clientHeight - 206;
    this.url = this.getUrl(this.const.PAGE);
    //計算彈出框的高與4寬度
    //如果有明細表，高度與4寬帶設置為0.9/0.82
    if (this.detail.columns && this.detail.columns.length > 0) {
      this.hasDetail = true;
      clientWidth = clientWidth * 0.8;
      clientHeight = clientHeight * 0.85;
      if (!this.detailOptions.height) {
        this.detailOptions.height =
          clientHeight - this.editFormOptions.length * 36 - 234;
        this.detailOptions.height =
          this.detailOptions.height < 240 ? 240 : this.detailOptions.height;
      }

      this.detailOptions.columns = this.detail.columns;
      this.detailOptions.pagination.sortName = this.detail.sortName;
      this.detailOptions.cnName = this.detail.cnName;
      this.detailOptions.key = this.detail.key;
      this.detailOptions.url = this.getUrl('getDetailPage');
      //計算彈出框整個table的寬度，根據寬度决定是否啟用第一行顯示的列為固定列
      this.setFiexdColumn(this.detail.columns, clientWidth);
    } else {
      let maxColumns = 1; //最大列數，根據列計算彈框的寬度
      this.editFormOptions.forEach((x) => {
        if (x.length > maxColumns) maxColumns = x.length;
      });
      let maxHeightRate = 0.7,
        maxWidthRate = 0.5;
      maxWidthRate = maxColumns / 10 + 0.3;
      maxHeightRate = (this.editFormOptions.length || 1) * 0.1 + 0.03;
      maxHeightRate = maxHeightRate > 0.9 ? 0.9 : maxHeightRate;
      clientWidth = clientWidth * maxWidthRate;
      clientHeight = clientHeight * maxHeightRate;
      // this.boxOptions.width = clientWidth * maxWidthRate;
      // this.boxOptions.height = clientHeight * maxHeightRate;
    }
    if (!this.boxOptions.height) {
      this.boxOptions.height = clientHeight + 10;
    }
    if (!this.boxOptions.width) {
      this.boxOptions.width = clientWidth + 30;
    }
  },
  rowOnChange(row) {
    this.rowChange(row);
  },
  rowChange(row) {
    //選中行checkbox行事件
  },
  rowOnClick({ row, column, event }) {
    this.rowClick({ row, column, event });
  },
  rowClick({ row, column, event }) {
    // 點擊行事件(2020.11.07)
  },
  rowOnDbClick({ row, column, event }) {
    this.rowDbClick({ row, column, event });
  },
  rowDbClick({ row, column, event }) {
    // 雙擊擊行事件(2021.05.23)
  },
  $error(message) {
    this.$message.error(message);
    // this.$message({
    //   type: 'error',
    //   content: message,
    //   duration: 5
    // });
  },
  $success(message) {
    this.$message.success(message);
  },
  setFiexdSearchForm(visiable) {
    //2020.09.011增加固定查詢表單功能,visiable=true默認將查詢表單展開
    this.fiexdSearchForm = true;
    let refreshBtn = this.buttons.find((x) => x.name == '刷 新');
    if (visiable) {
      this.searchBoxShow = true;
    }
    if (refreshBtn) {
      refreshBtn.name = '重 置';
      refreshBtn.onClick = function () {
        this.resetSearch();
      };
    }
  },
  tableBeginEdit(row, column, index) {
    //2021.03.19是否開啟查詢介面表格雙擊編輯結束方法,返回false不會結束編輯
    return this.beginEdit(row, column, index);
  },
  beginEdit(row, column, index) {
    //2021.03.19是否開啟查詢介面表格雙擊編輯結束方法,返回false不會結束編輯
    return true;
  },
  tableEndEditBefore(row, column, index) {
    return this.endEditBefore(row, column, index);
  },
  endEditBefore(row, column, index) {
    //2021.03.19是否開啟查詢介面表格雙擊編輯結束方法,返回false不會結束編輯
    return true;
  },
  filterPermission(tableName, permission) {
    //2021.03.19判斷是否有某個表的按鈕權限
    //:["Search","Add","Delete","Update","Import","Export","Upload","Audit"]
    const _result = (this.$store.state.permission || []).find((x) => {
      return x.url == '/' + tableName;
    });
    return _result && _result.permission.some((x) => x == permission);
  },
  destroyed() {
    //2021.04.11增加vue頁面銷毀方法,路由必須設置keepLive:false，設置方法見：前端開發文檔-》[禁用頁面緩存keepAlive]
  },
  loadTreeTableChildren(tree, treeNode, resolve) {
    this.loadTreeChildren.call(this, tree, treeNode, resolve);
  },
  loadTreeChildren(tree, treeNode, resolve) {
    //樹形結構加載子節點(2021.05.02),在onInit中設置了rowKey主鍵字段後才會生效
    return resolve([]);
  },
  importDetailAfter(data) {
    //2022.01.08增加明細表導入後處理
  },
  importExcelAfter(data) {
    //2022.01.08增加明細表導入後方法判斷

    if (!data.status) {
      return; // this.$message.error(data.message);
    }
    //明細表導入
    if (this.boxModel) {
      if (data.data) {
        data.data = JSON.parse(data.data);
      } else {
        data.data = [];
      }
      data.data.forEach((x) => {
        x[this.detail.key] = undefined;
        x[this.table.key] = undefined;
      });
      this.importDetailAfter(data); //增加明細表導入後處理
      this.$refs.detail.rowData.unshift(...data.data);
      this.upload.excel = false;
      return;
    }
    this.importAfter(data);
  },
  onGridModelClose(iconClick) {
    if (this.isBoxAudit) {
      this.initFormOptionType(false);
    }
    this.isBoxAudit = false;
    this.onModelClose(iconClick);
  },
  initAuditColumn() {

  },
  getWorkFlowSteps(row) {
    let table = this.table.url.replaceAll('/', '');
    let url = `api/Sys_WorkFlow/getSteps?tableName=${table}&id=${row[this.table.key]
      }`;
    this.http.get(url, {}, true).then((result) => {
      this.workFlowSteps.splice(0);
      //有可能沒有配置審批流程
      if (!result.list || !result.list.length) {
        result.list = [];
        this.auditParam.showAction = true;
        this.auditParam.height = 240;
        this.auditParam.showViewButton = row.AuditStatus == 0;
      } else {
        this.auditParam.showAction = result.list.some((c) => {
          return c.isCurrentUser;
        });
        this.auditParam.height = 511;
        this.auditParam.showViewButton = true;
      }
      this.auditParam.reason = '';
      this.auditParam.status = -1;
      this.auditParam.value = -1;
      if (result.his) {
        result.his.forEach((item) => {
          item.auditStatus = this.getAuditStatus(item.auditStatus);
        });
      }

      this.auditParam.auditHis = result.his;
      this.workFlowSteps.push(...result.list);
      this.isBoxAudit = true;
      this.initFormOptionType(true);
      this.edit(row);
      this.boxOptions.title = '審核';
    });
  },
  initFormOptionType(isReadonly) {
    this.editFormOptions.forEach((options) => {
      options.forEach((option) => {
        if (isReadonly) {
          if (!option.readonly) {
            this.formFieldsType.push(option.field);
            option.readonly = true;
          }
        } else {
          if (this.formFieldsType.indexOf(option.field) != -1) {
            option.readonly = false;
          }
        }
      });
    });
  },
  getAuditStatus(status) {
    let data = this.auditParam.data.find((x) => {
      return x.value == status;
    });
    if (!data) {
      return '-';
      //   return `審核值不正確:${status}`
    }
    return data.text;
  },
  initFlowQuery() {
    if (this.$route.query.viewflow) {
      this.$refs.table && this.search();
    }
  },
  fullscreen(full){ //彈出框全屏方法

  },
  //查詢頁面表格合併行或列，見https://element-plus.gitee.io/zh-CN/component/table.html#%E5%90%88%E5%B9%B6%E8%A1%8C%E6%88%96%E5%88%97
  spanMethod({row,column,rowIndex, columnIndex}){

  },
  //查詢頁面表格合併行或列，見https://element-plus.gitee.io/zh-CN/component/table.html#%E5%90%88%E5%B9%B6%E8%A1%8C%E6%88%96%E5%88%97
  detailSpanMethod({row,column,rowIndex, columnIndex}){

  }
};
import customColumns from './ViewGridCustomColumn.js';
//合併擴展方法
methods = Object.assign(methods, detailMethods, serviceFilter, customColumns);
export default methods;
