# DBFactory

A wrapper for rapidly accessing Microsoft SQL database

# Examples

## Get a table from db

```C#
    Page page = DBHelper.GetDataTable(connstr,"DrugTalks", "*", "", "drug asc", PageIndex, PageSize);
     if (page != null)
      {
         if (PageIndex <= page.PageCount)
          {

            if (page.DataTable != null && page.DataTable.Rows.Count > 0)
               for (int i = 0; i < page.DataTable.Rows.Count; i++)
                  {
                                ...
                   }

             }
    }               
```

## other useful functions

```C#
public static CommonPage GetCombinePage(string connStr, string table1,string table2, string[] fields1,string[] fields2,string on, string where, string orderby, int pageIndex, int pageSize);
public static CommonPage GetPageRaw(string connStr,string sql,int pageIndex,int pageSize);
public static CommonPage GetPage(string connStr, string tableName, string listfield, string where, string orderby, int pageIndex, int pageSize);
public static List<T> GetPageList<T>(string connStr, string tableName, string listfield, string where, string orderby, int pageIndex, int pageSize);
private static DataRow SelectSingleModel(string connStr, string tableName, string listfield, string where);
public static string GetValue(string connStr, string sql);
public static bool Exists<T>(string connStr, string tableName, string where);
public static T GetModel<T>(string connStr, string tableName, string listfield, string where);
public static int InsertDirect(string connStr, string tableName, string[] fields, object[] values);
public static int Insert(string connStr, string tableName, string[] fields, object[] values);
public static int Update(string connStr, string tableName, string[] fields,object[] values, string where);
public static int Delete(string connStr, string tableName, string where);   
```

