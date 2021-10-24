using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Common.Events
{
    public class UserAuthenticated : IEvent
    {
        public UserAuthenticated()
        {
        }

        public UserAuthenticated(string email)
        {
            Email = email;
        }

        public string Email { get; }



    }
}