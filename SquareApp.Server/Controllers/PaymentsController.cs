using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Square;
using Square.Exceptions;
using Square.Models;
using SquareApp.Server.Dtos;
using SquareApp.Server.Models;
using SquareApp.Server.Services;

namespace SquareApp.Server.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ISquareClient _squareClient;
        private readonly TokenService _tokenService;
        public PaymentsController(ISquareClient squareClient, TokenService tokenService)
        {
            _squareClient = squareClient;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> getTokens()
        {
            IEnumerable<TokenGet> tokens = await _tokenService.getAll();
            return Ok(tokens);
        }

        [HttpPost]
        [Route("sendToken")]
        public async Task<IActionResult> sendToken(TokenSendDTO token)
        {
            int dbId = await _tokenService.uploadToken(token);
            var amountMoney = new Money.Builder()
            .Amount(100L)
            .Currency("CAD")
            .Build();

            var body = new CreatePaymentRequest.Builder(sourceId: token.token, idempotencyKey: Guid.NewGuid().ToString())
              .AmountMoney(amountMoney)
              .Build();
            try
            {
                var result = await _squareClient.PaymentsApi.CreatePaymentAsync(body: body);
                Console.WriteLine($"Content Type: {result}");
                var summary = new
                {
                    PaymentId = result.Payment.Id,
                    Status = result.Payment.Status,
                    Amount = result.Payment.AmountMoney.Amount,
                    Currency = result.Payment.AmountMoney.Currency,
                    ReceiptUrl = result.Payment.ReceiptUrl,
                    dbId = dbId.ToString()
                };
                return Ok(summary);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Failed to make the request");
                Console.WriteLine($"Response Code: {e.ResponseCode}");
                Console.WriteLine($"Exception: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
