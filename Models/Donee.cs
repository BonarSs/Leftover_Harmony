using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    internal class Donee : User
    {
        private string _organization;
        private List<Request> _requests;
        public List<Request> Requests { get { return _requests; } }
        public Donee(string username, string password, string email, string phoneNumber) : base(username, password, email, phoneNumber)
        {
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Boolean AddRequest(Request request)
        {
            if (_requests.Contains(request)) return false;
            _requests.Add(request);
            return true;
        }
        public Boolean CancelRequest(Request request)
        {
            if (!_requests.Contains(request)) return false;
            _requests.Remove(request);
            return true;
        }
        public Boolean Approve(Request request, Donor donor)
        {
            if (!_requests.Contains(request)) return false;
            if (!request.Donors.Contains(donor)) return false;
            request.Approve(donor);
            donor.ApproveRequest(request);
            return true;
        }
        public Boolean Reject(Request request, Donor donor)
        {
            if (!_requests.Contains(request)) return false;
            if (!request.Donors.Contains(donor)) return false;
            request.Reject(donor);
            donor.RejectRequest(request);
            return true;
        }
    }
}
