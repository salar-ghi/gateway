using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGatewat.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddCors();

var authenticationProviderKey = "Bearer";
builder.Services
    //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5010/api/Auth/Index";
        options.Audience = "ApiOne";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
        };
    });




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services
    .AddOcelot(builder.Configuration);


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MultiAuthPolicy", policy =>
//    {
//        policy.AddAuthenticationSchemes("Scheme1", "Scheme2");
//        policy.RequireAuthenticatedUser();
//    });
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseOcelot();

app.UseRouting();
app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Define the protected routes

//app.Use(async (context, next) =>
//{
//    // check if the request requires authorization
//    var requiresAuthorization = context.Request.Path.StartsWithSegments("/apiOne");
//    if (requiresAuthorization && !context.User.Identity.IsAuthenticated)
//    {
//        context.Response.Redirect("https://localhost:5010/api/Auth/Index");
//        return;
//    }
//    await next();
//});

string[] protectedRoutes = { "/apiOne", "/Apione/Products" };
var identityServiceUrl = "https://localhost:5010/api/Auth/Index";

//?????????????????????????????
//app.UseMiddleware<CheckAuthMiddleware>(identityServiceUrl, protectedRoutes);
//app.UseMiddleware<CheckIdentityMiddleware>();

await app.UseOcelot();
//app.UseOcelot().Wait();
app.Run();
