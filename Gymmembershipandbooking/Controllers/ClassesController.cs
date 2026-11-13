// Owner: Salvador | Type: class (controller - class schedule; read = any user, write = Admin)
using GymApi.Dtos;
using GymApi.Models;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GymApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ClassesController : ApiControllerBase
{
    private readonly IClassService _service;

    public ClassesController(IClassService service) => _service = service;

    // GET api/v1/classes?upcomingOnly=true
    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] bool upcomingOnly = false) =>
        Execute(async () => Ok(await _service.GetAllAsync(upcomingOnly)));

    // GET api/v1/classes/instructors  (for the Admin form dropdown)
    [HttpGet("instructors")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> GetInstructors() => Execute(async () => Ok(await _service.GetInstructorsAsync()));

    // GET api/v1/classes/5
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id) => Execute(async () => Ok(await _service.GetByIdAsync(id)));

    // POST api/v1/classes  -> 201
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Create(GymClassInputDto dto) => Execute(async () =>
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    });

    // PUT api/v1/classes/5  -> 204
    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Update(int id, GymClassInputDto dto) => Execute(async () =>
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    });

    // DELETE api/v1/classes/5  -> 204 (soft delete: class is cancelled, open bookings are cancelled too)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public Task<IActionResult> Cancel(int id) => Execute(async () =>
    {
        await _service.CancelAsync(id);
        return NoContent();
    });
}
