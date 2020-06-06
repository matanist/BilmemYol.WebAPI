using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilmemYol.Data.Models;
using BilmemYol.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilmemYol.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BilmemYolContext _context;
        public ProductController(BilmemYolContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetProducts")]
        public ApiResponse GetProducts()
        {
            try
            {
                var products = _context.Product.ToList();
                return new ApiResponse { Code = 200, Message = "Success", Set = products };
            }
            catch (Exception exp)
            {
                return new ApiResponse { Code = 500, Message = "İç hata oluştu", Set = exp.StackTrace };
            }

        }
        [HttpGet("GetProductNameById")]
        public ApiResponse GetProductNameById(int? id)
        {
            return new ApiResponse { Code = 200, Message = "Success", Set = "Araba" };
        }
    }
}
