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
            string result = "";

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
          
            decompressMethod.DecodeFile(txt, name, file.FileName);
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
            compressMethods.BuildHuffman(textInBytes, name);
            compressMethods.WriteFile(textInBytes, name, file.FileName);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
