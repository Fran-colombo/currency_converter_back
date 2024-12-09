using Common.Enum;
using Common.Exceptions;
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
            if ((userToUpdate != null) & (0 < subId & subId < 4))
            {
                try
                {
                    userToUpdate.SubscriptionId = subId;
                    return subId;
                }
                catch (Exception ex)
                {
                    throw new CouldNotUpdateSubscriptionException("Something went wrong while you tried to change subscription");
                }

            }
            else if (userToUpdate == null)
            {
                throw new UserNotFoundException($"{username}you are looking for doesn´t exist.");
            }
            throw new SubscriptionIdNotFoundException("The subscriptionId you are looking for doesn´t exist.");

        }

        public List<Subscription> GetAllSubscriptions()
        {
            return _subscriptionRepository.GetAllSubs();
        }

        public int GetMaxConversion(SubscriptionType? type)
        {
            if (type == null) return 0;
            try
            {
                return _subscriptionRepository.GetMaxConversions(type);
            }
            catch (Exception ex)
            {
                throw new MaxConvertionsNotFound("We couldn´t get the max convertions");
            }
        }
    }
}
