using Bazaar.app.Dtos.ProfileDtos;
using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazaar.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileDataService _profileDataService;

        public ProfileController(IProfileDataService profileDataService)
        {
            _profileDataService = profileDataService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfileRequest profileRequest)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            if (profileRequest == null) return BadRequest("No Profile Data");

            Profile profile = profileRequest.ToModel();
            profile.UserId = userId;
            Profile createdProfile =  await _profileDataService.CreateAsync(profile);
            if(createdProfile == null) return BadRequest();   

            return Ok(new ProfileResponse(createdProfile));

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProfileRequest profileRequest)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            if (profileRequest == null) return BadRequest("No Profile Data");

            Profile userProfile = await _profileDataService.GetUserProfileAsync(userId);
            if (userProfile == null) return NotFound("Profile Not Found"); 

            Profile profile = profileRequest.ToModel();
          
            profile.UserId = userId;
            profile.Id = userProfile.Id;
            Profile updatedProfile = await _profileDataService.UpateAsync(profile);
            if (updatedProfile == null) return BadRequest();

            return Ok(new ProfileResponse(updatedProfile));

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            Profile userProfile = await _profileDataService.GetUserProfileAsync(userId);
            if (userProfile == null) return NotFound();

            return Ok(new ProfileResponse(userProfile));

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Profile userProfile = await _profileDataService.GetProfileByIdAsync(id);
            if (userProfile == null) return NotFound();
            return Ok(new ProfileResponse(userProfile));

        }
    }
}
