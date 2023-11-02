﻿using Leftover_Harmony.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Services
{
    /// <summary>
    /// Handles data retrieval and updates operations for class objects.
    /// </summary>
    public class DataAccessProvider
    {
        private static DataAccessProvider? _instance;
        private NpgsqlConnection? conn;
        private bool _caching = false;

        /// <summary>
        /// The singleton instance of the DataFetcher class.
        /// </summary>
        public static DataAccessProvider Instance { get { if (_instance == null) _instance = new DataAccessProvider(); return _instance; } }
        public DataAccessProvider() { }

        public void Connect(string connectionString)
        {
            conn = new NpgsqlConnection(connectionString);
        }

        public bool Caching() { return _caching; }
        public void Caching(bool value) { _caching = value; }

        #region Base Fetchers
        /// <summary>
        /// Retrieves a Donor by their ID.
        /// </summary>
        /// <param name="donor_id">The ID of the donor to fetch.</param>
        /// <returns>The Donor object retrieved based on the provided ID.</returns>
        public Donor FetchDonor(int donor_id)
        {
            if (_caching && VirtualDatabase.Instance.Donors.ContainsKey(donor_id)) return VirtualDatabase.Instance.Donors[donor_id];

            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donor\" WHERE donor_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donor_id);

            dataTable.Load(cmd.ExecuteReader());
            Donor donor = Donor.From(dataTable.Rows[0]);

            conn.Close();

            if (_caching) VirtualDatabase.Instance.Donors.Add(donor_id, donor);

            return donor;
        }
        /// <summary>
        /// Retrieves a Donation by its ID.
        /// </summary>
        /// <param name="donation_id">The ID of the donation to fetch.</param>
        /// <returns>The Donation object retrieved based on the provided ID.</returns>
        public Donation FetchDonation(int donation_id)
        {
            if (_caching && VirtualDatabase.Instance.Donations.ContainsKey(donation_id)) return VirtualDatabase.Instance.Donations[donation_id];

            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation_id);

            dataTable.Load(cmd.ExecuteReader());
            Donation donation = Donation.From(dataTable.Rows[0]);

            conn.Close();

            if (_caching) VirtualDatabase.Instance.Donations.Add(donation_id, donation);

            return donation;
        }
        /// <summary>
        /// Retrieves a Request by its ID.
        /// </summary>
        /// <param name="request_id">The ID of the request to fetch.</param>
        /// <returns>The Request object retrieved based on the provided ID.</returns>
        public Request FetchRequest(int request_id)
        {
            if (_caching && VirtualDatabase.Instance.Requests.ContainsKey(request_id)) return VirtualDatabase.Instance.Requests[request_id];

            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Request\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(cmd.ExecuteReader());
            Request request = Request.From(dataTable.Rows[0]);

            conn.Close();

            if (_caching) VirtualDatabase.Instance.Requests.Add(request_id, request);

            return request;
        }
        /// <summary>
        /// Retrieves a Leftover by its ID.
        /// </summary>
        /// <param name="leftover_id">The ID of the leftover to fetch.</param>
        /// <returns>The Leftover object retrieved based on the provided ID.</returns>
        public Leftover FetchLeftover(int leftover_id)
        {
            if (_caching && VirtualDatabase.Instance.Leftovers.ContainsKey(leftover_id)) return VirtualDatabase.Instance.Leftovers[leftover_id];

            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Leftover\" WHERE leftover_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", leftover_id);

            dataTable.Load(cmd.ExecuteReader());
            Leftover leftover = Leftover.From(dataTable.Rows[0]);

            conn.Close();

            if (_caching) VirtualDatabase.Instance.Leftovers.Add(leftover_id, leftover);

            return leftover;
        }
        /// <summary>
        /// Retrieves a Donee by its ID.
        /// </summary>
        /// <param name="donee_id">The ID of the donee to fetch.</param>
        /// <returns>The Donee object retrieved based on the provided ID.</returns>
        public Donee FetchDonee(int donee_id)
        {
            if (_caching && VirtualDatabase.Instance.Donees.ContainsKey(donee_id)) return VirtualDatabase.Instance.Donees[donee_id];

            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donee\" WHERE donee_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donee_id);

            dataTable.Load(cmd.ExecuteReader());
            Donee donee = Donee.From(dataTable.Rows[0]);

            conn.Close();

            VirtualDatabase.Instance.Donees.Add(donee_id, donee);

            return donee;
        }
        #endregion

        #region Specific Fetchers
        /// <summary>
        /// Retrieves a Request associated with a specific Donation.
        /// </summary>
        /// <param name="donation">The Donation object for which the associated Request is to be fetched.</param>
        /// <returns>The Request object associated with the provided Donation.</returns>
        public Request FetchRequest(Donation donation)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT request_id FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation.Id);

            dataTable.Load(cmd.ExecuteReader());
            int request_id = (int)dataTable.Rows[0]["request_id"];

            conn.Close();

            return FetchRequest(request_id);
        }
        /// <summary>
        /// Retrieves a Donor associated with a specific Donation.
        /// </summary>
        /// <param name="donation">The Donation object for which the associated Donor is to be fetched.</param>
        /// <returns>The Donor object associated with the provided Donation.</returns>
        public Donor FetchDonor(Donation donation)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT donor_id FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation.Id);

            dataTable.Load(cmd.ExecuteReader());
            int donor_id = (int)dataTable.Rows[0]["donor_id"];

            conn.Close();

            return FetchDonor(donor_id);
        }
        /// <summary>
        /// Retrieves a Donee associated with a specific Request.
        /// </summary>
        /// <param name="request">The Request object for which the associated Donee is to be fetched.</param>
        /// <returns>The Donee object associated with the provided Request.</returns>
        public Donee FetchDonee(Request request)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT donee_id FROM \"Request\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request.Id);

            dataTable.Load(cmd.ExecuteReader());
            int donee_id = (int)dataTable.Rows[0]["donee_id"];

            conn.Close();

            return FetchDonee(donee_id);
        }
        /// <summary>
        /// Retrieves a Leftover associated with a specific Donation.
        /// </summary>
        /// <param name="donation">The Donation object for which the associated Leftover is to be fetched.</param>
        /// <returns>The Leftover object associated with the provided Donation.</returns>
        public Leftover FetchLeftover(Donation donation)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT leftover_id FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation.Id);

            dataTable.Load(cmd.ExecuteReader());
            int leftover_id = (int)dataTable.Rows[0]["leftover_id"];

            conn.Close();

            return FetchLeftover(leftover_id);
        }
        /// <summary>
        /// Retrieves a list of Requests associated with a specific Donee by their ID.
        /// </summary>
        /// <param name="donee_id">The ID of the Donee for which associated Requests are to be fetched.</param>
        /// <returns>A list of Request objects associated with the provided Donee ID.</returns>
        public List<Request> FetchDoneeRequests(int donee_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Request> requests = new List<Request>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT request_id FROM \"Request\" WHERE donee_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donee_id);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                requests.Add(FetchRequest((int)row["request_id"]));
            }

            conn.Close();

            return requests;
        }
        /// <summary>
        /// Retrieves a list of Requests associated with a specific Donee.
        /// </summary>
        /// <param name="donee">The Donee object for which associated Requests are to be fetched.</param>
        /// <returns>A list of Request objects associated with the provided Donee.</returns>
        public List<Request> FetchDoneeRequests(Donee donee) { return FetchDoneeRequests(donee.Id); }
        /// <summary>
        /// Retrieves a list of Donations associated with a specific Donor by their ID.
        /// </summary>
        /// <param name="donor_id">The ID of the Donor for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Donor ID.</returns>
        public List<Donation> FetchDonorDonations(int donor_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Donation> donations = new List<Donation>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT donation_id FROM \"Donation\" WHERE donor_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donor_id);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                donations.Add(FetchDonation((int)row["donation_id"]));
            }

            conn.Close();

            return donations;
        }
        /// <summary>
        /// Retrieves a list of Donations associated with a specific Donor.
        /// </summary>
        /// <param name="donor">The Donor object for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Donor.</returns>
        public List<Donation> FetchDonorDonations(Donor donor) { return FetchDonorDonations(donor.Id); }
        /// <summary>
        /// Retrieves a list of Leftovers associated with a specific Request by its ID.
        /// </summary>
        /// <param name="request_id">The ID of the Request for which associated Leftovers are to be fetched.</param>
        /// <returns>A list of Leftover objects associated with the provided Request ID.</returns>
        public List<Leftover> FetchRequestLeftover(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Leftover> leftovers = new List<Leftover>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT leftover_id FROM \"RequestLeftover\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                leftovers.Add(FetchLeftover((int)row["request_id"]));
            }

            conn.Close();

            return leftovers;
        }
        /// <summary>
        /// Retrieves a list of Leftovers associated with a specific Request.
        /// </summary>
        /// <param name="request">The Request object for which associated Leftovers are to be fetched.</param>
        /// <returns>A list of Leftover objects associated with the provided Request.</returns>
        public List<Leftover> FetchRequestLeftover(Request request) { return FetchRequestLeftover(request.Id); }
        /// <summary>
        /// Retrieves a list of Donations associated with a specific Request by its ID.
        /// </summary>
        /// <param name="request_id">The ID of the Request for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Request ID.</returns>
        public List<Donation> FetchRequestDonation(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Donation> donations = new List<Donation>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT donation_id FROM \"Donation\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                donations.Add(FetchDonation((int)row["donation_id"]));
            }

            conn.Close();

            return donations;
        }
        /// <summary>
        /// Retrieves a list of Donations associated with a specific Request.
        /// </summary>
        /// <param name="request">The Request object for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Request.</returns>
        public List<Donation> FetchRequestDonation(Request request) { return FetchRequestDonation(request.Id); }
        #endregion

        public bool UpdateDonor(Donor donor)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE \"Donor\" " +
                "SET " +
                "username = :_username, " +
                "password = :_password, " +
                "email = :_email, " +
                "phone_number = :_phone_number, " +
                "display_name = :_display_name, " +
                "bio = :_bio, " +
                "image = :_image " +
                "WHERE donor_id = :_id "
                , conn);
            cmd.Parameters.AddWithValue("_id", donor.Id);
            cmd.Parameters.AddWithValue("_username", donor.Username);
            cmd.Parameters.AddWithValue("_password", donor.Password);
            cmd.Parameters.AddWithValue("_email", donor.Email);
            cmd.Parameters.AddWithValue("_phone_number", donor.PhoneNumber);
            cmd.Parameters.AddWithValue("_display_name", (donor.DisplayName != null)? donor.DisplayName : DBNull.Value);
            cmd.Parameters.AddWithValue("_bio", (donor.Bio != null) ? donor.Bio : DBNull.Value);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (donor.Image != null) ? donor.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            return true;
        }
    }
}