<template>
  <!-- <div style="border: 1px solid black; width: 150px; margin: 10px auto"> -->
  <div style="border: 1px solid black; width: 150px;">
    我是子組件
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
  mounted() {
    this.$on('childFun_emit', function() {
        console.log('我是子組件方法');
        this.msg = '我的方法被調用了'
    });
  },
};
</script>

<style scoped></style>