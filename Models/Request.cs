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
        private string _status = "ongoing";

        public Leftover Item {  get { return _item; } }
        public string Title { get { return _title; } }
        public string Description { get { return _description; } }
        public DateTime Date { get { return _date; } }
        public Donee Donee { get { return _donee; } }
        public string Status { get { return _status; } }

        Request(Leftover item, string title, string description, DateTime date, Donee donee)
        {
            _item = item;
            _title = title;
            _description = description;
            _date = date;
            _donee = donee;
        }

        public void Approve() { _status = "approved"; }
        public void Reject() { _status = "rejected"; }

    }
}
