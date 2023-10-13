import gridHeader from './App_News/App_NewGirdHeader'

import { h, resolveComponent } from 'vue';
let extension = {
    components: {//動態擴充組件或組件路徑
        gridHeader: gridHeader,
        gridBody: {
            render() {
                return [
                    h(resolveComponent('el-alert'), {
                        style: { 'margin-bottom': '12px' },
                        'show-icon': true, type: 'warning',
                        closable: false, title: '發佈静態頁面、頁面預覽見:App_News.js'
                    }, ''),
                ]
            }
        },
        gridFooter: '',
        modelHeader: '',
        modelFooter: ''
    },
    text: "静態文件沒有提交到github，本地重新生成下静態頁面與4設置封面即可預覽",
    buttons: {
        view: [//ViewGrid查詢介面擴展的按鈕
        ]
    },
    methods: {
        initButton() {
            //當前用戶是否有編輯或新建權限
            if (this.currentReadonly) { return; }

            this.buttons.splice(2, 0, {
                name: "設置封面", icon: 'el-icon-picture', value: 'Edit', type: 'primary',
                onClick: function () { this.setCover(); }
            })
            //添加彈出框生成静態頁面的按鈕
            this.boxButtons.splice(0, 0, ...[{
                name: "生成静態頁面", icon: 'el-icon-tickets', type: 'primary',
                onClick: function () { this.publish(); }
            },
            {
                name: "預覽頁面", icon: 'el-icon-view', type: 'primary',
                onClick: function () {
                    if (!this.currentRow
                        || !this.currentRow.Content
                        || !this.currentRow.DetailUrl) {
                        return this.$Message.error("請先【保存】,再點擊【生成静態頁面】")
                    }
                    this.preview(this.currentRow);
                }
            }])
        },
        onInit() { //初始化預覽與4彈出框大小
            //設置表格内容超出換行
            this.textInline=false;
            //根據用戶權限初始化按鈕
            this.initButton();
            //設置保存成功後，不關閉彈出框
            this.boxOptions.saveClose = false;
            //設置查詢表格只能單選
            this.single = true;

            //設置table表格DetailUrl字段點擊預覽静態頁面
            this.editFormOptions.forEach(x => {
                x.forEach(item => {
                    this.columns.forEach(item => {
                        //設置url點擊事件
                        if (item.field == 'DetailUrl') {
                            item.title = "頁面預覽";
                            item.formatter = (row) => { return '<a>預覽</a>' }
                            item.click = (row, column, event) => {
                                this.preview(row);
                            }
                        }
                    })
                })
            })
        },
        onInited(){
            this.height = this.height - 50;
        },
        addBefore(formData) {//新建前驗證
            return this.validContent(formData);
        },
        updateBefore(formData) { //修改前驗證
            return this.validContent(formData);
        },
        validContent(formData) {
            if (!this.editFormFields.Content) {
                this.$Message.error("請編輯要發佈的内容");
                return false;
            }
            return true;
        },
        preview(row) { //預覽html頁面
            if (!row.DetailUrl || row.DetailUrl.indexOf('.html') == -1 || !this.base.isUrl(this.http.ipAddress + row.DetailUrl)) {
                return this.$Message.error("請先發佈静態頁面")
            }
            window.open(this.http.ipAddress + row.DetailUrl + '?r=' + Math.random());
        },
        publish() { //生成静態頁面
            if (!this.currentRow || !this.currentRow[this.table.key]) {
                return this.$Message.error("請先保存數據")
            }
            if (!this.currentRow.Content) {
                return this.$Message.error("請編輯要發佈的内容")
            }
            this.http.post("api/news/createPage", this.currentRow).then(x => {
                if (x.status) {
                    // this.editFormFields.DetailUrl = x.data.url;
                    this.currentRow.DetailUrl = x.data.url;
                }
                this.refresh();
                return this.$Message.info(x.message)
            })
        },
        setCover() {  //設置封面圖片
            let rows = this.getSelectRows();
            if (rows.length == 0) {
                return this.$Message.error("請選中要設置封面的行")
            }
            //設置封面圖片，先獲取當前已經圖片fileInfo格式見volupdate組件參數說明
            let fileInfo = this.getFilePath(rows[0].ImageUrl) || [];
            this.$refs.gridHeader.open(fileInfo)
        },
        getFilePath(pathSring) {//拆分url並初始化圖片到上傳組件中
            //獲取表的圖片與4文件顯示
            if (!pathSring) return "";
            let fileInfo = [], filePath = pathSring.replace(/\\/g, "/").split(",");
            for (let index = 0; index < filePath.length; index++) {
                let file = filePath[index];
                if (file.indexOf(".") == -1) { continue; }
                let splitFile = file.split("/");
                if (splitFile.length == 0) { continue; }
                fileInfo.push({
                    name: splitFile[splitFile.length - 1],
                    path: this.base.isUrl(file) ? file : this.http.ipAddress + file
                });
            }
            return fileInfo;
        }
    }
};
export default extension;