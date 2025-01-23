using ECB.Appilcation.Wallets.Commands;
using ECB.Appilcation.Wallets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECB.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateWallet")]
    public async Task<ActionResult<long>> CreateWallet([FromQuery] decimal initialBalance, [FromQuery] string currency)
    {
        var createWalletCommand = new CreateWalletCommand
        {
            InitialBalance = initialBalance,
            Currency = currency
        };

        var walletId = await _mediator.Send(createWalletCommand);
        return Ok(walletId);
    }
    
    [HttpGet("{walletId}")]
    public async Task<ActionResult<decimal>> GetWalletBalance(long walletId, [FromQuery] string currency)
    {
        var getWalletBalanceQuery = new GetWalletBalanceQuery
        {
            WalletId = walletId,
            TargetCurrency = currency
        };

        var balance = await _mediator.Send(getWalletBalanceQuery);
        return Ok(balance);
    }

    [HttpPost("{walletId}/adjustbalance")]
    public async Task<IActionResult> AdjustWalletBalance(long walletId, [FromQuery] decimal amount, [FromQuery] string currency, [FromQuery] string strategy)
    {
        var adjustWalletBalanceCommand = new AdjustWalletBalanceCommand
        {
            WalletId = walletId,
            Amount = amount,
            Currency = currency,
            Strategy = strategy
        };

        await _mediator.Send(adjustWalletBalanceCommand);
        return NoContent();
    }
}