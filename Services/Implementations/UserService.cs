using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;
using Common.Exceptions;
using Common.Models;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ISubscriptionRepository _subRepo;
       private readonly IConvertionsRepository _convRepo;
        
        public UserService(IUserRepository repository, ISubscriptionRepository subRepo, IConvertionsRepository convRepo)
        {
            _repository = repository;
            _subRepo = subRepo;
            _convRepo = convRepo;
            
        }

            public void AddUser(UserForCreationDto userDto)
            {
            User? user = _repository.GetAllUsers().FirstOrDefault(u => u.Username == userDto.Username);
            var email = _repository.GetAllUsers().FirstOrDefault(u => u.Email == userDto.Email)?.Email;

            if ((user == null) & (email == null))
            {

                var subscription = _subRepo.GetSubscriptionByType(userDto.Subscription);
                if (subscription != null)
                {
                    try {
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
                    catch (Exception ex) {
                        throw new NotAbleCreateUser("Something went wrong while you tried to create the user");
                    }
                }
                else 
                {
                    throw new SubscriptionIdNotFoundException("Subscription not found");
                } 
            }
            else if (email != null)
            {
                throw new EmailAlreadyExistException("The email you submited already exist.");

            }
            else
            {
                    throw new UserAlreadyExistException("The username already exists");
            }
            }


        public string UpdateUserSub(string username, int subId)
        {
            var userToUpdate = _repository.GetUserByUsername(username);
            var sub = _subRepo.GetSubscriptionById(subId);
            var userConv = _convRepo.GetConvertionsByMonths(userToUpdate.Id);

            if (userToUpdate == null)
            {
                throw new UserNotFoundException("The user you are looking for doesn´t exist");
            }

            else if (userConv > sub.MaxConversions)
            {
                throw new ArgumentException($"The user already has {userConv} conversions, which exceeds the limit ({sub.MaxConversions}) for the selected subscription type.");
            }
            else
            {
                try
                {
                    _repository.UpdateUserSub(username, subId);
                    return username;
                }
                catch (Exception ex)
                {
                    throw new FailUpdatingUserException("Something went wrong while you tried to update the user subscription");
                }
            }
        }


        public IEnumerable<UserDto> GetUsers()
        {
            try
            {
                var users = _repository.GetAllUsers();

                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    //int currentMonthConversions = _convRepo.GetConvertionsByMonths(user.Id);

                    var userDto = new UserDto
                    {
                        Username = user.Username,
                        Email = user.Email,
                        Subscription = new SubscriptionDto
                        {
                            Id = user.Subscription.Id,
                            SubscriptionType = user.Subscription.SubscriptionType,
                            MaxConversions = user.Subscription.MaxConversions
                        },
                        Conversions = _convRepo.GetConvertionsByMonths(user.Id), 
                        Role = user.Role
                    };

                    userDtos.Add(userDto);
                }

                return userDtos;
            }
            catch (Exception ex)
            {
                throw new UsersNotFoundException("We couldn’t get the users.");
            }
        }



        public UserDto? GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if (user != null)
                try
                {
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
                        Conversions = _convRepo.GetConvertionsByMonths(user.Id),
                        Role = user.Role
                    };
                    return newUser;
                }
                catch (UserNotFoundException)
                {
                    throw;
                }

            else
            {
                return null;
            }
        }
         public UserDto? GetUserByUsername(string username)
        {
            var user = _repository.GetUserByUsername(username);
            //int userConv = _convRepo.GetConvertionsByMonths(user.Id);
            
            if (user != null)
            {
                try
                {
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
                        Conversions = _convRepo.GetConvertionsByMonths(user.Id),
                        Role = user.Role
                    };
                    return newUser;
                }
                catch (UserNotFoundException)
                {
                    throw ;
                }
            }
            else
            {
                return null;
            }
        }

        public UserDto? GetUserByEmail(string email)
        {
            var user = _repository.GetUserByUsername(email);

            if (user != null)
            {
                try
                {
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
                        Conversions = _convRepo.GetConvertionsByMonths(user.Id),
                        Role = user.Role
                    };
                    return newUser;

                }
                catch (UserNotFoundException)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }

        }

        public void DeleteUser(string username)
        {
            var user = _repository.GetUserByUsername(username);
            if (user != null)
            {
                try
                {
                    _repository.DeleteUser(username);
                }
                catch (NotAbleDeleteUser)
                {
                    throw ;
                }
            }
            else{
                throw new UserNotFoundException("The user you are looking for doesn´t exist"); 
            }
        }

        public User? AuthenticateUser(CredentialsToAuthenticateDto credentials)
        {
            return _repository.Authenticate(credentials);
        }


    }
}
