<template>
	<!--地址picker-->
	<view :status="checkStatus" v-if="lotusAddressData.visible" class="lotus-address-mask">
		<view :class="lotusAddressData.visible?'lotus-address-box':'lotus-address-box lotus-address-box-out'">
			<view class="lotus-address-action">
				<text @tap="cancelPicker" class="lotus-address-action-cancel">取消</text>
				<text @tap="chosedVal" class="lotus-address-action-affirm">確認</text>
			</view>
			<view class="lotus-address-picker-box">
				<!--省-->
				<scroll-view scroll-y :scroll-into-view="'pid'+pChoseIndex" class="lotus-address-picker-box-item">
					<view @tap="clickPicker(0,pIndex,pItem);" :id="'pid'+pIndex" :class="pIndex === pChoseIndex?'lotus-address-picker lotus-address-picker2':'lotus-address-picker'"  v-for="(pItem,pIndex) in province" :key="pIndex">{{pItem}}</view>
				</scroll-view>
				<!--市-->
				<scroll-view scroll-y :scroll-into-view="'cid'+cChoseIndex" class="lotus-address-picker-box-item">
					<view @tap="clickPicker(1,cIndex,cItem);" :id="'cid'+cIndex" :class="cIndex === cChoseIndex?'lotus-address-picker lotus-address-picker2':'lotus-address-picker'" v-for="(cItem,cIndex) in city" :key="cIndex">{{cItem}}</view>
				</scroll-view>
				<!--區-->
				<scroll-view scroll-y :scroll-into-view="'tid'+tChoseIndex" class="lotus-address-picker-box-item">
					<view @tap="clickPicker(2,tIndex,tItem);" :id="'tid'+tIndex" :class="tIndex === tChoseIndex?'lotus-address-picker lotus-address-picker2':'lotus-address-picker'" v-for="(tItem,tIndex) in town" :key="tIndex">{{tItem}}</view>
				</scroll-view>
				<!--區END-->
			</view>
		</view>
	</view>
	<!--地址picker END-->
</template>

<script>
	import {lotusAddressJson} from  "./Winglau14-lotusAddress.js";
	export default {
		props:['lotusAddressData'],
		data() {
			return {
				visible: false,
				province:[],
				city:[],
				town:[],
				provinceName:'',
				cityName:'',
				townName:'',
				type:0,//0新增1編輯
				pChoseIndex:-1,
				cChoseIndex:-1,
				tChoseIndex:-1
			};
		},
		methods:{
			//取消
			cancelPicker(){
				const provinceCode = this.getTarId(this.provinceName);
				const cityCode = this.getTarId(this.cityName);
				const townCode = this.getTarId(this.townName);
				this.visible = false;
				this.$emit("choseVal",{
					province:this.provinceName,
					provinceCode,
					city:this.cityName,
					cityCode,
					town:this.townName,
					townCode,
					isChose:0,
					visible:false
				});
			},
			//獲取最後選擇的省市區的值
			chosedVal() {
				this.type = 1;
				const provinceCode = this.getTarId(this.provinceName);
				const cityCode = this.getTarId(this.cityName);
				const townCode = this.getTarId(this.townName);
				this.visible = false;
				let isChose = 0;
				//已選省市區 isChose = 1
				if((this.provinceName&&this.cityName)||(this.provinceName&&this.cityName&&this.townName)){
					isChose = 1;
				}
				this.$emit("choseVal",{
					province:this.provinceName,
					provinceCode,
					city:this.cityName,
					cityCode,
					town:this.townName,
					townCode,
					isChose,
					visible:false
				});
			},
			//獲取省市區value
			getTarId(name,type){
			    let id = 0;
			    lotusAddressJson.map((item,index)=>{
			        if(item.name === name){
			            id = item.value;
			        }
			    });
			    return id;
			},
			//獲取市數據
			getCityArr(parentId){
			    let city = [];
			    lotusAddressJson.map((item,index)=>{
			        if(item.parent === parentId){
			            city.push(item.name);
			        }
			    });
			    return city;
			},
			//獲取區數據
			getTownArr(parentId){
			    let town = [];
			    lotusAddressJson.map((item,index)=>{
			        if(index>34&&item.parent === parentId){
			            town.push(item.name);
			        }
			    });
			    return town;
			},
			//初始化數據
			initFn(){
				if(!this.province.length){
					lotusAddressJson.map((item,index)=>{
						if(index<=34){
							this.province.push(item.name);
						}
					});
				}
				//已選擇省市區，高亮顯示對應選擇省市區
				const p = this._props.lotusAddressData.provinceName;
				const c = this._props.lotusAddressData.cityName;
				const t = this._props.lotusAddressData.townName;
				//已選省
				if(p){
					this.pChoseIndex = this.getTarIndex(this.province,p);
				}
				//已選市
				if(p&&c){
					const pid = this.getTarId(p);
					this.city = this.getCityArr(pid);
					this.cChoseIndex = this.getTarIndex(this.city,c);
				}
				//已選區
				if(p&&c&&t){
					const cid= this.getTarId(c);
					this.town = this.getTownArr(cid);
					this.tChoseIndex = this.getTarIndex(this.town,t);
				}
				//未選省市區
				if(!p&&!c&&!t){
					this.pChoseIndex = -1;
					this.cChoseIndex = -1;
					this.tChoseIndex = -1;
					this.city = [];
					this.town = [];
				}
			},
			//獲取已選省市區
			getChosedData(){
				const pid = this.getTarId(this.provinceName,'province');
				this.city = this.getCityArr(pid);
				const cid= this.getTarId(this.cityName,'city');
				this.town = this.getTownArr(cid);
				//已選省市區獲取對應index
				if(this.provinceName){
					this.pChoseIndex = this.getTarIndex(this.province,this.provinceName);
				}
				if(this.cityName){
					this.cChoseIndex = this.getTarIndex(this.city,this.cityName);
				}
				if(this.townName){
					this.tChoseIndex = this.getTarIndex(this.town,this.townName);
				}
			},
			//選擇省市區交互
			clickPicker(type,index,name){
				//省
				if(type === 0){
					this.pChoseIndex = index;
					this.provinceName = name;
					this.cChoseIndex = -1;
					this.tChoseIndex = -1;
					this.cityName = '';
					this.townName = '';
				}
				//市
				if(type ===1){
					this.cChoseIndex = index;
					this.cityName = name;
					this.tChoseIndex = -1;
					this.townName = '';
				}
				//區
				if(type === 2){
					this.tChoseIndex = index;
					this.townName = name;
				}
				//獲取省市區數據
				this.getChosedData();
			},
			//獲取已選省市區index
			getTarIndex(arr,tarName){
			    let cIndex = 0;
			    arr.map((item,index)=>{
			        if(item === tarName){
			            cIndex = index;
			        }
			    });
			    return cIndex;
			}
		},
		computed:{
			checkStatus(){
				let t = null;
				const _this = this;
				if(!_this.visible){
					_this.visible = _this._props.lotusAddressData.visible;
					//獲取省市區
					_this.provinceName = _this._props.lotusAddressData.provinceName;
					_this.cityName = _this._props.lotusAddressData.cityName;
					_this.townName = _this._props.lotusAddressData.townName;
					//生成初始化數據
					_this.initFn();
					t = _this.visible;
				}
				return t;
			}
		}
	}
</script>

<style lang="less">
@import "./Winglau14-lotusAddress.css";
</style>
