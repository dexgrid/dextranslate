using DexTranslate.Abstractions.Service;
using DexTranslate.Api.Mapping;
using DexTranslate.ApiContract.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DexTranslate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "api")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _service;

        public ProjectController(ILogger<ProjectController> logger, IProjectService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/Language
        [HttpGet]
        public async Task<IEnumerable<Project>> Get()
        {
            return (await _service.GetAll()).ToContract();
        }

        // POST: api/Language
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> Post([FromBody] Project value)
        {
            try
            {
                if (await _service.ExistsAsync(value.Key))
                {
                    return Conflict(new ApiResponse(HttpStatusCode.Conflict, "A project with this key already exists"));
                }
                await _service.Add(value.FromContract());
                return Ok();
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        // PUT: api/Language/5
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Put([FromBody]Project value)
        {
            try
            {
                if (!(await _service.ExistsAsync(value.Key)))
                {
                    return NotFound(new ApiResponse(HttpStatusCode.NotFound, $"Could not find item with key: {HttpUtility.HtmlEncode(value.Key)}"));
                }

                await _service.Update(value.FromContract());
                return Ok();
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{key}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(string key)
        {
            try
            {
                if (!(await _service.ExistsAsync(key)))
                {
                    return NotFound(new ApiResponse(HttpStatusCode.NotFound, $"Could not find item with key: {HttpUtility.HtmlEncode(key)}"));
                }
                await _service.Delete(key);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}