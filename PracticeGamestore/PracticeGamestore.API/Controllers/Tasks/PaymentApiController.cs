using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Controllers.Tasks.Models;

namespace PracticeGamestore.Controllers.Tasks;

[ApiController, Route("externalPayment")]
public class PaymentApiController : ControllerBase
{
    private readonly Random _random = new();

    [HttpPost("iban")]
    public IActionResult PayIban([FromBody] IbanModel model)
    {
        if (model is null)
        {
            return BadRequest("Invalidcparameter");
        }

        var result = _random.Next(0, 10);

        if (result > 6)
        {
            return Ok($"super {result}");
        }

        return BadRequest($"something went wrong {result}");
    }

    [HttpPost("card")]
    public IActionResult PayCard([FromBody] CardModel model)
    {
        if (model is null)
        {
            return BadRequest("Invalidcparameter");
        }

        var result = _random.Next(0, 10);

        if (result > 6)
        {
            return Ok($"super {result}");
        }

        return BadRequest($"something went wrong {result}");
    }

    [HttpPost("ibox")]
    public IActionResult PayIbox([FromBody] IboxModel model)
    {
        if (model is null)
        {
            return BadRequest("Invalidcparameter");
        }

        var result = _random.Next(0, 10);

        if (result > 6)
        {
            return Ok($"super {result}");
        }

        return BadRequest($"something went wrong {result}");
    }
}
