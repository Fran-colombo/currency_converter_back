using Common.Enum;
using Data.Entities;

namespace Services.Interfaces
{
    public interface ISubscriptionService
    {
        SubscriptionType CreateSubscription(Subscription sub);
        List<Subscription> GetAllSubscriptions();
        int ChangeSubscription(string username , int subId);
        int GetMaxConversion(SubscriptionType? type);
    }
}