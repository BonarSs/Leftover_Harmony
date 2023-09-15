using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    internal class Leftover
    {
        private string _name;
        private string _description;
        private int _quantity;
        private Category _category;

        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public Category Category { get { return _category; } set { _category = value; } }
        public Leftover(string name, string description, int quantity, Category category)
        {
            _name = name;
            _description = description;
            _quantity = quantity;
            _category = category;
        }
    }
}
