using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    public class Donor : User
    {
        // private List<Donation>? _donations;
        public List<Donation> Donations {
            get {
                /*
                if (_donations == null) _donations = DataFetcher.Instance.FetchDonorDonations(this);
                return _donations;
                */
                return DataFetcher.Instance.FetchDonorDonations(this);
            }
        }

        public Donor(int id, string username, string password, string email, string phoneNumber) : base(id, username, password, email, phoneNumber) { }

        public static Donor From(DataRow dataRow)
        {
            string[] expectedFields = { "donor_id", "username", "password", "email", "phone_number" };
            foreach (string field in expectedFields)
            {
                if (!dataRow.Table.Columns.Contains(field)) throw new MissingFieldException($"The field {field} does not exist.");
            }

            Donor donor = new Donor(
                (int)dataRow["donor_id"],
                (string)dataRow["username"],
                (string)dataRow["password"],
                (string)dataRow["email"],
                (string)dataRow["phone_number"]
            );

            if (dataRow.Table.Columns.Contains("display_name") && dataRow["display_name"] != DBNull.Value) donor.ChangeDisplayName((string)dataRow["display_name"]);
            if (dataRow.Table.Columns.Contains("bio") && dataRow["bio"] != DBNull.Value) donor.ChangeBio((string)dataRow["bio"]);
            if (dataRow.Table.Columns.Contains("image") && dataRow["image"] != DBNull.Value) donor.ChangeImage((byte[])dataRow["image"]);

            return donor;
        }

        public bool Donate(Request request)
        {
            if (request == null) return false;

            // NOT IMPLEMENTED

            /*
            Donation donation = new Donation(this, request);
            request.AddDonation(donation);
            _donations.Add(donation);
            */
            return true;
        }
        public bool CancelDonation(Donation donation)
        {
            if (!Donations.Contains(donation)) return false;

            // NOT IMPLEMENTED

            // donation.Cancel();

            return true;
        }
        public bool CancelDonationOn(Request request)
        {
            // NOT IMPLEMENTED

            // Donation? donation = request.FindDonationByDonor(this);
            // if (donation == null) return false;
            // donation.Cancel();
            return true;
        }
    }
}
