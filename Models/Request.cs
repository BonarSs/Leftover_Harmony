using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    public class Request
    {
        private readonly int _id;
        private string _title;
        private string _description;
        private DateTime _date_created;

        private byte[]? _image; 
        // private List<Leftover>? _leftovers;
        // private Donee? _donee;
        // private List<Donation>? _donations;

        public int Id { get { return _id; } }
        
        public string Title { get { return _title; } set { _title = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public DateTime Date { get { return _date_created; } }
        public Donee Donee { get { return DataAccessProvider.Instance.FetchDonee(this); } }
        public List<Leftover> Leftovers { get { return DataAccessProvider.Instance.FetchRequestLeftover(this); } }
        public List<Donation> Donations { get { return DataAccessProvider.Instance.FetchRequestDonation(this); } }

        public Request(int id, string title, string description, DateTime date)
        {
            _id = id;
            _title = title;
            _description = description;
            _date_created = date;
        }
        public static Request From(DataRow dataRow)
        {
            string[] expectedFields = { "request_id", "title", "description", "date_created" };
            foreach (string field in expectedFields)
            {
                if (!dataRow.Table.Columns.Contains(field)) throw new MissingFieldException($"The field {field} does not exist.");
            }

            string description = dataRow["description"] != DBNull.Value ? (string)dataRow["description"] : "";

            Request request = new Request(
                (int)dataRow["request_id"],
                (string)dataRow["title"],
                description,
                (DateTime)dataRow["date_created"]
            );

            if (dataRow.Table.Columns.Contains("image") && dataRow["image"] != DBNull.Value) request.SetImage((byte[])dataRow["image"]);
            return request;
        }
        public void SetImage(byte[] image) { _image = image; }
        public List<Donation> AllDonations()
        {
            return Donations;
        }
        public List<Donation> ApprovedDonations()
        {
            return Donations.Where(donation => donation.Status == DonationStatus.Approved).ToList();   
        }
        public List<Donation> RejectedDonations()
        {
            return Donations.Where(donation => donation.Status == DonationStatus.Rejected).ToList();
        }
        public bool AddDonation(Donation donation)
        {
            if (Donations.Contains(donation)) { return false; }
            Donations.Add(donation);
            return true;
        }
        public bool RemoveDonation(Donation donation)
        {
            if (!Donations.Contains(donation)) { return false; }
            Donations.Remove(donation);
            return true;
        }
        public Donation? FindDonationByDonor(Donor donor)
        {
            try
            {
                return Donations.Find(donation => donation.Donor == donor);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}
