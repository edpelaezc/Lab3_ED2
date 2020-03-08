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
        [HttpGet("{name}")]
        public void Get([FromForm(Name = "file")] IFormFile file, string name)
        {
            Huffman decompressMethod = new Huffman();
            var result = new StringBuilder();
            using (StreamReader sr = new StreamReader(file.OpenReadStream()))
            {
                while (sr.Peek() >= 0)
                    result.AppendLine(sr.ReadLine());
            }

            byte[] textInBytes = ByteGenerator.ConvertToBytes(result.ToString());
            decompressMethod.DecodeFile(textInBytes, name, file.FileName);
        }

        // POST: api/compress
        [HttpPost("{name}")]
        public void Post([FromForm(Name = "file")] IFormFile file, string name)
        {
            Huffman compressMethods = new Huffman();
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            byte[] textInBytes = Encoding.UTF8.GetBytes(result.ToString());
            compressMethods.BuildHuffman(textInBytes);
            compressMethods.WriteFile(textInBytes, name, file.FileName);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
