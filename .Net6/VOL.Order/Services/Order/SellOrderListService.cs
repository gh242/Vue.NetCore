/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能導致被代碼生成器覆蓋
 *所有業務編寫全部應在Partial文件夾下SellOrderListService與ISellOrderListService中編寫
 */
using VOL.Order.IRepositories;
using VOL.Order.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace VOL.Order.Services
{
    public partial class SellOrderListService : ServiceBase<SellOrderList, ISellOrderListRepository>, ISellOrderListService, IDependency
    {
        public SellOrderListService(ISellOrderListRepository repository)
             : base(repository) 
        { 
           Init(repository);
        }
        public static ISellOrderListService Instance
        {
           get { return AutofacContainerModule.GetService<ISellOrderListService>(); }
        }
    }
}
