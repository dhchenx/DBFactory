using DBFactory.Factory;
using DBFactory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBFactory.Common
{
    // Author: Donghua Chen
    public class CommonDBHelper
    {

        public static CommonPage GetCombinePage(string connStr, string table1,string table2, string[] fields1,string[] fields2,string on, string where, string orderby, int pageIndex, int pageSize)
        {
            string sql = "select ";

            for (int i = 0; i < fields1.Length; i++)
            {
                sql += "" + table1 + "." + fields1[i];
                //if (i != fields1.Length - 1)
                    sql += ",";
                
            }
            for (int i = 0; i < fields2.Length; i++)
            {
                sql += "" + table2 + "." + fields2[i];
                if (i != fields2.Length - 1)
                    sql += ",";
            }

            if (fields1 == null || fields2 == null)
            {
                sql += " * ";
            }

            sql += " from " + table1 +" ";
            sql += " left outer join "+ table2+" on " + on;

            if (!string.IsNullOrEmpty(where))
                sql += " where " + where + " ";

            if (!string.IsNullOrEmpty(orderby))
                sql += " order by " + orderby;

            sql = sql.Trim();


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            DataTable dt = db.ExecuteDataTable(pageSize, pageIndex);

            CommonPage cp = new CommonPage();
            cp.RecordCount = db.TotalCount;
            cp.DataTable = dt;

            db.Close();

            return cp;
        }


        public static string GetConnectionStr()
        {
            return DBHandlerFactory.DB_CONIFIG_STR;
        }

        public static CommonPageWithCat GetPageWithCat(string connStr, string tableName, string listfield, string where, string orderby, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();


            string sql = "select " + listfield + " from " + tableName + " ";

            if (!string.IsNullOrEmpty(where))
                sql += " where " + where + " ";

            if (!string.IsNullOrEmpty(orderby))
                sql += " order by " + orderby + " ";

            sql = sql.Trim();


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            DataTable dt = db.ExecuteDataTable(pageSize, pageIndex);

            CommonPageWithCat cp = new CommonPageWithCat();
            cp.RecordCount = db.TotalCount;

            int pg = 0;
            pg = cp.RecordCount / pageSize;

            if (cp.RecordCount % pageSize > 0)
            {
                pg++;
            }

            cp.PageCount = pg;

            cp.DataTable = dt;

            db.Close();

            return cp;
        }


        public static CommonPage GetPageRaw(string connStr,string sql,int pageIndex,int pageSize)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();


        


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            DataTable dt = db.ExecuteDataTable();

            CommonPage cp = new CommonPage();
            cp.RecordCount = db.TotalCount;

            int pg = 0;
            pg = cp.RecordCount / pageSize;

            if (cp.RecordCount % pageSize > 0)
            {
                pg++;
            }

            cp.PageCount = pg;

            cp.DataTable = dt;

            db.Close();

            return cp;
        }

        public static CommonPage GetPage(string connStr, string tableName, string listfield, string where, string orderby, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();


            string sql = "select " + listfield + " from " + tableName + " ";

            if (!string.IsNullOrEmpty(where))
                sql += " where " + where + " ";

            if (!string.IsNullOrEmpty(orderby))
                sql += " order by " + orderby + " ";

            sql = sql.Trim();


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            DataTable dt = db.ExecuteDataTable(pageSize, pageIndex);

            CommonPage cp = new CommonPage();
            cp.RecordCount = db.TotalCount;

            int pg = 0;
            pg = cp.RecordCount / pageSize;

            if (cp.RecordCount % pageSize > 0)
            {
                pg++;
            }

            cp.PageCount = pg;

            cp.DataTable = dt;

            db.Close();

            return cp;
        }

        public static List<T> GetPageList<T>(string connStr, string tableName, string listfield, string where, string orderby, int pageIndex, int pageSize)
        {
            CommonPage cp = CommonDBHelper.GetPage(connStr, tableName, listfield, where, orderby, pageIndex, pageSize);
            if (cp != null && cp.DataTable != null)
            {
                List<T> list = new List<T>();
                for (int i = 0; i < cp.DataTable.Rows.Count; i++)
                {
                    DataRow dr = cp.DataTable.Rows[i];
                    T t = GetModelByRow<T>(dr);
                    list.Add(t);

                }
                return list;
            }
            else
                return new List<T>();
        }

        private static DataRow SelectSingleModel(string connStr, string tableName, string listfield, string where)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            int pageIndex = 1;
            int pageSize = 1;
            string orderby = "id desc";

            string sql = "select " + listfield + " from " + tableName + " ";

            if (!string.IsNullOrEmpty(where))
                sql += " where " + where + " ";

            if (!string.IsNullOrEmpty(orderby))
                sql += " order by " + orderby + " ";

            sql = sql.Trim();


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            DataTable dt = db.ExecuteDataTable(pageSize, pageIndex);

            CommonPage cp = new CommonPage();
            cp.RecordCount = db.TotalCount;
            cp.DataTable = dt;

            db.Close();

            if (cp != null && cp.DataTable != null && cp.DataTable.Rows.Count > 0)
            {
                return cp.DataTable.Rows[0];
            }
            else
                return null;

             
        }

        public static string GetValue(string connStr, string sql)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();
 


            DBHandler db = DBHandlerFactory.GetHandler(connStr);
            db.CommandText = sql;
            db.CommandType = CommandType.Text;
            object str =  db.ExecuteScalar();
            db.Close();
            if (str == null)
                return null;
            else
               return  Convert.ToString(str);
 

        }


        public static T GetModelByRow<T>(DataRow dr)
        {
          
            if (dr == null)
            {
                return default(T);
            }

            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                if (propertyInfo != null && dr[i] != DBNull.Value)
                {

                    if (propertyInfo.PropertyType.Name.ToLower() == "binary")
                    {
                        propertyInfo.SetValue(model, new Binary((byte[])dr[i]), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(model, dr[i], null);
                    }
                }
                else continue;
            }

            return model;
        }


        public static bool Exists<T>(string connStr, string tableName, string where)
        {
            T dt = GetModel<T>(connStr, tableName, "id", where);
            if (dt == null)
                return false;
            return true;
        }


        public static T GetModel<T>(string connStr, string tableName, string listfield, string where)
        {

            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            DataRow dr = SelectSingleModel(connStr, tableName, listfield, where);


            if (dr == null)
            {
                return default(T);
            }

            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                if (propertyInfo != null && dr[i] != DBNull.Value)
                {

                    if (propertyInfo.PropertyType.Name.ToLower() == "binary")
                    {
                        propertyInfo.SetValue(model, new Binary((byte[])dr[i]), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(model, dr[i], null);
                    }
                }
                else continue;
            }

            return model;
        }

        public static int InsertDirect(string connStr, string tableName, string[] fields, object[] values)
        {

            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            DBHandler db = DBHandlerFactory.GetHandlerDirect(connStr);


            string sql = "insert into " + tableName + " (";

            for (int i = 0; i < fields.Length; i++)
            {
                sql += fields[i];
                if (i != fields.Length - 1)
                    sql += ",";

                if (values[i] == null)
                    values[i] = "";

                db.AddParameter(fields[i], values[i]);

            }

            sql += ")values(";

            for (int i = 0; i < values.Length; i++)
            {
                sql += "@" + fields[i] + "";
                if (i != values.Length - 1)
                    sql += ",";
            }
            sql += ")";

            db.CommandText = sql;
            db.CommandType = CommandType.Text;


            int ret = db.ExecuteNonQuery();
            db.Close();

            return ret;
        }


        public static int Insert(string connStr, string tableName, string[] fields, object[] values)
        {

            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            DBHandler db = DBHandlerFactory.GetHandler(connStr);

          
            string sql = "insert into " + tableName + " (";
 
            for (int i = 0; i < fields.Length; i++)
            {
                sql += fields[i];
                if (i != fields.Length - 1)
                    sql += ",";

                if (values[i] == null)
                    values[i] = "";

                db.AddParameter(fields[i], values[i]);
               
            }

            sql += ")values(";

            for (int i = 0; i < values.Length; i++)
            {
                sql += "@"+fields[i]+"";
                if (i != values.Length - 1)
                    sql += ",";
            }
            sql += ")";

            db.CommandText = sql;
            db.CommandType = CommandType.Text;


            int ret = db.ExecuteNonQuery();
            db.Close();

            return ret;
        }


        public static int Update(string connStr, string tableName, string[] fields,object[] values, string where)
        {

            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            DBHandler db = DBHandlerFactory.GetHandler(connStr);


            string sql = "update " + tableName + "  set ";
            for (int i = 0; i < fields.Length; i++)
            {
                sql += fields[i] + "=@" + fields[i];
                if (i != fields.Length - 1)
                    sql += ",";

                string v = (string)values[i];
                if (v == null)
                    v = "";

                db.AddParameter(fields[i], v);
               
            }
            if (!string.IsNullOrEmpty(where))
                sql += " where " + where;


            db.CommandText = sql;
            db.CommandType = CommandType.Text;


            int ret = db.ExecuteNonQuery();
            db.Close();

            return ret;
        }

        public static int Delete(string connStr, string tableName, string where)
        {

            if (string.IsNullOrEmpty(connStr))
                connStr = GetConnectionStr();

            DBHandler db = DBHandlerFactory.GetHandler(connStr);


            string sql = "delete from " + tableName ;
            if (!string.IsNullOrEmpty(where))
                sql += " where " + where;


            db.CommandText = sql;
            db.CommandType = CommandType.Text;


            int ret = db.ExecuteNonQuery();
            db.Close();

            return ret;
            
        }

    }
}
