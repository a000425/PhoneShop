using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MP.Models;
using MP.Repository;
using MP.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddDbContext<PhoneContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MPDatabase")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<BackService>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<BackRepository>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5501")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
        options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5500")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]))
        };
        options.SaveToken = true;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("Token", out string token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
