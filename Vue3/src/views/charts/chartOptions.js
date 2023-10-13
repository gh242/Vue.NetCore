let options= {
    bar:{
      // title: {
      //   text: 'World Population'
      // },
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'shadow'
        }
      },
      // legend: {},
      grid: {
        top:10,
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
      },
      xAxis: {
        type: 'value',
        boundaryGap: [0, 0.01]
      },
      yAxis: {
        type: 'category',
        data: ['Brazil', 'Indonesia', 'USA', 'India', 'China', 'World']
      },
      series: [
        {
          name: '2011',
          type: 'bar',
          data: [18203, 23489, 29034, 14970, 31744, 60230]
        },
        {
          name: '2012',
          type: 'bar',
          data: [19325, 23438, 31000, 11594, 24141, 6807]
        }
      ]
    },
    pie: {
      tooltip: {
        trigger: "item",
        formatter: "{a} <br/>{b} : {c} ({d}%)"
      },
      legend: {
        top: 20,
        // orient: "vertical",
        // right: 300,
        // top: 200,
        // bottom: 20,
        data: [
          "圖例1",
          "圖例2",
          "圖例3",
          "圖例4",
          "圖例5",
          "圖例6",
          "圖例7"
        ]
      },
      series: [
        {
          name: "圖例1",
          type: "pie",
          radius: ['40%', '70%'],
          selectedMode: "single",
          itemStyle: {
            borderRadius: 6,
            borderColor: '#fff',
            borderWidth: 2
          },
          data: [
            {
              value: 2563,
              name: "圖例1",
              itemStyle: {
                color: "rgb(45, 140, 240)"
              }
            },
            {
              value: 727,
              name: "圖例2",
              itemStyle: {
                color: "rgb(92, 173, 255)"
              }
            },
            {
              value: 2182,
              name: "圖例3",
              itemStyle: {
                color: "rgb(25, 190, 107)"
              }
            },
            {
              value: 1419,
              name: "圖例4",
              itemStyle: {
                color: "#00e5ff"
              }
            },
            {
              value: 984,
              name: "圖例5",
              itemStyle: {
                color: "#ff80ab"
              }
            },
            {
              value: 870,
              name: "圖例6",
              itemStyle: {
                color: "rgb(237, 64, 20)"
              }
            },
            {
              value: 1670,
              name: "圖例7",
              itemStyle: {
                color: "#ffb445"
              }
            }
          ]
        }
      ]
    },
    line: {
      legend: {
        data: ["郵件營銷", "聯盟廣告"]
      },
      grid: {
        left: "3%",
        right: "4%",
        bottom: "3%",
        containLabel: true
      },
      toolbox: {
        feature: {
          saveAsImage: {}
        }
      },
      xAxis: {
        type: "category",
        boundaryGap: false,
        data: [
          "1月",
          "2月",
          "3月",
          "4月",
          "5月",
          "6月",
          "7月",
          "8月",
          "9月",
          "10月",
          "11月",
          "12月"
        ]
      },
      yAxis: {
        type: "value"
      },
      series: [
        {
          name: "郵件營銷",
          type: "line",
          stack: "總量",
          itemStyle: {
            color: "rgb(25, 190, 107)"
          },
          smooth: true,
          data: [
            7.0,
            6.9,
            9.5,
            12.5,
            18.2,
            21.5,
            22.5,
            23.3,
            18.3,
            13.9,
            9.6
          ]
        },
        {
          name: "聯盟廣告",
          type: "line",
          stack: "總量",
          smooth: true,
          itemStyle: {
            color: "rgb(92, 173, 255)"
          },
          data: [
            7.0,
            6.9,
            9.5,
            14.5,
            18.2,
            21.5,
            22.5,
            21.3,
            18.3,
            13.9,
            9.6
          ]
        }
      ]
    }
  }

  export default options;