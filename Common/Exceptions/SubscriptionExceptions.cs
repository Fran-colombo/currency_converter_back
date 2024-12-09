using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class SubscriptionIdNotFoundException : Exception
    {
        public SubscriptionIdNotFoundException() : base("Subscription id not found.") { }
        public SubscriptionIdNotFoundException(string message) : base(message) { }
    }
    public class MaxConvertionsNotFound : Exception
    {
        public MaxConvertionsNotFound() : base("Max convertions not found.") { }
        public MaxConvertionsNotFound(string message) : base(message) { }
    }
    public class CouldNotUpdateSubscriptionException : Exception
    {
        public CouldNotUpdateSubscriptionException() : base("Something went wrong, we could not update the subscription.") { }
        public CouldNotUpdateSubscriptionException(string message) : base(message) { }
    }
}
