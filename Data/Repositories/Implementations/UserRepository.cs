using Common.Enum;
using Common.Models;
using Data;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CurrencyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(CurrencyContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Username;
        }

        public void DeleteUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

                _context.Remove(user);
                _context.SaveChanges();
            
        }
    

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Subscription).ToList();
        }


        public User? GetUserById(int id)
        {

          return _context.Users.Include(u => u.Subscription).FirstOrDefault(u => u.Id == id);
     
        }
        public User? GetUserByUsername(string username)
        {
            
          return _context.Users.Include(u => u.Subscription).FirstOrDefault(u => u.Username == username);

        }

        public string UpdateUserSub(string username, int idSub) {
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Username == username);

            userToUpdate.SubscriptionId = idSub;
            _context.SaveChanges();
            return username;
          
            
        
        }

     
        public User? Authenticate(CredentialsToAuthenticateDto credentials)
        {
            return _context.Users.FirstOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password);
        }



        public User? GetSubscriptionType(int id, int subId)
        {
            return _context.Users.Include(s => s.Subscription).SingleOrDefault(u => u.Id == id);

        }

        public int SumConversion()
        {
            
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Busca el usuario en la base de datos
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                // Incrementa la cantidad de conversiones
                user.conversions += 1;
                _context.SaveChanges();
                return user.conversions;
            }
            if (user.conversions > user.Subscription.MaxConversions)
            {
                throw new Exception("Has alcanzado el límite máximo de conversiones para tu suscripción.");
            }
            return 0; 
        }



    }
}
