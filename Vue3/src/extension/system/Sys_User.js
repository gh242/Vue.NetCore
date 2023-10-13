import {  defineAsyncComponent } from "vue";
let extension = {
    components: { //動態擴充組件或組件路徑
        //表單header、content、footer對應位置擴充的組件
        gridHeader: defineAsyncComponent(() =>
            import("./Sys_User/Sys_UserGridHeader.vue")),
        gridBody: '',
        gridFooter: '',
        //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    text: "只能看到當前角色下的所有帳號",
    buttons: [], //擴展的按鈕
    methods: { //事件擴展
        onInit() {
            this.boxOptions.height = 530;
            this.columns.push({
                title: '操作',
                hidden: false,
                align: "center",
                fixed: 'right',
                width: 120,
                render: (h, { row, column, index }) => {
                    return h(
                        "div", { style: { 'font-size': '13px', 'cursor': 'pointer', 'color': '#409eff' } }, [
                        h(
                            "a", {
                            style: { 'margin-right': '15px' },
                            onClick: (e) => {
                                e.stopPropagation()
                                this.$refs.gridHeader.open(row);
                            }
                        }, "修改密碼"
                        ),
                        h(
                            "a", {
                            style: {},
                            onClick: (e) => {
                                e.stopPropagation()
                                this.edit(row);
                            }
                        },
                            "編輯"
                        ),
                    ])
                }
            })
        },
        onInited() { },
        addAfter(result) { //用戶新建後，顯示随機生成的密碼
            if (!result.status) {
                return true;
            }
            //顯示新建用戶的密碼
            //2020.08.28優化新建成後提示方式
            this.$confirm(result.message, '新建用戶成功', {
                confirmButtonText: '確定',
                type: 'success',
                center: true
            }).then(() => { })

            this.boxModel = false;
            this.refresh();
            return false;
        },
        modelOpenAfter() {
            //點擊彈出框後，如果是編輯狀態，禁止編輯用戶名，如果新建狀態，將用戶名字段設置為可編輯
            let isEDIT = this.currentAction == this.const.EDIT;
            this.editFormOptions.forEach(item => {
                item.forEach(x => {
                    if (x.field == "UserName") {
                        x.disabled=isEDIT;
                    }
                })
                //不是新建，性別默認值設置為男
                if (!isEDIT) {
                    this.editFormFields.Gender = "0";
                }
            })
        }

    }
};
export default extension;