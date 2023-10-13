using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VOL.Core.DBManager;
using VOL.Core.Extensions;
using VOL.Core.Infrastructure;
using VOL.Entity.DomainModels;

namespace VOL.Core.Utilities
{
    public class EPPlusHelper
    {
        /// <summary>
        /// 導入模板(僅限框架導出模板使用)(202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="exportColumns">指定導出的列</param>
        /// <param name="ignoreColumns">忽略不導出的列(如果設置了exportColumns,ignoreColumns不會生效)</param>
        /// <param name="ignoreSelectValidationColumns">忽略下拉框數據源驗證的字段</param>
        /// <returns></returns>

        public static WebResponseContent ReadToDataTable<T>(string path,
            Expression<Func<T, object>> exportColumns = null,
            List<string> ignoreTemplate = null,
            Func<string, ExcelWorksheet, ExcelRange, int, int, string> readValue = null,
            Expression<Func<T, object>> ignoreSelectValidationColumns = null
            )
        {
            WebResponseContent responseContent = new WebResponseContent();

            FileInfo file = new FileInfo(path);
            if (!file.Exists) return responseContent.Error("未找到上傳的文件,請重新上傳");

            List<T> entities = new List<T>();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                if (package.Workbook.Worksheets.Count == 0 ||
                    package.Workbook.Worksheets.FirstOrDefault().Dimension.End.Row <= 1)
                    return responseContent.Error("未導入數據");
                //2020.08.11修復獲取表結構信息時，表為別名時查不到數據的問題
                //typeof(T).GetEntityTableName()
                List<CellOptions> cellOptions = GetExportColumnInfo(typeof(T).Name, false, false, columns: exportColumns?.GetExpressionToArray());
                //設置忽略的列
                if (exportColumns != null)
                {
                    cellOptions = cellOptions
                        .Where(x => exportColumns.GetExpressionToArray().Select(s => s.ToLower()).Contains(x.ColumnName.ToLower()))
                        .ToList();
                }
                else if (ignoreTemplate != null)
                {
                    cellOptions = cellOptions
                        .Where(x => !ignoreTemplate.Select(s => s.ToLower()).Contains(x.ColumnName.ToLower()))
                        .ToList();
                }


                ExcelWorksheet sheetFirst = package.Workbook.Worksheets.FirstOrDefault();

                for (int j = sheetFirst.Dimension.Start.Column, k = sheetFirst.Dimension.End.Column; j <= k; j++)
                {
                    string columnCNName = sheetFirst.Cells[1, j].Value?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(columnCNName))
                    {
                        CellOptions options = cellOptions.Where(x => x.ColumnCNName == columnCNName).FirstOrDefault();
                        if (options == null)
                        {
                            return responseContent.Error("導入文件列[" + columnCNName + "]不是模板中的列");
                        }
                        if (options.Index > 0)
                        {
                            return responseContent.Error("導入文件列[" + columnCNName + "]不能重覆");
                        }
                        options.Index = j;
                    }
                }
                if (cellOptions.Exists(x => x.Index == 0))
                {
                    return responseContent.Error("導入文件列必須與導入模板相同");
                }
                string[] ignoreSelectValidationFields =null;
                if (ignoreSelectValidationColumns!=null)
                {
                    ignoreSelectValidationFields = ignoreSelectValidationColumns.GetExpressionToArray();
                }
                else
                {
                    ignoreSelectValidationFields = new string[0];
                }

                PropertyInfo[] propertyInfos = typeof(T).GetProperties()
                       .Where(x => cellOptions.Select(s => s.ColumnName).Contains(x.Name))
                       .ToArray();
                ExcelWorksheet sheet = package.Workbook.Worksheets.FirstOrDefault();
                for (int m = sheet.Dimension.Start.Row + 1, n = sheet.Dimension.End.Row; m <= n; m++)
                {
                    T entity = Activator.CreateInstance<T>();
                    for (int j = sheet.Dimension.Start.Column, k = sheet.Dimension.End.Column; j <= k; j++)
                    {

                        string value = sheet.Cells[m, j].Value?.ToString();
                        //2022.06.20增加原生excel讀取方法
                        if (readValue != null)
                        {
                            value = readValue(value, sheet, sheet.Cells[m, j], m, j);
                        }

                        CellOptions options = cellOptions.Where(x => x.Index == j).FirstOrDefault();
                        PropertyInfo property = propertyInfos.Where(x => x.Name == options.ColumnName).FirstOrDefault();
                        //2021.06.04優化判斷
                        if (string.IsNullOrEmpty(value))
                        {
                            if (options.Requierd)
                            {
                                return responseContent.Error($"第{m}行[{options.ColumnCNName}]驗證未通過,不能為空。");
                            }
                            continue;
                        }


                        //驗證字典數據
                        //2020.09.20增加判斷數據源是否有值
                        if (!string.IsNullOrEmpty(options.DropNo) && !string.IsNullOrEmpty(value)&&!ignoreSelectValidationFields.Contains(property.Name))
                        {
                            if (options.KeyValues == null)
                            {
                                return responseContent.Error($"[{options.ColumnCNName}]字段數字典編號[{options.DropNo}]缺失,請檢查字典配置");
                            }
                            string key = null;
                            //2022.11.21增加導入多選的支持
                            if ((options.EditType == "selectList" || options.EditType == "checkbox") && !string.IsNullOrEmpty(value))
                            {
                                var cellValues = value.Replace("，", ",").Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                var keys = options.KeyValues.Where(x => cellValues.Contains(x.Value))
                                    .Select(s => s.Key).ToArray();
                                if (cellValues.Length == keys.Length)
                                {
                                    key = string.Join(",", keys);
                                }
                            }
                            else
                            {
                                key = options.KeyValues.Where(x => x.Value == value)
                                    .Select(s => s.Key)
                                    .FirstOrDefault();
                            }

                            if (key == null)//&& options.Requierd
                            {
                                //小於20個字典項，直接提示所有可選value
                                string values = options.KeyValues.Count < 20 ? (string.Join(',', options.KeyValues.Select(s => s.Value))) : options.ColumnCNName;
                                return responseContent.Error($"第{m}行[{options.ColumnCNName}]驗證未通過,必須是字典數據中[{values}]的值。");
                            }
                            //將值設置為數據字典的Key,如果導入為是/否字典項，存在表中應該對為1/0
                            value = key;
                        }
                        else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            //2021.06.04增加日期格式處理
                            if (value.Length == 5 && int.TryParse(value, out int days))
                            {
                                property.SetValue(entity, new DateTime(1900, 1, 1).AddDays(days - 2));
                            }
                            else
                            {
                                property.SetValue(entity, value.ChangeType(property.PropertyType));
                            }
                            continue;
                        }

                        //驗證導入與實體數據類型是否相同
                        (bool, string, object) result = property.ValidationProperty(value, options.Requierd);

                        if (!result.Item1)
                        {
                            return responseContent.Error($"第{m}行[{options.ColumnCNName}]驗證未通過,{result.Item2}");
                        }

                        property.SetValue(entity, value.ChangeType(property.PropertyType));
                    }
                    entity.SetCreateDefaultVal();
                    entities.Add(entity);
                }
            }
            return responseContent.OK(null, entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnCNName">key為字段名, ValueTuple<string, int>為字段中文名及列寬度</param>
        /// <param name="dicNos"> List<ValueTuple<string, string, string>>item1列名,item2 字典value,item3字典name </param>
        /// <returns>返回文件保存的路徑</returns>
        public static string Export(DataTable table, List<CellOptions> cellOptions, string savePath, string fileName)
        {
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //獲取所有有值的數據源
            var dicNoKeys = cellOptions
                 .Where(x => !string.IsNullOrEmpty(x.DropNo) && x.KeyValues != null && x.KeyValues.Keys.Count > 0)
                 .Select(x => new { x.DropNo, x.ColumnName }).Distinct().ToList();



            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    using (ExcelRange range = worksheet.Cells[1, i + 1])
                    {
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.Gray);  //背景色
                        worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                    }
                    CellOptions options = cellOptions.Where(x => x.ColumnName == table.Columns[i].ColumnName).FirstOrDefault();
                    if (options != null)
                    {
                        worksheet.Column(i + 1).Width = options.ColumnWidth / 6.00;
                        worksheet.Cells[1, i + 1].Value = options.ColumnCNName;
                    }
                    else
                    {
                        worksheet.Column(i + 1).Width = 15;
                        worksheet.Cells[1, i + 1].Value = table.Columns[i].ColumnName;
                    }
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string cellValue = (table.Rows[i][j] ?? "").ToString();
                        if (dicNoKeys.Exists(x => x.ColumnName == table.Columns[j].ColumnName))
                        {
                            cellOptions.Where(x => x.ColumnName == table.Columns[j].ColumnName)
                                .Select(s => s.KeyValues)
                                .FirstOrDefault()
                                .TryGetValue(cellValue, out string result);
                            cellValue = result ?? cellValue;
                        }
                        worksheet.Cells[i + 2, j + 1].Value = cellValue;
                    }
                }
                package.SaveAs(new FileInfo(savePath + fileName));
            }
            return savePath + fileName;
        }


        /// <summary>
        /// 下載導出模板(僅限框架導出模板使用)(202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exportColumns">指定導出的列</param>
        /// <param name="ignoreColumns">忽略不導出的列(如果設置了exportColumns,ignoreColumns不會生效)</param>
        /// <param name="savePath">導出文件的絕對路徑</param>
        /// <param name="fileName">導出的文件名+後缀,如:123.xlsx</param>
        /// <returns></returns>
        public static string ExportTemplate<T>(List<string> exportColumns, List<string> ignoreColumns, string savePath, string fileName)
        {
            return Export<T>(null, exportColumns, ignoreColumns, savePath, fileName, true);
        }

        /// <summary>
        /// 下載導出模板(僅限框架導出模板使用)(202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exportColumns">指定導出的列</param>
        /// <param name="ignoreColumns">忽略不導出的列(如果設置了exportColumns,ignoreColumns不會生效)</param>
        /// <param name="savePath">導出文件的絕對路徑</param>
        /// <param name="fileName">導出的文件名+後缀,如:123.xlsx</param>
        /// <returns></returns>
        public static string ExportTemplate<T>(Expression<Func<T, object>> exportColumns, List<string> ignoreColumns, string savePath, string fileName)
        {
            return Export<T>(null, exportColumns?.GetExpressionToArray(), ignoreColumns, savePath, fileName, true);
        }

        /// <summary>
        /// 下載導出模板(僅限框架導出模板使用)(202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreColumns">忽略不導出的列</param>
        /// <param name="savePath">導出文件的絕對路徑</param>
        /// <param name="fileName">導出的文件名+後缀,如:123.xlsx</param>
        /// <returns></returns>
        public static string ExportTemplate<T>(List<string> ignoreColumns, string savePath, string fileName)
        {
            return Export<T>(null, null, ignoreColumns, savePath, fileName, true);
        }

        /// <summary>
        /// 導出excel文件(導入功能里的導出模板也使用的此功能，list傳的null，導出的文件只有模板的標題)
        /// (202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="cellOptions">對應代碼生成器的配置</param>
        /// <param name="ignore">忽略不導出的字段</param>
        /// <param name="savePath"></param>
        /// <param name="fileName"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string Export<T>(List<T> list, Expression<Func<T, object>> ignore, string savePath, string fileName, bool template = false)
        {
            return Export(list, null, ignore?.GetExpressionProperty(), savePath, fileName, template);
        }

        /// <summary>
        /// 導出excel文件(導入功能里的導出模板也使用的此功能，list傳的null，導出的文件只有模板的標題)
        /// (202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">導出的對象</param>
        /// <param name="exportColumns">指定導出的列</param>
        /// <param name="ignoreColumns">忽略不導出的列(如果設置了exportColumns,ignoreColumns不會生效)</param>
        /// <param name="savePath">保存路徑</param>
        /// <param name="fileName">保存的文件名</param>
        ///  <param name="template">是否為下載模板</param>
        /// <returns></returns>
        public static string Export<T>(List<T> list, IEnumerable<string> exportColumns, IEnumerable<string> ignoreColumns, string savePath, string fileName, bool template = false)
        {
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //獲取代碼生成器對應的配置信息
            //  List<CellOptions> cellOptions = GetExportColumnInfo(typeof(T).GetEntityTableName(), template);
            //2020.06.02修復使用表別名時讀取不到配置信息
            List<CellOptions> cellOptions = GetExportColumnInfo(typeof(T).Name, template, columns: exportColumns?.ToArray());
            string fullPath = savePath + fileName;
            //獲取所有有值的數據源
            var dicNoKeys = cellOptions
                 .Where(x => !string.IsNullOrEmpty(x.DropNo) && x.KeyValues != null && x.KeyValues.Keys.Count > 0)
                 .Select(x => new { x.DropNo, x.ColumnName, x.SearchType, x.EditType }).Distinct().ToList();
            //2021.01.24修復多選類型，導出excel文件沒有轉換數據源的問題
            var selectList = dicNoKeys.Where(x => x.SearchType == "checkbox" || x.SearchType == "selectList" || x.EditType == "checkbox" || x.EditType == "selectList")
                  .Select(s => s.ColumnName).ToArray();

            List<PropertyInfo> propertyInfo = null;

            /*導出時，代碼生成器中的表配置信息Sys_TableInfo/Sys_TableColumn必須與當前數據庫相同，否則導出來可能沒有數據*/

            //2020.06.02優化讀取導出列配置信息
            //導出指定的列
            //如果指定了導出的標題列，忽略的標題列不再起作用
            if (exportColumns != null && exportColumns.Count() > 0)
            {
                propertyInfo = new List<PropertyInfo>();
                var properties = typeof(T).GetProperties();
                foreach (var name in exportColumns)
                {
                    var property = properties.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    if (property != null)
                    {
                        propertyInfo.Add(property);
                    }
                }
                //propertyInfo =
                //   typeof(T).GetProperties()
                //  .Where(x => exportColumns.Select(g => g.ToLower()).Contains(x.Name.ToLower())).ToList();
                //.Where(x => cellOptions.Select(s => s.ColumnName) //獲取代碼生成器配置的列
                //.Contains(x.Name)).ToList();
            }
            else if (ignoreColumns != null && ignoreColumns.Count() > 0)
            {
                propertyInfo = typeof(T).GetProperties()
                  .Where(x => !ignoreColumns.Select(g => g.ToLower()).Contains(x.Name.ToLower()))
                  .Where(x => cellOptions.Select(s => s.ColumnName).Contains(x.Name)) //獲取代碼生成器配置的列
                  .ToList();
            }
            else
            {
                //默認導出代碼生成器中配置【是否顯示】=是的列
                propertyInfo = typeof(T).GetProperties()
                  .Where(x => cellOptions.Select(s => s.ColumnName).Contains(x.Name)) //獲取代碼生成器配置的列
                  .ToList();
                /*
                 * 如果propertyInfo查出來的長度=0
                 * 1、代碼生成器中的配置信息是否同步到當前數據庫
                 * 2、代碼生成器中的配置列名與model的字段是否大小寫一致
                 */
            }
            string[] dateArr = null;
            if (!template)
            {
                dateArr = propertyInfo.Where(x => x.PropertyType == typeof(DateTime)
                || x.PropertyType == typeof(DateTime?))
                .Select(s => s.Name).ToArray();
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");
                for (int i = 0; i < propertyInfo.Count; i++)
                {
                    string columnName = propertyInfo[i].Name;
                    using (ExcelRange range = worksheet.Cells[1, i + 1])
                    {
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //默認灰色背景，白色字體
                        Color backgroundColor = Color.Gray;
                        //字體顏色
                        Color fontColor = Color.White;
                        //下載模板並且是必填項，將表格設置為黄色
                        if (template)
                        {
                            fontColor = Color.Black;
                            if (cellOptions.Exists(x => x.ColumnName == columnName && x.Requierd))
                            {
                                backgroundColor = Color.Yellow;  //黄色必填
                            }
                            else
                            {
                                backgroundColor = Color.White;
                            }
                        }
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(backgroundColor);  //背景色
                        worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(fontColor);//字體顏色
                    }
                    CellOptions options = cellOptions.Where(x => x.ColumnName == columnName).FirstOrDefault();
                    if (options != null)
                    {
                        worksheet.Column(i + 1).Width = options.ColumnWidth / 6.00;
                        worksheet.Cells[1, i + 1].Value = options.ColumnCNName;
                    }
                    else
                    {
                        worksheet.Column(i + 1).Width = 15;
                        worksheet.Cells[1, i + 1].Value = columnName;
                    }
                }
                //下載模板直接返回
                if (template)
                {
                    package.SaveAs(new FileInfo(fullPath));
                    return fullPath;
                }
                //2021.01.24修復多選類型，導出excel文件沒有轉換數據源的問題
                IEnumerable<string> GetListValues(string cellValues, string propertyName)
                {
                    var values = cellValues.Split(",");
                    for (int i = 0; i < values.Length; i++)
                    {
                        cellOptions.Where(x => x.ColumnName == propertyName)
                      .Select(s => s.KeyValues)
                      .FirstOrDefault()
                     .TryGetValue(values[i], out string result);
                        yield return result ?? values[i];
                    }

                }
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < propertyInfo.Count; j++)
                    {
                        object cellValue = null;
                        if (dateArr != null && dateArr.Contains(propertyInfo[j].Name))
                        {
                            object value = propertyInfo[j].GetValue(list[i]);
                            cellValue = value == null ? "" : ((DateTime)value).ToString("yyyy-MM-dd HH:mm:sss").Replace(" 00:00:00", "");
                        }
                        else
                        {
                            cellValue = propertyInfo[j].GetValue(list[i]);
                        }
                        if (cellValue != null && dicNoKeys.Exists(x => x.ColumnName == propertyInfo[j].Name))
                        {
                            //2021.01.24修復多選類型，導出excel文件沒有轉換數據源的問題
                            if (selectList.Contains(propertyInfo[j].Name))
                            {
                                cellValue = string.Join(",", GetListValues(cellValue.ToString(), propertyInfo[j].Name));
                            }
                            else
                            {
                                cellOptions.Where(x => x.ColumnName == propertyInfo[j].Name)
                            .Select(s => s.KeyValues)
                            .FirstOrDefault()
                            .TryGetValue(cellValue.ToString(), out string result);
                                cellValue = result ?? cellValue;
                            }

                        }
                        worksheet.Cells[i + 2, j + 1].Value = cellValue;
                    }
                }

                package.SaveAs(new FileInfo(fullPath));
            }
            return fullPath;
        }


        /// <summary>
        /// 獲取導出的列的數據信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="temlate">是否為下載模板</param>
        /// filterKeyValue是否過濾Key相同的數據
        /// <returns></returns>
        private static List<CellOptions> GetExportColumnInfo(string tableName, bool temlate = false, bool filterKeyValue = true, string[] columns = null)
        {
            //&& x.IsDisplay == 1&&x.IsReadDataset==0只導出代碼生器中設置為顯示並且不是只讀的列，可根據具體業務設置導出列
            // && x.IsReadDataset == 0
            //2020.06.02增加不區分大表名大小寫: 原因mysql可能是表名是小寫，但生成model的時候强制大寫
            //x => x.TableName.ToLower() == tableName.ToLower()
            var query = DBServerProvider.DbContext.Set<Sys_TableColumn>().Where(x => x.TableName.ToLower() == tableName.ToLower());
            if (columns != null && columns.Length > 0)
            {
                query = query.Where(x => columns.Contains(x.ColumnName));
            }
            else
            {
                query = query.Where(x => x.IsDisplay == 1);
            }
            List<CellOptions> cellOptions = query.OrderByDescending(x => x.OrderNo).Select(c => new CellOptions()
            {
                ColumnName = c.ColumnName,
                ColumnCNName = c.ColumnCnName,
                DropNo = c.DropNo,
                Requierd = c.IsNull > 0 ? false : true,
                ColumnWidth = c.ColumnWidth ?? 90,
                EditType = c.EditType,
                SearchType = c.SearchType

            }).ToList();


            if (temlate) return cellOptions;

            var dicNos = cellOptions.Where(x => !string.IsNullOrEmpty(x.DropNo)).Select(c => c.DropNo);

            if (dicNos.Count() == 0) return cellOptions;

            var dictionaries = DictionaryManager.GetDictionaries(dicNos);
            //獲取綁定字典數據源下拉框的值
            foreach (string dicNo in dicNos.Distinct())
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                List<Sys_DictionaryList> dictionaryLists = dictionaries
                   .Where(x => x.DicNo == dicNo && x.Sys_DictionaryList != null)
                   .Select(s => s.Sys_DictionaryList).FirstOrDefault();
                if (dictionaryLists == null || dictionaryLists.Count == 0) continue;
                foreach (var item in dictionaryLists)
                {
                    ////filterKeyValue為true過濾keyvalue相不的項,key==value相同的則不處理
                    if (filterKeyValue && item.DicName == item.DicValue) continue;
                    if (keyValues.ContainsKey(item.DicValue)) continue;
                    keyValues.Add(item.DicValue, item.DicName);
                }

                foreach (CellOptions options in cellOptions.Where(x => x.DropNo == dicNo))
                {
                    options.KeyValues = keyValues;
                }
            }
            return cellOptions;
        }

        public static string ExportGeneralExcel(
         List<Dictionary<string, object>> rows,
         string fileName,
         string path = null,
         Action<ExcelWorksheet, int, int, object> onFillCell = null,
         Action<ExcelWorksheet> saveBefore = null)
        {
            return ExportGeneralExcel(rows.Select(item => item as IDictionary<string, object>).ToList(), fileName, path, onFillCell, saveBefore);
        }

        /// <summary>
        /// 2021.01.10增加通過excel導出功能
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="onFillCell"></param>
        /// <param name="saveBefore"></param>
        /// <returns></returns>
        public static string ExportGeneralExcel(
                List<IDictionary<string, object>> rows,
                string fileName,
                string path = null,
                Action<ExcelWorksheet, int, int, object> onFillCell = null,
                Action<ExcelWorksheet> saveBefore = null)
        {
            path = path ?? $"Download/ExcelExport/{ DateTime.Now.ToString("yyyyyMMdd")}/";
            string fullPath = path.MapPath();
            fileName = Guid.NewGuid() + "_" + fileName;
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");
                int j = 0;
                foreach (var item in rows[0])
                {
                    object cellValue = item.Key;

                    int i = 0;
                    worksheet.Cells[1 + i, j + 1].Value = cellValue;
                    //worksheet.Column(j + 1).Width = 11;
                    worksheet.Row(i + 1).Height = 20;//設置行高
                    var style = worksheet.Cells[i + 1, j + 1].Style;
                    style.Fill.PatternType = ExcelFillStyle.Solid;
                    style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//水平居中
                    style.VerticalAlignment = ExcelVerticalAlignment.Center;//垂直剧中
                    style.Font.Bold = true;//字體為粗體
                                           //style
                    style.Fill.BackgroundColor.SetColor(Color.FromArgb(216, 216, 216));//背景色
                    style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));//邊框

                    onFillCell?.Invoke(worksheet, i, j, cellValue);
                    j++;
                }
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    j = 0;
                    foreach (var item in row)
                    {
                        object cellValue = item.Value;
                        worksheet.Cells[i + 2, j + 1].Value = cellValue;

                        onFillCell?.Invoke(worksheet, i + 1, j, cellValue);
                        j++;
                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        worksheet.Column(col).Width = worksheet.Column(col).Width + 2;
                    }
                }
                saveBefore?.Invoke(worksheet);
                package.SaveAs(new FileInfo(fullPath + fileName));
            }
            return path + fileName;
        }


    }

    public class CellOptions
    {
        public string ColumnName { get; set; }//導出表的列
        public string ColumnCNName { get; set; }//導出列的中文名
        public string DropNo { get; set; }//字典編號
        public int ColumnWidth { get; set; }//導出列的寬度,代碼生成維护的寬度
        public bool Requierd { get; set; } //是否必填
        public int Index { get; set; }//列所在模板的序號(導入用)
        //2021.01.24修復多選類型，導出excel文件沒有轉換數據源的問題
        public string EditType { get; set; }
        public string SearchType { get; set; }
        //對應字典項維护的Key,Value
        public Dictionary<string, string> KeyValues { get; set; }
        //public string Value { get; set; } //對應字典項維护的Value
        //public string Name { get; set; } //對應字典項顯示的名稱
    }
}
