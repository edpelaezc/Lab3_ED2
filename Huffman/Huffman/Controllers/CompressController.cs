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
        [HttpPost("{name}/{method}")]
        public void Post([FromForm(Name = "file")] IFormFile file, string name, string method)
        {
            //lectura del archivo
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            byte[] textInBytes = Encoding.ASCII.GetBytes(result.ToString());

            //ejecuta según el método de compresión escogido
            if (method.ToLower().Equals("huffman"))
            {
                Huffman compressMethods = new Huffman();                
                compressMethods.BuildHuffman(textInBytes, name);
                compressMethods.WriteFile(textInBytes, name, file.FileName);
            }
            else if (method.ToLower().Equals("lzw"))
            {
                LZW compressMethods = new LZW();
                compressMethods.GetText(result);
                compressMethods.InitializeDictionary(result, name);                
                compressMethods.Compress(textInBytes, name, file.FileName);

            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
