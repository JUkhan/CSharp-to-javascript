using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper.JS
{
   public delegate void SQLResultSetCallback(SQLResultSet res);
    public class SQLight
    {
        public static List<T> getList<T>(SQLResultSet res) { return null; }
        public static void setDbOptions(string fileName, string version, string displayName, int maxSize) { }
        public SQLight query(string sql, List<object> paramsArray) { return null; }
        public SQLight query(string sql) { return null; }
        public SQLight success(SQLResultSetCallback callback) { return null; }
        public SQLight error(SQLResultSetCallback callback) { return null; }
    }
    public class SQLResultSet {
        public int rowsAffected { get; set; }
        public int insertId { get; set; }
        public SQLResultSetRowList rows { get; set; }
    }
    public class SQLResultSetRowList
    {
        public int length { get; set; }
        public object item(int index) { return null; }
    }
}
