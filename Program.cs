using System.Text;
using LibraryManagementSystemApp.Components;
using LibraryManagementSystemApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Secret key
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
var key = Encoding.ASCII.GetBytes(jwtKey);


// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "ServerCookie";
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie("ServerCookie")
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5263") });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
