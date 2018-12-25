using DexTranslate.Abstractions.Service;
using DexTranslate.Api.Mapping;
using DexTranslate.ApiContract.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DexTranslate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "api")]
    [Route("api/v{version:apiVersion}/[controller]/{languageKey}/{projectKey}")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly ITranslationService _service;

        public TranslationController(ILogger<TranslationController> logger, ITranslationService translationService)
        {
            _logger = logger;
            _service = translationService;
        }

        // GET: api/Translation
        [HttpGet]
        public async Task<IEnumerable<Translation>> Get(string languageKey, string projectKey)
        {
            return (await _service.GetAll(languageKey, projectKey)).Select(m => m.ToContract());
        }

        // GET: api/Translation/5
        [HttpGet("{key}", Name = "Get")]
        public async Task<Translation> Get(string languageKey, string projectKey, string key)
        {
            return (await _service.GetByKey(languageKey, projectKey, key)).ToContract();
        }

        // POST: api/Translation
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Post([FromBody] Translation value)
        {
            try
            {
                if (await _service.ExistsAsync(value.LanguageKey, value.ProjectKey, value.Key))
                {
                    return Conflict(new ApiResponse(HttpStatusCode.Conflict, "A translation with this key already exists"));
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

        // PUT: api/Translation/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Put([FromBody]Translation value)
        {
            try
            {
                if (!(await _service.ExistsAsync(value.LanguageKey, value.ProjectKey, value.Key)))
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
        public async Task<IActionResult> Delete(string languageKey, string projectKey, string key)
        {
            try
            {
                if (!(await _service.ExistsAsync(languageKey, projectKey, key)))
                {
                    return NotFound(new ApiResponse(HttpStatusCode.NotFound, $"Could not find item with key: {HttpUtility.HtmlEncode(key)}"));
                }
                await _service.DeleteAsync(languageKey, projectKey, key);
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