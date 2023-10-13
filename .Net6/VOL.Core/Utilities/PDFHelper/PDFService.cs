
﻿//using WkHtmlToPdfDotNet;
//using WkHtmlToPdfDotNet.Contracts;

//namespace VOL.Core.Utilities.PDFHelper
//{
//    /// <summary>
//    /// pdf實現
//    /// </summary>
//    public class PDFService : IPDFService
//    {
//        private IConverter _converter;
//        public PDFService(IConverter converter)
//        {
//            _converter = converter;
//        }

//        /// <summary>
//        /// 創建PDF
//        /// </summary>
//        /// <param name="htmlContent">傳入html字符串</param>
//        /// <returns></returns>
//        public byte[] CreatePDF(string htmlContent)
//        {
//            var globalSettings = new GlobalSettings
//            {
//                ColorMode = ColorMode.Color,
//                Orientation = Orientation.Portrait,
//                PaperSize = PaperKind.A4,
//                //Margins = new MarginSettings
//                //{
//                //    Top = 10,
//                //    Left = 0,
//                //    Right = 0,
//                //},
//                DocumentTitle = "SuZong PDF Report",
//            };
//            var objectSettings = new ObjectSettings
//            {
//                PagesCount = true,
//                HtmlContent = htmlContent,
//                //Page = "www.baidu.com", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE  這裡是用現有的網頁生成PDF
//                //WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
//                WebSettings = { DefaultEncoding = "utf-8" },
//                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
//                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
//                //允許本地文件訪問
//                LoadSettings = new LoadSettings { BlockLocalFileAccess = false }
//            };
//            var pdf = new HtmlToPdfDocument()
//            {
//                GlobalSettings = globalSettings,
//                Objects = { objectSettings }
//            };
 
//            var file = _converter.Convert(pdf);
//            //return File(file, "application/pdf");
//            return file;
//        }

//    }
//}
