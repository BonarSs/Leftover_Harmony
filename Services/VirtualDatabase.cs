using Leftover_Harmony.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Services
{
    /// <summary>
    /// Class to cache class objects from database.
    /// </summary>
    public class VirtualDatabase
    {
        private static VirtualDatabase? _instance;
        private Dictionary<int, Donor> _donors;
        private Dictionary<int, Donee> _donees;
        private Dictionary<int, Request> _requests;
        private Dictionary<int, Donation> _donations;
        private Dictionary<int, Leftover> _leftovers;

        private Dictionary<Type, List<int>> _dirty;

        public Dictionary<int, Donor> Donors { get { return _donors; } }
        public Dictionary<int, Donee> Donees { get { return _donees; } }
        public Dictionary<int, Request> Requests { get { return _requests; } }
        public Dictionary<int, Donation> Donations { get { return _donations; } }
        public Dictionary<int, Leftover> Leftovers { get { return _leftovers; } }

        public VirtualDatabase() 
        {
            _donors = new Dictionary<int, Donor>();
            _donees = new Dictionary<int, Donee>();
            _requests = new Dictionary<int, Request>();
            _donations = new Dictionary<int, Donation>();
            _leftovers = new Dictionary<int, Leftover>();
        }

        public void MarkDirty<T>(int id) where T : class
        {
            Type[] types = { typeof(Donor), typeof(Donee), typeof(Request), typeof(Donation), typeof(Leftover) };
            foreach (Type type in types)
            {
                if (typeof(T) != type) continue;
                if (!_donors.ContainsKey(id)) return;

                if (!_dirty.ContainsKey(typeof(Donor))) _dirty.Add(typeof(Donor), new List<int>());
                _dirty[typeof(Donor)].Add(id);
            }
        }

        public static VirtualDatabase Instance {  get { if (_instance == null) _instance = new VirtualDatabase(); return _instance; } }
    }
}
