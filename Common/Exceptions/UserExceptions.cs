using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }
        public UserNotFoundException(string message) : base(message) { }
    }
    public class UsersNotFoundException : Exception
    {
        public UsersNotFoundException() : base("We weren´t able to find the user/s you were looking for.") { }
        public UsersNotFoundException(string message) : base(message) { }
    }
    public class NotAbleCreateUser : Exception
    {
        public NotAbleCreateUser() : base("Something went wrong, we couldn´t create the user.") { }
        public NotAbleCreateUser(string message) : base(message) { }
    }
    public class NotAbleDeleteUser : Exception
    {
        public NotAbleDeleteUser() : base("Something went wrong, we couldn´t create the user.") { }
        public NotAbleDeleteUser(string message) : base(message) { }
    }
    public class FailUpdatingUserException : Exception
    {
        public FailUpdatingUserException() : base("Something went wrong, we couldn´t update the user.") { }
        public FailUpdatingUserException(string message) : base(message) { }
    }
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException() : base("Something went wrong, we couldn´t create the user.") { }
        public UserAlreadyExistException(string message) : base(message) { }
    }
    public class EmailAlreadyExistException : Exception
    {
        public EmailAlreadyExistException() : base("The eail you tried to use already exists.") { }
        public EmailAlreadyExistException(string message) : base(message) { }
    }
}
