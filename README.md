# DBFactory

A C# wrapper for rapidly accessing Microsoft SQL database

# Examples

Use ```DBHelper``` or ```CommonDBHelper``` for executing procedures of accessing and operating the MSSQL Server. 

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

## Other useful functions

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

The parameters' meanings are as follows:

```connstr``` is the connection string of MSSSQL server. 

```where``` is the search condition like ```a=1 and b=2```.

```fields[]``` and ```values[]``` represents key and value respectively. For example, ```field[0]``` is the key and the ```values[0]``` is the value of ```field[0]```. 

```table``` represents the target table's name. 

```order by``` has two values, ```[YOUR FIELD] asc``` or ```[YOUR FIELD] desc```.

```listfield``` represents a set of fields returned by the method, default is set to ```*```, which means returning all fields. 

```pageIndex``` represent the order of page in a paging procedure.

```pageSize``` represent the number of records in a page returned by the method. 
