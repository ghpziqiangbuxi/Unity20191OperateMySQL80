using System;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Assets.Scripts.MySql
{
    public class MySqlAccess
    {

        private MySqlConnection m_MySqlConnection;//连接类对象

        public void CreateLocalDbIfNotExsit()
        {
            string connStr = DbConfig.GetLocalConnectString();
            string dbName = DbConfig.DbName;
            if (!IsDbExist(connStr, dbName)) CreateDb(connStr, dbName);
        }

        public void CreateRemoteDbIfNotExsit()
        {
            string connStr = DbConfig.GetRemoteConnectString();
            string dbName = DbConfig.DbName;
            if (!IsDbExist(connStr, dbName)) CreateDb(connStr, dbName);
        }

        public void CreateLocalTableIfNotExsit(string tableName, string[] colNames, string[] colTypes)
        {
            string connStr = DbConfig.GetLocalConnectDbString();
            Open(connStr);
            if (!IsTableExsit(tableName)) CreateTableAutoId(tableName, colNames, colTypes);
            Close();
        }

        public void CreateRemoteTableIfNotExsit(string tableName, string[] colNames, string[] colTypes)
        {
            string connStr = DbConfig.GetRemoteConnectDbString();
            Open(connStr);
            if (!IsTableExsit(tableName)) CreateTableAutoId(tableName, colNames, colTypes);
            Close();
        }

        public void LocalInsert(string tableName, string[] colNames, string[] values)
        {
            string connStr = DbConfig.GetLocalConnectDbString();
            Open(connStr);
            Insert(tableName, colNames, values);
            Close();
        }

        public void RemoteInsert(string tableName, string[] colNames, string[] values)
        {
            string connStr = DbConfig.GetRemoteConnectDbString();
            Open(connStr);
            Insert(tableName, colNames, values);
            Close();
        }

        private bool IsDbExist(string connStr, string dbName)
        {
            try
            {
                MySqlConnection sqlConn = new MySqlConnection(connStr);
                string sql = $"SELECT * FROM information_schema.SCHEMATA where SCHEMA_NAME='{dbName}';";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, sqlConn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0) return true;
            }
            catch (Exception e)
            {
                Debug.Log($"IsDbExist:{e.Message}");
            }
            return false;
        }

        private void CreateDb(string connStr, string dbName)
        {
            try
            {
                MySqlConnection sqlConn = new MySqlConnection(connStr);
                string sql = $"CREATE DATABASE {dbName};";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConn);
                sqlConn.Open();
                cmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"CreateDb:{e.Message}");
            }
        }

        private bool IsTableExsit(string tableName)
        {
            try
            {
                string query = $"SELECT * FROM information_schema.TABLES where table_name='{tableName}' and TABLE_SCHEMA ='{DbConfig.DbName}';";
                DataSet ds = QuerySet(query);
                if (ds.Tables[0].Rows.Count > 0) return true;
            }
            catch (Exception e)
            {
                Debug.Log($"IsTableExsit:{e.Message}");
            }
            return false;
        }

        private DataSet CreateTableAutoId(string name, string[] colName, string[] colType)
        {
            if (colName == null || colType == null || colName.Length != colType.Length)
            {
                Debug.Log($"CreateTableAutoId: params error..."); return null;
            }
            string query = "CREATE TABLE  " + name + " (" + colName[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";
            for (int i = 1; i < colName.Length; ++i)
            {
                query += ", " + colName[i] + " " + colType[i];
            }
            query += ", PRIMARY KEY (" + colName[0] + ")" + ")";
            return QuerySet(query);
        }

        private DataSet Insert(string tableName, string[] colNames, string[] values)
        {
            if (colNames == null || values == null || values.Length != colNames.Length)
            {
                Debug.Log($"Insert: params error..."); return null;
            }
            string query = "INSERT INTO " + tableName + " (" + colNames[0];
            for (var i = 1; i < colNames.Length; ++i)
            {
                query += ", " + colNames[i];
            }
            query += ") VALUES (" + "'" + values[0] + "'";
            for (var i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }
            query += ")";
            return QuerySet(query);
        }

        private DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
        {
            if (items == null || items.Length == 0 || whereColName == null || operation == null | value == null ||
                whereColName.Length != operation.Length || operation.Length != value.Length)
            {
                Debug.Log($"Select: params error..."); return null;
            }
            string query = "SELECT " + items[0];
            for (var i = 1; i < items.Length; i++)
            {
                query += "," + items[i];
            }
            query += "  FROM  " + tableName + "  WHERE " + " " + whereColName[0] + operation[0] + " '" + value[0] + "'";
            for (var i = 1; i < whereColName.Length; i++)
            {
                query += " AND " + whereColName[i] + operation[i] + "' " + value[i] + "'";
            }
            return QuerySet(query);
        }

        private DataSet Update(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
        {
            if (cols == null || colsvalues == null || cols.Length != colsvalues.Length)
            {
                Debug.Log($"Update: params error..."); return null;
            }
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
            for (var i = 1; i < colsvalues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + colsvalues[i];
            }
            query += " WHERE " + selectkey + " = " + selectvalue + " ";
            return QuerySet(query);
        }

        private DataSet Delete(string tableName, string[] cols, string[] colsvalues)
        {
            if (cols == null || colsvalues == null || cols.Length != colsvalues.Length)
            {
                Debug.Log($"Delete: params error..."); return null;
            }
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
            for (var i = 1; i < colsvalues.Length; ++i)
            {
                query += " or " + cols[i] + " = " + colsvalues[i];
            }
            return QuerySet(query);
        }

        private void Open(string connStr)
        {
            try
            {
                m_MySqlConnection = new MySqlConnection(connStr);
                m_MySqlConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log($"Open: {e.Message}");
            }
        }

        private void Close()
        {
            try
            {
                if (m_MySqlConnection != null)
                {
                    m_MySqlConnection.Close();
                    m_MySqlConnection.Dispose();
                    m_MySqlConnection = null;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Close: {e.Message}");
            }
        }

        private DataSet QuerySet(string sqlString)
        {
            try
            {
                if (m_MySqlConnection != null && m_MySqlConnection.State == ConnectionState.Open)
                {
                    DataSet ds = new DataSet();
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlString, m_MySqlConnection);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                    mySqlDataAdapter.Fill(ds);
                    return ds;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"SQL: {sqlString} \n {e.Message}");
            }
            return null;
        }
    }
}