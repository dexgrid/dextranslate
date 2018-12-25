using DexTranslate.Abstractions.Service;
using DexTranslate.ApiContract.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DexTranslate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "api")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IImportService _importService;

        public ImportController(ILogger<ImportController> logger, IImportService importService)
        {
            _logger = logger;
            _importService = importService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile importFile, [FromForm]string languageKeyImport, [FromForm]bool deleteMissingTranslations)
        {
            if (importFile == null)
            {
                return BadRequest();
            }

            try
            {
                await _importService.ImportTranslations(languageKeyImport, deleteMissingTranslations, importFile.OpenReadStream());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on importing csv file");
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}