
<template>
  <div id="big-data-container" class="big-data-container">
    <div class="head">
      <h1>大屏數據統計分析顯示</h1>
    </div>
    <div class="data-container">
      <div class="data-left">
        <div class="data-left-item">
          <div class="title">商品銷量分類</div>
          <div id="chart-vleft-1" style="height: calc(100% - 30px)"></div>
          <div class="data-foot-line"></div>
        </div>
        <div class="data-left-item">
          <div class="title">本月商品銷量</div>
          <div id="chart-vleft-3" style="height: calc(100% - 30px)"></div>

          <div class="data-foot-line"></div>
        </div>
        <div class="data-left-item">
          <div class="title">7日訂單銷量</div>
          <div id="chart-vleft-2" style="height: calc(100% - 30px)"></div>
          <div class="data-foot-line"></div>
        </div>
      </div>
      <div class="data-center">
        <!-- <div class="title">中間位置</div> -->
        <div class="center-top-num">
          <div class="item">
            <div class="text">累計銷量</div>
            <div class="num">220,000</div>
          </div>
          <div class="item">
            <div class="text">累計銷售金額</div>
            <div class="num">58,000,000</div>
          </div>
          <div class="item">
            <div class="text">購買用戶人數</div>
            <div class="num">15,000</div>
          </div>
          <div class="data-foot-line"></div>
        </div>
        <div
          class="center-top"
          style="height: 260px; padding-top: 25px; overflow: hidden"
        >
          <!-- <div class="title">用戶活躍信息-1</div> -->
          <div id="chart-vgauge-1" style="height: 400px"></div>
          <!-- <iview-circle :size="200" style="padding: 8px 0;"></iview-circle> -->
          <div class="data-foot-line"></div>
        </div>
        <div class="title">訂單銷售統計</div>
        <div id="chart-vcenter" style="height:400px;" class="chart-vcenter"></div>
      </div>
      <div class="data-right">
        <div class="data-right-item">
          <div class="title">銷售情況走勢</div>
          <div id="chart-vright-1" style="height: calc(100% - 30px)"></div>
          <div class="data-foot-line"></div>
        </div>
        <div class="data-right-item" style="height: 220px; padding-top: 25px">
          <!-- <div class="title">用戶活躍信息</div> -->
          <!-- <iview-circle></iview-circle> -->
          <div id="chart-vgauge-2" style="height: 300px"></div>
          <div class="data-foot-line"></div>
        </div>
        <div class="data-right-item right-3">
          <div class="title">商品銷售排行</div>
          <div id="chart-vright-3" class="right-item">
            <div class="item">
              <div class="top">排名</div>
              <div class="pro-name">商品名稱</div>
              <div class="num">銷量</div>
              <div class="num">銷售金額</div>
            </div>
            <div class="item">
              <div class="top top-1">
                <span>1</span>
              </div>
              <div class="pro-name">卡帝樂鰐魚</div>
              <div class="num">2,200</div>
              <div class="num">360,00</div>
            </div>
            <div class="item">
              <div class="top top-2">
                <span>2</span>
              </div>
              <div class="pro-name">春夏男T恤</div>
              <div class="num">1,700</div>
              <div class="num">24,500</div>
            </div>
            <div class="item">
              <div class="top top-3">
                <span>3</span>
              </div>
              <div class="pro-name">男女同款休閑鞋</div>
              <div class="num">1,120</div>
              <div class="num">12,700</div>
            </div>
          </div>
          <div class="boxfoot"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
var echarts = require("echarts");
let $chartLeft1,
  $chartLeft2,
  $chartLeft3,
  $chartCenter,
  $chartRight1,
  $chartGauge1,
  $chartGauge2;
import {
  chartLeft1,
  chartLeft2,
  chartLeft3,
  chartRight1,
  gauge,
} from "./bigdata/chart-options";
// import IviewCircle from "./bigdata/IviewCircle";
import "./bigdata/layout.less";
export default {
  components: {
    // "iview-circle": IviewCircle
  },
  data() {
    return {};
  },
  created() {
    console.log("chart");
  },
  mounted() {
    if ($chartLeft1) {
      $chartLeft1.dispose();
      $chartLeft2.dispose();
      $chartLeft3.dispose();
      $chartCenter.dispose();
      $chartRight1.dispose();
      $chartGauge1.dispose();
      $chartGauge2.dispose();
    }
    $chartLeft1 = echarts.init(document.getElementById("chart-vleft-1"));
    $chartLeft1.setOption(chartLeft1, true);

    $chartLeft2 = echarts.init(document.getElementById("chart-vleft-2"));
    $chartLeft2.setOption(chartLeft2, true);

    $chartLeft3 = echarts.init(document.getElementById("chart-vleft-3"));
    $chartLeft3.setOption(chartLeft3, true);

    $chartCenter = echarts.init(document.getElementById("chart-vcenter"));
    $chartCenter.setOption(chartRight1, true);

    $chartRight1 = echarts.init(document.getElementById("chart-vright-1"));
    $chartRight1.setOption(chartRight1, true);

    $chartGauge1 = echarts.init(document.getElementById("chart-vgauge-1"));
    $chartGauge1.setOption(gauge, true);

    $chartGauge2 = echarts.init(document.getElementById("chart-vgauge-2"));
    $chartGauge2.setOption(gauge);
  },
  destroyed() {
    $chartLeft1 = null;
    $chartLeft2 = null;
    $chartLeft3 = null;
    $chartCenter = null;
    $chartRight1 = null;
    $chartGauge1 = null;
    $chartGauge2 = null;
  },
};
</script>
<style scoped>
/* .chart-center {
  display: flex;
  border: 1px solid #0000ff;
  height: 200px;
  flex-direction: column;
  margin-top: 20px;
}
.chart-center .item {
  text-align: center;
  border: 1px solid #00c1b3;
  flex: 1;
} */
.right-3 {
  display: flex;
  flex-direction: column;
  /* margin-top: 20px; */
}

.right-3 .right-item {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.right-3 .item {
  text-align: left;
  border-bottom: 1px solid #549069;
  flex: 1;
  display: flex;
  padding: 5px 10px;
  margin: 0 10px;
  font-size: 14px;
  line-height: 30px;
}

.right-3 .item:last-child {
  border-bottom: 0;
}

.right-3 .item > div {
  color: white;
}

.right-3 .top {
  width: 60px;
  position: relative;
}

.right-3 .top span {
  position: absolute;
  width: 20px;
  line-height: 20px;
  top: 5px;
  text-align: center;
  border-radius: 5px;
}

.right-3 .top-1 span {
  background: #e80d0d;
}

.right-3 .top-2 span {
  background: #00c935;
}

.right-3 .top-3 span {
  background: #0083f4;
}

.right-3 .num {
  width: 88px;
}

.right-3 .pro-name {
  flex: 1;
}
</style>




