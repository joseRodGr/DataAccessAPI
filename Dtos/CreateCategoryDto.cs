using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Dtos
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
