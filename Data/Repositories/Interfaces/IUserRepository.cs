using Common.Enum;
using Common.Models;
using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        string CreateUser(User user);
        IEnumerable<User> GetAllUsers();
        void DeleteUser(string username);
        User GetSubscriptionType(int id, int subId);
        User? Authenticate(CredentialsToAuthenticateDto credentials);
        string UpdateUserSub(string username, int idSub);
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        int SumConversion();
    }
}