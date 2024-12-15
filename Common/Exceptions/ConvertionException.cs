using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class ConvertionException
    {
        public class SomethingWentWrongInTheConvertionException : Exception
        {
            public SomethingWentWrongInTheConvertionException() : base("something went wrong while we where trying to make the convertion.") { }
            public SomethingWentWrongInTheConvertionException(string message) : base(message) { }
        }
        public class NotAbleGetUserConvertions : Exception
        {
            public NotAbleGetUserConvertions() : base("something went wrong while we where trying get the user convertions.") { }
            public NotAbleGetUserConvertions(string message) : base(message) { }
        }
        public class DontHaveMoreConvertions : Exception
        {
            public DontHaveMoreConvertions() : base("You do not have more convertions.") { }
            public DontHaveMoreConvertions(string message) : base(message) { }
        }
    }
}
