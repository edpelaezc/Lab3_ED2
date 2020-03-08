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

        /// <summary>
        /// Recibe un archivo .txt que será comprimido por medio del algoritmo "Huffman" y lo guardará con el nombre especificado.
        /// </summary>
        /// <param name="file">Archivo .txt</param>
        /// <param name="name">Nombre especificado para guardar el archivo</param>
        // POST: api/compress/fileName
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
