using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContext;
using System.Data;
using System.Data.SqlClient;

namespace JsonSql
{
    public class JsonSqlManager
    {
        /// <summary>
        /// Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
        /// </summary>
        public string ConnectionString { get; set; }
        public SqlConnection SqlConnection { get; set; }

        private int _rowsCount = 0;
        public int RowsCount
        {
            get { return _rowsCount; }
        }

        private int _colsCount = 0;
        public int ColumnsCount
        {
            get { return _colsCount; }
        }

        private DataColumn[] _dataColumns = null;
        public DataColumn[] Columns
        {
            get { return _dataColumns; }
        }

        private string _jsonResult = "";
        public string JsonResult
        {
            get { return _jsonResult; }
        }

        public SqlParameter[] paramList { get; set; }
        public CommandType commandType { get; set; }

        private string FormatString = "\"{0}\":\"{1}\"";
        private string FormatNull = "\"{0}\": null";
        private string FormatNumeric = "\"{0}\": {1}";
        private string FormatBoolean = "\"{0}\": {1}";


        public void SqlToJson(string name,CommandType commandType)
        {
            DataAccess da = new DataAccess();
            da.connectionString = this.ConnectionString;
            da.SqlConnection = this.SqlConnection;
            
            DataTable dt = null;

            if (commandType == CommandType.Text)
                dt = da.GetData(name, commandType);

            else if (commandType == CommandType.StoredProcedure)
            {
                da.plist = paramList;
                dt = da.GetData(name, commandType);
            }


            int cols = dt.Columns.Count;
            this._rowsCount = dt.Rows.Count;
            this._colsCount = dt.Columns.Count;
            this._dataColumns = new DataColumn[cols];
            dt.Columns.CopyTo(this._dataColumns, 0);

            string result = "[";

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    result += "{";
                    for (int j = 0; j < cols; j++)
                    {
                        result += GenerateItem(dt.Columns[j], dt.Rows[i][j]);
                        
                        if (j != cols - 1)
                            result += ",";

                        if (j == cols-1 && i != dt.Rows.Count-1)
                            result += "},";
                    }
                    
                    
                }    
            }

            result += "}]";

            this._jsonResult = result;
        }


        private string GenerateItem(DataColumn dc, object dr)
        {
            string output = "";

            if (dr == DBNull.Value)
                return string.Format(FormatNull, dc.ColumnName);

            switch (dc.DataType.Name)
            {
                case "Int32":
                    output = string.Format(FormatNumeric, dc.ColumnName, dr.ToString());
                    break;
                case "Decimal":
                    output = string.Format(FormatNumeric, dc.ColumnName, dr.ToString().Replace(",", "."));
                    break;
                case "Double":
                    output = string.Format(FormatNumeric, dc.ColumnName, dr.ToString().Replace(",", "."));
                    break;
                case "Boolean":
                    output = string.Format(FormatBoolean, dc.ColumnName, dr.ToString().ToLower());
                    break;
                default:
                    output = string.Format(FormatString, dc.ColumnName, FixString(dr.ToString()));
                    break;
            }

            return output;
        }

        private string FixString(string s)
        {
            s = s.Replace(@"\", @"\/");
            s = s.Replace("\"", "'");
            s = s.Replace("'", "");
            s = s.Replace(Environment.NewLine, "");

            return s;
        }

        
    }
}
