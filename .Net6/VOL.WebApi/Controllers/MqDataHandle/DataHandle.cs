using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOL.WebApi.Controllers.MqDataHandle
{

    /// <summary>
    /// 數據處理
    /// </summary>
    public class DataHandle
    {
        /// <summary>
        /// 構造  可注入service服務執行db
        /// </summary>
        public DataHandle()
        {

        }

        /// <summary>
        /// 報警數據處理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static bool AlarmData(string data)
        {
            //dapper入庫或其他業務操作

            return true;
        }

    }
}
