using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    public class User
    {
        private readonly int _id;
        private readonly string _username;
        private string _password;
        private string _email;
        private string _phoneNumber;
        private string? _displayName;
        private string? _bio;
        private byte[]? _image;

        public int Id { get { return _id; } }
        public string Username { get { return _username; } }
        public string Password { get { return _password; } }
        public string Email { get { return _email; } }
        public string PhoneNumber { get { return _phoneNumber; } }
        public string? DisplayName { get { return _displayName; } }
        public string? Bio { get { return _bio; } }
        public byte[]? Image { get { return _image; } }

        public User(int id, string username, string password, string email, string phoneNumber)
        {
            _id = id;
            _username = username;
            _password = password;
            _email = email;
            _phoneNumber = phoneNumber;
        }

        public bool ChangePassword(string newPassword)
        {
            if (_password.Equals(newPassword)) return false;
            _password = newPassword;
            return true;
        }
        public bool ChangeEmail(string newEmail)
        {
            if (_email.Equals(newEmail)) return false;
            _email = newEmail;
            return true;
        }
        public bool ChangePhoneNumber(string newPhoneNumber)
        {
            if (_phoneNumber.Equals(newPhoneNumber)) return false;
            _phoneNumber = newPhoneNumber;
            return true;
        }
        public bool ChangeDisplayName(string newDisplayName)
        {
            if (_displayName == newDisplayName) return false;
            _displayName = newDisplayName;
            return true;
        }
        public bool ChangeBio(string newBio)
        {
            _bio = newBio;
            return true;
        }
        public bool ChangeImage(byte[] newImage)
        {
            _image = newImage;
            return true;
        }
    }
}
