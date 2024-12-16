using Common.Enum;
using Common.Models;
using Data.Entities;

namespace Services.Interfaces
{
    public interface IUserService
    {
        void AddUser(UserForCreationDto userDto);
        User? AuthenticateUser(CredentialsToAuthenticateDto credentials);
        string UpdateUserSub(string username, int subId);
        void DeleteUser(string username);
        UserDto? GetUserById(int id);
        UserDto? GetUserByUsername(string username);
        UserDto? GetUserByEmail(string email);
        IEnumerable<UserDto> GetUsers();
        
    }
}