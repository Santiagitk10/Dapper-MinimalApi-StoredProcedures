using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    //Esta clase es la que le habla a SqlDataAccess y esta a su vez se conecta con la base de datos
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        //Se usa el arrow sin los curly braces sin el return porque el método cabe en una sola línea
        //Se usa dynamic porque en este caso no paso ningún parámetro al stored procedure, entonces necesito pasar
        //un objeto vacío
        public Task<IEnumerable<UserModel>> GetUsers() =>
            _db.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });

        //Se debe hacer uso de async y await porque FirstOrDefault es otra llamada asíncrona
        //El default es un valor nulo, por eso el ?
        public async Task<UserModel?> GetUser(int id) =>
            //Se pasa "Id" porque es el nombre del parámetro en el Stored Procedure
            (await _db.LoadData<UserModel, dynamic>("dbo.spUser_Get", new { Id = id })).FirstOrDefault();

        //El storedProcedure recibe los dos parámetros tal cuál se pasan en el nuevo objeto creado, es decir
        //los parámetros y el model tienen la misma capitalización
        public Task InsertUser(UserModel user) =>
            _db.saveData("dbo.spUser_Insert", new { user.FirstName, user.LastName });

        //Se pasa el user completo porque el objeto tiene todas las propiedades que espera el storedProcedure en orden
        public Task UpdatetUser(UserModel user) =>
            _db.saveData("dbo.spUser_Update", user);

        public Task DeletetUser(int id) =>
            _db.saveData("dbo.spUser_Delete", new { Id = id });
    }
}