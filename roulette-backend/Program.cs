using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Roulette.App.Auth;
using Roulette.App.Model.Database;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(
        int.Parse(builder.Configuration.GetSection("APIVersion:Major").Value),
        int.Parse(builder.Configuration.GetSection("APIVersion:Minor").Value)
      );
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("x-api-version"),
        new UrlSegmentApiVersionReader()
    );
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SessionDatabaseConfig>(builder.Configuration.GetSection("SessionDatabase"));
builder.Services.AddScoped<ISessionDatabase, SessionService>();
var connectionString = $"Host={builder.Configuration.GetSection("GameDatabase:Host").Value};" +
        $"Database={builder.Configuration.GetSection("GameDatabase:Database").Value};" +
        $"Port={builder.Configuration.GetSection("GameDatabase:Port").Value};" +
        $"Username={builder.Configuration.GetSection("GameDatabase:Username").Value};" +
        $"Password={builder.Configuration.GetSection("GameDatabase:Password").Value}";
//"Pooling=true;SSL Mode=Require;Trust Server Certificate=true;";

builder.Services.AddDbContext<RouletteDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "TokenAuthenticationScheme";
    options.DefaultChallengeScheme = "TokenAuthenticationScheme";
    options.DefaultScheme = "TokenAuthenticationScheme";
})
    .AddScheme<TokenAuthOptions, TokenAuthenticationScheme>("TokenAuthenticationScheme", options => { });

var CORSPolicyName = "SameOriginPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORSPolicyName,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()  
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors(CORSPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
