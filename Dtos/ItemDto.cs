using System;

namespace DataAccessAPI.Dtos
{
    public class ItemDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public string CategoryName { get; set; }
    }
}
