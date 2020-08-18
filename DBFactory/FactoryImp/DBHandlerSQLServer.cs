using DBFactory.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBFactory.FactoryImp
{
    // Author: Donghua Chen
    internal class DBHandlerSQLServer : DBHandler
    {
        public DBHandlerSQLServer(string connectionString)
            : base()
        {
            this.dbConnection = new SqlConnection(connectionString);
        }

        protected override DbCommand CreateCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = (SqlConnection)this.dbConnection;
            if (this.IsInTransaction)
            {
                cmd.Transaction = (SqlTransaction)this.dbTransaction;
            }
            if (this.CommandType == CommandType.TableDirect)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("select * from {0}", this.CommandText);
            }
            else
            {
                cmd.CommandType = CommandType;
                cmd.CommandText = CommandText;
            }
            if (this.parameters.Count > 0)
            {
                foreach (Parameter p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Name, p.Value);
                }
            }
            return cmd;
        }

        protected override DbDataAdapter CreateAdapter()
        {
            SqlCommand cmd = (SqlCommand)this.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            return adapter;
        }

        protected override void BuilderCommand(DbDataAdapter adapter)
        {
            new SqlCommandBuilder((SqlDataAdapter)adapter);
        }

        protected override int GetTotalCount()
        {
            this.CheckPageSQL();
            string sql = this.CommandText;
            string sqlWithOutOrderField = string.Empty;   
            int startIndex = sql.LastIndexOf("order by");
            if (startIndex >= 0)
            {
                sqlWithOutOrderField = sql.Substring(0, startIndex);
            }
            else
            {
                sqlWithOutOrderField = sql;
            }
            this.CommandText = string.Format("select count(*)from ({0}) t1", sqlWithOutOrderField);
            int r = int.Parse(this.ExecuteScalar().ToString());
            this.CommandText = sql;
            return r;
        }

        public override DataTable ExecuteDataTable(int pageSize, int currentPageIndex)
        {
            this.CheckPageSQL();
            string sql = this.CommandText; 
            string orderBy = string.Empty;
            string sqlWithSelectAndOrder = sql.Substring(6);  
            int startIndex = sqlWithSelectAndOrder.ToLower().LastIndexOf("order by");
            if (startIndex > 0)
            {
                orderBy = sqlWithSelectAndOrder.Substring(startIndex);
                sqlWithSelectAndOrder = sqlWithSelectAndOrder.Substring(0, startIndex);
            }
            else
            {
                throw new Exception("page query must have order by");
            }
            if (pageSize == 0) 
            {
                this.CommandText = sql;
                this.ExecuteDataTable();
            }
            DataTable dt = new DataTable();
            if (currentPageIndex == 1) 
            {
                this.CommandText = string.Format("select top {0} {1} {2}", pageSize, sqlWithSelectAndOrder, orderBy);
                dt = this.ExecuteDataTable();
            }
            else   
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from ");
                sb.AppendFormat("(select Row_Number() over ({0}) as RowNum,{1})t1", orderBy, sqlWithSelectAndOrder);
                sb.AppendFormat(" where RowNum between {0} and {1}", pageSize * (currentPageIndex -1)+1, pageSize * currentPageIndex );
                this.CommandText = sb.ToString();
                dt = this.ExecuteDataTable();
            }
            this.CommandText = sql;
            return dt;
        }

        public override int GetSequenceValue(string sequenceName)
        {
          

            this.CommandType = CommandType.Text;
            this.BegionTransaction();
            this.CommandText = string.Format("Update System_Sequence set Value=Value+1 where Name='{0}'", sequenceName);
            this.ExecuteNonQuery();
            this.CommandText = string.Format("select Value from System_Sequence where Name='{0}'", sequenceName);
            int r = this.ExecuteNonQuery();
            this.CommitTransaction();
            return r;
        }
    }

}
