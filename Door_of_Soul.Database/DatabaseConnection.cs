﻿using Door_of_Soul.Core.Protocol;
using System;
using System.Data.Common;

namespace Door_of_Soul.Database
{
    public abstract class DatabaseConnection<TDbConnection> : IDisposable where TDbConnection : DbConnection
    {
        public delegate OperationReturnCode QueryDelegate(TDbConnection connection, out string errorMessage);
        public delegate OperationReturnCode QueryWithResultDelegate<TQueryResult>(TDbConnection connection, out TQueryResult result, out string errorMessage);
        public TDbConnection Connection { get; protected set; }
        protected abstract string DatabaseName { get; }
        private object connectionLock = new object();

        public abstract bool Connect(string hostName, string userName, string password, string database, string charset, out string errorMessage);
        public void Dispose()
        {
            Connection?.Dispose();
        }
        public OperationReturnCode SendQuery(QueryDelegate query, out string errorMessage, bool useLock = false)
        {
            if (Connection != null)
            {
                if(useLock)
                {
                    lock(connectionLock)
                    {
                        return ExecuteQuery(query, out errorMessage);
                    }
                }
                else
                {
                    return ExecuteQuery(query, out errorMessage);
                }
            }
            else
            {
                errorMessage = $"{DatabaseName} Connection is null";
                return OperationReturnCode.NullObject;
            }
        }
        public OperationReturnCode SendQuery<TQueryResult>(QueryWithResultDelegate<TQueryResult> query, out TQueryResult result, out string errorMessage, bool useLock = false)
        {
            if (Connection != null)
            {
                if (useLock)
                {
                    lock (connectionLock)
                    {
                        return ExecuteQuery(query, out result, out errorMessage);
                    }
                }
                else
                {
                    return ExecuteQuery(query, out result, out errorMessage);
                }
            }
            else
            {
                result = default(TQueryResult);
                errorMessage = $"{DatabaseName} Connection is null";
                return OperationReturnCode.NullObject;
            }
        }
        private OperationReturnCode ExecuteQuery(QueryDelegate query, out string errorMessage)
        {
            Connection.Open();
            OperationReturnCode returnCode = OperationReturnCode.Successiful;
            DbTransaction transaction = Connection.BeginTransaction();
            try
            {
                returnCode = query(Connection, out errorMessage);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                returnCode = OperationReturnCode.DbTransactionFailed;
                errorMessage = $"{DatabaseName} Transaction Failed Message:{exception.Message}, StackTrace:{exception.StackTrace}";
            }
            Connection.Close();
            return returnCode;
        }
        private OperationReturnCode ExecuteQuery<TQueryResult>(QueryWithResultDelegate<TQueryResult> query, out TQueryResult result, out string errorMessage)
        {
            Connection.Open();
            OperationReturnCode returnCode = OperationReturnCode.Successiful;
            DbTransaction transaction = Connection.BeginTransaction();
            try
            {
                returnCode = query(Connection, out result, out errorMessage);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                returnCode = OperationReturnCode.DbTransactionFailed;
                result = default(TQueryResult);
                errorMessage = $"{DatabaseName} Transaction Failed Message:{exception.Message}, StackTrace:{exception.StackTrace}";
            }
            Connection.Close();
            return returnCode;
        }
    }
}