
import table1 from "../multil1Extension/table1.vue"
let extension = {
    components: {//動態擴充組件或組件路徑
        gridHeader:'',
        gridBody: "",
        gridFooter:table1,
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    text:"此處表單由代碼生成，也可引入單獨ViewGrid.vue手動配置數據",
    tableAction:'App_TransactionAvgPrice',//指定表權限
    buttons: [],//擴展的按鈕
    methods: {//事件擴展
        onInit() {
            this.tableMaxHeight=400;
        },
        onInited() {
         
        }
    }
};
export default extension;