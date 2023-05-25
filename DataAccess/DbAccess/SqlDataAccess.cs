using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess.DbAccess
{
    //Clase que habla con Sql a través de Dapper
    public class SqlDataAccess : ISqlDataAccess
    {
        //Para obtener la data de appsettings.json, secrets.json, appsettings.Development.json,
        //environment variables, keyvault, etc
        public IConfiguration _config { get; }

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        //async y await me permiten no bloquear la UI mientras se espera la respuesta
        public async Task<IEnumerable<T>> LoadData<T, U>(
            string storedProcedure,
            U parameters,
            string connectionId = "Default")
        {
            //El using indica que cuando se produzca un error o se llegue al cierre de este método se cierre
            //la conexión a la base de datos
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            //Usamos query porque estamos solicitando data. Lleva <T> porque me retorna el modelo que yo necesito
            return await connection.QueryAsync<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task saveData<T>(
            string storedProcedure,
            T parameters,
            string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

            //Usamos Execute porque estamos ejecutando algo, no solicitando data
            await connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}