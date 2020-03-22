using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace Huffman.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DecompressController : ControllerBase
    {
        // GET: api/Decompress
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Decompress/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Decompress/fileName        
        [HttpPost("{name}/{method}")]
        public void Post([FromForm(Name = "file")] IFormFile file, string name, string method)
        {                     

            string folder = @"C:\Compressions\";
            string fullPath = folder + file.FileName;

            byte[] txt = new byte[file.Length];
            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
            {
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fs.Read(txt, sum, txt.Length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }

            if (method.ToLower().Equals("huffman"))
            {
                Huffman huffmanMethods = new Huffman();
                huffmanMethods.DecodeFile(txt, name, file.FileName);
            }
            else if (method.ToLower().Equals("lzw"))
            {
                LZW lzwMethods = new LZW();
                lzwMethods.Decompress(txt);
            }
            
        }

        // PUT: api/Decompress/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
