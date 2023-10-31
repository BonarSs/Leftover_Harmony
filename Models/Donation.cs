using Leftover_Harmony.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    /// <summary>
    /// Represents status of donation.
    /// </summary>
    public enum DonationStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
    /// <summary>
    /// Represents a donation made by a donor to a request within a donation system.
    /// </summary>
    public class Donation
    {
        private readonly int _id;
        private DonationStatus _status;
        private string _description;
        private DateTime _date_donated;
        private int _amount;
        private Donor? _donor;
        private Request? _request;
        private Leftover? _leftover;

        public int Id { get { return _id; } }
        public DonationStatus Status { get { return _status; } }
        public string Description { get { return _description; } }
        public DateTime DateDonated { get { return _date_donated; } }
        public int Amount { get { return _amount; } }
        public Donor Donor { 
            get {
                /*
                if (_donor == null) _donor = DataFetcher.Instance.FetchDonor(this);
                return _donor; 
                */
                return DataFetcher.Instance.FetchDonor(this);
            } 
        }
        public Request Request {
            get {
                /*
                if (_request == null) _request = DataFetcher.Instance.FetchRequest(this);
                return _request;
                */
                return DataFetcher.Instance.FetchRequest(this);
            }
        }
        public Leftover Leftover
        {
            get
            {
                /*
                if (_leftover == null) _leftover = DataFetcher.Instance.FetchLeftover(this);
                return _leftover;
                */
                return DataFetcher.Instance.FetchLeftover(this);
            }
        }
        

        public Donation(int id, DonationStatus status, string description, DateTime date, int amount)
        {
            _id = id;
            _status = status;
            _description = description;
            _date_donated = date;
            _amount = amount;
        }

        public static Donation From(DataRow dataRow)
        {
            string[] expectedFields = { "donation_id", "status", "description", "date_donated", "amount" };
            foreach (string field in expectedFields)
            {
                if (!dataRow.Table.Columns.Contains(field)) throw new MissingFieldException($"The field {field} does not exist.");
            }

            DonationStatus status;
            if (!Enum.TryParse((string)dataRow["status"], out status)) status = DonationStatus.Pending;

            string description = dataRow["description"] != DBNull.Value ? (string)dataRow["description"] : "";

            return new Donation(
                (int)dataRow["donation_id"],
                status,
                description,
                (DateTime)dataRow["date_donated"],
                (int)dataRow["amount"]
            );
        }

        /// <summary>
        /// Changes the status of the donation to Approved.
        /// </summary>
        /// <returns></returns>
        public bool Approve()
        {
            if (_status != DonationStatus.Pending) { return false; }
            _status = DonationStatus.Approved;
            return true;
        }
        /// <summary>
        /// Changes the status of the donation to Rejected.
        /// </summary>
        /// <returns></returns>
        public bool Reject()
        {
            if (_status != DonationStatus.Pending) { return false; }
            _status = DonationStatus.Rejected;
            return true;
        }
        /// <summary>
        /// Changes the status of the donation to Cancelled.
        /// </summary>
        /// <returns></returns>
        public bool Cancel()
        {
            _status = DonationStatus.Cancelled;
            return true;
        }
    }
}
