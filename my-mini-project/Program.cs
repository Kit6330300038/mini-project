
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using my_mini_project.IServices;
using my_mini_project.Services;
using my_mini_project.ViewModel;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));
builder.Services.AddScoped(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    var database = builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName");
    return client.GetDatabase(database);
});
var jwtSettings = new JwtSettings();


builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);
// Register your database and collections

// Register other services
builder.Services.AddControllers();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
    {
        options.LoginPath = "/api/Account/login";
        options.LogoutPath = "/api/Account/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetValue<string>("JwtSettings:SecretKey")
        )),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true

    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(a =>
{
    a.AddSecurityDefinition("Berrer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    a.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                Id = "Berrer",
                Type = ReferenceType.SecurityScheme,
                },
            Scheme = "Berrer",
            Name = "Berrer",
            In = ParameterLocation.Header
            },
        new List<string>()
        }

    });
}
);
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IAccountServices, AccountService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
