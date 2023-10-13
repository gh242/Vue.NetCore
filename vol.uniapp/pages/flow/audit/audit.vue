<template>
	<vol-audit v-if="isInit" @onAudit="onAudit" :data="data"></vol-audit>
</template>
<script>
	export default {
		data() {
			return {
				isInit:false,
				data: {
					workTable: "",
					tableKey: ""
				}
			}
		},
		methods:{
			onAudit(){
				var pages = getCurrentPages(); // 獲取頁面栈(堆疊)
				var prevPage = pages[pages.length - 2]; // 上一個頁面
				prevPage.$vm.search();
			}
		},
		onLoad(options) {
			this.data.workTable = options.workTable;
			this.data.tableKey = options.tableKey;
			//console.log(this.data)
			this.isInit=true;
		}
	}
</script>
