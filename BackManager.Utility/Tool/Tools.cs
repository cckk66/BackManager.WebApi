using System;

namespace BackManager.Utility.Tool
{
    public static class Tools
    {

        private static string _ProjBinPath;
        /// <summary>
        /// 获取项bin目录
        /// </summary>
        /// <returns></returns>
        public static string ProjBinPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ProjBinPath))
                {
                    string baseDir = AppContext.BaseDirectory;
                    int indexBin = baseDir.IndexOf("bin");
                    if (indexBin > 0)
                    {
                        _ProjBinPath = baseDir.Substring(0, indexBin);
                    }
                    else
                    {
                        _ProjBinPath = baseDir;
                    }
                }
                return _ProjBinPath;
            }
        }
        private static string _ProjCSVPath;
        /// <summary>
        /// mysql table csv文件
        /// </summary>
        public static string ProjCSVPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ProjCSVPath))
                {
                    _ProjCSVPath= $"{Tools.ProjBinPath}TableCSV\\";
                }
                return _ProjCSVPath;
            }
        }
    }
}
