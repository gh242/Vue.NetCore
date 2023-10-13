using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VOL.Core.Extensions;

namespace VOL.Core.Utilities
{
    public class FileHelper
    {
        private static object _filePathObj = new object();

        /// <summary>
        /// 通過迭代器讀取平面文件行内容(必須是帶有\r\n換行的文件,百万行以上的内容讀取效率存在問題,适用於100M左右文件，行100W内，超出的會有卡顿)
        /// </summary>
        /// <param name="fullPath">文件全路徑</param>
        /// <param name="page">分頁頁數</param>
        /// <param name="pageSize">分頁大小</param>
        /// <param name="seekEnd"> 是否最後一行向前讀取,默認從前向後讀取</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadPageLine(string fullPath, int page, int pageSize, bool seekEnd = false)
        {
            if (page <= 0)
            {
                page = 1;
            }
            fullPath = fullPath.ReplacePath();
            var lines = File.ReadLines(fullPath, Encoding.UTF8);
            if (seekEnd)
            {
                int lineCount = lines.Count();
                int linPageCount = (int)Math.Ceiling(lineCount / (pageSize * 1.00));
                //超過總頁數，不處理
                if (page > linPageCount)
                {
                    page = 0;
                    pageSize = 0;
                }
                else if (page == linPageCount)//最後一頁，取最後一頁剩下所有的行
                {
                    pageSize = lineCount - (page - 1) * pageSize;
                    if (page == 1)
                    {
                        page = 0;
                    }
                    else
                    {
                        page = lines.Count() - page * pageSize;
                    }
                }
                else
                {
                    page = lines.Count() - page * pageSize;
                }
            }
            else
            {
                page = (page - 1) * pageSize;
            }
            lines = lines.Skip(page).Take(pageSize);

            var enumerator = lines.GetEnumerator();
            int count = 1;
            while (enumerator.MoveNext() || count <= pageSize)
            {
                yield return enumerator.Current;
                count++;
            }
            enumerator.Dispose();
        }
        public static bool FileExists(string path)
        {
            return File.Exists(path.ReplacePath());
        }

        public static string GetCurrentDownLoadPath()
        {
            return ("Download\\").MapPath();
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path.ReplacePath());
        }


        public static string Read_File(string fullpath, string filename, string suffix)
        {
            return ReadFile((fullpath + "\\" + filename + suffix).MapPath());
        }
        public static string ReadFile(string fullName)
        {
            //  Encoding code = Encoding.GetEncoding(); //Encoding.GetEncoding("gb2312");
            string temp = fullName.MapPath().ReplacePath();
            string str = "";
            if (!File.Exists(temp))
            {
                return str;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(temp);
                str = sr.ReadToEnd(); // 讀取文件
            }
            catch { }
            sr?.Close();
            sr?.Dispose();
            return str;
        }



        /// <summary>
        /// 取後缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">路徑 </param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">寫入的内容</param>
        /// <param name="appendToLast">是否將内容添加到未尾,默認不添加</param>
        public static void WriteFile(string path, string fileName, string content, bool appendToLast = false)
        {
            path = path.ReplacePath();
            fileName = fileName.ReplacePath();
            if (!Directory.Exists(path))//如果不存在就創建file文件夾
                Directory.CreateDirectory(path);

            using (FileStream stream = File.Open(path + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = Encoding.Default.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }



        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路徑</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path.ReplacePath());
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }


        /// <summary>
        /// 拷貝文件
        /// </summary>
        /// <param name="OrignFile">原始文件</param>
        /// <param name="NewFile">新文件路徑</param>
        public static void FileCoppy(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile.ReplacePath(), NewFile.ReplacePath(), true);
        }


        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="Path">路徑</param>
        public static void FileDel(string Path)
        {
            File.Delete(Path.ReplacePath());
        }

        /// <summary>
        /// 移動文件
        /// </summary>
        /// <param name="OrignFile">原始路徑</param>
        /// <param name="NewFile">新路徑</param>
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile.ReplacePath(), NewFile.ReplacePath());
        }

        /// <summary>
        /// 在當前目錄下創建目錄
        /// </summary>
        /// <param name="OrignFolder">當前目錄</param>
        /// <param name="NewFloder">新目錄</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder.ReplacePath());
            Directory.CreateDirectory(NewFloder.ReplacePath());
        }

        /// <summary>
        /// 創建文件夾
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判斷目標目錄是否存在如果不存在則新建之
            if (!Directory.Exists(Path.ReplacePath()))
                Directory.CreateDirectory(Path.ReplacePath());
        }


        public static void FileCreate(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path.ReplacePath()); //創建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }
        /// <summary>
        /// 遞歸刪除文件夾目錄及文件
        /// </summary>
        /// <param name="dir"></param>  
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            dir = dir.ReplacePath();
            if (Directory.Exists(dir)) //如果存在這個文件夾刪除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接刪除其中的文件                        
                    else
                        DeleteFolder(d); //遞歸刪除子文件夾 
                }
                Directory.Delete(dir, true); //刪除已空文件夾                 
            }
        }


        /// <summary>
        /// 指定文件夾下面的所有内容copy到目標文件夾下面
        /// </summary>
        /// <param name="srcPath">原始路徑</param>
        /// <param name="aimPath">目標文件夾</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                aimPath = aimPath.ReplacePath();
                // 檢查目標目錄是否以目錄分割字符結束如果不是則添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判斷目標目錄是否存在如果不存在則新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目錄的文件列表，該裡面是包含文件以及目錄路徑的一個數組
                //如果你指向copy目標文件下面的文件而不包含目錄請使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath.ReplacePath());
                //遍歷所有的文件和目錄
                foreach (string file in fileList)
                {
                    //先當作目錄處理如果存在這個目錄就遞歸Copy該目錄下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否則直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }

        /// <summary>
        /// 獲取文件夾大小
        /// </summary>
        /// <param name="dirPath">文件夾路徑</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            dirPath = dirPath.ReplacePath();
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        /// <summary>
        /// 獲取指定文件詳細屬性
        /// </summary>
        /// <param name="filePath">文件詳細路徑</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            filePath = filePath.ReplacePath();
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "詳細路徑:" + objFI.FullName + "<br>文件名稱:" + objFI.Name + "<br>文件長度:" + objFI.Length.ToString() + "字節<br>創建時間" + objFI.CreationTime.ToString() + "<br>最後訪問時間:" + objFI.LastAccessTime.ToString() + "<br>修改時間:" + objFI.LastWriteTime.ToString() + "<br>所在目錄:" + objFI.DirectoryName + "<br>擴展名:" + objFI.Extension;
            return str;
        }

    }
}
