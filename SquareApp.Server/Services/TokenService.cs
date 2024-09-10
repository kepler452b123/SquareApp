using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SquareApp.Server.Models;
using System.ComponentModel.Design.Serialization;
using SquareApp.Server.Dtos;

namespace SquareApp.Server.Services
{
    public class TokenService
    {
        public required string _connectionString { get; init; }

        public TokenService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> uploadToken(TokenSendDTO token)
        {
            SqlConnection connection = CreateConnection();
            string create = @"INSERT INTO PaymentTokens (Token, Status, Method, CardBrand, CardLast4, CardExpMonth, CardExpYear, BillingPostalCode) 
                            VALUES (@token, @status, @method, @cardBrand, @cardLast4, @cardExpMonth, @cardExpYear, @billingPostalCode) 
                            SELECT Cast(Scope_Identity() as int)";

            int id = await connection.ExecuteScalarAsync<int>(create, new { token = token.token, status=token.status, method=token.method, cardBrand=token.cardBrand, cardLast4=token.cardLast4, cardExpMonth=token.cardExpMonth, cardExpYear=token.cardExpYear, billingPostalCode=token.billingPostalCode });
            return id;
        }

        public async Task<IEnumerable<TokenGet>> getAll()
        {
            SqlConnection connection = CreateConnection();
            string selectAll = "SELECT * FROM PaymentTokens";

            var tokens = await connection.QueryAsync<TokenGet>(selectAll);
            //TODO: make a TokenGetDTO to store all the token information to send back to controller
            return tokens;
        }


    }
}
