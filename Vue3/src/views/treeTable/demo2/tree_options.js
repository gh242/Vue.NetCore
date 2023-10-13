let
  options = {
    tree: [
      { "id": 1, "parentId": 0, "text": "北京市" },
      { "id": 2, "parentId": 1, "text": "西城區" },
      { "id": 3, "parentId": 1, "text": "東城區" },
      { "id": 5, "parentId": 0, "text": "西藏自治區" },
      { "id": 6, "parentId": 5, "text": "拉薩市" },
      { "id": 7, "parentId": 5, "text": "昌都市" },
      { "id": 8, "parentId": 7, "text": "丁青縣" }
    ],
    //table數據
    tableData: [
      {
        code: "001",
        id: 1,//tree的id數據
        address: '海淀區',
        remark: '北京市-海淀區',
        enable: 1,
        createDate: '2020-04-01 20:00'
      }, {
        code: "001",
        id: 1,//tree的id數據
        address: '朝阳區',
        remark: '北京市-朝阳區',
        enable: 1,
        createDate: '2020-04-01 20:00'
      },{
      code: "001",
      id: 2,//tree的id數據
      address: '恭王府 ',
      remark: '世界最大的四合院除皇帝和家眷外，任何人是不得住進紫禁城的',
      enable: 1,
      createDate: '2020-04-01 20:00'
    },
    {
      code: "002",
      id: 3,//tree的id數據
      address: '白塔寺 ',
      remark: '妙應寺白塔位於阜城門内大街路北的妙應寺内。因寺内有通體涂以白垩的塔，故俗稱“白塔寺”',
      enable: 0,
      createDate: '2020-04-01 20:00'
    }, {
      code: "004",
      id: 6,//tree的id數據
      address: '布達拉宫',
      remark: '布達拉宫不僅是西藏的象征，更是朝聖者心中的聖地',
      enable: 1,
      createDate: '2020-04-01 20:00'
    },
    {
      code: "005",
      id: 5,//tree的id數據
      address: '大昭寺',
      remark: '在藏民心中大昭寺在拉薩的中心地位',
      enable: 1,
      createDate: '2020-04-01 20:00'
    },
    {
      code: "007",
      id: 7,
      address: '測試 ',
      remark: '測試測試',
      enable: 1,
      createDate: '2020-04-01 20:00'
    },
    {
      code: "008",
      id: 5,
      address: '日客則 ',
      remark: '位於定日縣境内的珠穆朗玛峰是世界第一高峰。峰頂常年積雪',
      enable: 0,
      createDate: '2020-04-01 20:00'
    }]
  }

export default options;
