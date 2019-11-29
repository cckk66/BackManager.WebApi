using System.Data;
using System.IO;
using System.Text;

namespace BackManager.Infrastructure
{
    public static class CSVEx
    {
        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static void ToCsv(this DataTable table)
        {
            table.ToCsv("");
        }
        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        public static void ToCsv(this DataTable table, string Path)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                Path = table.TableName + ".csv";
            }

            {
                int LastIndex = Path.LastIndexOf('\\');
                string FileUrl = Path.Substring(0, LastIndex);
                if (!Directory.Exists(FileUrl))
                    Directory.CreateDirectory(FileUrl);
            }

            StringBuilder sb = ToStringBuilder(table);

            File.WriteAllText(Path, sb.ToString());

        }

        private static StringBuilder ToStringBuilder(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb;
        }

        /// <summary>
        /// 将DataTable转换MemoryStream 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream ToStream(this DataTable table)
        {
            StringBuilder sb = ToStringBuilder(table);
            byte[] array = Encoding.UTF8.GetBytes(sb.ToString());
            return new MemoryStream(array);
        }
    }
}
