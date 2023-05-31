using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess.DbAccess
{
    //Clase que habla con Sql a través de Dapper
    public class SqlDataAccess : ISqlDataAccess, IDisposable
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

        //PARA LAS TRANSACTIONS
        //No se puede usar el "using" de los métodos LoadData y saveData porque este cierra la conexión y se deben
        //hacer multiples transacciones antes de cerrar la conexión

        //Tiene underscore porque ya tengo otra conexión para los métodos que no son en transacción
        private IDbConnection _connection;

        private IDbTransaction _transaction;

        //Open connection/start transaction
        public void StartTransaction(string connectionId = "Default")
        {
            _connection = new SqlConnection(_config.GetConnectionString(connectionId));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        //Save using the transaction
        //Ya no necesitamos crear la conexión porque ya se habría creado al momento de iniciar la transacción
        public async Task SaveDataInTransaction<T>(
            string storedProcedure,
            T parameters)
        {
            await _connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        //Load using the transaction
        public async Task<IEnumerable<T>> LoadDataInTransaction<T, U>(
            string storedProcedure,
            U parameters)
        {
            return await _connection.QueryAsync<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        //Close connection/stop transaction
        public void CommitTransaction()
        {
            //Si la transacción es nula o la conexión no se ejecutan los métodos, por eso el ?. Así se puede llamar el
            //método en multiples ocasiones
            _transaction?.Commit();
            _connection?.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        //Dispose
        public void Dispose()
        {
            CommitTransaction();
        }
    }
}