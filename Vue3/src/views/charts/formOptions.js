let formFields1 = {
    Variety: "",
    AgeRange: "",
    DateRange: [],
    City: "",
    AvgPrice: 8.88,
    Date: "",
    IsTop: "還沒想好..."
}
let formRules1 = [
    //兩列的表單，formRules數據格式為:[[{},{}]]
    [
        {
            link:true,
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
            title: "下拉",
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
            title: "測試",
            dataKey: "age",
            placeholder: "此處數據源為手動綁定",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [{ key: "1", value: "測試1" }, { key: "2", value: "測試2" }],
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
]

let formFields2 = {
    Variety: "一次性用品",
    City: "北京市",
    DateRange: "2019-09-01",
    AvgPrice: 8.88,
    Variety1: "",
    DateRange1: "2019-09-02",
    AvgPrice1: 7.72,
    Address:"北京市海淀區001號",
    IsChange: 1
}
let formRules2 = [
    //兩列的表單，formRules數據格式為:[[{},{}]]
    [
        {
            title: "商品類型",
            dataKey: "age",
            //如果這裡綁定了data數據，後台不會加載此數據源
            data: [{ key: "1", value: "1" }, { key: "2", value: "2" }],
            field: "Variety",
            disabled: true,
            type: "select"
        },
        {
            dataKey: "city",
            title: "所在城市",
            field: "City",
            disabled: true,
            type: "select",
            data: []
        }],
    [
        {
            title: "銷售日期",
            field: "DateRange",
            disabled: true,
        },
        {
            title: "銷售價格",
            field: "AvgPrice",
            disabled: true
        }],
    [
        {
            title: "生產日期",
            field: "DateRange1",
            disabled: true,
        },
        {
            title: "中間價格",
            field: "AvgPrice1",
            disabled: true
        }],
    [
        {
            title: "銷售地址",
            field: "Address",
            disabled: true,
        },
        {
            title: "是否成交",
            field: "IsChange",
            dataKey: "enable",//這裡設置了數據字典源的編號會自動從後台加載數據源的key/value
            data: [],
            disabled: true
        }]
]


export { formFields1, formRules1, formFields2, formRules2 }