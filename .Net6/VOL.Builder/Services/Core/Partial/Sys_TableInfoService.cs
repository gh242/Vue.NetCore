using VOL.Builder.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using VOL.Core.Const;
using VOL.Core.DBManager;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
using VOL.Entity.DomainModels.Sys;
using VOL.Entity.SystemModels;

namespace VOL.Builder.Services
{
    public partial class Sys_TableInfoService
    {
        private int totalWidth = 0;
        private int totalCol = 0;
        private string webProject = null;
        private string apiNameSpace = null;
        private string startName = "";
        private string StratName
        {
            get
            {
                if (startName == "")
                {
                    startName = WebProject.Substring(0, webProject.LastIndexOf('.'));
                }
                return startName;
            }
        }
        private string WebProject
        {
            get
            {
                if (webProject != null)
                    return webProject;
                webProject = ProjectPath.GetLastIndexOfDirectoryName(".WebApi") ?? ProjectPath.GetLastIndexOfDirectoryName("Api") ?? ProjectPath.GetLastIndexOfDirectoryName(".Web");
                if (webProject == null)
                {
                    throw new Exception("未獲取到以.WebApi結尾的項目名稱,無法創建頁面");
                }
                return webProject;
            }
        }
        private string ApiNameSpace
        {
            get
            {
                if (apiNameSpace != null)
                    return apiNameSpace;
                apiNameSpace = ProjectPath.GetLastIndexOfDirectoryName(".WebApi");
                if (apiNameSpace == null)
                {
                    throw new Exception("未獲取到以.WebApi,無法創建Api控制器");
                }
                return apiNameSpace;
            }
        }

        /// <summary>
        /// 獲取生成配置的樹開菜單
        /// </summary>
        /// <returns></returns>
        public async Task<(string, string)> GetTableTree()
        {
            var treeData = await repository.FindAsIQueryable(x => 1 == 1)
                .Select(c => new
                {
                    id = c.Table_Id,
                    pId = c.ParentId,
                    parentId = c.ParentId,
                    name = c.ColumnCNName,
                    orderNo = c.OrderNo
                }).OrderByDescending(c => c.orderNo).ToListAsync();
            var treeList = treeData.Select(a => new
            {
                a.id,
                a.pId,
                a.parentId,
                a.name,
                isParent = treeData.Select(x => x.pId).Contains(a.id)
            });
            string startsWith = WebProject.Substring(0, WebProject.IndexOf('.'));
            return (treeList.Count() == 0 ? "[]" : treeList.Serialize() ?? "", ProjectPath.GetProjectFileName(startsWith));
            ;

        }

        /// <summary>
        /// 2020.05.17增加mysql獲取表結構時區分當前所在數據庫
        /// </summary>
        /// <returns></returns>
        private string GetMysqlTableSchema()
        {
            try
            {
                string dbName = DBServerProvider.GetConnectionString().Split("Database=")[1].Split(";")[0]?.Trim();
                if (!string.IsNullOrEmpty(dbName))
                {
                    dbName = $" and table_schema = '{dbName}' ";
                }
                return dbName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"獲取mysql數據庫名異常:{ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// 獲取Mysql表結構信息
        /// 2020.06.14增加對mysql數據類型double區分
        /// </summary>
        /// <returns></returns>
        private string GetMySqlModelInfo()
        {
            return $@"SELECT
DISTINCT
           CONCAT(NUMERIC_PRECISION,',',NUMERIC_SCALE) as Prec_Scale,
        CASE
                 WHEN data_type IN( 'BIT', 'BOOL','bit', 'bool') THEN
                'bool'
                 WHEN data_type in('smallint','SMALLINT') THEN 'short'
				WHEN data_type in('tinyint', 'TINYINT') THEN 'sbyte'
                WHEN data_type IN('MEDIUMINT','mediumint', 'int','INT','year', 'Year') THEN
                'int'
                WHEN data_type in ( 'BIGINT','bigint') THEN
                'bigint'
                WHEN data_type IN('FLOAT',  'DECIMAL','float', 'decimal') THEN
                'decimal'
							 WHEN data_type IN( 'DOUBLE', 'double') THEN
                'double'
                WHEN data_type IN('CHAR', 'VARCHAR', 'TINY TEXT', 'TEXT', 'MEDIUMTEXT', 'LONGTEXT', 'TINYBLOB', 'BLOB', 'MEDIUMBLOB', 'LONGBLOB', 'Time','char', 'varchar', 'tiny text', 'text', 'mediumtext', 'longtext', 'tinyblob', 'blob', 'mediumblob', 'longblob', 'time') THEN
                'nvarchar'
                WHEN data_type IN('Date', 'DateTime', 'TimeStamp','date', 'datetime', 'timestamp') THEN
                'datetime' ELSE 'nvarchar'
            END AS ColumnType, Column_Name AS ColumnName
            FROM
                information_schema.COLUMNS
            WHERE
                table_name = ?tableName {GetMysqlTableSchema()};";
        }


        /// <summary>
        /// 獲取SqlServer表結構信息
        /// </summary>
        /// <returns></returns>
        private string GetSqlServerModelInfo()
        {
            return $@"
	SELECT CASE WHEN t.ColumnType IN ('DECIMAL','smallmoney','money') THEN 
                    CONVERT(VARCHAR(30),t.Prec)+','+CONVERT(VARCHAR(30),t.Scale)  ELSE ''
                     END 
                    AS Prec_Scale,t.ColumnType,t.ColumnName
                      FROM (
                    SELECT col.prec AS  'Prec',col.scale AS 'Scale',t.name AS ColumnType,col.name AS ColumnName FROM          dbo.syscolumns col
                                                LEFT  JOIN dbo.systypes t ON col.xtype = t.xusertype
                                                INNER JOIN dbo.sysobjects obj ON col.id = obj.id
                                                                                 AND obj.xtype IN ('U','V')
                                                                                 AND obj.status >= 0
                                                LEFT  JOIN dbo.syscomments comm ON col.cdefault = comm.id
                                                LEFT  JOIN sys.extended_properties ep ON col.id = ep.major_id
                                                                                  AND col.colid = ep.minor_id
                                                                                  AND ep.name = 'MS_Description'
                                                LEFT  JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id
                                                                                  AND epTwo.minor_id = 0
                                                                                  AND epTwo.name = 'MS_Description'
                                      WHERE     obj.name =@tableName) AS t";
        }
        /// <summary>
        /// 獲取PgSQl表結構信息
        /// 2020.08.07完善PGSQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        private string GetPgSqlModelInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("			SELECT ");
            stringBuilder.Append("				col.COLUMN_NAME AS \"ColumnName\", ");
            stringBuilder.Append("			CASE ");
            stringBuilder.Append("					WHEN col.udt_name = 'uuid' THEN ");
            stringBuilder.Append("					'guid'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'int2') THEN ");
            stringBuilder.Append("					'short'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'int4' ) THEN ");
            stringBuilder.Append("					'int'  ");
            stringBuilder.Append("					WHEN col.udt_name = 'int8' THEN ");
            stringBuilder.Append("					'long'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'char', 'varchar', 'text', 'xml', 'bytea' ) THEN ");
            stringBuilder.Append("					'string'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'bool' ) THEN ");
            stringBuilder.Append("					'bool'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'date','timestamp' ) THEN ");
            stringBuilder.Append("					'DateTime'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'decimal', 'money','numeric' ) THEN ");
            stringBuilder.Append("					'decimal'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'float4', 'float8' ) THEN ");
            stringBuilder.Append("					'float' ELSE'string '  ");
            stringBuilder.Append("				END  as ColumnType ");
            stringBuilder.Append("from 	information_schema.COLUMNS col  ");
            stringBuilder.Append("WHERE	\"lower\" ( TABLE_NAME ) = \"lower\" (@tableName )  ");
            return stringBuilder.ToString();
        }

        private WebResponseContent ExistsTable(string tableName, string tableTrueName)
        {
            WebResponseContent webResponse = new WebResponseContent(true);
            //如果是第一次創建model，此處反射獲取到的是已經緩存過的文件，必須重新運行項目否則新增的文件無法做判斷文件是否創建，需要重新做反射實際文件，待修改...
            var compilationLibrary = DependencyContext
                .Default
                .CompileLibraries
                .Where(x => !x.Serviceable && x.Type == "project");
            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    foreach (var entity in AssemblyLoadContext.Default
                .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                .GetTypes().Where(x => x.GetTypeInfo().BaseType != null
                    && x.BaseType == typeof(BaseEntity)))
                    {
                        if (entity.Name == tableTrueName && !string.IsNullOrEmpty(tableName) && tableName != tableTrueName)
                            return webResponse.Error($"實際表名【{tableTrueName }】已創建實體，不能創建別名【{tableName}】實體");

                        if (entity.Name != tableName)
                        {
                            var tableAttr = entity.GetCustomAttribute<TableAttribute>();
                            if (tableAttr != null && tableAttr.Name == tableTrueName)
                            {
                                return webResponse.Error($"實際表名【{tableTrueName }】已被【{entity.Name}】創建建實體,不能創建別名【{tableName}】實體,請將別名更換為【{entity.Name}】");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("查找文件異常：" + ex.Message);
                }
            }
            return webResponse;

        }

        /// <summary>
        /// 生成實體Model
        /// </summary>
        /// <param name="sysTableInfo"></param>
        /// <returns></returns>
        public string CreateEntityModel(Sys_TableInfo sysTableInfo)
        {
            if (sysTableInfo == null
                || sysTableInfo.TableColumns == null
                || sysTableInfo.TableColumns.Count == 0)
                return "提交的配置數據不完整";

            WebResponseContent webResponse = ValidColumnString(sysTableInfo);
            if (!webResponse.Status)
                return webResponse.Message;

            string tableName = sysTableInfo.TableName;
            webResponse = ExistsTable(tableName, sysTableInfo.TableTrueName);
            if (!webResponse.Status)
            {
                return webResponse.Message;
            }
            if (!string.IsNullOrEmpty(sysTableInfo.TableTrueName) && sysTableInfo.TableTrueName != tableName)
            {
                tableName = sysTableInfo.TableTrueName;
            }

            string sql = "";
            switch (DBType.Name)
            {
                case "MySql":
                    sql = GetMySqlModelInfo();
                    break;
                case "PgSql":
                    sql = GetPgSqlModelInfo();
                    break;
                default:
                    sql = GetSqlServerModelInfo();
                    break;
            }
            List<TableColumnInfo> tableColumnInfoList = repository.DapperContext.QueryList<TableColumnInfo>(sql, new { tableName });
            List<Sys_TableColumn> list = sysTableInfo.TableColumns;
            string msg = CreateEntityModel(list, sysTableInfo, tableColumnInfoList, 1);
            if (msg != "")
                return msg;
            //if (list.Any(c => c.ApiInPut > 0))
            //{
            //    CreateEntityModel(list.Where(c => c.ApiInPut > 0).ToList(), sysTableInfo, tableColumnInfoList, 2);
            //}
            //if (list.Any(c => c.ApiOutPut > 0))
            //{
            //    CreateEntityModel(list.Where(c => c.ApiOutPut > 0).ToList(), sysTableInfo, tableColumnInfoList, 3);
            //}
            return "Model創建成功!";
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="sysTableInfo"></param>
        /// <returns></returns>
        public WebResponseContent SaveEidt(Sys_TableInfo sysTableInfo)
        {
            WebResponseContent webResponse = ValidColumnString(sysTableInfo);
            if (!webResponse.Status) return webResponse;
            //2020.05.07新增禁止選擇上級角色為自己
            if (sysTableInfo.Table_Id == sysTableInfo.ParentId)
            {
                return WebResponseContent.Instance.Error($"父級id不能為自己");
            }
            if (sysTableInfo.TableColumns != null && sysTableInfo.TableColumns.Any(x => !string.IsNullOrEmpty(x.DropNo) && x.ColumnName == sysTableInfo.ExpressField))
            {
                return WebResponseContent.Instance.Error($"不能將字段【{sysTableInfo.ExpressField}】設置為快捷編輯,因為已經設置了數據源");
            }
            if (sysTableInfo.TableColumns != null)
            {
                sysTableInfo.TableColumns.ForEach(x =>
                {
                    x.TableName = sysTableInfo.TableName;
                });
            }

            sysTableInfo.TableColumns?.ForEach(x =>
            {
                if (x.IsReadDataset == null)
                {
                    x.IsReadDataset = 0;
                }
            });
            return repository.UpdateRange<Sys_TableColumn>(sysTableInfo, true, true, null, null, true);
        }

        /// <summary>
        /// 2020.08.07完善PGSQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetCurrentSql(string tableName)
        {
            string sql;
            if (DBType.Name.ToLower() == DbCurrentType.MySql.ToString().ToLower())
            {
                sql = GetMySqlStructure(tableName);
            }
            else if (DBType.Name.ToLower() == DbCurrentType.PgSql.ToString().ToLower())
            {
                sql = GetPgSqlStructure(tableName);
            }
            else
            {
                sql = GetSqlServerStructure(tableName);
            }
            return sql;
        }

        /// <summary>
        /// 將表結構重新同步到代碼生成配置
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SyncTable(string tableName)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (string.IsNullOrEmpty(tableName)) return webResponse.OK("表名不能為空");

            Sys_TableInfo tableInfo = repository.FindAsIQueryable(x => x.TableName == tableName)
          .Include(o => o.TableColumns).FirstOrDefault();
            if (tableInfo == null)
                return webResponse.Error("未獲取到【" + tableName + "】的配置信息，請使用新建功能");
            if (!string.IsNullOrEmpty(tableInfo.TableTrueName) && tableInfo.TableTrueName != tableName)
            {
                tableName = tableInfo.TableTrueName;
            }

            string sql = GetCurrentSql(tableName);

            //獲取表結構
            List<Sys_TableColumn> columns = repository.DapperContext
                  .QueryList<Sys_TableColumn>(sql, new { tableName });
            if (columns == null || columns.Count == 0)
                return webResponse.Error("未獲取到【" + tableName + "】表結構信息，請確認表是否存在");


            //獲取現在配置好的表結構
            List<Sys_TableColumn> detailList = tableInfo.TableColumns ?? new List<Sys_TableColumn>();
            List<Sys_TableColumn> addColumns = new List<Sys_TableColumn>();
            List<Sys_TableColumn> updateColumns = new List<Sys_TableColumn>();
            foreach (Sys_TableColumn item in columns)
            {
                Sys_TableColumn tableColumn = detailList.Where(x => x.ColumnName == item.ColumnName)
                    .FirstOrDefault();
                //新加的列
                if (tableColumn == null)
                {
                    item.TableName = tableInfo.TableName;
                    item.Table_Id = tableInfo.Table_Id;
                    addColumns.Add(item);
                    continue;
                }
                //修改了數據類庫或字段長度
                if (item.ColumnType != tableColumn.ColumnType || item.Maxlength != tableColumn.Maxlength || (item.IsNull ?? 0) != (tableColumn.IsNull ?? 0))
                {
                    tableColumn.ColumnType = item.ColumnType;
                    tableColumn.Maxlength = item.Maxlength;
                    tableColumn.IsNull = item.IsNull;
                    updateColumns.Add(tableColumn);
                }
            }
            //刪除的列
            List<Sys_TableColumn> delColumns = detailList.Where(a => !columns.Select(c => c.ColumnName).Contains(a.ColumnName)).ToList();
            if (addColumns.Count + delColumns.Count + updateColumns.Count == 0)
            {
                return webResponse.Error("【" + tableName + "】表結構未發生變化");
            }
            repository.AddRange(addColumns);
            repository.DbContext.Set<Sys_TableColumn>().RemoveRange(delColumns);
            repository.UpdateRange(updateColumns, x => new { x.ColumnType, x.Maxlength, x.IsNull });
            await repository.DbContext.SaveChangesAsync();

            return webResponse.OK($"新加字段【{addColumns.Count}】個,刪除字段【{delColumns.Count}】,修改字段【{updateColumns.Count}】");
        }

        /// <summary>
        /// 生成Services/Repository與對應的Partial類
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="foldername"></param>
        /// <param name="webController"></param>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public string CreateServices(string tableName, string nameSpace, string foldername, bool webController, bool apiController)
        {
            var tableColumn = repository.FindAsyncFirst<Sys_TableColumn>(x => x.TableName == tableName).Result;

            if (tableColumn == null)
            {
                return $"沒有查到{tableName}表信息";
            }

            if (string.IsNullOrEmpty(nameSpace) || string.IsNullOrEmpty(foldername))
            {
                return $"命名空間、項目文件夾都不能為空";
            }

            string domainContent = "";

            string frameworkFolder = ProjectPath.GetProjectDirectoryInfo()?.FullName;
            string[] splitArr = nameSpace.Split('.');
            string projectName = splitArr.Length > 1 ? splitArr[splitArr.Length - 1] : splitArr[0];
            string baseOptions = "\"" + projectName + "\"," + "\"" + foldername + "\"," + "\"" + tableName + "\"";

            if (apiController)
            {
                string apiPath = ProjectPath.GetProjectDirectoryInfo().GetDirectories().Where(x => x.Name.ToLower().EndsWith(".webapi")).FirstOrDefault()?.FullName;
                if (string.IsNullOrEmpty(apiPath))
                {
                    return "未找到webapi類庫,請確認是存在weiapi類庫命名以.webapi結尾";
                }
                apiPath += $"\\Controllers\\{projectName}\\";
                //生成Partial Api控制器
                if (!FileHelper.FileExists($"{apiPath}Partial\\{tableName}Controller.cs"))
                {
                    string partialController = FileHelper.ReadFile(@"Template\\Controller\\ControllerApiPartial.html")
                       .Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
                    FileHelper.WriteFile($"{apiPath}Partial\\", tableName + "Controller.cs", partialController);
                }
                //生成Api控制器
                domainContent = FileHelper.ReadFile(@"Template\\Controller\\ControllerApi.html")
                    .Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName).Replace("{BaseOptions}", baseOptions);
                FileHelper.WriteFile(apiPath, tableName + "Controller.cs", domainContent);
            }

            //生成Repository類
            domainContent = FileHelper.ReadFile("Template\\Repositorys\\BaseRepository.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
            FileHelper.WriteFile(
           frameworkFolder + string.Format("\\{0}\\Repositories\\{1}\\", nameSpace, foldername)
                          , tableName + "Repository.cs", domainContent);
            //生成IRepository類
            domainContent = FileHelper.ReadFile("Template\\IRepositorys\\BaseIRepositorie.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
            FileHelper.WriteFile(
            frameworkFolder + string.Format("\\{0}\\IRepositories\\{1}\\", nameSpace, foldername),
                   "I" + tableName + "Repository.cs", domainContent);


            string path = $"{frameworkFolder}\\{nameSpace}\\IServices\\{ foldername}\\";

            string fileName = "I" + tableName + "Service.cs";

            //生成Partial  IService類
            if (!FileHelper.FileExists(path + "Partial\\" + fileName))
            {
                domainContent = FileHelper.ReadFile("Template\\IServices\\IServiceBasePartial.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
                FileHelper.WriteFile(path + "Partial\\", fileName, domainContent);
            }

            //生成IService類
            domainContent = FileHelper.ReadFile("Template\\IServices\\IServiceBase.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
            FileHelper.WriteFile(path, fileName, domainContent);


            path = $"{frameworkFolder}\\{nameSpace}\\Services\\{ foldername}\\";
            fileName = tableName + "Service.cs";
            //生成Partial Service類
            domainContent = FileHelper.ReadFile("Template\\Services\\ServiceBasePartial.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
            if (!FileHelper.FileExists(path + "Partial\\" + fileName))
            {
                domainContent = FileHelper.ReadFile("Template\\Services\\ServiceBasePartial.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{StartName}", StratName);
                FileHelper.WriteFile(path + "Partial\\", fileName, domainContent);
            }

            //生成Service類
            domainContent = FileHelper.ReadFile("Template\\Services\\ServiceBase.html")
                .Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName)
                .Replace("{StartName}", StratName);
            FileHelper.WriteFile(path, fileName, domainContent);


            if (webController)
            {
                path = $"{frameworkFolder}\\{nameSpace}\\Controllers\\{ foldername}\\";
                fileName = tableName + "Controller.cs";
                //生成Partial web控制器
                if (!FileHelper.FileExists(path + "Partial\\" + fileName))
                {
                    domainContent = FileHelper.ReadFile("Template\\Controller\\ControllerPartial.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{BaseOptions}", baseOptions).Replace("{StartName}", StratName);
                    FileHelper.WriteFile(path + "Partial\\", tableName + "Controller.cs", domainContent);
                }

                //生成web控制器
                domainContent = FileHelper.ReadFile("Template\\Controller\\Controller.html").Replace("{Namespace}", nameSpace).Replace("{TableName}", tableName).Replace("{BaseOptions}", baseOptions).Replace("{StartName}", StratName);
                FileHelper.WriteFile(path, tableName + "Controller.cs", domainContent);
            }
            return "業務類創建成功!";
        }

        /// <summary>
        /// 獲取介面查詢字段
        /// </summary>
        /// <param name="panelHtml"></param>
        /// <param name="sysColumnList"></param>
        /// <param name="vue"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        private List<object> GetSearchData(List<List<PanelHtml>> panelHtml, List<Sys_TableColumn> sysColumnList, bool vue = false, bool edit = false, bool app = false)
        {
            if (edit)
            {
                GetPanelData(sysColumnList, panelHtml, x => x.EditRowNo, c => c.EditRowNo != null && c.EditRowNo > 0, false, q => q.EditRowNo, vue, app: app);
            }
            else
            {
                GetPanelData(sysColumnList, panelHtml, x => x.SearchRowNo, c => c.SearchRowNo != null, true, q => q.SearchRowNo, vue, app: app);
            }

            List<object> list = new List<object>();

            int index = 0;
            bool group = panelHtml.Exists(x => x.Count > 1);
            panelHtml.ForEach(x =>
            {
                index++;
                List<Dictionary<string, object>> keyValuePairs = new List<Dictionary<string, object>>();
                //List<KeyValuePair<string, object>> keyValuePairs = new List<KeyValuePair<string, object>>();
                x.ForEach(s =>
                {
                    Dictionary<string, object> keyValues = new Dictionary<string, object>();
                    if (vue)
                    {
                        //  keyValues.Add("columnType", s.columnType);
                        if (!string.IsNullOrEmpty(s.dataSource) && s.dataSource != "''")
                        {
                            if (app)
                            {
                                keyValues.Add("key", s.dataSource);
                            }
                            else
                            {
                                keyValues.Add("dataKey", s.dataSource);
                            }
                            //2020.09.11增加vue頁面數據源配置默認空數據源
                            keyValues.Add("data", new string[] { });
                        }
                        keyValues.Add("title", s.text);
                        if (s.require)
                        {
                            keyValues.Add("required", s.require);
                        }

                        keyValues.Add("field", s.id);
                        if (s.disabled)
                        {
                            keyValues.Add("disabled", true);
                        }
                        if (s.colSize > 0 && !app)
                        {
                            keyValues.Add("colSize", s.colSize);
                        }
                        if (!string.IsNullOrEmpty(s.displayType) && s.displayType != "''")
                        {
                            keyValues.Add("type", s.columnType == "img" ? s.columnType : s.displayType);
                        }
                    }
                    else
                    {
                        keyValues.Add("columnType", s.columnType);
                        if (!string.IsNullOrEmpty(s.dataSource))
                        {
                            keyValues.Add("dataSource", s.dataSource);
                        }
                        keyValues.Add("text", s.text);
                        if (s.require)
                        {
                            keyValues.Add("require", s.require);
                        }
                        keyValues.Add("id", s.id);
                    }
                    if (!app)
                    {
                        keyValuePairs.Add(keyValues);
                    }
                    else
                    {
                        list.Add(keyValues);
                    }
                });
                if (!app)
                {
                    list.Add(keyValuePairs);
                }
                else
                {
                    //app頁面添加分組
                    if (index != panelHtml.Count() && group)
                    {
                        list.Add(new { type = "group" });
                    }
                }
            });
            return list;
        }

        /// <summary>
        /// 生成Vue頁面
        /// </summary>
        /// <param name="sysTableInfo"></param>
        /// <param name="vuePath">為本地Vue項目Views所在的絕對路徑:E:/web/myProject/Views</param>
        /// <returns></returns>
        public string CreateVuePage(Sys_TableInfo sysTableInfo, string vuePath)
        {
            bool isApp = HttpContext.Current.Request.Query["app"].GetInt() > 0;
            if (string.IsNullOrEmpty(vuePath))
            {
                return isApp ? "請設置App路徑" : "請設置Vue所在Views的絕對路徑!";
            }

            if (!FileHelper.DirectoryExists(vuePath)) return $"未找項目路徑{vuePath}!";

            if (sysTableInfo == null
              || sysTableInfo.TableColumns == null
              || sysTableInfo.TableColumns.Count == 0)
                return "提交的配置數據不完整";

            vuePath = vuePath.Trim().TrimEnd('/').TrimEnd('\\');

            List<Sys_TableColumn> sysColumnList = sysTableInfo.TableColumns;
            string[] eidtTye = new string[] { "select", "selectList", "drop", "dropList", "checkbox" };
            if (sysColumnList.Exists(x => eidtTye.Contains(x.EditType) && string.IsNullOrEmpty(x.DropNo)))
            {
                return $"編輯類型為[{string.Join(',', eidtTye)}]時必須選擇數據源";
            }
            if (sysColumnList.Exists(x => eidtTye.Contains(x.SearchType) && string.IsNullOrEmpty(x.DropNo)))
            {
                return $"查詢類型為[{string.Join(',', eidtTye)}]時必須選擇數據源";
            }
            if (isApp && !sysColumnList.Exists(x => x.Enable > 0))
            {
                return $"請設置[app列]";
            }
            StringBuilder sb = GetGridColumns(sysColumnList, sysTableInfo.ExpressField, false, true, app: isApp);
            if (sb.Length == 0) return "未獲取到數據!";
            string columns = sb.ToString().Trim();
            columns = columns.Substring(0, columns.Length - 1);
            string key = sysColumnList.Where(c => c.IsKey == 1).Select(x => x.ColumnName).First() ?? "";

            //{ key: 1, value: "顯示/查詢/編輯" },
            //{ key: 2, value: "顯示/編輯" },
            //{ key: 3, value: "顯示/查詢" },
            //{ key: 4, value: "顯示" },
            //{ key: 5, value: "查詢/編輯" },
            //{ key: 6, value: "查詢" },
            //{ key: 7, value: "編輯" },
            Func<Sys_TableColumn, bool> editFunc = c => c.EditRowNo != null && c.EditRowNo > 0;
            if (isApp)
            {
                editFunc = x => new int[] { 1, 2, 5, 7 }.Any(c => c == x.Enable);
            }
            var formFileds = sysColumnList.Where(editFunc)
                .OrderBy(o => o.EditRowNo)
                .ThenByDescending(t => t.OrderNo)
                .Select(x => new KeyValuePair<string, object>(x.ColumnName, x.EditType == "checkbox" || x.EditType == "selectList" || x.EditType == "cascader" ? new string[0] : "" as object))
                .ToList().ToDictionary(x => x.Key, x => x.Value).Serialize();

            List<List<PanelHtml>> panelHtml = new List<List<PanelHtml>>();

            List<object> list = GetSearchData(panelHtml, sysColumnList, true, app: isApp);

            string pageContent = null;

            if (isApp)
            {
                pageContent = FileHelper.ReadFile("Template\\Page\\app\\options.html");
            }
            else if (HttpContext.Current.Request.Query.ContainsKey("v3"))   //2021.08.01增加vue3頁面模板
            {
                pageContent = FileHelper.ReadFile("Template\\Page\\Vue3SearchPage.html");
            }
            else
            {
                pageContent = FileHelper.ReadFile("Template\\Page\\VueSearchPage.html");
            }


            if (string.IsNullOrEmpty(pageContent))
            {
                return "未找到Template模板文件";
            }

            //{ key: 1, value: "顯示/查詢/編輯" },
            //{ key: 2, value: "顯示/編輯" },
            //{ key: 3, value: "顯示/查詢" },
            //{ key: 4, value: "顯示" },
            //{ key: 5, value: "查詢/編輯" },
            //{ key: 6, value: "查詢" },
            //{ key: 7, value: "編輯" },
            Func<Sys_TableColumn, bool> func = c => c.SearchRowNo != null && c.SearchRowNo > 0;

            if (isApp)
            {
                func = x => new int[] { 1, 3, 5, 6 }.Any(c => c == x.Enable);
            }
            var searchFormFileds = sysColumnList.Where(func)
                .Select(x => new KeyValuePair<string, object>(x.ColumnName, x.SearchType == "checkbox"
                || x.SearchType == "selectList" || x.EditType == "cascader" ? new string[0] : x.SearchType == "range" ? new string[] { null, null } : "" as object))
                .ToList().ToDictionary(x => x.Key, x => x.Value).Serialize();

            pageContent = pageContent.Replace("#searchFormFileds", searchFormFileds)
                .Replace("#searchFormOptions", list.Serialize() ?? ""
                .Replace("},{", "},\r\n                               {")
                .Replace("],[", "],\r\n                              [")
                );
            panelHtml = new List<List<PanelHtml>>();
            //編輯
            string formOptions = GetSearchData(panelHtml, sysColumnList.Where(editFunc).ToList(), true, true, app: isApp).Serialize() ?? "";

            string[] arr = sysTableInfo.Namespace.Split(".");
            string spaceFolder = (arr.Length > 1 ? arr[arr.Length - 1] : arr[0]).ToLower();

            pageContent = pageContent.Replace("#columns", columns).
                            Replace("#SortName", string.IsNullOrEmpty(sysTableInfo.SortName) ? key : sysTableInfo.SortName).
                            Replace("#key", key).
                            Replace("#Foots", " ").
                            Replace("#cnName", sysTableInfo.ColumnCNName).
                            Replace("#url", "/" + sysTableInfo.TableName + "/").
                            Replace("#folder", spaceFolder).
                            Replace("#editFormFileds", formFileds).
                            Replace("#editFormOptions", formOptions.
                            Replace("},{", "},\r\n                               {").
                            Replace("],[", "],\r\n                              ["));
            vuePath = vuePath.Replace("//", "\\").Trim('\\');



            //如果有明細，加載明細的數據
            if (!string.IsNullOrEmpty(sysTableInfo.DetailName) && !isApp)
            {
                Sys_TableInfo detailTable = repository.FindAsIQueryable(x => x.TableName == sysTableInfo.DetailName)
                    .Include(x => x.TableColumns).FirstOrDefault();
                if (detailTable == null)
                    return $"請先生成明細表{ sysTableInfo.DetailName}的配置!";
                if (detailTable.TableColumns == null || detailTable.TableColumns.Count == 0)
                    return $"明細表{ sysTableInfo.DetailName}沒有列的信息,請確認是否有列數據或列數據是否被刪除!";
                var _name = detailTable.TableColumns.Where(x => x.IsImage < 4 && x.EditRowNo > 0).Select(s => s.ColumnName).FirstOrDefault();
                //if (!string.IsNullOrEmpty(_name))
                //{
                //    return $"明細表【{_name}】字段【table顯示類型】設置為了【文件或圖片】,編輯行只能設置為0或不設置";
                //}
                //明細列數據
                List<Sys_TableColumn> detailList = detailTable.TableColumns;
                //替換明細列數據
                sb = GetGridColumns(detailList, detailTable.ExpressField, true, true);
                key = detailList.Where(c => c.IsKey == 1).Select(x => x.ColumnName).First();
                columns = sb.ToString().Trim();
                columns = columns.Substring(0, columns.Length - 1);
                pageContent = pageContent.Replace("#detailColumns", columns).
                    Replace("#detailCnName", detailTable.ColumnCNName).
                    Replace("#detailTable", detailTable.TableName).
                    Replace("#detailKey", detailTable.TableColumns.Where(c => c.IsKey == 1).Select(x => x.ColumnName).First()).
                    Replace("#detailSortName", string.IsNullOrEmpty(detailTable.SortName) ? key : detailTable.SortName);
            }
            else
            {
                pageContent = pageContent.Replace("#detailColumns", "")
                    .Replace("#detailKey", "")
                    .Replace("#detailSortName", "");
            }


            //生成擴展邏輯頁面(只創建一次)
            //獲取view的上一級目錄
            string srcPath = new DirectoryInfo(vuePath.MapPath()).Parent.FullName;
            string extensionPath = isApp ? $"{srcPath}\\pages\\{spaceFolder}\\" : $"{srcPath}\\extension\\{spaceFolder}\\";
            //  sysTableInfo.TableName = sysTableInfo.TableName.ToLower();
            string exFileName = sysTableInfo.TableName + ".js";
            string tableName = sysTableInfo.TableName;

            if (!isApp)
            {
                if (!FileHelper.FileExists(extensionPath + exFileName)
                    || FileHelper.FileExists($"{extensionPath}+\\{ sysTableInfo.FolderName.ToLower()}\\{ exFileName}"))
                {
                    //2021.03.06增加前端生成文件到指定文件夾(以前生成過的文件不受影響)
                    extensionPath = $"{srcPath}\\extension\\{spaceFolder}\\{ sysTableInfo.FolderName.ToLower()}\\";
                    spaceFolder = spaceFolder + "\\" + sysTableInfo.FolderName.ToLower();
                    tableName = sysTableInfo.FolderName.ToLower() + "/" + tableName;
                }
            }


            if (!isApp && !FileHelper.FileExists(extensionPath + exFileName))
            {

                string exContent = FileHelper.ReadFile("Template\\Page\\VueExtension.html");
                FileHelper.WriteFile(extensionPath, exFileName, exContent);
            }

            pageContent = pageContent.Replace("#TableName", tableName);

            if (isApp)
            {
                if (!FileHelper.FileExists($"{vuePath}\\{ spaceFolder}\\{sysTableInfo.TableName}\\{sysTableInfo.TableName}Extend.js"))
                {
                    //生成擴展文件
                    string pageContentEx = FileHelper.ReadFile("Template\\Page\\app\\extension.html");
                    FileHelper.WriteFile($"{vuePath}\\{ spaceFolder}\\{sysTableInfo.TableName}\\", sysTableInfo.TableName + "Extend.js", pageContentEx);
                }
                //生成app配置options.js文件
                FileHelper.WriteFile($"{vuePath}\\{ spaceFolder}\\{sysTableInfo.TableName}\\", sysTableInfo.TableName + "Options.js", pageContent);

                if (!FileHelper.FileExists($"{vuePath}\\{ spaceFolder}\\{sysTableInfo.TableName}\\{sysTableInfo.TableName}.vue"))
                {
                    //生成vue文件
                    pageContent = FileHelper.ReadFile("Template\\Page\\app\\page.html").Replace("#TableName", sysTableInfo.TableName);
                    FileHelper.WriteFile($"{vuePath}\\{ spaceFolder}\\{sysTableInfo.TableName}\\", sysTableInfo.TableName + ".vue", pageContent);
                }

                string name = FileHelper.ReadFile(@$"{srcPath}\pages.json");
                string appPath = $"pages/{spaceFolder}/{sysTableInfo.TableName}/{sysTableInfo.TableName}";
                if (!name.Contains(appPath))
                {
                    int index = name.IndexOf("]");
                    string fragment1 = name.Substring(0, index);
                    string fragment2 = name.Substring(index);

                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("		,{");
                    builder.AppendLine("			\"path\": \"" + appPath + "\",");
                    builder.AppendLine("			\"style\": {");
                    builder.AppendLine("				\"navigationBarTitleText\": \"" + sysTableInfo.ColumnCNName + "\"");
                    builder.AppendLine("			}");
                    builder.AppendLine("		}");

                    string fragment = fragment1 + builder.ToString() + "	" + fragment2;
                    FileHelper.WriteFile(srcPath, "\\pages.json", fragment);
                }
            }
            else
            {
                //   spaceFolder = spaceFolder; //+ "\\" + sysTableInfo.FolderName.ToLower();
                //生成vue頁面
                FileHelper.WriteFile($"{vuePath}\\{ spaceFolder}\\", sysTableInfo.TableName + ".vue", pageContent);

                //生成路由
                string routerPath = $"{srcPath}\\router\\viewGird.js";
                string routerContent = FileHelper.ReadFile(routerPath);
                if (!routerContent.Contains($"path: '/{sysTableInfo.TableName}'"))
                {
                    string routerTemplate = FileHelper.ReadFile("Template\\Page\\router.html")
                     .Replace("#TableName", sysTableInfo.TableName)
                     .Replace("#folder", spaceFolder.Replace("\\", "/"));
                    routerContent = routerContent.Replace("]", routerTemplate);
                    FileHelper.WriteFile($"{srcPath}\\router\\", "viewGird.js", routerContent);
                }
            }
            return "頁面創建成功!";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        public string CreateExtensionPage(Sys_TableInfo tableInfo)
        {
            return "開發中。。。";
        }

        /// <summary>
        /// 獲取Mysql表結構信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetMySqlStructure(string tableName)
        {
            return $@"SELECT  DISTINCT
                    Column_Name AS ColumnName,
                     '{ tableName}'  as tableName,
	                Column_Comment AS ColumnCnName,
                        CASE
                          WHEN data_type IN( 'BIT', 'BOOL', 'bit', 'bool') THEN
                'bool'
		             WHEN data_type in('smallint','SMALLINT') THEN 'short'
								WHEN data_type in('tinyint','TINYINT') THEN 'sbyte'
                        WHEN data_type IN('MEDIUMINT','mediumint', 'int','INT','year', 'Year') THEN
                    'int'
                    WHEN data_type in ( 'BIGINT','bigint') THEN
                    'bigint'
                    WHEN data_type IN('FLOAT', 'DOUBLE', 'DECIMAL','float', 'double', 'decimal') THEN
                    'decimal'
                    WHEN data_type IN('CHAR', 'VARCHAR', 'TINY TEXT', 'TEXT', 'MEDIUMTEXT', 'LONGTEXT', 'TINYBLOB', 'BLOB', 'MEDIUMBLOB', 'LONGBLOB', 'Time','char', 'varchar', 'tiny text', 'text', 'mediumtext', 'longtext', 'tinyblob', 'blob', 'mediumblob', 'longblob', 'time') THEN
                    'string'
                    WHEN data_type IN('Date', 'DateTime', 'TimeStamp','date', 'datetime', 'timestamp') THEN
                    'DateTime' ELSE 'string'
                END AS ColumnType,
	              case WHEN CHARACTER_MAXIMUM_LENGTH>8000 THEN 0 ELSE CHARACTER_MAXIMUM_LENGTH end  AS Maxlength,
            CASE
                    WHEN COLUMN_KEY <> '' THEN  
                    1 ELSE 0
                END AS IsKey,
            CASE
                    WHEN Column_Name IN( 'CreateID', 'ModifyID', '' ) 
		            OR COLUMN_KEY<> '' THEN
                        0 ELSE 1
                        END AS IsDisplay,
		            1 AS IsColumnData,
                    120 AS ColumnWidth,
                    0 AS OrderNo,
                CASE
                        WHEN IS_NULLABLE = 'NO' THEN
                        0 ELSE 1
                    END AS IsNull,
	            CASE
                        WHEN COLUMN_KEY <> '' THEN
                        1 ELSE 0
                    END AS IsReadDataset,
                ordinal_position
                FROM
                    information_schema.COLUMNS
                WHERE
                    table_name = ?tableName {GetMysqlTableSchema()}
               order by ordinal_position";
        }

        /// <summary>
        /// 獲取SqlServer表結構信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetSqlServerStructure(string tableName)
        {
            return $@"
            SELECT TableName,
                LTRIM(RTRIM(ColumnName)) AS ColumnName,
                ColumnCNName,
                CASE WHEN ColumnType = 'uniqueidentifier' THEN 'guid'
                     WHEN ColumnType IN('smallint', 'INT') THEN 'int'
                     WHEN ColumnType = 'BIGINT' THEN 'long'
                     WHEN ColumnType IN('CHAR', 'VARCHAR', 'NVARCHAR',
                                          'text', 'xml', 'varbinary', 'image')
                     THEN 'string'
                     WHEN ColumnType IN('tinyint')
                     THEN 'byte'

                       WHEN ColumnType IN('bit') THEN 'bool'
                     WHEN ColumnType IN('time', 'date', 'DATETIME', 'smallDATETIME')
                     THEN 'DateTime'
                     WHEN ColumnType IN('smallmoney', 'DECIMAL', 'numeric',
                                          'money') THEN 'decimal'
                     WHEN ColumnType = 'float' THEN 'float'
                     ELSE 'string '
                END ColumnType,
                CASE WHEN   ColumnType IN ('NVARCHAR','NCHAR') THEN [Maxlength]/2 ELSE [Maxlength] END  [Maxlength],
                IsKey,
                CASE WHEN ColumnName IN('CreateID', 'ModifyID', '')
                          OR IsKey = 1 THEN 0
                     ELSE 1
                END AS IsDisplay ,
				1 AS IsColumnData,

              CASE   WHEN ColumnType IN('time', 'date', 'DATETIME', 'smallDATETIME') THEN 150

                     WHEN ColumnName IN('Modifier', 'Creator') THEN 130

                     WHEN ColumnType IN('int', 'bigint') OR ColumnName IN('CreateID', 'ModifyID', '') THEN 80
                     WHEN[Maxlength] < 110 AND[Maxlength] > 60 THEN 120

                     WHEN[Maxlength] < 200 AND[Maxlength] >= 110 THEN 180

                     WHEN[Maxlength] > 200 THEN 220
                     ELSE 110
                   END AS ColumnWidth ,
                0 AS OrderNo,
                --CASE WHEN IsKey = 1 OR t.[IsNull]=0 THEN 0
                --     ELSE 1 END
                t.[IsNull] AS
                 [IsNull],
            CASE WHEN IsKey = 1 THEN 1 ELSE 0 END IsReadDataset,
            CASE WHEN IsKey!=1 AND t.[IsNull] = 0 THEN 0 ELSE NULL END AS EditColNo
        FROM    (SELECT obj.name AS TableName ,
                            col.name AS ColumnName ,
                            CONVERT(NVARCHAR(100),ISNULL(ep.[value], '')) AS ColumnCNName,
                            t.name AS ColumnType ,
                           CASE WHEN  col.length<1 THEN 0 ELSE  col.length END  AS[Maxlength],
                            CASE WHEN EXISTS (SELECT   1
                                               FROM dbo.sysindexes si
                                                        INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id
                                                              AND si.indid = sik.indid
                                                        INNER JOIN dbo.syscolumns sc ON sc.id = sik.id
                                                              AND sc.colid = sik.colid
                                                        INNER JOIN dbo.sysobjects so ON so.name = si.name
                                                              AND so.xtype = 'PK'
                                               WHERE sc.id = col.id
                                                        AND sc.colid = col.colid)
                                 THEN 1
                                 ELSE 0
                            END AS IsKey ,
                            CASE WHEN col.isnullable = 1 THEN 1
                                 ELSE 0
                            END AS[IsNull],
                            col.colorder
                  FROM      dbo.syscolumns col
                            LEFT JOIN dbo.systypes t ON col.xtype = t.xusertype
                           INNER JOIN dbo.sysobjects obj ON col.id = obj.id

                                                            AND obj.xtype IN ( 'U','V')
                                                          --   AND obj.status >= 01
                            LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
                            LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id
                                                              AND col.colid = ep.minor_id
                                                              AND ep.name = 'MS_Description'
                            LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id
                                                              AND epTwo.minor_id = 0
                                                              AND epTwo.name = 'MS_Description'
                  WHERE obj.name = @tableName--表名
                ) AS t
            ORDER BY t.colorder";
        }

        /// <summary>
        /// 2020.08.07完善PGSQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetPgSqlStructure(string tableName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            stringBuilder.Append("	MM.\"TableName\", ");
            stringBuilder.Append("	MM.\"ColumnName\", ");
            stringBuilder.Append(" 	MM.\"ColumnCNName\", ");
            stringBuilder.Append("	MM.\"ColumnType\", ");
            stringBuilder.Append("	MM.\"Maxlength\", ");
            stringBuilder.Append("	MM.\"IsKey\", ");
            stringBuilder.Append("	MM.\"IsDisplay\", ");
            stringBuilder.Append("	MM.\"IsColumnData\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("		 ");
            stringBuilder.Append("		WHEN MM.\"ColumnType\" = 'DateTime' THEN ");
            stringBuilder.Append("		150  ");
            stringBuilder.Append("		WHEN MM.\"ColumnType\" = 'int' THEN ");
            stringBuilder.Append("		80  ");
            stringBuilder.Append("		WHEN MM.\"Maxlength\" < 110  ");
            stringBuilder.Append("		AND MM.\"Maxlength\" > 60 THEN ");
            stringBuilder.Append("			120  ");
            stringBuilder.Append("			WHEN MM.\"Maxlength\" < 200  ");
            stringBuilder.Append("			AND MM.\"Maxlength\" >= 110 THEN ");
            stringBuilder.Append("				180  ");
            stringBuilder.Append("				WHEN MM.\"Maxlength\" > 200 THEN ");
            stringBuilder.Append("				220 ELSE 110  ");
            stringBuilder.Append("			END AS \"ColumnWidth\", ");
            stringBuilder.Append("			MM.\"OrderNo\", ");
            stringBuilder.Append("		 case WHEN MM.\"IsKey\"=1 or \"lower\"(MM.\"IsNull\")='no' then 0 else 1 end as 	\"IsNull\" , ");
            stringBuilder.Append("			MM.\"IsReadDataset\", ");
            stringBuilder.Append("			MM.\"EditColNo\"  ");
            stringBuilder.Append("		FROM ");
            stringBuilder.Append("			( ");
            stringBuilder.Append("			SELECT ");
            stringBuilder.Append("				col.TABLE_NAME AS \"TableName\", ");
            stringBuilder.Append("				col.COLUMN_NAME AS \"ColumnName\", ");
            stringBuilder.Append("				attr.description AS \"ColumnCNName\", ");
            stringBuilder.Append("			CASE ");
            stringBuilder.Append("					 ");
            stringBuilder.Append("					WHEN col.udt_name = 'uuid' THEN ");
            stringBuilder.Append("					'guid'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'int2') THEN ");
            stringBuilder.Append("					'short'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'int4' ) THEN ");
            stringBuilder.Append("					'int'  ");
            stringBuilder.Append("					WHEN col.udt_name = 'int8' THEN ");
            stringBuilder.Append("					'long'  ");
            stringBuilder.Append("					WHEN col.udt_name = 'BIGINT' THEN ");
            stringBuilder.Append("					'long'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'char', 'varchar', 'text', 'xml', 'bytea' ) THEN ");
            stringBuilder.Append("					'string'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'bool' ) THEN ");
            stringBuilder.Append("					'bool'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'date','timestamp' ) THEN ");
            stringBuilder.Append("					'DateTime'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'decimal', 'money','numeric' ) THEN ");
            stringBuilder.Append("					'decimal'  ");
            stringBuilder.Append("					WHEN col.udt_name IN ( 'float4', 'float8' ) THEN ");
            stringBuilder.Append("					'float' ELSE'string '  ");
            stringBuilder.Append("				END \"ColumnType\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("	 ");
            stringBuilder.Append("	WHEN col.udt_name = 'varchar' THEN ");
            stringBuilder.Append("	col.character_maximum_length  ");
            stringBuilder.Append("	WHEN col.udt_name IN ( 'int2', 'int4', 'int8', 'float4', 'float8' ) THEN ");
            stringBuilder.Append("	col.numeric_precision ELSE 1024  ");
            stringBuilder.Append("	END \"Maxlength\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("	 ");
            stringBuilder.Append("	WHEN keyTable.IsKey = 1 THEN ");
            stringBuilder.Append("	1 ELSE 0  ");
            stringBuilder.Append("	END \"IsKey\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("	 ");
            stringBuilder.Append("	WHEN keyTable.IsKey = 1 THEN ");
            stringBuilder.Append("	0 ELSE 1  ");
            stringBuilder.Append("	END \"IsDisplay\", ");
            stringBuilder.Append("	1 AS \"IsColumnData\", ");
            stringBuilder.Append("	0 AS \"OrderNo\", ");
            stringBuilder.Append("	col.is_nullable AS \"IsNull\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("		 ");
            stringBuilder.Append("		WHEN keyTable.IsKey = 1 THEN ");
            stringBuilder.Append("		1 ELSE 0  ");
            stringBuilder.Append("	END \"IsReadDataset\", ");
            stringBuilder.Append("CASE ");
            stringBuilder.Append("	 ");
            stringBuilder.Append("	WHEN keyTable.IsKey IS NULL  ");
            stringBuilder.Append("	AND col.is_nullable = 'NO' THEN ");
            stringBuilder.Append("	0 ELSE NULL  ");
            stringBuilder.Append("	END AS \"EditColNo\"  ");
            stringBuilder.Append("FROM ");
            stringBuilder.Append("	information_schema.COLUMNS col  ");
            stringBuilder.Append("  LEFT JOIN ( ");
            stringBuilder.Append("	SELECT col_description(a.attrelid,a.attnum) as description,a.attname as name ");
            stringBuilder.Append("FROM pg_class as c,pg_attribute as a  ");
            stringBuilder.Append("where \"lower\"(c.relname) = \"lower\"(@tableName) and a.attrelid = c.oid and a.attnum>0 ");
            stringBuilder.Append("	) as attr on attr.name=col.COLUMN_NAME ");
            stringBuilder.Append("	LEFT JOIN ( ");
            stringBuilder.Append("	SELECT ");
            stringBuilder.Append("		pg_attribute.attname AS colname, ");
            stringBuilder.Append("		1 AS IsKey  ");
            stringBuilder.Append("	FROM ");
            stringBuilder.Append("		pg_constraint ");
            stringBuilder.Append("		INNER JOIN pg_class ON pg_constraint.conrelid = pg_class.oid ");
            stringBuilder.Append("		INNER JOIN pg_attribute ON pg_attribute.attrelid = pg_class.oid  ");
            stringBuilder.Append("		AND pg_attribute.attnum = pg_constraint.conkey [1]  ");
            stringBuilder.Append("	WHERE ");
            stringBuilder.Append("		\"lower\" ( pg_class.relname ) = \"lower\" ( @tableName )  ");
            stringBuilder.Append("		AND pg_constraint.contype = 'p'  ");
            stringBuilder.Append("	) keyTable ON col.COLUMN_NAME = keyTable.colname  ");
            stringBuilder.Append("WHERE ");
            stringBuilder.Append("	\"lower\" ( TABLE_NAME ) = \"lower\" ( @tableName )  ");
            stringBuilder.Append("ORDER BY ");
            stringBuilder.Append("	ordinal_position  ");
            stringBuilder.Append("	) MM; ");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 設置介面table td單元格的寬度
        /// </summary>
        /// <param name="columns"></param>
        private void SetMaxLength(List<Sys_TableColumn> columns)
        {
            columns.ForEach(x =>
            {
                if (x.ColumnType.ToLower() == "datetime")
                {
                    x.ColumnWidth = 150;
                }
                else if (x.ColumnName.ToLower() == "modifier" || x.ColumnName.ToLower() == "creator")
                {
                    x.ColumnWidth = 100;
                }
                else if (x.ColumnName.ToLower() == "modifyid" || x.ColumnName.ToLower() == "createid")
                {
                    x.ColumnWidth = 100;
                }
                else if (x.Maxlength > 200)
                {
                    x.ColumnWidth = 220;
                }
                else if (x.Maxlength > 110 && x.Maxlength <= 200)
                {
                    x.ColumnWidth = 180;
                }
                else if (x.Maxlength > 60 && x.Maxlength <= 110)
                {
                    x.ColumnWidth = 120;
                }
                else
                {
                    x.ColumnWidth = 110;
                }
            });
        }

        /// <summary>
        /// 初始化生成配置對應表的數據信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="tableName"></param>
        /// <param name="columnCNName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="foldername"></param>
        /// <param name="tableId"></param>
        /// <param name="isTreeLoad"></param>
        /// <returns></returns>
        private int InitTable(int parentId, string tableName, string columnCNName, string nameSpace, string foldername, int tableId, bool isTreeLoad)
        {
            if (isTreeLoad)
                return tableId;
            if (string.IsNullOrEmpty(tableName))
                return -1;
            tableId = repository.Find(x => x.TableName == tableName, s => s.Table_Id).FirstOrDefault();
            if (tableId > 0)
                return tableId;
            bool isMySql = DBType.Name == DbCurrentType.MySql.ToString();
            Sys_TableInfo tableInfo = new Sys_TableInfo()
            {
                ParentId = parentId,
                ColumnCNName = columnCNName,
                CnName = columnCNName,
                TableName = tableName,
                Namespace = nameSpace,
                FolderName = foldername,
                Enable = 1
            };
            List<Sys_TableColumn> columns = repository.DapperContext
                .QueryList<Sys_TableColumn>(GetCurrentSql(tableName), new { tableName });

            int orderNo = (columns.Count + 10) * 50;
            for (int i = 0; i < columns.Count; i++)
            {
                columns[i].OrderNo = orderNo;
                orderNo = orderNo - 50;
                columns[i].EditRowNo = 0;
            }

            SetMaxLength(columns);
            var result = base.Add<Sys_TableColumn>(tableInfo, columns, false);
            if (!result.Status)
            {
                throw new Exception($"加載表結構寫入異常：{result.Message}");
            }
            return tableInfo.Table_Id;
        }

        /// <summary>
        /// 介面加載表的配置信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="tableName"></param>
        /// <param name="columnCNName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="foldername"></param>
        /// <param name="tableId"></param>
        /// <param name="isTreeLoad">true只加載表數據</param>
        /// <returns></returns>
        public object LoadTable(int parentId, string tableName, string columnCNName, string nameSpace, string foldername, int tableId, bool isTreeLoad)
        {
            if (!UserContext.Current.IsSuperAdmin && !isTreeLoad)
            {
                return new WebResponseContent().Error("只有超級管理員才能進行此操作");
            }
            tableId = InitTable(parentId, tableName?.Trim(), columnCNName, nameSpace, foldername, tableId, isTreeLoad);
            Sys_TableInfo tableInfo = repository
                .FindAsIQueryable(x => x.Table_Id == tableId)
                .Include(c => c.TableColumns)
                .FirstOrDefault();
            if (tableInfo.TableColumns != null)
            {
                tableInfo.TableColumns = tableInfo.TableColumns.OrderByDescending(x => x.OrderNo).ToList();
            }
            return new WebResponseContent().OK(null, tableInfo);
        }

        public async Task<WebResponseContent> DelTree(int table_Id)
        {
            if (table_Id == 0) return new WebResponseContent().Error("沒有傳入參數");
            Sys_TableInfo tableInfo = await repository.FindAsIQueryable(x => x.Table_Id == table_Id)
                .Include(c => c.TableColumns)
                .FirstOrDefaultAsync();
            if (tableInfo == null) return new WebResponseContent().OK();
            if (tableInfo.TableColumns != null && tableInfo.TableColumns.Count > 0)
            {
                return new WebResponseContent().Error("當前刪除的節點存在表結構信息,只能刪除空節點");
            }
            if (repository.Exists(x => x.ParentId == table_Id))
            {
                return new WebResponseContent().Error("當前刪除的節點存在子節點，不能刪除");
            }
            repository.Delete(tableInfo, true);
            return new WebResponseContent().OK();
        }

        /// <summary>
        /// 生成表格的columns的配置信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expressField"></param>
        /// <param name="detail"></param>
        /// <param name="vue"></param>
        /// <returns></returns>
        private StringBuilder GetGridColumns(List<Sys_TableColumn> list, string expressField, bool detail, bool vue = false, bool app = false)
        {
            totalCol = 0;
            totalWidth = 0;
            StringBuilder sb = new StringBuilder();

            //{ key: 1, value: "顯示/查詢/編輯" },
            //{ key: 2, value: "顯示/編輯" },
            //{ key: 3, value: "顯示/查詢" },
            //{ key: 4, value: "顯示" },
            //{ key: 5, value: "查詢/編輯" },
            //{ key: 6, value: "查詢" },
            //{ key: 7, value: "編輯" },
            Func<Sys_TableColumn, bool> func = x => true;
            bool sort = false;
            if (app)
            {
                func = x => new int[] { 1, 2, 3, 4 }.Any(c => c == x.Enable) && (x.IsDisplay == null || x.IsDisplay == 1);
            }
            foreach (Sys_TableColumn item in list.Where(func).OrderByDescending(x => x.OrderNo))
            {
                if (item.IsColumnData == 0) { continue; }
                sb.Append("{field:'" + item.ColumnName + "',");
                sb.Append("title:'" + (string.IsNullOrEmpty(item.ColumnCnName) ? item.ColumnName : item.ColumnCnName) + "',");
                if (vue)
                {
                    string colType = item.ColumnType.ToLower();
                    if (item.IsImage == 1)
                    {
                        colType = "img";
                    }
                    else if (item.IsImage == 2)
                    {
                        colType = "excel";
                    }
                    else if (item.IsImage == 3)
                    {
                        colType = "file";
                    }
                    //2021.07.27增加table列顯示類型date(自動格式化)
                    else if (item.IsImage == 4)
                    {
                        colType = "date";
                    }
                    sb.Append("type:'" + colType + "',");
                    if (!string.IsNullOrEmpty(item.DropNo))
                    {
                        sb.Append("bind:{ key:'" + item.DropNo + "',data:[]},");
                    }
                    if (expressField != null && expressField.ToLower() == item.ColumnName.ToLower())
                    {
                        sb.Append("link:true,");
                    }
                    //2021.01.09增加代碼生成器設置table排序功能
                    if (item.Sortable == 1 && !app)
                    {
                        sb.Append("sort:true,");
                    }
                }
                else
                {
                    sb.Append("datatype:'" + item.ColumnType + "',");
                }

                if (!app)
                {
                    sb.Append("width:" + (item.ColumnWidth ?? 90) + ",");
                }
                if (item.IsDisplay == 0)
                {
                    sb.Append("hidden:true,");
                }
                else
                {
                    totalCol++;
                    totalWidth += item.ColumnWidth == null ? 0 : Convert.ToInt32(item.ColumnWidth);
                }
                if (item.IsReadDataset == 1)
                {
                    sb.Append("readonly:true,");
                }
                //detail明細才啟用表格編輯
                if (item.EditRowNo != null && item.EditRowNo > 0 && detail)//!string.IsNullOrEmpty(item.EditType))
                {
                    string editText = vue ? "edit" : "editor";
                    if (vue)
                    {
                        sb.Append("edit:{type:'" + item.EditType + "'},");
                    }
                    else
                    {

                        switch (item.EditType)
                        {
                            case "date":
                                sb.Append("editor:'datebox',");
                                break;
                            case "datetime":
                                sb.Append("editor:'datetimebox',");
                                break;
                            case "drop":
                            case "dropList":
                            case "select":
                            case "selectList":
                                if (!vue && !string.IsNullOrEmpty(item.DropNo))
                                {
                                    sb.Append(editText + ": { type: 'combobox', options: optionConfig" + item.DropNo + " },");
                                }
                                else
                                {
                                    sb.Append(editText + ": 'text',");
                                }
                                break;
                            default:
                                sb.Append(editText + ":'text',");
                                break;
                        }
                    }
                }
                if (!vue)
                {
                    //快速查看字段
                    if (expressField != null && expressField.ToLower() == item.ColumnName.ToLower())
                    {
                        sb.Append("formatter:function (val, row, index) { return $.fn.layOut('createViewField',{row:row,val:val,index:index})},");
                    }
                    else if (!string.IsNullOrEmpty(item.Script))
                    {
                        sb.Append("formatter:" + item.Script + ",");
                    }
                    else if (item.IsImage == 1)//啟用圖片
                    {
                        sb.Append("formatter:function (val, row, index) { return $.fn.layOut('createImageUrl',{row:row,key:'" + item.ColumnName + "'})},");
                    }
                    else if (!string.IsNullOrEmpty(item.DropNo) && !detail)
                    {
                        sb.AppendLine("formatter: function (val, row, index) {");
                        sb.AppendLine(string.Format("    return dataSource{0}.textFormatter(optionConfig{0}, val, row, index);", item.DropNo));
                        sb.AppendLine("    },");
                    }
                }

                if (item.IsNull == 0 && !app)
                {
                    sb.Append("require:true,");
                }

                if (!app && (item.ColumnType.ToLower() == "datetime" || (item.IsDisplay == 1 & !sort)))
                {
                    //2021.09.05修改排序名稱
                    sb.Append("align:'left',sort:true},");
                    if (item.IsDisplay == 1)
                    {
                        sort = true;
                    }
                }
                else
                {
                    if (!app)
                    {
                        sb.Append("align:'left'},");
                    }
                }
                if (app)
                {
                    sb.Append("},").Replace(",},", "},");
                }

                sb.AppendLine();
                sb.Append("                       ");
            }
            return sb;
        }

        /// <summary>
        /// 創建實體類
        /// </summary>
        /// <param name="sysColumn"></param>
        /// <param name="tableInfo"></param>
        /// <param name="createType">1、創建實體類,2創建apiinput類,3、創建apioutput類</param>
        private string CreateEntityModel(List<Sys_TableColumn> sysColumn, Sys_TableInfo tableInfo, List<TableColumnInfo> tableColumnInfoList, int createType)
        {
            string template = "";
            if (createType == 1)
            {
                template = "DomainModel.html";
            }
            else if (createType == 2)
            {
                template = "ApiInputDomainModel.html";
            }
            else
            {
                template = "ApiOutputDomainModel.html";
            }
            string domainContent = FileHelper.ReadFile("Template\\DomianModel\\" + template);
            string partialContent = domainContent;
            StringBuilder AttributeBuilder = new StringBuilder();
            sysColumn = sysColumn.OrderByDescending(c => c.OrderNo).ToList();
            bool addIgnore = false;
            foreach (Sys_TableColumn column in sysColumn)
            {
                column.ColumnType = (column.ColumnType ?? "").Trim();
                AttributeBuilder.Append("/// <summary>");
                AttributeBuilder.Append("\r\n");
                AttributeBuilder.Append("       ///" + column.ColumnCnName + "");
                AttributeBuilder.Append("\r\n");
                AttributeBuilder.Append("       /// </summary>");
                AttributeBuilder.Append("\r\n");
                if (column.IsKey == 1) { AttributeBuilder.Append(@"       [Key]" + ""); AttributeBuilder.Append("\r\n"); }
                AttributeBuilder.Append("       [Display(Name =\"" + (
                    string.IsNullOrEmpty(column.ColumnCnName) ? column.ColumnName : column.ColumnCnName
                    ) + "\")]");
                AttributeBuilder.Append("\r\n");

                TableColumnInfo tableColumnInfo = tableColumnInfoList.Where(x => x.ColumnName.ToLower().Trim() == column.ColumnName.ToLower().Trim()).FirstOrDefault();
                if (tableColumnInfo != null && (tableColumnInfo.ColumnType == "varchar" && column.Maxlength > 8000)
                             || (tableColumnInfo.ColumnType == "nvarchar" && column.Maxlength > 4000))
                {
                    column.Maxlength = 0;
                }

                if (column.ColumnType == "string" && column.Maxlength > 0 && column.Maxlength < 8000)
                {

                    AttributeBuilder.Append("       [MaxLength(" + column.Maxlength + ")]");
                    AttributeBuilder.Append("\r\n");
                }
                //不是數據列的，返回頁面數據前不包含此列的數據
                if (column.IsColumnData == 0 && createType == 1)
                {
                    addIgnore = true;
                    AttributeBuilder.Append("       [JsonIgnore]");
                    AttributeBuilder.Append("\r\n");
                }
                //[Column(TypeName="bigint")]如果與字段類型不同會產生異常

                if (tableColumnInfo != null)
                {
                    if (!string.IsNullOrEmpty(tableColumnInfo.Prec_Scale) && !tableColumnInfo.Prec_Scale.EndsWith(",0"))
                    {
                        AttributeBuilder.Append("       [DisplayFormat(DataFormatString=\"" + tableColumnInfo.Prec_Scale + "\")]");
                        AttributeBuilder.Append("\r\n");
                    }
              
                    if ((column.IsKey == 1 && (column.ColumnType == "uniqueidentifier")) ||
                        tableColumnInfo.ColumnType.ToLower() == "guid"
                        || (IsMysql() && column.ColumnType == "string" && column.Maxlength == 36))
                    {
                        tableColumnInfo.ColumnType = "uniqueidentifier";
                    }


                    string maxLength = string.Empty;
                    if (tableColumnInfo.ColumnType != "uniqueidentifier")
                    {
                        if (column.IsKey != 1 && column.ColumnType.ToLower() == "string")
                        {
                            //沒有指定長度的字符串字段 ，如varchar,nvarchar，text等都默認生成varchar(max),nvarchar(max)
                            if (column.Maxlength <= 0
                                || (tableColumnInfo.ColumnType == "varchar" && column.Maxlength > 8000)
                                || (tableColumnInfo.ColumnType == "nvarchar" && column.Maxlength > 4000))
                            {
                                maxLength = "(max)";
                            }
                            else
                            {
                                maxLength = "(" + column.Maxlength + ")";
                            }
                        }
                         else if (column.IsKey == 1 && column.ColumnType.ToLower() == "string" && column.Maxlength!=36)
                        {
                            maxLength = "(" + column.Maxlength + ")";
                        }
                    }
                    AttributeBuilder.Append("       [Column(TypeName=\"" + tableColumnInfo.ColumnType + maxLength + "\")]");
                    AttributeBuilder.Append("\r\n");


                    //if ((tableColumnInfo.ColumnType == "int" || tableColumnInfo.ColumnType == "bigint" || tableColumnInfo.ColumnType == "long") && column.ColumnType.ToLower() == "string")
                    if (tableColumnInfo.ColumnType == "int" || tableColumnInfo.ColumnType == "bigint" || tableColumnInfo.ColumnType == "long")
                    {
                        column.ColumnType = tableColumnInfo.ColumnType == "int" ? "int" : "long";
                    }
                    if (tableColumnInfo.ColumnType == "bool")
                    {
                        column.ColumnType = "bit";
                    }
                }

                if (column.EditRowNo != null)
                {
                    AttributeBuilder.Append("       [Editable(true)]");
                    AttributeBuilder.Append("\r\n");
                }
                // && column.ColumnType.ToLower() == "string"
                if (column.IsNull == 0 || (createType == 2 && column.ApiIsNull == 0))
                {
                    AttributeBuilder.Append("       [Required(AllowEmptyStrings=false)]");
                    AttributeBuilder.Append("\r\n");
                }
                string columnType = (column.ColumnType == "Date" ? "DateTime" : column.ColumnType).Trim();
                if (tableColumnInfo?.ColumnType?.ToLower() == "guid")
                {
                    columnType = "Guid";
                }
                if (column.ColumnType.ToLower() != "string" && column.IsNull == 1)
                {
                    columnType = columnType + "?";
                }
                //如果主鍵是string,則默認為是Guid或者使用的是mysql數據，字段類型是字符串並且長度是36則默認為是Guid
                if ((column.IsKey == 1
                    && (column.ColumnType == "uniqueidentifier"))
                       || column.ColumnType == "guid"
                   || (IsMysql() && column.ColumnType == "string" && column.Maxlength == 36))
                {
                    columnType = "Guid" + (column.IsNull == 1 ? "?" : "");
                }
                AttributeBuilder.Append("       public " + columnType + " " + column.ColumnName + " { get; set; }");
                AttributeBuilder.Append("\r\n\r\n       ");
            }
            if (!string.IsNullOrEmpty(tableInfo.DetailName) && createType == 1)
            {
                AttributeBuilder.Append("[Display(Name =\"" + tableInfo.DetailCnName + "\")]");
                AttributeBuilder.Append("\r\n       ");
                //2019.12.20增加明細表屬性的ForeignKey配置(EF Core 3.1配項)
                AttributeBuilder.Append("[ForeignKey(\"" + sysColumn.Where(x => x.IsKey == 1).FirstOrDefault().ColumnName + "\")]");
                AttributeBuilder.Append("\r\n       ");
                AttributeBuilder.Append("public List<" + tableInfo.DetailName + "> " + tableInfo.DetailName + " { get; set; }");
                AttributeBuilder.Append("\r\n");
            }
            if (addIgnore && createType == 1)
            {
                domainContent = "using Newtonsoft.Json;\r\n" + domainContent + "\r\n";
            }
            //獲取的是本地開發代碼所在目錄，不是布後的目錄
            string mapPath = ProjectPath.GetProjectDirectoryInfo()?.FullName; //new DirectoryInfo(("~/").MapPath()).Parent.FullName;
                                                                              //  string folderPath= string.Format("\\VOL.Framework.Core.\\DomainModels\\{0}\\", foldername);
            if (string.IsNullOrEmpty(mapPath))
            {
                return "未找到生成的目錄!";
            }
            string[] splitArrr = tableInfo.Namespace.Split('.');
            //    foldername = splitArrr.Length == 2 ? splitArrr[1] : foldername;
            domainContent = domainContent.Replace("{TableName}", tableInfo.TableName).Replace("{AttributeList}", AttributeBuilder.ToString()).Replace("{StartName}", StratName);
            //  {AttributeManager}

            List<string> entityAttribute = new List<string>();
            entityAttribute.Add("TableCnName = \"" + tableInfo.ColumnCNName + "\"");
            if (!string.IsNullOrEmpty(tableInfo.TableTrueName))
            {
                //如果使用的是pgsql數據庫，並且數據庫表都是小寫，請將下面的.ToLower()這段開啟.2020.08.07
                //entityAttribute.Add("TableName = \"" + tableInfo.TableTrueName.ToLower() + "\"");
                entityAttribute.Add("TableName = \"" + tableInfo.TableTrueName + "\"");
            }
            if (!string.IsNullOrEmpty(tableInfo.DetailName) && createType == 1)
            {
                //  'typeof('+[1,2].join('),typeof(')+')'
                string typeArr = " new Type[] { typeof(" + string.Join("),typeof(", tableInfo.DetailName.Split(',')) + ")}";
                entityAttribute.Add("DetailTable = " + typeArr + "");
            }
            if (!string.IsNullOrEmpty(tableInfo.DetailCnName))
            {

                entityAttribute.Add("DetailTableCnName = \"" + tableInfo.DetailCnName + "\"");
            }
            if (!string.IsNullOrEmpty(tableInfo.DBServer) && createType == 1)
            {
                entityAttribute.Add("DBServer = \"" + tableInfo.DBServer + "\"");
            }

            if (createType == 1)
            {
                if (sysColumn.Any(x => x.ApiInPut > 0))
                {
                    entityAttribute.Add("ApiInput = typeof(Api" + tableInfo.TableName + "Input)");
                }
                if (sysColumn.Any(x => x.ApiOutPut > 0))
                {
                    entityAttribute.Add("ApiOutput = typeof(Api" + tableInfo.TableName + "Output)");
                }
            }
            string modelNameSpace = StratName + ".Entity";
            string tableAttr = string.Join(",", entityAttribute);
            if (tableAttr != "")
            {
                tableAttr = "[Entity(" + tableAttr + ")]";
            }
            //entityAttribute.Add("TableCnName = \"" + tableInfo.ColumnCNName + "\"");
            //if (!string.IsNullOrEmpty(tableInfo.TableTrueName) && tableInfo.TableName != tableInfo.TableTrueName)
            //{
            //    entityAttribute.Add("TableName = \"" + tableInfo.TableTrueName + "\"");
            //    entityAttribute.Add("Table(\"" + tableInfo.TableTrueName + "\")");
            //}
            if (!string.IsNullOrEmpty(tableInfo.TableTrueName) && tableInfo.TableName != tableInfo.TableTrueName)
            {
                string tableTrueName = tableInfo.TableTrueName;
                //2020.06.14 pgsql數據庫，設置表名為小寫(數據庫創建表的時候也要使用小寫)
                if (DBType.Name == DbCurrentType.PgSql.ToString())
                {
                    tableTrueName = tableTrueName.ToLower();
                }
                tableAttr = tableAttr + "\r\n[Table(\"" + tableInfo.TableTrueName + "\")]";
            }
            domainContent = domainContent.Replace("{AttributeManager}", tableAttr).Replace("{Namespace}", modelNameSpace);

            string folderName = tableInfo.FolderName;
            string tableName = tableInfo.TableName;
            if (createType == 2)
            {
                folderName = "ApiEntity\\Input";
                tableName = "Api" + tableInfo.TableName + "Input";
            }
            else if (createType == 3)
            {
                folderName = "ApiEntity\\OutPut";
                tableName = "Api" + tableInfo.TableName + "Output";
            }
            //mapPath +
            //  string.Format(
            //  "\\" + modelNameSpace + "\\DomainModels\\{0}\\", folderName
            //  )
            string modelPath = $"{mapPath}\\{modelNameSpace}\\DomainModels\\{folderName}\\";
            FileHelper.WriteFile(modelPath, tableName + ".cs", domainContent);
            //partialContent
            modelPath += "partial\\";
            if (!FileHelper.FileExists(modelPath + tableName + ".cs"))
            {
                partialContent = partialContent.Replace("{AttributeManager}", "")
                    .Replace("{AttributeList}", @"//此處配置字段(字段配置見此model的另一個partial),如果表中沒有此字段請加上 [NotMapped]屬性，否則會異常")
                    .Replace(":BaseEntity", "")
                    .Replace("{TableName}", tableInfo.TableName).Replace("{Namespace}", modelNameSpace);
                FileHelper.WriteFile(modelPath, tableName + ".cs", partialContent);
            }
            if (createType == 1)
            {
                string mappingConfiguration = FileHelper.
              ReadFile("Template\\DomianModel\\MappingConfiguration.html")
              .Replace("{TableName}", tableInfo.TableName).Replace("{Namespace}", modelNameSpace).Replace("{StartName}", StratName);
                FileHelper.WriteFile(
                    mapPath +
                    string.Format("\\" + modelNameSpace + "\\MappingConfiguration\\{0}\\"
                    , tableInfo.FolderName)
                    , tableInfo.TableName + "MapConfig.cs",
                    mappingConfiguration);
            }
            return "";
        }

        private static string[] formType = new string[] { "bigint", "int", "decimal", "float", "byte" };
        private string GetDisplayType(bool search, string searchType, string editType, string columnType)
        {
            string type = "";
            if (search)
            {
                type = searchType == "無" ? "" : searchType ?? "";
            }
            else
            {
                type = editType == "無" ? "" : editType ?? "";
            }
            if (type == "" && formType.Contains(columnType))
            {
                if (columnType == "decimal" || columnType == "float")
                {
                    type = "decimal";
                }
                else
                {
                    type = "number";
                }
            }
            return type;
        }

        private string GetDropString(string dropNo, bool vue)
        {
            if (string.IsNullOrEmpty(dropNo))
                return vue ? "''" : "__[]__";
            if (vue)
                return dropNo;
            return "__" + "optionConfig" + dropNo + "__";
        }

        /// <summary>
        /// 生成查詢面板的數據對象結構
        /// </summary>
        /// <param name="list"></param>
        /// <param name="panelHtml"></param>
        /// <param name="keySelector"></param>
        /// <param name="predicate"></param>
        /// <param name="search"></param>
        /// <param name="orderBy"></param>
        /// <param name="vue"></param>
        private void GetPanelData(List<Sys_TableColumn> list, List<List<PanelHtml>> panelHtml, Func<Sys_TableColumn, int?> keySelector, Func<Sys_TableColumn, bool> predicate, bool search, Func<Sys_TableColumn, int?> orderBy, bool vue = false, bool app = false)
        {
            //{ key: 1, value: "顯示/查詢/編輯" },
            //{ key: 2, value: "顯示/編輯" },
            //{ key: 3, value: "顯示/查詢" },
            //{ key: 4, value: "顯示" },
            //{ key: 5, value: "查詢/編輯" },
            //{ key: 6, value: "查詢" },
            //{ key: 7, value: "編輯" },

            if (app)
            {
                list.ForEach(x =>
                {
                    if (x.EditRowNo == 0)
                    {
                        x.EditRowNo = 99999;
                    }
                });
                var arr = search ? new int[] { 1, 3, 5, 6 } : new int[] { 1, 2, 5, 7 };
                predicate = x => arr.Any(c => c == x.Enable);
            }

            var whereReslut = list.Where(predicate).OrderBy(orderBy).ThenByDescending(c => c.OrderNo).ToList();
            foreach (var item in whereReslut.GroupBy(keySelector))
            {
                panelHtml.Add(item.OrderBy(c => search ? c.SearchColNo : c.EditColNo).Select(
                    x => new PanelHtml
                    {
                        text = x.ColumnCnName ?? x.ColumnName,
                        id = x.ColumnName,
                        displayType = GetDisplayType(search, x.SearchType, x.EditType, x.ColumnType),
                        require = !search && x.IsNull == 0 ? true : false,
                        columnType = vue && x.IsImage == 1 ? "img" : (x.ColumnType ?? "string").ToLower(),
                        disabled = !search && x.IsReadDataset == 1 ? true : false,
                        dataSource = GetDropString(x.DropNo, vue),
                        colSize = search && x.SearchType != "checkbox" ? 0 : (x.ColSize ?? 0)
                    }).ToList());
            }
        }


        private static bool IsMysql()
        {
            return DBType.Name.ToLower() == DbCurrentType.MySql.ToString().ToLower();
        }
        private WebResponseContent ValidColumnString(Sys_TableInfo tableInfo)
        {
            WebResponseContent webResponse = new WebResponseContent(true);
            if (tableInfo.TableColumns == null || tableInfo.TableColumns.Count == 0) return webResponse;

            if (!string.IsNullOrEmpty(tableInfo.DetailName))
            {
                Sys_TableColumn mainTableColumn = tableInfo.TableColumns
                     .Where(x => x.IsKey == 1)
                     // .Select(s => s.ColumnName)
                     .FirstOrDefault();
                if (mainTableColumn == null)
                    return webResponse.Error($"請勾選表[{tableInfo.TableName}]的主鍵");

                string key = mainTableColumn.ColumnName;

                //明細表外鍵列的配置信息
                Sys_TableColumn tableColumn = repository
                    .Find<Sys_TableColumn>(x => x.TableName == tableInfo.DetailName && x.ColumnName == key)
                    ?.FirstOrDefault();

                if (tableColumn == null)
                    return webResponse.Error($"明細表必須包括[{tableInfo.TableName}]主鍵字段[{key}]");

                if (mainTableColumn.ColumnType?.ToLower() != tableColumn.ColumnType?.ToLower())
                {
                    return webResponse.Error($"明細表的字段[{tableColumn.ColumnName}]類型必須與主表的主鍵的類型相同");
                }

                if (!IsMysql()) return webResponse;

                if (mainTableColumn.ColumnType?.ToLower() == "string"
                    && tableColumn.Maxlength != 36)
                {
                    return webResponse.Error($"主表主鍵類型為Guid，明細表[{tableInfo.DetailName}]配置的字段[{key}]長度必須是36，請重將明細表字段[{key}]長度設置為36，點擊保存與生成Model");
                }


                //mysql如果主鍵使用的是guid，需要判斷明細表的外鍵是否配置正確


            }

            //if (tableInfo.TableColumns.Exists(x => x.ColumnType == "string" && (x.Maxlength ?? 0) <= 0))
            //{
            //    webResponse.Error("數據類型為string的列，必須輸入[列最大長度]的值");
            //}
            return webResponse;
        }


    }
    public class PanelHtml
    {
        public string text { get; set; }

        public string id { get; set; }
        public string displayType { get; set; }
        public string dataSource { get; set; }
        public string columnType { get; set; }
        public bool require { get; set; }
        public bool disabled { get; set; }
        public int colSize { get; set; }
        public int fileMaxCount { get; set; }
    }
}

