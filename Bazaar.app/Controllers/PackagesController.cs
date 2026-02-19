using Bazaar.app.Dtos.PackageDtos;
using Bazaar.Entityframework.Exceptions;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Models.UserWallet;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageDataService _packageDataService;
        private readonly IUserWalletService _userWalletService;

        public PackagesController(IPackageDataService packageDataService, IUserWalletService userWalletService)
        {
            _packageDataService = packageDataService;
            _userWalletService = userWalletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPackages()
        {
            IEnumerable<Package> packages = await _packageDataService.GetAllAsync();
            var response = packages.Select(x => new PackageResponse(x)).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] PackageRequest packageRequest)
        {
            if (packageRequest == null) return BadRequest();

            Package package = packageRequest.ToModel();
            Package createdPackage = await _packageDataService.CreateAsync(package);
            return Ok(new PackageResponse(createdPackage));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] PackageRequest packageRequest)
        {
            if (packageRequest == null) return BadRequest();

            Package package = packageRequest.ToModel();
            package.Id = id;
            Package createdPackage = await _packageDataService.UpdateAsync(package);
            return Ok(new PackageResponse(createdPackage));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            await _packageDataService.DeleteByIdAsync(id);
            return Ok();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyPackages()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            UserWallet userWallet = await _userWalletService.GetUserWallet(userId);
            return Ok(new UserWalletResponse(userWallet));
        }
        [HttpPost("my")]
        public async Task<IActionResult> BuyPackage([FromBody] PackageBundleRequest packageBundleRequest)
        {
            try
            {
                if (packageBundleRequest == null) return BadRequest();
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();
                await _userWalletService.CreatePackageBundle(userId, packageBundleRequest.PackageId);
                return Ok();
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
