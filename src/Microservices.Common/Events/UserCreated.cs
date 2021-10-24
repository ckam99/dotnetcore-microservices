using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Common.Events
{
    public class UserCreated : IEvent
    {


        public string Email { get; }
        public string Name { get; }

        public UserCreated()
        {

        }

        public UserCreated(string email, string name)
        {
            Email = email;
            Name = name;
        }

    }
}