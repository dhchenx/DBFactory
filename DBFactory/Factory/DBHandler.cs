using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBFactory.Factory
{
    // Author: Donghua Chen
    public abstract class DBHandler
    {
        public DBHandler() { }

        #region 

        protected DbConnection dbConnection = null;   
        protected DbTransaction dbTransaction = null;  
        protected abstract DbCommand CreateCommand();  
        protected abstract DbDataAdapter CreateAdapter(); 
        protected abstract void BuilderCommand(DbDataAdapter adapter); 
        protected abstract int GetTotalCount();

        #endregion

        #region 

        protected List<Parameter> parameters = new List<Parameter>();
        protected bool IsInTransaction = false;  
        protected void CheckPageSQL()
        {
            this.CommandType = CommandType.Text;
            if (!this.CommandText.StartsWith("select", true, null))
            {
                throw new Exception("sql must start with select!");
            }
            if (IsInTransaction)
            {
                throw new Exception("paging is not in transation");
            }
        }

        #endregion

        #region 


        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public void AddParameter(string paraName, object paraValue)
        {
            this.parameters.Add(new Parameter(paraName, paraValue));
        }

        public void ClearParameter()
        {
            this.parameters.Clear();
        }

        public int TotalCount
        {
            get
            {
                return this.GetTotalCount();
            }
        }

        #endregion

        #region 获取数据库的返回值

        public object ExecuteScalar()
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
                DbCommand cmd = this.CreateCommand();
                object r = cmd.ExecuteScalar();
                if (!this.IsInTransaction)
                {
                    dbConnection.Close();
                }
                return r;
            }
            catch (Exception ex)
            {
                this.dbConnection.Close();
                throw new Exception(ex.Message);
            }
        }

        public int ExecuteNonQuery()
        {
            try
            {
                if (this.dbConnection.State != ConnectionState.Open)
                {
                    this.dbConnection.Open();
                }
                DbCommand cmd = this.CreateCommand();
                int r = cmd.ExecuteNonQuery();
                if (!IsInTransaction)
                {
                    dbConnection.Close();
                }
                return r;
            }
            catch (Exception ex)
            {
                dbConnection.Close();
                throw new Exception(ex.Message);
            }
        }

        public DataTable ExecuteDataTable()
        {
            try
            {
                if (this.dbConnection.State != ConnectionState.Open)
                {
                    this.dbConnection.Open();
                }
                DbDataAdapter adapter = this.CreateAdapter();
                DataTable dt = new DataTable();

                DataSet ds = new DataSet();
                ds.EnforceConstraints = false;
                adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds);

                dt = ds.Tables[0];

                if (!IsInTransaction)
                {
                    dbConnection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                dbConnection.Close();
                throw new Exception(ex.Message);
            }
        }


        public bool Close()
        {
            try
            {
                if (dbConnection != null)
                {
                    dbConnection.Close();
                    dbConnection = null;
                    return true;
                }
            }
            catch (Exception)
            {
                dbConnection = null;
              
            }
            return false;
        }

        public abstract DataTable ExecuteDataTable(int pageSize, int currentPageIndex);

        #endregion

        #region 将DataTable更新到数据库中

        public int UpdateData(DataTable dt)
        {
            try
            {
                if (this.dbConnection.State != ConnectionState.Open)
                {
                    this.dbConnection.Open();
                }
                DbDataAdapter adapter = this.CreateAdapter();
                if (this.CommandType == CommandType.StoredProcedure)
                {
                    this.CommandType = CommandType.Text;
                }
                this.BuilderCommand(adapter);
                int r = adapter.Update(dt);
                if (!IsInTransaction)
                {
                    dbConnection.Close();
                }
                return r;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 事务处理

        public void BegionTransaction()
        {
            try
            {
                if (this.dbConnection.State != ConnectionState.Open)
                {
                    this.dbConnection.Open();
                }
                this.dbConnection.BeginTransaction();
                this.IsInTransaction = true;
            }
            catch (Exception ex)
            {
                this.dbConnection.Close();
                this.IsInTransaction = false;
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                this.dbTransaction.Rollback();
                this.dbConnection.Close();
                this.IsInTransaction = false;
            }
            catch (Exception ex)
            {
                this.dbConnection.Close();
                this.IsInTransaction = false;
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                this.dbTransaction.Commit();
                this.dbConnection.Close();
                this.IsInTransaction = false;
            }
            catch (Exception ex)
            {
                this.dbConnection.Close();
                this.IsInTransaction = false;
                throw ex;
            }
        }

        #endregion

        #region 


        #endregion

        #region 

        public abstract int GetSequenceValue(string sequenceName);

        #endregion
    }


    public enum DatabaseType
    {
        SqlServer = 1,
        Oracle = 2,
        ODBC = 3,
        OLEDB = 4
    }

  
}
