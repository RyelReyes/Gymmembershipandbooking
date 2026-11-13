// Owner: Reyes | Type: class (controller - register / login)
using GymApi.Dtos;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMemberService _memberService;

    public AuthController(IAuthService authService, IMemberService memberService)
    {
        _authService = authService;
        _memberService = memberService;
    }

    // POST api/v1/auth/register  -> 201 (public sign-up always creates a Member, never an Admin)
    [AllowAnonymous]
    [HttpPost("register")]
    public Task<IActionResult> Register(RegisterDto dto) => Execute(async () =>
    {
        var created = await _memberService.CreateAsync(new MemberCreateDto
        {
            Username = dto.Username,
            Password = dto.Password,
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone
        });

        return Created($"/api/v1/members/{created.Id}", created);
    });

    // POST api/v1/auth/login  -> 200 + token, or 401
    [AllowAnonymous]
    [HttpPost("login")]
    public Task<IActionResult> Login(LoginDto dto) => Execute(async () =>
    {
        var result = await _authService.LoginAsync(dto);
        if (result is null) return Unauthorized(new { error = "Invalid username or password." });

        return Ok(result);
    });
}
