using System;
using System.Collections.Generic;
using System.Text;

namespace VOL.Core.Utilities.PDFHelper
{
    /// <summary>
    /// pdf接口
    /// </summary>
    public interface IPDFService
    {
        /// <summary>
        /// 創建PDF
        /// </summary>
        /// <param name="htmlContent">傳入html字符串</param>
        /// <returns></returns>
        byte[] CreatePDF(string htmlContent);

    }
}
