using Leftover_Harmony.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        /// <summary>
        /// The singleton instance of the DataFetcher class.
        /// </summary>
        public static DataAccessProvider Instance { get { if (_instance == null) _instance = new DataAccessProvider(); return _instance; } }
        public DataAccessProvider() { }

        public void Connect(string connectionString)
        {
            conn = new NpgsqlConnection(connectionString);
        }

        #region Base Fetchers
        /// <summary>
        /// Retrieves a Donee by its ID.
        /// </summary>
        /// <param name="donee_id">The ID of the donee to fetch.</param>
        /// <returns>The Donee object retrieved based on the provided ID.</returns>
        public Donee FetchDonee(int donee_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donee\" WHERE donee_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donee_id);

            dataTable.Load(cmd.ExecuteReader());
            Donee donee = Donee.From(dataTable.Rows[0]);

            conn.Close();

            return donee;
        }
        /// <summary>
        /// Asynchronously retrieves a Donee by its ID.
        /// </summary>
        /// <param name="donee_id">The ID of the donee to fetch.</param>
        /// <returns>The Donee object retrieved based on the provided ID.</returns>
        public async Task<Donee> FetchDoneeAsync(int donee_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donee\" WHERE donee_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donee_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            Donee donee = Donee.From(dataTable.Rows[0]);

            conn.Close();

            return donee;
        }
        /// <summary>
        /// Retrieves a Donor by their ID.
        /// </summary>
        /// <param name="donor_id">The ID of the donor to fetch.</param>
        /// <returns>The Donor object retrieved based on the provided ID.</returns>
        public Donor FetchDonor(int donor_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donor\" WHERE donor_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donor_id);

            dataTable.Load(cmd.ExecuteReader());
            Donor donor = Donor.From(dataTable.Rows[0]);

            conn.Close();

            return donor;
        }
        /// <summary>
        /// Asynchronously retrieves a Donor by their ID.
        /// </summary>
        /// <param name="donor_id">The ID of the donor to fetch.</param>
        /// <returns>The Donor object retrieved based on the provided ID.</returns>
        public async Task<Donor> FetchDonorAsync(int donor_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donor\" WHERE donor_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donor_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            Donor donor = Donor.From(dataTable.Rows[0]);

            conn.Close();

            return donor;
        }
        /// <summary>
        /// Retrieves a Donation by its ID.
        /// </summary>
        /// <param name="donation_id">The ID of the donation to fetch.</param>
        /// <returns>The Donation object retrieved based on the provided ID.</returns>
        public Donation FetchDonation(int donation_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation_id);

            dataTable.Load(cmd.ExecuteReader());
            Donation donation = Donation.From(dataTable.Rows[0]);

            conn.Close();

            return donation;
        }
        /// <summary>
        /// Asynchronously retrieves a Donation by its ID.
        /// </summary>
        /// <param name="donation_id">The ID of the donation to fetch.</param>
        /// <returns>The Donation object retrieved based on the provided ID.</returns>
        public async Task<Donation> FetchDonationAsync(int donation_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Donation\" WHERE donation_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donation_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            Donation donation = Donation.From(dataTable.Rows[0]);

            conn.Close();

            return donation;
        }
        /// <summary>
        /// Retrieves a Request by its ID.
        /// </summary>
        /// <param name="request_id">The ID of the request to fetch.</param>
        /// <returns>The Request object retrieved based on the provided ID.</returns>
        public Request FetchRequest(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Request\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(cmd.ExecuteReader());
            Request request = Request.From(dataTable.Rows[0]);

            conn.Close();

            return request;
        }
        /// <summary>
        /// Asynchronously retrieves a Request by its ID.
        /// </summary>
        /// <param name="request_id">The ID of the request to fetch.</param>
        /// <returns>The Request object retrieved based on the provided ID.</returns>
        public async Task<Request> FetchRequestAsync(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Request\" WHERE request_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            Request request = Request.From(dataTable.Rows[0]);

            conn.Close();

            return request;
        }
        /// <summary>
        /// Retrieves a Leftover by its ID.
        /// </summary>
        /// <param name="leftover_id">The ID of the leftover to fetch.</param>
        /// <returns>The Leftover object retrieved based on the provided ID.</returns>
        public Leftover FetchLeftover(int leftover_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Leftover\" WHERE leftover_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", leftover_id);

            dataTable.Load(cmd.ExecuteReader());
            Leftover leftover = Leftover.From(dataTable.Rows[0]);

            conn.Close();

            return leftover;
        }
        
        #endregion

        #region Specific Fetchers
        /// <summary>
        /// Retrieves a user based on the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user to fetch.</param>
        /// <param name="password">The password of the user to fetch.</param>
        /// <returns>
        /// The User object associated with the provided username and password if found; 
        /// otherwise, returns null if no user matches the given credentials.
        /// </returns>
        public User? FetchUser(string username, string password)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM find_account(:_username, :_password);", conn);
            cmd.Parameters.AddWithValue("_username", username);
            cmd.Parameters.AddWithValue("_password", password);

            dataTable.Load(cmd.ExecuteReader());
            int account_type = (int)dataTable.Rows[0]["account_type"];
            int id = (int)dataTable.Rows[0]["id"];

            User? account = null;
            if (account_type == 0) account = FetchDonor(id);
            else if (account_type == 1) account = FetchDonee(id);

            conn.Close();

            return account;
        }
        /// <summary>
        /// Asynchronously retrieves a user based on the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user to fetch.</param>
        /// <param name="password">The password of the user to fetch.</param>
        /// <returns>
        /// The User object associated with the provided username and password if found; 
        /// otherwise, returns null if no user matches the given credentials.
        /// </returns>
        public async Task<User?> FetchUserAsync(string username, string password)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM find_account(:_username, :_password);", conn);
            cmd.Parameters.AddWithValue("_username", username);
            cmd.Parameters.AddWithValue("_password", password);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            int account_type = (int)dataTable.Rows[0]["account_type"];
            int id = (int)dataTable.Rows[0]["id"];

            User? account = null;
            if (account_type == 0)
                account = await FetchDonorAsync(id);
            else if (account_type == 1)
                account = await FetchDoneeAsync(id);

            conn.Close();
            return account;
        }
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
        /// Retrieves a list of all available requests.
        /// </summary>
        /// <returns>A List of Request objects containing all available requests.</returns>
        public List<Request> FetchAllRequests()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Request> requests = new List<Request>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM \"Request\"", conn);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                requests.Add(Request.From(row));
            }

            conn.Close();

            return requests;
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
        /// Asynchronously retrieves a list of Requests associated with a specific Donee by their ID.
        /// </summary>
        /// <param name="donee_id">The ID of the Donee for which associated Requests are to be fetched.</param>
        /// <returns>A list of Request objects associated with the provided Donee ID.</returns>
        public async Task<List<Request>> FetchDoneeRequestsAsync(int donee_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Request> requests = new List<Request>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT request_id FROM \"Request\" WHERE donee_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donee_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            foreach (DataRow row in dataTable.Rows)
            {
                requests.Add(FetchRequest((int)row["request_id"]));
            }

            conn.Close();

            return requests;
        }
        /// <summary>
        /// Asynchronously retrieves a list of Requests associated with a specific Donee.
        /// </summary>
        /// <param name="donee">The Donee object for which associated Requests are to be fetched.</param>
        /// <returns>A list of Request objects associated with the provided Donee.</returns>
        public async Task<List<Request>> FetchDoneeRequestsAsync(Donee donee) { return await FetchDoneeRequestsAsync((int)donee.Id); }
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
        /// Asynchronously retrieves a list of Donations associated with a specific Donor by their ID.
        /// </summary>
        /// <param name="donor_id">The ID of the Donor for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Donor ID.</returns>
        public async Task<List<Donation>> FetchDonorDonationsAsync(int donor_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<Donation> donations = new List<Donation>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT donation_id FROM \"Donation\" WHERE donor_id = :_id", conn);
            cmd.Parameters.AddWithValue("_id", donor_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            foreach (DataRow row in dataTable.Rows)
            {
                donations.Add(await FetchDonationAsync((int)row["donation_id"]));
            }

            conn.Close();

            return donations;
        }
        /// <summary>
        /// Asynchronously retrieves a list of Donations associated with a specific Donor.
        /// </summary>
        /// <param name="donor">The Donor object for which associated Donations are to be fetched.</param>
        /// <returns>A list of Donation objects associated with the provided Donor.</returns>
        public async Task<List<Donation>> FetchDonorDonationsAsync(Donor donor) { return await FetchDonorDonationsAsync(donor.Id); }
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
        public List<object> FetchRequestLeftoverAmounts(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<object> leftovers = new List<object>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT get_request_leftovers(:_id)", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(cmd.ExecuteReader());
            foreach (DataRow row in dataTable.Rows)
            {
                leftovers.Add(new {
                    Leftover = Leftover.From(row),
                    RequestedAmount = (int)row["requested_amount"],
                    DonatedAmount = (int)row["donated_amount"]
                });
            }

            conn.Close();

            return leftovers;
        }
        public List<object> FetchRequestLeftoverAmounts(Request request) { return FetchRequestLeftoverAmounts(request.Id); }
        public async Task<List<object>> FetchRequestLeftoverAmountsAsync(int request_id)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<object> leftovers = new List<object>();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_request_leftovers(:_id)", conn);
            cmd.Parameters.AddWithValue("_id", request_id);

            dataTable.Load(await cmd.ExecuteReaderAsync());
            // foreach (DataColumn col in dataTable.Columns) Trace.WriteLine(col.ColumnName);
            foreach (DataRow row in dataTable.Rows)
            {
                leftovers.Add(new
                {
                    Leftover = Leftover.From(row),
                    RequestedAmount = (int)row["requested_amount"],
                    DonatedAmount = (int)row["donated_amount"]
                });
            }

            conn.Close();

            return leftovers;
        }
        public async Task<List<object>> FetchRequestLeftoverAmountsAsync(Request request) { return await FetchRequestLeftoverAmountsAsync(request.Id); }
        public bool IsUsernameExists(string username)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM username_exists(:_username)", conn);
            cmd.Parameters.AddWithValue("_username", username);

            object? result = cmd.ExecuteScalar();
            if (result == null) return false;
            return (bool)result;
        }
        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM username_exists(:_username)", conn);
            cmd.Parameters.AddWithValue("_username", username);

            object? result = await cmd.ExecuteScalarAsync();
            if (result == null) return false;
            return (bool)result;
        }
        public bool IsEmailExists(string email)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM email_exists(:_email)", conn);
            cmd.Parameters.AddWithValue("_email", email);

            object? result = cmd.ExecuteScalar();
            if (result == null) return false;
            return (bool)result;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable dataTable = new DataTable();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM email_exists(:_email)", conn);
            cmd.Parameters.AddWithValue("_email", email);

            object? result = await cmd.ExecuteScalarAsync();
            if (result == null) return false;
            return (bool)result;
        }
        #endregion

        #region Base Updater
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
        public async Task<bool> UpdateDonorAsync(Donor donor)
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
            cmd.Parameters.AddWithValue("_display_name", (donor.DisplayName != null) ? donor.DisplayName : DBNull.Value);
            cmd.Parameters.AddWithValue("_bio", (donor.Bio != null) ? donor.Bio : DBNull.Value);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (donor.Image != null) ? donor.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            await cmd.ExecuteNonQueryAsync();

            conn.Close();

            return true;
        }
        public bool UpdateDonee(Donee donee)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE \"Donee\" " +
                "SET " +
                "username = :_username, " +
                "password = :_password, " +
                "email = :_email, " +
                "phone_number = :_phone_number, " +
                "organization = :_organization, " +
                "display_name = :_display_name, " +
                "bio = :_bio, " +
                "image = :_image " +
                "WHERE donee_id = :_id "
                , conn);
            cmd.Parameters.AddWithValue("_id", donee.Id);
            cmd.Parameters.AddWithValue("_username", donee.Username);
            cmd.Parameters.AddWithValue("_password", donee.Password);
            cmd.Parameters.AddWithValue("_email", donee.Email);
            cmd.Parameters.AddWithValue("_phone_number", donee.PhoneNumber);
            cmd.Parameters.AddWithValue("_organization", donee.Organization);
            cmd.Parameters.AddWithValue("_display_name", (donee.DisplayName != null) ? donee.DisplayName : DBNull.Value);
            cmd.Parameters.AddWithValue("_bio", (donee.Bio != null) ? donee.Bio : DBNull.Value);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (donee.Image != null) ? donee.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            return true;
        }
        public async Task<bool> UpdateDoneeAsync(Donee donee)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE \"Donee\" " +
                "SET " +
                "username = :_username, " +
                "password = :_password, " +
                "email = :_email, " +
                "phone_number = :_phone_number, " +
                "organization = :_organization, " +
                "display_name = :_display_name, " +
                "bio = :_bio, " +
                "image = :_image " +
                "WHERE donee_id = :_id "
                , conn);
            cmd.Parameters.AddWithValue("_id", donee.Id);
            cmd.Parameters.AddWithValue("_username", donee.Username);
            cmd.Parameters.AddWithValue("_password", donee.Password);
            cmd.Parameters.AddWithValue("_email", donee.Email);
            cmd.Parameters.AddWithValue("_phone_number", donee.PhoneNumber);
            cmd.Parameters.AddWithValue("_organization", donee.Organization);
            cmd.Parameters.AddWithValue("_display_name", (donee.DisplayName != null) ? donee.DisplayName : DBNull.Value);
            cmd.Parameters.AddWithValue("_bio", (donee.Bio != null) ? donee.Bio : DBNull.Value);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (donee.Image != null) ? donee.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            await cmd.ExecuteNonQueryAsync();

            conn.Close();

            return true;
        }
        public bool UpdateLeftover(Leftover leftover)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE \"Leftover\" " +
                "SET " +
                "name = :_name, " +
                "description = :_description, " +
                "image = :_image " +
                "WHERE leftover_id = :_id "
                , conn);
            cmd.Parameters.AddWithValue("_id", leftover.Id);
            cmd.Parameters.AddWithValue("_name", leftover.Name);
            cmd.Parameters.AddWithValue("_description", leftover.Description);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (leftover.Image != null) ? leftover.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            return true;
        }
        public bool UpdateRequest(Request request)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE \"Request\" " +
                "SET " +
                "title = :_title, " +
                "description = :_description, " +
                "date_created = :_date, " +
                "image = :_image " +
                "WHERE request_id = :_id "
                , conn);
            cmd.Parameters.AddWithValue("_id", request.Id);
            cmd.Parameters.AddWithValue("_title", request.Title);
            cmd.Parameters.AddWithValue("_description", request.Description);
            cmd.Parameters.AddWithValue("_date", request.Date);
            cmd.Parameters.AddWithValue("_image", NpgsqlTypes.NpgsqlDbType.Bytea, (request.Image != null) ? request.Image : DBNull.Value);

            if (conn.State == ConnectionState.Closed) conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            return true;
        }
        #endregion
    }
}
