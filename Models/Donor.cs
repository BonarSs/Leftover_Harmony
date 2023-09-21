using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    internal class Donor : User
    {
        private List<Request> _requests;
        private List<Request> _approvedRequest;
        private List<Request> _rejectedRequest;

        public Donor(string username, string password, string email, string phoneNumber) : base(username, password, email, phoneNumber)
        {
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public bool Donate(Request request)
        {
            if (request == null) return false;
            request.AddDonor(this);
            _requests.Add(request);
            return true;
        }
        public bool ApproveRequest(Request request)
        {
            if (!_requests.Contains(request)) return false;
            _approvedRequest.Add(request);
            _requests.Remove(request);
            return true;
        }
        public bool RejectRequest(Request request)
        {
            if (!_requests.Contains(request)) return false;
            _rejectedRequest.Add(request);
            _requests.Remove(request);
            return true;
        }
    }
}
