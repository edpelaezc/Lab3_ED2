using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Huffman.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompressionsController : ControllerBase
    {
        // GET: api/Compressions
        [HttpGet]
        public IEnumerable<CompressionsCollection> Get()
        {
            return Data.Instance.archivos;
        }


        // POST: api/Compressions
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Compressions/5
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
