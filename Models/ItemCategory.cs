using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Models
{
    public class ItemCategory
    {
        public int ItemCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        ICollection<Item> Items { get; set; }
    }
}
