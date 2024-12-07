using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using Common.Models;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ISubscriptionRepository _subRepo;
        
        public UserService(IUserRepository repository, ISubscriptionRepository subRepo)
        {
            _repository = repository;
            _subRepo = subRepo;
            
        }

            public void AddUser(UserForCreationDto userDto)
            {
                
                if (!_repository.GetAllUsers().Any(u => u.Username == userDto.Username))
                {
                    
                    var subscription = _subRepo.GetSubscriptionByType(userDto.Subscription);
                    if (subscription == null)
                    {
                        throw new ArgumentException("Subscription not found");
                    }

                    
                    var newUser = new User
                    {
                        Username = userDto.Username,
                        Password = userDto.Password,
                        confirmPassword = userDto.confirmPassword,
                        Email = userDto.Email,
                        Subscription = _subRepo.GetSubscriptionByType(userDto.Subscription)!
                    };

                    _repository.CreateUser(newUser);
                }
                else
                {
                    throw new ArgumentException("The username already exists");
                }
            }


        public string UpdateUserSub(string username, int subId)
        {
            _repository.UpdateUserSub(username, subId);
            return username;

        }
        public IEnumerable<UserDto> GetUsers()
        {
            return _repository.GetAllUsers().Select
                (u => new UserDto
                {
                    Username = u.Username,
                    Email = u.Email,
                    Subscription = new SubscriptionDto
                    {
                        Id = u.Subscription.Id,
                        SubscriptionType = u.Subscription.SubscriptionType,
                        MaxConversions = u.Subscription.MaxConversions
                        },
                    Conversions = u.conversions,
                    Role = u.Role
                    }).ToList();
        }


        public User? GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }
         public UserDto? GetUserByUsername(string username)
        {
            var user = _repository.GetUserByUsername(username);
            if (user == null)
            {
                throw new ArgumentException("user not found");
            }
            var newUser = new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Subscription = new SubscriptionDto
                {
                    Id = user.Subscription.Id,
                    SubscriptionType = user.Subscription.SubscriptionType,
                    MaxConversions = user.Subscription.MaxConversions
                },
                Conversions = user.conversions,
                Role = user.Role
            };
            return newUser;

        }

        public void DeleteUser(string username)
        {
            _repository.DeleteUser(username);
        }

        public User? AuthenticateUser(CredentialsToAuthenticateDto credentials)
        {
        return _repository.Authenticate(credentials);
        
        }


    }
}
