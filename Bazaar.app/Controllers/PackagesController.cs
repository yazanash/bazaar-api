using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetPackages()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreatePackage()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePackage(int id)
        {
            return Ok();
        }

        [HttpGet("my")]
        public IActionResult GetMyPackages(int id)
        {
            return Ok();
        }
        [HttpPost("my")]
        public IActionResult BuyPackage()
        {
            return Ok();
        }
        [HttpDelete("my/{id}")]
        public IActionResult RemovePackage(int id)
        {
            return Ok();
        }
    }
}
