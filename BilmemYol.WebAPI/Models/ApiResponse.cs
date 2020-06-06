using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilmemYol.WebAPI.Models
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Set { get; set; }
    }
}
