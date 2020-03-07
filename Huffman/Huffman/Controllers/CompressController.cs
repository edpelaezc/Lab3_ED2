using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Huffman.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class compressController : ControllerBase
    {
        // GET: api/compress
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/compress/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/compress
        [HttpPost("{nombre}")]
        public string Post([FromForm(Name = "file")] IFormFile file, string nombre)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();            
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
