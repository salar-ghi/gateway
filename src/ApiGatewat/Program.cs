using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGatewat.Configuration;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//builder.Services.AddCors();

var ocelotdata = builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
var authenticationProviderKey = "Bearer";
builder.Services.AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, options =>
    {
        options.Authority = "https://localhost:5010/api/Auth/Index"; // auth server
        options.Audience = "NitroIdentity"; // audience
        options.ClaimsIssuer = "NitroIdentityJwt";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),

        };
    })
    .AddJwtBearer("AnotherAuthScheme", options =>
    {
        options.Authority = "https://localhost:5010/"; // auth server
        options.Audience = "NitroIdentity"; // audience
    });

//builder.Services.AddHttpContextAccessor();

//builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<AuthDelegatingHandler>();


builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<AuthDelegatingHandler>(true);


//builder.Services.AddAuthorization(options =>
//    options.AddPolicy("ApiScope", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("scope", "api1");
//    })
//);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();

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

//app.UseMiddleware<CheckIdentityMiddleware>();
//app.UseMiddleware<AuthDelegatingHandler>();

//await app.UseOcelot();


//Console.Clear();
app.Run();
//Console.Clear();