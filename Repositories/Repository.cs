using bom.Models;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace bom.Repositories
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class, IEntity, new()
    {
        protected readonly IDbConnection db;

        public Repository(string connectionString)
        {
            this.db = new SqlConnection(connectionString);
        }
        //public Repository()
        //{
        //    this.db = new SqlConnection("");
        //}

        public async Task<T> AddAsync(T t)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var id = await this.db.InsertAsync(t, transaction);
                    t.Id = (int)id;
                    CommitTransaction(transaction);
                }
                catch
                {
                    RollbackTransaction(transaction);
                    throw;
                }
            }
            return t;
        }

        public async Task DeleteAsync(int id)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    await this.db.DeleteAsync(new T { Id = id }, transaction);
                    CommitTransaction(transaction);
                }
                catch
                {
                    RollbackTransaction(transaction);
                    throw;
                }
            }
        }

        public async Task<T> GetAsync(int id)
        {
            T result = null;

            using (var transaction = BeginTransaction())
            {
                try
                {
                    result = await this.db.GetAsync<T>(id, transaction);
                    CommitTransaction(transaction);
                }
                catch
                {
                    RollbackTransaction(transaction);
                    throw;
                }
            }

            return result;
        }

        public async Task<List<T>> GetAllAsync()
        {
            IEnumerable<T> result = null;

            using (var transaction = BeginTransaction())
            {
                try
                {
                    result = await this.db.GetAllAsync<T>(transaction);
                    CommitTransaction(transaction);
                }
                catch
                {
                    RollbackTransaction(transaction);
                    throw;
                }
            }
            return result.ToList();
        }

        public async Task<T> UpdateAsync(T t)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    await this.db.UpdateAsync(t, transaction);
                    CommitTransaction(transaction);
                }
                catch
                {
                    RollbackTransaction(transaction);
                    throw;
                }
            }
            return t;
        }

        public IDbTransaction BeginTransaction()
        {
            if (this.db.State == ConnectionState.Closed)
            {
                this.db.Open();
            }
            return this.db.BeginTransaction();
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
            this.db.Close();
        }

        public void RollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
            this.db.Close();
        }

        public void Dispose()
        {
            if (this.db != null)
            {
                this.db.Dispose();
            }
        }
    }
}