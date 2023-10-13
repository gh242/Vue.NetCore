/*
*所有關於SellOrder類的業務代碼接口應在此處編寫
*/
using VOL.Core.BaseProvider;
using VOL.Entity.DomainModels;
namespace VOL.Order.IServices
{
    public partial interface ISellOrderService
    {
        string GetServiceDate();
    }
 }
