using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Helpers
{
    public class ApiError
    {
        public ApiError(int statusCode, string errorMessage, string requestId, string errorDetails = null)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            RequestId = requestId;
            ErrorDetails = errorDetails;
        }

        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }
        public string ErrorDetails { get; set; }
    }
}
