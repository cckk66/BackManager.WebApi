using BackManager.Utility.Helper;
using BackManager.Utility.Tool;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackManager.Infrastructure
{
    public static class InsertExtend
    {
        public static Task<int> MySqlBulkInsert<TEntity>(this DbContext @this, List<TEntity> UpdateData)
    => MySqlBulkInsert<TEntity>(@this, UpdateData.ToDataTable());

        public static Task<int> MySqlBulkInsert<TEntity>(this DbContext @this, DataTable dt)
        {
            string TableName = typeof(TEntity).Name;
            return @this.MySqlBulkInsertByStreamAsync(TableName, dt);
        }
        public static Task<int> MySqlBulkInsertByStreamAsync(this DbContext @this, string TableName, DataTable dt)
        {
            try
            {
                DbConnection Connection = @this.Database.GetDbConnection();
                MySqlConnection mySqlConnection = (MySqlConnection)Connection;


                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }

                var columns = dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                MemoryStream SourceStream = dt.ToStream();
                MySqlBulkLoader bulk = new MySqlBulkLoader(mySqlConnection)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    SourceStream = SourceStream,
                    NumberOfLinesToSkip = 0,
                    TableName = TableName
                };

                bulk.Columns.AddRange(columns);
                return bulk.LoadAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Task<int> MySqlBulkInsertByCSVAsync(this DbContext @this, string TableName, DataTable dt)
        {
            try
            {
                DbConnection Connection = @this.Database.GetDbConnection();
                MySqlConnection mySqlConnection = (MySqlConnection)Connection;


                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }

                var columns = dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                string tableCsvPath = $"{ Tools.ProjCSVPath}{TableName}.csv";
                dt.ToCsv(tableCsvPath);
                MySqlBulkLoader bulk = new MySqlBulkLoader(mySqlConnection)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = tableCsvPath,
                    NumberOfLinesToSkip = 0,
                    TableName = TableName,
                    Local = true,


                };

                bulk.Columns.AddRange(columns);
                return bulk.LoadAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
