// Owner: Fulgar | Type: class (controller - members CRUD, Admin only except /me)
using GymApi.Dtos;
using GymApi.Extensions;
using GymApi.Models;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GymApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MembersController : ApiControllerBase
{
    private readonly IMemberService _service;

    public MembersController(IMemberService service) => _service = service;

    // GET api/v1/members
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> GetAll() => Execute(async () => Ok(await _service.GetAllAsync()));

    // GET api/v1/members/me  (identity comes from the token)
    [HttpGet("me")]
    [Authorize(Roles = Roles.Member)]
    public Task<IActionResult> GetMine() => Execute(async () => Ok(await _service.GetByUserIdAsync(User.GetUserId())));

    // GET api/v1/members/5
    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> GetById(int id) => Execute(async () => Ok(await _service.GetByIdAsync(id)));

    // POST api/v1/members  -> 201
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Create(MemberCreateDto dto) => Execute(async () =>
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    });

    // PUT api/v1/members/5  -> 204
    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Update(int id, MemberUpdateDto dto) => Execute(async () =>
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    });

    // PATCH api/v1/members/5/renew  -> 200
    [HttpPatch("{id:int}/renew")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Renew(int id, MemberRenewDto dto) => Execute(async () => Ok(await _service.RenewAsync(id, dto)));

    // DELETE api/v1/members/5  -> 204 (soft delete: member is deactivated, history is kept)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Deactivate(int id) => Execute(async () =>
    {
        await _service.DeactivateAsync(id);
        return NoContent();
    });
}
