<template>
    <div class="editor-example">
    <el-alert type="info" show-icon  style="margin-bottom: 15px;" :closable="false" title="編輯器使用">
        <p>編輯器集成的是wangeditor,框架只做了一個基本功能集成</p>
        框架代碼生成器默認集成了編輯器,代碼生成器裡面編輯類型選擇editor即可自動生成,更多功能：<a href="https://www.wangeditor.com/" target="_blank">查看wangeditor文檔</a>
    </el-alert>
    <vol-wang-editor v-model="content" :height="350" url="api/App_Expert/Upload"></vol-wang-editor>
    </div>
</template>

<script>
import VolWangEditor from '@/components/editor/VolWangEditor.vue'
import {ref,defineComponent} from 'vue'
export default defineComponent({
   components:{
       'vol-wang-editor':VolWangEditor
   },
   setup(props) {
       const content=ref('請輸入。。。。');
       return {content}
   }
})
</script>
<style lang="less" scoped>
.editor-example{
    padding: 25px;
}
</style> 