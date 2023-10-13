<template>
  <!-- <div style="border: 1px solid black; width: 150px; margin: 10px auto"> -->
  <div style="border: 1px solid black; width: 250px; padding-bottom: 20px;">
    我是子組件(ChildPage_ref)
    <div style="color: red"> {{ msg }} </div>
  </div>

</template>

<script>
export default {
  data() {
    return {
      msg: '',
    }
  },
  props: ['FatherMsg'],
  methods: {
    childFun() {
      // console.log('我是子組件的方法 childFun');
      this.msg = '我的方法(childFun)被調用了'
    },
  },
};
</script>

<style scoped></style>