using Square;
using Square.Authentication;
using SquareApp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var accessToken = builder.Configuration["AuthTokens:AccessToken"];
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<ISquareClient>(sp =>
    new SquareClient.Builder()
        .BearerAuthCredentials(
         new BearerAuthModel.Builder(
             accessToken
         )
         .Build())
    .Environment(Square.Environment.Sandbox)
    .Build());
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();
Console.WriteLine(accessToken);
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
