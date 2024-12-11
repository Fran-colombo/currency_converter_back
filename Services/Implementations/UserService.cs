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
            if (userToUpdate != null)
            {
                try
                {
                    _repository.UpdateUserSub(username, subId);
                    return username;
                }
                catch (Exception ex)
                {
                    throw new FailUpdatingUserException("Something went wrong while ypu tried to update the user subscription");
                }
            }
            else
            {
                throw new UserNotFoundException("The user you are looking for doesn´t exist");
            }

        }
        //public IEnumerable<UserDto> GetUsers()
        //{
        //    try
        //    {
        //        return _repository.GetAllUsers().Select
        //            (u => new UserDto
        //            {
        //                Username = u.Username,
        //                Email = u.Email,
        //                Subscription = new SubscriptionDto
        //                {
        //                    Id = u.Subscription.Id,
        //                    SubscriptionType = u.Subscription.SubscriptionType,
        //                    MaxConversions = u.Subscription.MaxConversions
        //                },
        //                Conversions = u.conversions,
        //                Role = u.Role
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UsersNotFoundException("We couldn´t get the users.");
        //    }
        //}
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



        public User? GetUserById(int id)
        {
            try
            {
            return _repository.GetUserById(id);
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException("There is sth wrong in your code");
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
                catch (Exception ex)
                {
                    throw new UserNotFoundException("Something went wrong when we tried to find the user");
                }
            }
            else
            {
                throw new UserNotFoundException("user not found");
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
                catch (Exception ex)
                {
                    throw new NotAbleDeleteUser("Something went wrong while we tried to delete the user");
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
