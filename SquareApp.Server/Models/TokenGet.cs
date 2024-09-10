namespace SquareApp.Server.Models
{
    public class TokenGet
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public required string Status { get; set; }
        public required string Method { get; set; }
        public required string CardBrand { get; set; }
        public required string CardLast4 { get; set; }
        public required int CardExpMonth { get; set; }
        public required int CardExpYear { get; set; }
        public required string BillingPostalCode { get; set; }
        public required DateTime CreatedAt { get; set; }



    }
}
