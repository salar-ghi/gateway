using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGatewat.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

//builder.Services.AddCors();
//builder.Services
//    .AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.Authority = "https://localhost:5010/";
//        options.Audience = "NitroIdentity";
//        options.ClaimsIssuer = "NitroIdentityJwt";
//        //options.Audience = "ApiOne";
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
//        };
//    });


//builder.Services.AddHttpContextAccessor();

//builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<AuthDelegatingHandler>();

var ocelotdata = builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
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

//app.UseAuthentication().UseOcelot().Wait();
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

//app.UseMiddleware<CheckIdentityMiddleware>();
//app.UseMiddleware<AuthDelegatingHandler>();

//await app.UseOcelot();

app.UseOcelot().Wait();
app.Run();
