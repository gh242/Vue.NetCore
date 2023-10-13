<template>
  <div>
    <Button @click="volBoxFrom" style="margin-left: 8px">彈出框帶表單</Button>
    <Button @click="volBoxFromObject" style="margin-left: 8px">獲取彈出框表單對象</Button>
  </div>
</template>

<script>

import VolBoxForm from "@/components/basic/VolBoxForm.vue";

var formParam = {
  components: {
    VolBoxForm
  }, 
  methods: {
    volBoxFrom() {
   
    },
    volBoxFromObject() {
    
    }
  },
  created() {},
  data() {
    return {
     
    };
  }
};
extend(formParam);
export default formParam;
</script>

