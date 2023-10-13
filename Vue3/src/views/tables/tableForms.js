let options = {
    formFileds1: {//表單配置
        Variety: "",
        AgeRange: "",
        DateRange: [],
        City: "",
        AvgPrice: 8.88,
        Date: "",
        IsTop: "還沒想好..."
    }
    , formRules1: [//表單配置
        [
            {
                dataKey: "city",
                title: "城市",
                required: true,
                field: "City",
                data: [],
                type: "select"
            },
            {
                title: "多選日期",
                range: true, //設置為true可以選擇開始與4結束日期
                required: false,
                field: "DateRange",
                //   colSize: 4,//設置寬度為1/3
                type: "date"
            }
        ],
        [
            {
                dataKey: "age", //後台下拉框對應的數據字典編號
                data: [], //loadKey設置為true,會根據dataKey從後台的下拉框數據源中自動加載數據
                title: "月齡",
                required: true, //設置為必選項
                field: "AgeRange",
                type: "select"
            },
            {
                title: "日期",
                required: true,
                field: "Date",
                placeholder: "你可以設置colSize屬性决定標籤的長度，可選值12/8/6/4",
                //  colSize: 8,//設置寬度為2/3
                type: "datetime"
            }
        ],
        [
            {
                title: "品種",
                dataKey: "age",
                placeholder: "此處數據源為手動綁定",
                //如果這裡綁定了data數據，後台不會加載此數據源
                data: [{ key: "1", value: "1" }, { key: "2", value: "2" }],
                required: false,
                field: "Variety",
                type: "select"
            },
            {
                type: "decimal",
                title: "價格",
                required: true,
                placeholder: "你可以自己定義placeholder顯示的文字",
                field: "AvgPrice"
            }
        ],
        [
            {
                title: "備註",
                required: true,
                field: "IsTop",
                colSize: 12, //設置12，此列占100%寬度
                type: "textarea"
            }
        ]
    ],
    table: { //table表單配置
        data: [{ ExpertName: "移動手機", AuditStatus: 0, CreateDate: "2019-11-01", UserTrueName: "沈萬三" },
        { ExpertName: "電子產品", AuditStatus: 1, CreateDate: "2019-11-02", UserTrueName: "鲁班" },
        { ExpertName: "生活用品", AuditStatus: 2, CreateDate: "2019-11-03", UserTrueName: "二貨" },
        { ExpertName: "家具辦公", AuditStatus: 0, CreateDate: "2019-11-04", UserTrueName: "二手" }],
        columns: [{
            field: "ExpertName",
            title: "商品名稱"
        },
        {
            field: "AuditStatus",
            title: "審核狀態",
            bind: {
                key: "audit",
                data: [
                    { key: "0", value: "審核中" },
                    { key: "1", value: "審核通過" },
                    { key: "2", value: "審核未通過" }
                ]
            }
        }, {
            field: "UserTrueName",
            title: "申請人",
            width: 120
        }, {
            field: "CreateDate",
            title: "申請時間",
            type: "datetime"
        }]
    }
}
export default options;