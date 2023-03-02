using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TripPlanner.Context;
using TripPlanner.Controllers.Mappers;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Repositories;
using TripPlanner.Logic.Services;
using TripPlanner.Logic.Services.EmailService;
using TripPlanner.Logic.Services.EmailService.Smtp;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<TripPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TripPlannerConnectionString") ?? throw new InvalidOperationException("Connection string 'TripPlannerConnectionString' not found.")));

// Identity
builder.Services.AddIdentity<User,Role>(options=>
                                        {
                                            options.SignIn.RequireConfirmedAccount = false;
                                            options.Lockout.MaxFailedAccessAttempts = 5;
                                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
	                                    })
                .AddEntityFrameworkStores<TripPlannerContext>()
                .AddDefaultTokenProviders();
builder.Services.Configure<EmailServiceConfiguration>(builder.Configuration.GetSection("EmailServiceConfigurationModel"));
// Dependency Injection for Repositories
builder.Services.AddScoped<IBusinessTripRequestRepository, BusinessTripRequestRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<ISmtpClient, RealSmtpClient>();
builder.Services.AddScoped<IEmailService, EmailService>();
// AutoMapper
builder.Services.AddAutoMapper(typeof(BusinessTripRequestProfile));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
                                   {
                                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                    })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new
        SymmetricSecurityKey
        (Encoding.UTF8.GetBytes
        (builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddCors(options => options.AddPolicy(name: "TripPlanner-UI",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Trip Planner API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("TripPlanner-UI");

app.MapControllers();

app.Run();
