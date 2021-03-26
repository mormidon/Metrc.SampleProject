using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Metrc.SampleProject.WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        // GET: api/HomeApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HomeApi/5
        [HttpGet("{id}", Name = "GetHome")]
        public string Get(Int64 id)
        {
            return "value";
        }

        // POST: api/HomeApi
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/HomeApi/5
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
