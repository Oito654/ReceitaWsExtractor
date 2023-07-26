using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReceitaWsExtractor.Application.Queries;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Infra.Context;
using ReceitaWsExtractor.CrossCutting.IoC;
using ReceitaWsExtractor.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ReceitaWsExtractor",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});
builder.Services.AddIdentity<Client, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}
                                           )
                .AddEntityFrameworkStores<ReceitaWsExtractorDbContext>();

var jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");
builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);
var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtBearerTokenSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtBearerTokenSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


builder.Services.AddInfra(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddMediatR(typeof(GetAllClientsQuery).GetTypeInfo().Assembly);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.json", "V1"); x.RoutePrefix = "swagger"; });
app.UseAuthentication();

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();