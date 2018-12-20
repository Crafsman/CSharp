using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DownloadFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _config;
        private string _contentRoot;
        private string _fileRelativePath;
        public double LatestVersion { get; set; } = 0.0;
        public ValuesController(IConfiguration config)
        {
            _config = config;
            _contentRoot = config.GetValue<string>(WebHostDefaults.ContentRootKey);
            _fileRelativePath = config["Path:ReleaseFiles"];

            string productFolder = Path.Combine(_contentRoot, _fileRelativePath);

            if (!Directory.Exists(productFolder))
            {
                Directory.CreateDirectory(productFolder);
            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<double> Get()
        {
            GetLatestFileVersionName();
            return LatestVersion;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var fileName = GetLatestFileVersionName();

            // 1. Read file
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);

            // 2. Encrypt file Bytes
            return File(Encrypt(fileBytes), System.Net.Mime.MediaTypeNames.Application.Zip);

        }

        // Retrieve wwwroot/releases folder, and find the latest file
        private string GetLatestFileVersionName()
        {
            string productFolder = Path.Combine(_contentRoot, _fileRelativePath);

            List<double> fileNamesWithoutExtension = new List<double>();
            string[] fileFullPathLists = Directory.GetFiles(productFolder);
            string maxFilePath = "";
            double maxNumber = 0.0;
            if (fileFullPathLists.Length > 0)
            {
                maxFilePath = fileFullPathLists[0];
            }

            for (int i = 0; i < fileFullPathLists.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(fileFullPathLists[i]);
                // ss-0.1

                double number = Convert.ToDouble(name.Substring(name.LastIndexOf('-') + 1));
                if (number > maxNumber)
                {
                    maxFilePath = fileFullPathLists[i];
                    LatestVersion = number;
                }
            }

            return maxFilePath;
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
