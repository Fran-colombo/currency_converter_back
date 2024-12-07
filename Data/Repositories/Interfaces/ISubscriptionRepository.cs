using Common.Enum;
using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        int ChangeSubsctiption(int id, int subId);
        SubscriptionType CreateSubscription(Subscription sub);
        int GetMaxConversions(SubscriptionType? type);
        List<Subscription> GetAllSubs();
        Subscription? GetSubscriptionById(int id);
        Subscription? GetSubscriptionByType(SubscriptionType type);
    }
}