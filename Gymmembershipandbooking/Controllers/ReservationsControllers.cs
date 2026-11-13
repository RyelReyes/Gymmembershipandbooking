// Owner: Salvador | Type: class (controller - bookings + status workflow)
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
public class ReservationsController : ApiControllerBase
{
    private readonly IReservationService _service;

    public ReservationsController(IReservationService service) => _service = service;

    // GET api/v1/reservations?classId=3
    // Member -> own bookings | Instructor -> bookings of own classes | Admin -> all (decided by the token)
    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] int? classId) =>
        Execute(async () => Ok(await _service.GetAllAsync(User.GetUserId(), User.GetRole(), classId)));

    // GET api/v1/reservations/5
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id) =>
        Execute(async () => Ok(await _service.GetByIdAsync(id, User.GetUserId(), User.GetRole())));

    // POST api/v1/reservations  -> 201 (Member only)
    [HttpPost]
    [Authorize(Roles = Roles.Member)]
    public Task<IActionResult> Create(ReservationCreateDto dto) => Execute(async () =>
    {
        var created = await _service.CreateAsync(User.GetUserId(), dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    });

    // PATCH api/v1/reservations/5/status  -> 200
    [HttpPatch("{id:int}/status")]
    public Task<IActionResult> UpdateStatus(int id, ReservationStatusDto dto) =>
        Execute(async () => Ok(await _service.UpdateStatusAsync(id, User.GetUserId(), User.GetRole(), dto.Status)));
}
