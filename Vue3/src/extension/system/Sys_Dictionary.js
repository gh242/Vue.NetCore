import { h, resolveComponent } from 'vue';
let extension = {
    components: { //動態擴充組件或組件路徑
        //表單header、content、footer對應位置擴充的組件
        //擴展組件引入方式
        gridHeader: '',
        gridBody: {
            render () {
                return [
                    h(resolveComponent('el-alert'), {
                        style: { 'margin-bottom': '12px' },
                        'show-icon': true, type: 'error',
                        closable: false, title: '介面下拉框、多選、checkbox等數據源都在此處維護，也是代碼生成器中的數據源'
                    }, ''),
                ]
            }
        },
        gridFooter: '',
        //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    buttons: [], //擴展的按鈕
    methods: { //事件擴展
        onInit () {
            //點擊單元格編輯與4結束編輯(默認是點擊單元格編輯，鼠標離開結束編輯)
            this.detailOptions.clickEdit = true;
            this.editFormOptions.forEach(x => {
                x.forEach(item => {
                    if (item.field == 'ParentId') {
                        item.min = 0;
                    }
                    if (item.field == "DbSql") {
                        item.placeholder = "如果從數據庫加載數據源，請按此格式配置sql語句：select orderType as key,orderName as value from order  如果需要根據用戶信息加載數據源，請配置好此sql,再修改後台DictionaryHandler.GetCustomDBSql方法";
                    }
                })
            })
            this.detailOptions.columns.forEach(x => {
                if (x.field == 'OrderNo') {
                    x.summary = true;
                }
            })
            //保存後不關閉編輯框
            this.boxOptions.saveClose = false;
        },
        onInited () {
            this.boxOptions.height = document.body.clientHeight * 0.87
            this.height = this.height - 45;
        },
        addBefore (formData) {
            return this.saveBefore(formData);
        },
        updateBefore (formData) {
            return this.saveBefore(formData);
        },
        saveBefore (formData) {
            if (this.editFormFields.DbSql &&
                (this.editFormFields.DbSql.indexOf('value') == -1 ||
                    this.editFormFields.DbSql.indexOf('key') == -1)
            ) {
                this.$message.error("sql語句必須包括key/value字段,如:select orderType as key,orderName as value from order");
                return false;
            }
            return true;
        },
        searchBefore (param) {
            return true;
        },
        searchAfter (result) {
            return true;
        }
    }
};
export default extension;