using LockoutSample.Application.Interfaces;
using LockoutSample.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LockoutSample.Presentation.Controllers
{
    /// <summary>
    /// Handles lockout actions
    /// </summary>
    [ApiController]
    [Route("lockout")]
    public class LockoutController(ILockoutService lockoutService) : ControllerBase
    {
        /// <summary>
        /// Creates a resource access code
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        public async Task<NoContentResult> CreateCode([FromBody] CreateCodeRequest request)
        {
            await lockoutService.CreateCode(request, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Accesses a resource
        /// </summary>
        [HttpGet("resource")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        public async Task<NoContentResult> AccessResourec([FromQuery] AccessResourceRequest request)
        {
            await lockoutService.AccessResource(request, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
