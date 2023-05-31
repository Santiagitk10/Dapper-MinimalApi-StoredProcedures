using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IUserData
    {
        Task DeletetUser(int id);

        Task<UserModel?> GetUser(int id);

        Task<IEnumerable<UserModel>> GetUsers();

        Task InsertUser(UserModel user);

        Task UpdatetUser(UserModel user);

        Task InsertUserInTransaction(UserModel user);
    }
}