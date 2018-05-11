using Dapper;
using Isf.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Isf.Core.Stores
{
    public class DapperStore : IDisposable
    {
        private IDbConnection currentConnection;
        private readonly ILogger logger;
        private readonly IConnectionFactory connectionFactory;

        public IDbConnection GetOpenConnection()
        {
            try
            {
                if (currentConnection != null && currentConnection.State == ConnectionState.Open)
                {
                    return currentConnection;
                }

                currentConnection = connectionFactory.CreateConnection();

                currentConnection.Open();

                return currentConnection;
            }
            catch (Exception ex)
            {
                logger.Error("Failed to open connection", ex);
                throw ex;
            }
        }

        protected void HandleDatabaseError(string sql, object param, Exception ex)
        {
            logger.Error($"failed to run query: {sql} {JsonConvert.SerializeObject(param)}", ex);
        }

        protected async Task<int> GetCount(string sql, IDictionary<string, object> param)
        {
            sql = $"select count(*) from ({sql}) t;";

            using (var connection = GetOpenConnection())
            {
                return await connection.QuerySingleAsync<int>(sql, param);
            }
        }

        protected async Task<PagedList<T>> PagedQueryAsync<T>(string sql, IDictionary<string, object> param, Paging paging)
        {
            dynamic queryParams = new ExpandoObject();

            try
            {
                if (!paging.IsValid())
                {
                    throw new ArgumentException($"Paging definition not valid");
                }

                //get the total row count
                paging.TotalItems = await GetCount(sql, param);

                //clean orderBy by removing any non-alpha-numeric or underscore
                //cannot use a parameter; sql requires a string for the order by column name
                paging.OrderBy = Regex.Replace(paging.OrderBy, @"[^\w ]+\.", string.Empty);

                string pagination = $"order by {paging.OrderBy} offset @skip rows fetch next @pageSize rows only";

                sql = $"{sql} {pagination}";

                queryParams.skip = (paging.Page - 1) * paging.PageSize;
                queryParams.pageSize = paging.PageSize;

                foreach (var prop in param)
                {
                    ((IDictionary<string, object>)queryParams)[prop.Key] = prop.Value;
                }

                var rows = await QueryAsync<T>(sql, queryParams);

                return new PagedList<T>
                {
                    List = rows,
                    Paging = paging
                };
            }
            catch (Exception ex)
            {
                HandleDatabaseError(sql, queryParams, ex);
                throw ex;
            }
        }

        protected async Task<T> QuerySingleAsync<T>(string sql, object param = null)
        {
            try
            {
                return await GetOpenConnection()
                    .QuerySingleAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                HandleDatabaseError(sql, param, ex);
                throw ex;
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            try
            {
                return await GetOpenConnection()
                    .QueryAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                HandleDatabaseError(sql, param, ex);
                throw ex;
            }
        }

        public void Dispose()
        {
            if(currentConnection != null)
            {
                currentConnection.Close();
                currentConnection.Dispose();
            }
        }
    }
}
