using Leftover_Harmony.Models;
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
    /// Class that fetches class objects from the database.
    /// </summary>
    public class DataFetcher
    {
        private static DataFetcher? _instance;
        private NpgsqlConnection? conn;
        private bool _caching = false;

        public static DataFetcher Instance { get { if (_instance == null) _instance = new DataFetcher(); return _instance; } }
        public DataFetcher() { }

        public void Connect(string connectionString)
        {
            conn = new NpgsqlConnection(connectionString);
        }

        public bool Caching() { return _caching; }
        public void Caching(bool value) { _caching = value; }

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

            VirtualDatabase.Instance.Donors.Add(donor_id, donor);

            return donor;
        }
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

            VirtualDatabase.Instance.Donations.Add(donation_id, donation);

            return donation;
        }
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

            VirtualDatabase.Instance.Requests.Add(request_id, request);

            return request;
        }
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

            VirtualDatabase.Instance.Leftovers.Add(leftover_id, leftover);

            return leftover;
        }
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
        public List<Donation> FetchDonorDonations(Donor donor) { return FetchDonorDonations(donor.Id); }
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
        public List<Leftover> FetchRequestLeftover(Request request) { return FetchRequestLeftover(request.Id); }
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
        public List<Donation> FetchRequestDonation(Request request) { return FetchRequestDonation(request.Id); }
    }
}
