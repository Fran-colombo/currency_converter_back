using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class SomethingWentWrongInTheConvertionException : Exception
    {
        public SomethingWentWrongInTheConvertionException() : base("something went wrong while we where trying to make the convertion.") { }
        public SomethingWentWrongInTheConvertionException(string message) : base(message) { }
    }
    public class CurrencyAlreadyExistException : Exception
    {
        public CurrencyAlreadyExistException() : base("The currency already exists.") { }
        public CurrencyAlreadyExistException(string message) : base(message) { }
    }
    public class CurrencyNotExistException : Exception
    {
        public CurrencyNotExistException() : base("The currency you are looking for does not exist.") { }
        public CurrencyNotExistException(string message) : base(message) { }
    }
    public class NotAbleCreateCurrencyException : Exception
    {
        public NotAbleCreateCurrencyException() : base("We couldn´t create the currency.") { }
        public NotAbleCreateCurrencyException(string message) : base(message) { }
    }
    public class NotAbleDeleteCurrency : Exception
    {
        public NotAbleDeleteCurrency() : base("We could not delete the currency.") { }
        public NotAbleDeleteCurrency(string message) : base(message) { }
    }
    public class NotAbleUpdateCurrencyException : Exception
    {
        public NotAbleUpdateCurrencyException() : base("We couldn´t create the currency.") { }
        public NotAbleUpdateCurrencyException(string message) : base(message) { }
    }
    public class NotAbleFindCurrencyException : Exception
    {
        public NotAbleFindCurrencyException() : base("We couldn´t find the currency/ies.") { }
        public NotAbleFindCurrencyException(string message) : base(message) { }
    }
    public class ConvertionIndexLowerThanZeroException : Exception
    {
        public ConvertionIndexLowerThanZeroException() : base("The convertion index never can be lower than 0.") { }
        public ConvertionIndexLowerThanZeroException(string message) : base(message) { }
    }
}
