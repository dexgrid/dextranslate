using DexTranslate.Abstractions.Service;
using DexTranslate.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DexTranslate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "api")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DisplayController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public DisplayController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAll();

            var list = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                var languageKeys = await _projectService.GetLanguagesForProjectsAsync(project.Id);

                foreach (var language in languageKeys)
                {
                    list.Add(new ProjectViewModel
                    {
                        Key = project.Key,
                        Title = project.Title,
                        Language = language
                    });
                }
            }

            return Ok(list);
        }
    }
}