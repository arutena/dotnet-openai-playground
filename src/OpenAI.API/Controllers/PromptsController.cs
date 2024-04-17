using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.API.Services;

namespace OpenAI.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PromptsController : ControllerBase
{
    private readonly IPromptService _promptService;

    public PromptsController(IPromptService promptService)
    {
        _promptService = promptService;
    }

    [HttpGet(Name = "TriggerOpenAI")]
    public async Task<IActionResult> TriggerOpenAI([FromQuery] string input)
    {
        var response = await _promptService.TriggerOpenAI(input);
        return Ok(response);
    }
}
