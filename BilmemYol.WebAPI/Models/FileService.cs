using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BilmemYol.WebAPI.Models
{
    public class FileService
    {
        public static byte[] ConvertByteArrayFromFormFile(IFormFile file)
        {
            var stream = file.OpenReadStream();
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var byteDizisi = ms.ToArray();
                return byteDizisi;               
            }
        }
    }
}
