using Common.Enum;
using Data;
using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly CurrencyContext _context;

        public SubscriptionRepository(CurrencyContext context)
        {
            _context = context;
        }

        public SubscriptionType CreateSubscription(Subscription sub)
        {

            _context.Add(sub);
            _context.SaveChanges();
            return sub.SubscriptionType;
        }
        public List<Subscription> GetAllSubs()
        {
            return _context.Subscriptions.ToList();
        }
        public Subscription? GetSubscriptionById(int id)
        {
            return _context.Subscriptions.FirstOrDefault(s => s.Id == id);
        }

        public Subscription? GetSubscriptionByType(SubscriptionType type)
        {
            return _context.Subscriptions.FirstOrDefault(s => s.SubscriptionType == type);
        }

        public int ChangeSubsctiption(int id, int subId)
        {
            var usersubtype = _context.Users.SingleOrDefault(u => u.Id == id);
            try
            {
                usersubtype.SubscriptionId = subId;
                _context.SaveChanges();
                return id;
            }
            catch (Exception ex) 
            {
                throw new ArgumentException("Something ocurred and you weren´t able to change the subscription");
            }
            //return null;

        }
        public int GetMaxConversions(SubscriptionType? type)
        {
            if(type == null) return 0;
            return type switch
            {
                SubscriptionType.Free => 10,
                SubscriptionType.Trial => 100,
                SubscriptionType.Pro => int.MaxValue,
                _ => 0
            };
        }
    }
}
