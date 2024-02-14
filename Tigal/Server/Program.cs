using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Tigal.Server.Data;
using Tigal.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
NSwag.OpenApiSecurityScheme openApiSecurityScheme = new NSwag.OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header
    };

List<string> token = new List<string>();
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["key"]));

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAllIPs", builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Option => {
    Option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        IssuerSigningKey = key,
        ValidateIssuer = false,
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x => {
    var actioncontext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actioncontext);
});

builder.Services.AddSwaggerDocument(config =>
    {
        config.AddSecurity("Bearer", token, openApiSecurityScheme);
        config.PostProcess = document =>
        {
            document.Info.Version = "v1";
            document.Info.Title = "Tigal Limited";
            document.Info.Description = "Tigal Api";
            document.Info.TermsOfService = "https://tigal.africa";
            document.Info.Contact = new NSwag.OpenApiContact
            {
                Name = "Stephen Kaguri",
                Email = "kaguris96@gmail.com",
                Url = "https://tigal.africa"
            };
            document.Info.License = new NSwag.OpenApiLicense
            {
                Name = "USE UNDER TIGAL LICENSE API RULES AND REGULATIONS",
                Url = "https://tigal.africa"
            };
        };
    });

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ManagementDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CashDBContext") ?? throw new InvalidOperationException("Connection string 'CashDBContext' not found.")));
//builder.Services.AddSignalR();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenApi();
app.UseSwaggerUi3();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllIPs");
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
