// Owner: Fulgar | Type: class (controller - reports, Admin only)
using GymApi.Models;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GymApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class ReportsController : ApiControllerBase
{
    private readonly IReportService _service;

    public ReportsController(IReportService service) => _service = service;

    // GET api/v1/reports/overdue-memberships
    [HttpGet("overdue-memberships")]
    public Task<IActionResult> GetOverdueMemberships() => Execute(async () => Ok(await _service.GetOverdueMembersAsync()));

    // GET api/v1/reports/attendance
    [HttpGet("attendance")]
    public Task<IActionResult> GetAttendance() => Execute(async () => Ok(await _service.GetAttendanceAsync()));
}
