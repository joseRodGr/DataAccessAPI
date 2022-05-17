using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Dtos
{
    public class CreateItemDto
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public decimal ItemPrice { get; set; }

        [Required]
        public int ItemCategoryId { get; set; }
    }
}
