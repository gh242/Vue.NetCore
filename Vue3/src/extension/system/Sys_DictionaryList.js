

let extension = {
    components: {//動態擴充組件或組件路徑
        //表單header、content、footer對應位置擴充的組件
        gridHeader:'',
        gridbody:'',
        gridFooter: '',
        //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的組件
        modelHeader: '',
        modelBody: '',
        modelFooter: ''
    },
    buttons: [],//擴展的按鈕
    methods: {//事件擴展
        onInit() {
        },
        onInited() {
        }
    }
};
export default extension;