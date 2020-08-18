using DBFactory.FactoryImp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DBFactory.Factory
{
    // Author: Donghua Chen
    public class DBHandlerFactory
    {

        public static string DB_CONIFIG_STR = "DPSystem.ConnectionString";

  
        private DBHandlerFactory() { }

        public static DBHandler GetHandler(string connStr)
        {
            ConnectionStringSettings ccs = ConfigurationManager.ConnectionStrings[connStr];
            string providerName = ccs.ProviderName.ToLower();
            DatabaseType dbType = DatabaseType.SqlServer;
            switch (providerName)
            {
                case "":
                case "sqlserver":
                case "system.data.sqlclient":
                    dbType = DatabaseType.SqlServer;
                    break;
                case "oracle":
                case "system.data.oracleclient":
                    dbType = DatabaseType.Oracle;
                    break;
                case "odbc":
                case "system.data.odbc":
                    dbType = DatabaseType.ODBC;
                    break;
                case "oledb":
                case "system.data.oledb":
                    dbType = DatabaseType.OLEDB;
                    break;
                default:
                    throw new Exception("please provide proper provider name");
            }
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new DBHandlerSQLServer(ccs.ConnectionString);
                case DatabaseType.Oracle:
                    return null;
                case DatabaseType.ODBC:
                    return null;
                case DatabaseType.OLEDB:
                    return null;
                default:
                    return null;
            }
        }

        public static DBHandler GetHandlerDirect(string connStr)
        {

            string providerName = "system.data.sqlclient";
            DatabaseType dbType = DatabaseType.SqlServer;
            switch (providerName)
            {
                case "":
                case "sqlserver":
                case "system.data.sqlclient":
                    dbType = DatabaseType.SqlServer;
                    break;
                case "oracle":
                case "system.data.oracleclient":
                    dbType = DatabaseType.Oracle;
                    break;
                case "odbc":
                case "system.data.odbc":
                    dbType = DatabaseType.ODBC;
                    break;
                case "oledb":
                case "system.data.oledb":
                    dbType = DatabaseType.OLEDB;
                    break;
                default:
                    throw new Exception("Please define proper connection string");
            }
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new DBHandlerSQLServer(connStr);
                case DatabaseType.Oracle:
                    return null;
                case DatabaseType.ODBC:
                    return null;
                case DatabaseType.OLEDB:
                    return null;
                default:
                    return null;
            }
        }

        public static DBHandler GetHandler(string connStr, DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new DBHandlerSQLServer(connStr);
                case DatabaseType.Oracle:
                    return null;
                case DatabaseType.ODBC:
                    return null;
                case DatabaseType.OLEDB:
                    return null;
                default:
                    return null;
            }
        }
    }
}
