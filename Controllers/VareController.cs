using vareAPI.Models;
using vareAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace vareAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class vareController : ControllerBase
{
    private readonly ILogger<vareController> _logger;

    private readonly VareService _vareService;

    public vareController(ILogger<vareController> logger, VareService vareService)
    {
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into VareController");
        _vareService = vareService;
    }

    [HttpGet]
    public async Task<List<Vare>> Get() =>
        await _vareService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Vare>> Get(string id)
    {
        var vare = await _vareService.GetAsync(id);

        if (vare is null)
        {
            return NotFound();
        }

        return vare;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Vare newVare)
    {
        await _vareService.CreateAsync(newVare);

        return CreatedAtAction(nameof(Get), new { id = newVare.ProductId }, newVare);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Vare updatedVare)
    {
        var vare = await _vareService.GetAsync(id);

        if (vare is null)
        {
            return NotFound();
        }

        updatedVare.ProductId = vare.ProductId;

        await _vareService.UpdateAsync(id, updatedVare);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var vare = await _vareService.GetAsync(id);

        if (vare is null)
        {
            return NotFound();
        }

        await _vareService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("version")]
    public IEnumerable<string> GetVersion()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }
        return properties;
    }
}
