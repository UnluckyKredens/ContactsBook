using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Domain.Exceptions
{
    public class UserOperationException : Exception
    {
        public UserOperationException() : base() { }
        public UserOperationException(string message) : base(message) { }
    }
}
