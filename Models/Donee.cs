using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    public class Donee : User
    {
        private string _organization;
        private readonly List<Request>? _requests;
        public string Organization { get { return _organization; } }
        public List<Request> Requests { get { return _requests; } }

        public Donee(int id, string username, string password, string email, string phoneNumber, string organization) : base(id, username, password, email, phoneNumber)
        {
            _organization = organization;
        }

        public static Donee From(DataRow dataRow)
        {
            string[] expectedFields = { "donee_id", "username", "password", "email", "phone_number", "organization" };
            foreach (string field in expectedFields)
            {
                if (!dataRow.Table.Columns.Contains(field)) throw new MissingFieldException($"The field {field} does not exist.");
            }

            Donee donee = new Donee(
                (int)dataRow["donee_id"],
                (string)dataRow["username"],
                (string)dataRow["password"],
                (string)dataRow["email"],
                (string)dataRow["phone_number"],
                (string)dataRow["organization"]
            );

            if (dataRow.Table.Columns.Contains("display_name") && dataRow["display_name"] != DBNull.Value) donee.ChangeDisplayName((string)dataRow["display_name"]);
            if (dataRow.Table.Columns.Contains("bio") && dataRow["bio"] != DBNull.Value) donee.ChangeBio((string)dataRow["bio"]);
            if (dataRow.Table.Columns.Contains("image") && dataRow["image"] != DBNull.Value) donee.ChangeImage((byte[])dataRow["image"]);

            return donee;
        }
        /// <summary>
        /// Adds a new request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool AddRequest(Request request)
        {
            if (_requests.Contains(request)) return false;
            _requests.Add(request);
            return true;
        }
        /// <summary>
        /// Removes an existing request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool RemoveRequest(Request request)
        {
            if (!_requests.Contains(request)) return false;
            _requests.Remove(request);
            return true;
        }
        /// <summary>
        /// Creates a new request.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CreateRequest(Leftover item, string title, string description)
        {
            /*
            Request request = new Request(new List<Leftover> { item } , title, description, DateTime.Now, this);
            AddRequest(request); 
            */
            return true;
        }
        /// <summary>
        /// Creates a new request.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CreateRequest(List<Leftover> item, string title, string description)
        {
            /*
            Request request = new Request(item, title, description, DateTime.Now, this);
            AddRequest(request);
            */
            return true;
        }
        public bool Approve(Donation donation) {
            if (!_requests.Contains(donation.Request)) return false;
            return donation.Approve();
        }
        public bool ApproveDonorInRequest(Request request, Donor donor)
        {
            if (!_requests.Contains(request)) return false;
            Donation? donation = request.FindDonationByDonor(donor);
            if (donation == null) return false;
            donation.Approve();
            return true;
        }
        public bool Reject(Donation donation) {
            if (!_requests.Contains(donation.Request)) return false;
            return donation.Reject(); 
        }
        public bool RejectDonorInRequest(Request request, Donor donor)
        {
            if (!_requests.Contains(request)) return false;
            Donation? donation = request.FindDonationByDonor(donor);
            if (donation == null) return false;
            donation.Reject();
            return true;
        }
        public bool ChangeOrganization(string newOrganization)
        {
            if (_organization.Equals(newOrganization)) return false;
            _organization = newOrganization;
            return true;
        }
    }
}
