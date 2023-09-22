using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    class Request
    {
        private Leftover _item;
        private string _title;
        private string _description;
        private DateTime _date;
        private Donee _donee;
        private List<Donor> _donors;
        private List<Donor> _approvedDonors;

        public Leftover Item {  get { return _item; } }
        public string Title { get { return _title; } }
        public string Description { get { return _description; } }
        public DateTime Date { get { return _date; } }
        public Donee Donee { get { return _donee; } }
        public List<Donor> Donors { get { return _donors; } }

        Request(Leftover item, string title, string description, DateTime date, Donee donee)
        {
            _item = item;
            _title = title;
            _description = description;
            _date = date;
            _donee = donee;
        }

        public bool Approve(Donor donor) {
            if (!_donors.Contains(donor)) return false;
            _approvedDonors.Add(donor);
            return true;
        }
        public bool Reject(Donor donor)
        {
            return RemoveDonor(donor);
        }
        public bool AddDonor(Donor donor) {
            if (_donors.Contains(donor)) return false;
            _donors.Add(donor);
            return true;
        }
        public bool RemoveDonor(Donor donor) {
            if (!_donors.Contains(donor)) return false;
            _donors.Remove(donor);
            return true;
        }

    }
}
