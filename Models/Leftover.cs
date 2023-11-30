using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Models
{
    public class Leftover
    {
        private readonly int _id;
        private string _name;
        private string _description;
        private byte[]? _image;
        private Category? _category;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public byte[]? Image { get { return _image; } }
        public Leftover(int id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }
        public static Leftover From(DataRow dataRow)
        {
            string[] expectedFields = { "leftover_id", "name", "description" };
            foreach (string field in expectedFields)
            {
                if (!dataRow.Table.Columns.Contains(field)) throw new MissingFieldException($"The field {field} does not exist.");
            }

            string description = dataRow["description"] != DBNull.Value ? (string)dataRow["description"] : "";

            Leftover leftover = new Leftover(
                (int)dataRow["leftover_id"],
                (string)dataRow["name"],
                description);

            if (dataRow.Table.Columns.Contains("image") && dataRow["image"] != DBNull.Value) leftover.SetImage((byte[])dataRow["image"]);

            return leftover;
        }

        public void SetImage(byte[] image)
        {
            _image = image;
        }
    }
}
