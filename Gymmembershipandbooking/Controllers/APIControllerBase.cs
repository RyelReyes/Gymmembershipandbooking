// Owner: Reyes | Type: abstract class (shared base for all controllers)
using GymApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

/// <summary>
/// Every controller inherits this. Execute() wraps an action in ONE try/catch so each
/// error becomes the right status code with the same body shape: { "error": "message" }.
/// (Same idea as the try/catch in the lesson's controllers - just written once.)
///   NotFoundException      -> 404
///   ForbiddenException     -> 403
///   BusinessRuleException  -> 409
///   ArgumentException      -> 400
///   anything else          -> 500 (logged)
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{
    protected async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ForbiddenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            var logger = HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
            logger.LogError(ex, "Unhandled error in {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Unexpected server error. Please try again." });
        }
    }
}
