using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    internal class Donor : User
    {
        public Donor(string username, string password, string email, string phoneNumber) : base(username, password, email, phoneNumber)
        {
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
