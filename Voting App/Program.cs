using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voting_App;
using Voting_App.Entities;
using Voting_App.Exceptions;
using Voting_App.Services;


var builder = WebApplication.CreateBuilder(args);
var settingsValues = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var settings = new JwtSettings()
{
    Issuer = settingsValues.Issuer,
    Audience = settingsValues.Audience,
    Key = settingsValues.Key
};

// Add services to the container.

builder.Services.AddAuthentication(
    option =>
    {
        option.DefaultAuthenticateScheme = "Bearer";
        option.DefaultScheme = "Bearer";
        option.DefaultChallengeScheme = "Bearer";
    }
).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = settings.Issuer,
        ValidAudience = settings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
    };
});


builder.Services.AddControllers();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSingleton<VotingDbContext>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton(settings);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSingleton<VoteService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndApp", o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

app.UseCors("FrontEndApp");

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
