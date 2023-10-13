<template>
  <div style="border:1px solid black;width:400px; height: 130px;">
    <h3>我是子組件</h3>
    <button @click="send">子組件(ChildPage.vue)將值傳遞給父組件</button>
    <div>子組件(ChildPage.vue)接收父組件的值:{{FatherMsg}}</div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      data: 'I am your children',
    }
  },
  props: ['FatherMsg'],
  methods: {
    send() {
      this.$emit('ListenChild', this.data);
    }
  }
}
</script>

<style scoped></style>