
import mtable from "../multil2Extension/mtable.vue"
let extension = {
    components: {//動態擴充組件或組件路徑
        gridHeader:'',
        gridBody: "",
        gridFooter:mtable,
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    text:"見multi2.vue、multi2.js",
    buttons: [],//擴展的按鈕
    methods: {//事件擴展
        onInit() {
            this.tableMaxHeight=200;
        },
        onInited() {
         
        }
    }
};
export default extension;