using System;

namespace DataAccessAPI.Dtos
{
    public class UpdateItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
