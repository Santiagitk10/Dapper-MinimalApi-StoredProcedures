using Microsoft.Extensions.Configuration;

namespace DataAccess.DbAccess
{
    public interface ISqlDataAccess
    {
        IConfiguration _config { get; }

        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
        Task saveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
    }
}