## Vue + .NetCore前後端分離，不一樣的快速發開框架(提供Vue2/Vue3版本)

## 框架核心
 - 快速開發(基礎功能全部由代碼生成器生成)
 - 支持前端、後台自定義業務代碼擴展,後台提供了大量常用擴展與通用類
 - 前端、後台提供了近300個擴展方法與屬性,開發人員可在此功能上編寫擴展自定義業務代碼
 - 代碼生成(代碼生成器可直接生成主/從表前後端業務代碼,有30多種屬性可在線配置生成的代碼)
 - 前端table自動轉換key/value
 - 前端表單select/checkbox自動綁定數據源,不需要寫任何代碼
 - 支持(主從表)一對一前後端代碼全自動生成、並支持數據源自動綁定與業務代碼擴展,不需要寫任何代碼
 - 支持一對多從表自定義擴展(不限從表類型與從表數量) , 一對多從表使用擴展可轻松實現
 - 如果能上手框架，可以體會到不用996,更不用掉頭發的感觉^_^

## 框架适用範圍 
 - 前後端分離項目
 - 編寫各種後台restful api接口。後台基礎代碼由代碼生成器完成,在生成的代碼上繼續編寫業務即可
 - 前端表單開發(直接上手看demo即可)
 - 配合app做H5或全h5開發
 - 移動端開發、app、微信小程序(uniapp)，見下面介绍
 - 在現有的代碼生成器功能上，繼續定制開發代碼生成器功能,解決重覆性工作
## 框架開發依賴環境
 - 後台：VS2019、vs2022 、.NetCore3.1 、.Net6、EFCore3.1/6.0、JWT、Dapper、SignalR、Quartz.Net、Autofac、SqlServer/MySql/PGSql/Oracle、Redis
 - 前端：VsCode、Vue2/vue3（需要安装nodejs)、vuex、axios、promise、element ui、element plus
## 連接

## [vol框架視頻](https://www.cctalk.com/m/group/90268531)
## [vol框架企業版](http://pro.volcore.xyz/)
## [NET視頻教程(微软MVP-ACE錄制)](https://space.bilibili.com/525836469)
## [元讯趣編程交流社區](https://www.qubcedu.com/)

## 項目啟動與上手
 - http://v2.volcore.xyz/document/guide
## vue2版本
 - http://v2.volcore.xyz
## vue3版本
 - http://www.volcore.xyz
## 演示地址2
 - http://120.48.115.252:9990/
## App/H5開發
 - http://v2.volcore.xyz/app/guide

## 2023.05.13增加審批流程分支、條件功能
![Home](/imgs/flow.png)  
![Home](/imgs/flow2.png)  
![Home](/imgs/flow3.png)  


## 框架移動端（uniapp）已發佈,同樣全自動生成代碼,掃描小程序二維碼即可查看

![Home](/imgs/qrcode.png)  

![Home](/imgs/app-01.png)  
![Home](/imgs/app-02.png)  
![Home](/imgs/m001.png)  
![Home](/imgs/m002.png)  
## 框架已支持Vue3版本
![Home](/imgs/v3.png)  
## 框架已增加低代碼設計器
![Home](/imgs/fd01.png)  
![Home](/imgs/fd02.png)  
## 框架2.0已更新(部分新增功能截圖)
增加切換皮肤功能
![Home](/imgs/h.png)  
![Home](/imgs/home_them.png)  
增加可復用的後台請求參數校驗
![Home](/imgs/validator.png)  
增加樹形菜單與代碼生成頁面使用
![Home](/imgs/x7tree.png)  
增加文本編輯器直接發佈静態頁面功能
![Home](/imgs/editor.png)  
一對一多從表顯示(只需要少量代碼就可完成成，其他都由代碼生成器生成)
![Home](/imgs/m1.png)  
表合併顯示 (只需要幾行代碼完成代碼生成器生成的頁面實現擴展)
![Home](/imgs/span.png)  
從圖上傳圖片 (只需要幾行代碼完成代碼生成器生成的頁面實現擴展)
![Home](/imgs/p1.png)  
一對多從表(不限從表數量)擴展
![Home](/imgs/multi.png)  
圖表
![Home](/imgs/charts.png)  



## 1、只讀基礎表單
整個只讀的基礎表單的所有前後端代碼，全部由代碼生成器生成，代碼生成器中幾乎不需要配置，並支持並後端業務代碼擴展，直接生成代碼後，配置菜單權限即可
![Home](/imgs/table1.png)  

## 2、自動綁定下拉框數據表單
整個自動綁定下拉框數據表單的所有前後端代碼，全部由代碼生成器生成，並支持並後端業務代碼擴展，在代碼生成器中只需要指定數據源編號，頁面加載時會根據編號自動加載數據源並綁定  
![Home](/imgs/table2.png)  

## 3、啟用圖片支持、審核表單
整個啟用圖片支持、審核表單的所有前後端代碼，全部由代碼生成器生成，並支持並後端業務代碼擴展，審核功能需要在菜單配置權限、代碼生成器中勾選啟用圖片支持
![Home](/imgs/table3.png)  

## 4、高級查詢
整個表單的所有前後端代碼，全部由代碼生成器生成，並支持並後端業務代碼擴展，查詢字段、類型(下拉框、日期、TextArea等)、所在行與列都由代碼生成器完成，不需要寫任何代碼  
    ![Home](/imgs/tablesearch4.png)  
    
## 5、主從表新建、編輯
主從表新建、編輯所有前後端代碼，全部由代碼生成器生成，並支持並後端業務代碼擴展，新建、編輯從表配置、字段、類型(下拉框、日期、TextArea等)、所在行與列、字段是否只讀、標籤顯示的長度等都由代碼生成器完成，不需要寫任何代碼  
![Home](/imgs/editTbale2.png)  


## 6、excel導入
excel導入整個頁面都由代碼生成器生成，導入的字段、字段是否必填，下載模板也由代碼生成器上配置(自己根據實際需要决定是否采用此方法)，導入時會驗證是否為空與數據的合法性，邏輯校驗自己實現擴展方法即可  
![Home](/imgs/importTable1.png)  


## 7、H5開發
![Home](/imgs/h5.jpg)  

## 8、權限分配
目前只實現了對用戶的角色的Action進行權限分配
![Home](/imgs/auth.png)  

## 9、代碼生成器
代碼生成器提供了20多種可配置的屬性，可灵活配置顯示、查詢、編輯、導入、導出、主從關系等功能<a href="http://132.232.2.109/document/coder">點擊看代碼生成器文欄</a>
![Home](/imgs/coder.png)  

其他功能。。。。。

## 框架預覽
 - 框架内置了大量的通用組件可直接使用,並内置了基於本框架定制開發的代碼生成器，盡量避免重覆性代碼編寫。
 - 框架不僅僅是快速開發，更多的是倾向於業務代碼擴展的編寫與代碼規範。
 - 如果有什麼問題或建議，提issue或加QQ：283591387

 - QQ2群：913189178
 - QQ3群：743852316
 - 
 - vue3地址：http://www.volcore.xyz
 - vue2地址：http://v2.volcore.xyz
 - 帳號：admin666密碼：123456（本地超級管理員帳號：admin密碼123456)
 - github地址：https://github.com/cq-panda/vue.netcore
 - gitee碼雲：https://gitee.com/x_discoverer/Vue.NetCore
 - 框架文欄：http://v2.volcore.xyz/document/guide
 - 框架更新日誌：http://v2.volcore.xyz/document/log
 
