using Azure;
using Bazaar.app.Dtos;
using Bazaar.app.Services;
using Bazaar.app.Services.TesterServices;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IJwtTokenService _tokenService;
        private readonly IOTPGenerateService _otpGenerateService;
        private readonly IBypassService _bypassService;

        public AuthenticationController(UserManager<AppUser> userManager, EmailService emailService, IJwtTokenService tokenService, IOTPGenerateService otpGenerateService, IBypassService bypassService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _otpGenerateService = otpGenerateService;
            _bypassService = bypassService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RequestOtp([FromBody] EmailDto emailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(emailDto.Email?.Trim()) || !IsValidEmail(emailDto.Email))
                {
                    return BadRequest("Invalid Email");
                }
                var response = new
                {
                    message = "Email sent successfully"
                };
                if (_bypassService.IsTester(emailDto.Email))
                    return Ok(response);
                OTPModel model = await _otpGenerateService.GenerateAsync(emailDto.Email.Trim());
                await _emailService.SendEmailAsync(emailDto.Email.Trim(), model.Otp);
               
                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }

        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch { return false; }
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyOtp([FromBody] OTPDto oTPDto)
        {
            try
            {
                if (string.IsNullOrEmpty(oTPDto.Email?.Trim()) || !IsValidEmail(oTPDto.Email))
                {
                    return BadRequest("Invalid Email");
                }
                bool isAuthorized = _bypassService.IsValidTester(oTPDto.Email, oTPDto.Otp.ToString());

                if (!isAuthorized)
                {
                    var oTPModel = await _otpGenerateService.VerifyAsync(oTPDto.Email, oTPDto.Otp);
                    if (oTPModel == null) return Unauthorized("Invalid OTP");
                }

                AppUser? user = await _userManager.FindByEmailAsync(oTPDto.Email!);
                if (user == null)
                {
                    user = new()
                    {
                        Email = oTPDto.Email,
                        UserName = oTPDto.Email!.Split('@')[0]
                    };
                    IdentityResult result = await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, "User");
                }
                IList<string> roles = await _userManager.GetRolesAsync(user!);
                JwtTokenResult token = _tokenService.GenerateToken(user, roles);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // على الإنتاج لازم يكون https
                    SameSite = SameSiteMode.None,
                    Expires = token.ExpiresAt,
                    Path = "/"
                };
                Response.Cookies.Append("auth", token.Token, cookieOptions);
                return Created("created", token);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            if (!User!.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user!);
                    JwtTokenResult token = _tokenService.GenerateToken(user, roles);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = token.ExpiresAt,
                        Path = "/"
                    };
                    Response.Cookies.Append("auth", token.Token, cookieOptions);
                    return Ok(token);
                }
            }
            return Forbid();
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth", new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
