using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Storytel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<post> post = new List<post>();
                post.Add(new Controllers.post() { id = 1 , name ="Omid" });
                post.Add(new Controllers.post() { id = 2 , name ="ALi" });
                post.Add(new Controllers.post() { id = 3 , name ="Hasan" });
                return Ok(post);

            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

    class post
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}