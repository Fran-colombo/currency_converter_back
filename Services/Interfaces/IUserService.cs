﻿using Common.Enum;
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
        User? GetUserById(int id);
        UserDto? GetUserByUsername(string username);
        IEnumerable<UserDto> GetUsers();
        
    }
}