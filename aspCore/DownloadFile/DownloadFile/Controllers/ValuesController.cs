using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DownloadFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            // 1. Read file
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\code\CSharp\aspCore\DownloadFile\DownloadFile\Books\default.zip");

            // 2. Encrypt file Bytes
            return File(Encrypt(fileBytes), System.Net.Mime.MediaTypeNames.Application.Octet);
            //return new FileStreamResult(stream, "application/pdf");

            //This class is used to return a file from a byte array.
            //return new FileContentResult(byteArray, "application/pdf");

            //return File(@"C:\code\CSharp\aspCore\DownloadFile\DownloadFile\Books\default.pdf", "application/pdf");
        }

        public static byte[] MD5Hash(byte[] buffer)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            return md5provider.ComputeHash(buffer);
        }

        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        public static byte[] Encrypt(byte[] textBytes)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        return transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        // return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static byte[] Decrypt(byte[] cipherBytes)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        return transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                    }
                }
            }
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
