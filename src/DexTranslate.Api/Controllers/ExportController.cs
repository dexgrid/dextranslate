using DexTranslate.Abstractions.Service;
using DexTranslate.ApiContract.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DexTranslate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "api")]
    [Route("api/v{version:apiVersion}/[controller]/{languageKey}/{projectKey}")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly ILogger<ExportController> _logger;
        private readonly IExportService _exportService;

        public ExportController(ILogger<ExportController> logger, IExportService exportService)
        {
            _logger = logger;
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string languageKey, string projectKey)
        {
            try
            {
                var result = await _exportService.GetFileExportAsync(languageKey, projectKey);

                if (!result.Any())
                {
                    return BadRequest(new ApiResponse(System.Net.HttpStatusCode.BadRequest, "Project or language not found"));
                }

                return File(result, "text/csv", $"{projectKey}_{languageKey}_{DateTime.Now:yyyy-MM-dd_HHmmSS}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on exporting csv file");
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}