using Common.Enum;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IUserRepository userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
        }

        public SubscriptionType CreateSubscription(Subscription sub)
        {
            if (!_subscriptionRepository.GetAllSubs().Any(s => s.SubscriptionType == sub.SubscriptionType)) {
                var newSub = _subscriptionRepository.CreateSubscription(
                    new Subscription
                    {
                        SubscriptionType = sub.SubscriptionType,
                        MaxConversions = sub.MaxConversions
                    });
                return sub.SubscriptionType; 
            }
            else {
                throw new ArgumentException("Sth went wrong");
             }   
            
        }
         
 
        public int ChangeSubscription(string username, int subId)
        {

            var userToUpdate = _userRepository.GetUserByUsername(username);
            if (userToUpdate != null)
            {
                userToUpdate.SubscriptionId = subId;
                return subId;
            }
            throw new ArgumentException("The user you are looking for doesn´t exist.");

        }

        public List<Subscription> GetAllSubscriptions()
        {
            return _subscriptionRepository.GetAllSubs();
        }

        public int GetMaxConversion(SubscriptionType type)
        {
            return _subscriptionRepository.GetMaxConversions(type);
        }
    }
}
