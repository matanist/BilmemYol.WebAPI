using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BilmemYol.Data.Models;
using BilmemYol.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [HttpPost("InsertProductPicture")]
        public ApiResponse InsertProductPicture([FromForm]IList<IFormFile> files)
        {
            try
            {
                var productId = HttpContext.Request.Form["ProductId"].FirstOrDefault();
                if (!string.IsNullOrEmpty(productId))
                {
                    var pId = Convert.ToInt32(productId);
                    var product = _context.Product.FirstOrDefault(p => p.Id == pId);
                    if (product != null)
                    {
                        if (files.Count > 0)
                        {
                            var file = files.FirstOrDefault();
                            var stream = file.OpenReadStream();
                            var byteDizisi=FileService.ConvertByteArrayFromFormFile(file);
                                product.Image = byteDizisi;
                                _context.SaveChanges();
                                return new ApiResponse { Code = 200, Message = "Success", Set = null };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse { Code = 500, Message = "Sunucu iç hatası", Set = ex.StackTrace };
            }
            return null;
           
            
        }
        
    }
}
