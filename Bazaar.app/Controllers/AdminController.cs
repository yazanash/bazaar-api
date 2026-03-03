using Bazaar.app.Dtos.AdminDto;
using Bazaar.Entityframework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet("CheckAdminStatus")]
        public IActionResult VerifyAdminStatus()
        {
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin) return Forbid(); 

            return Ok(new { status = "Authorized", role = "Admin" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAdmins()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            var result = adminUsers.Select(u => new {
                id = u.Id,
                email = u.Email,
                name = u.UserName
            }).ToList();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddAdminRole([FromBody] AdminUserRequest model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest("Empty Email not allowed");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new { message = "User not found" });

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return Ok(new { message = $"User {model.Email} is now an Admin" });
            }

            return BadRequest(result.Errors);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> RevokeAdmin(string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == id)
                return Forbid("you can't remove yourself");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok(new { message = "تم سحب الصلاحية بنجاح" });

            return BadRequest(result.Errors);
        }
    }
}
