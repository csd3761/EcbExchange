using ECB.Appilcation.CurrencyRates.Dtos;
using ECB.Appilcation.CurrencyRates.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECB.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyRatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetRates")]
    public async Task<ActionResult<CurrencyRatesDto>> GetRates()
    {
        var rates = await _mediator.Send(new GetCurrencyRatesQuery());
        return Ok(rates);
    }
    
}