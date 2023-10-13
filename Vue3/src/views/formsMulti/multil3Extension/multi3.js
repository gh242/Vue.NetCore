import comMulti from "../multil3Extension/comMulti.vue"
let extension = {
    components: {//動態擴充組件或組件路徑
        gridHeader:'',
        gridBody: "",
        gridFooter:comMulti,
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    text:"見multi3.js、multi3.vue",
    buttons: [],//擴展的按鈕
    methods: {//事件擴展
        onInit() {
            this.tableMaxHeight=300;
        },
        onInited() {
         
        }
    }
};
export default extension;