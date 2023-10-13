<template>
  <div>
    <vol-box :lazy="true"
             v-model="model.box1"
             title="彈出框1"
             :height="400"
             :width="700"
             :padding="15">
      <div>彈出框1
        <el-button type="primary"
        size="mini"
                @click="getParent">獲取父組件對象</el-button>
        <p>通過 this.$emit("parentCall", $vue => {})可以訪問父組件ViewGird中的任何屬性、對象、方法</p>
        <p>{{model.box1Text}}</p>
      </div>
    </vol-box>

    <!-- 彈出框2 -->
    <vol-box :lazy="true"
             v-model="model.box2"
             title="彈出框2"
             :height="420"
             :width="900"
             :padding="10">
      <div>
        <el-tabs type="border-card"
                 style="height: 358px;">
          <el-tab-pane>
            <template #label>  <span ><i class="el-icon-date"></i> 我的行程</span></template>
          
            我的行程
          </el-tab-pane>
          <el-tab-pane :lazy="true"
                       label="消息中心">消息中心</el-tab-pane>
          <el-tab-pane :lazy="true"
                       label="角色管理">角色管理</el-tab-pane>
          <el-tab-pane :lazy="true"
                       label="定時任務補">定時任務補</el-tab-pane>
        </el-tabs>

      </div>
    </vol-box>

    <!-- 彈出框3 -->
    <vol-box :lazy="true"
             v-model="model.box3"
             title="彈出框3"
             :height="400"
             :width="700"
             :padding="15">
  <div>當前操作的行數據{{JSON.stringify(model.box3Row)}}</div>
        <template #footer> 
      <div >
        <el-button type="primary"
        size="mini"
                @click="$Message.error('點擊確認')">確認</el-button>
        <el-button type="default"
           size="mini"
                @click="model.box3=false">點擊關閉彈出框</el-button>
      </div></template>
    </vol-box>
  </div>
</template>
<script>
import VolBox from "@/components/basic/VolBox.vue";
export default {
  components: { "vol-box": VolBox },
  methods: {},
  data() {
    return {
      model: {
        box1Text: "",
        box1: false,
        box1Text: "",
        box2: false,
        box3: false,   
        box3Row: {},
      },
    };
  },
  methods: {
    getParent() {
      this.$emit("parentCall", ($vue) => {
        this.model.box1Text = JSON.stringify($vue.buttons);
      });
    },
    getTestData() {
      return "這裡是獲取到的子組件對象" + JSON.stringify(this.model);
    },
    //彈出框1
    open1() {
      this.model.box1 = true;
    },
    //彈出框2
    open2() {
      this.model.box2 = true;
    },
    //彈出框3
    open3(row) {
      this.model.box3Row = row;
      this.model.box3 = true;
    },
  },
};
</script>

