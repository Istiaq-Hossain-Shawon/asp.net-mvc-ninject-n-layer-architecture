using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MVC.DAL.Extensions;
using MVC.DAL;

namespace MVC.DAL.Repositories
{
    public class Repository<TEntity> where TEntity : new()
    {
        DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        protected DbContext Context
        {
            get
            {
                return this._context;
            }
        }

        protected IEnumerable<TEntity> ToList(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<TEntity> items = new List<TEntity>();
                while (record.Read())
                {
                    items.Add(Map<TEntity>(record));
                }
                return items;
            }
            
        }

        protected bool ExecuteNonQuery(IDbCommand command)
        {

            command.Transaction = command.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            
            try
            {
                var data=command.ExecuteNonQuery();
                command.Transaction.Commit();
                if (data == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                throw ex;
            }
            
        }
        protected bool ExecuteNonInsertOrUpdateQuery(IList<IDbCommand> commands, IDbTransaction transaction)
        {
            try
            {
                foreach (IDbCommand command in commands)
                {
                    command.Transaction = transaction;
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
        protected bool ExecuteNonInsertOrUpdateQuery(IDbCommand command,Hashtable parameters)
        {
            command.Transaction = command.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                IDictionaryEnumerator en = parameters.GetEnumerator();
                while(en.MoveNext())
                {
                    var p = command.CreateParameter();
                    p.ParameterName = en.Key.ToString();

                    if (en.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    else
                    {
                        p.Value = en.Value;

                    }
                    command.Parameters.Add(p);
                }
                command.ExecuteNonQuery();
                command.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                throw ex;
            }
        }

        public DataTable Select(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                var dataTable = new DataTable();
                dataTable.Load(record);
                return dataTable;
            }
        }
        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            List<T> lstItems = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
                foreach (DataRow row in dt.Rows)
                    lstItems.Add(ConvertDataRowToGenericType<T>(row));
            else
                lstItems = null;
            return lstItems;
        }
        private static T ConvertDataRowToGenericType<T>(DataRow row) where T : class,new()
        {
            Type entityType = typeof(T);
            T objEntity = new T();
            foreach (DataColumn column in row.Table.Columns)
            {
                object value = row[column.ColumnName];
                if (value == DBNull.Value) value = null;
                PropertyInfo property = entityType.GetProperty(column.ColumnName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                try
                {
                    if (property != null && property.CanWrite)
                        property.SetValue(objEntity, value, null);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objEntity;
        }


        public static List<T> SelectToList<T>(IDbCommand command) where T : class, new()
        {
            List<T> lstItems = new List<T>();
            using (var record = command.ExecuteReader())
            {
                var dataTable = new DataTable();
                dataTable.Load(record);
                lstItems = DataTableToList<T>(dataTable);
                return lstItems;
            }
        }

        protected bool ExecuteNonQuery(IList<IDbCommand> commands, IDbTransaction transaction)
        {
            try
            {
                foreach (IDbCommand command in commands)
                {
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        protected TEntity Map<TEntity>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<TEntity>();

            try
            {
                foreach (var property in typeof(TEntity).GetProperties())
                {
                    try
                    {
                        if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                            property.SetValue(objT, record[property.Name]);
                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                   
                }
                return objT;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }
    }
}
