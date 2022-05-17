using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Helpers
{
    public class ServerResponse<T> where T: class
    {
        public bool IsSuccessful { get; set; }
        public T Content { get; set; }
        public string Message { get; set; }
    }
}
