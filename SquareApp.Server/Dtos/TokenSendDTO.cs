using System.ComponentModel.DataAnnotations;

namespace SquareApp.Server.Dtos
{
    public record class TokenSendDTO(string token, string status, string method, string cardBrand, string cardLast4, int cardExpMonth, int cardExpYear, string? billingPostalCode, int amount)
    {

    }
}
