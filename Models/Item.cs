using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }

        [Column(TypeName = "money")]
        public decimal ItemPrice { get; set; }
        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }
    }
}
