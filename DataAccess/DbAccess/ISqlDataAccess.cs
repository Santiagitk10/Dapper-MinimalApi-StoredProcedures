using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess : IDisposable
    {
        IConfiguration _config { get; }

        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");

        Task saveData<T>(string storedProcedure, T parameters, string connectionId = "Default");

        //void StartTransaction(string connectionId = "Default");

        Task SaveDataInTransaction<T>(string storedProcedure, T parameters, IDbTransaction trans);

        Task<IEnumerable<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);

        void CommitTransaction();

        void RollbackTransaction();

        void Dispose();
    }
}