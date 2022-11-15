using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trainer_calender_version_2._0._0.Models;
using Trainer_calender_version_2._0._0.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<ITrainerService, TrainerService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();
app.MapGet("/", () => "Hello World!");
app.MapPost("/login", (UserLogin user, IUserService service) => Get(user, service));
IResult Get(UserLogin user, IUserService service)
{
    if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.NotFound("User not found");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,loggedInUser.UserName),
            new Claim(ClaimTypes.Role,loggedInUser.Role)
        };
        var token = new JwtSecurityToken(
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audiece"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
            );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
    return Results.NotFound("Something went wrong");
}
app.MapGet("/GetAllTrainer", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")] (IAdminService service) => GetAllTrainer(service));
app.MapGet("/GetTrainerById", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")] (IAdminService service,int id) => GetTrainerById(service,id));
app.MapGet("/GetTrainersBySkill", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")] (IAdminService service, int id) => GetTrainersBySkill(service, id));
app.MapGet("/GetAllSkill", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")] (IAdminService service) => GetAllSkill(service));
app.MapGet("/GetAllSession", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")] (IAdminService service) => GetAllSession(service));
app.MapPost("/AddSession", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Trainer")] (ITrainerService service,Session session) => AddSession(service, session));
app.MapPut("/UpdateSession", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Trainer")] (ITrainerService service, int id,Session session) => UpdateSession(service, id,session));
app.MapGet("/DeleteSession", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Trainer")] (ITrainerService service,int id) => DeleteSession(service, id));

IResult AddSession(ITrainerService service,Session session)
{
    var ans = service.AddSession(session);
    if(ans != null) return Results.Ok(ans);
    return Results.BadRequest("Something went wrong");
}
IResult UpdateSession(ITrainerService service, int id,Session session)
{
    var ans = service.UpdateSession(id,session);
    if (ans != null) return Results.Ok(ans);
    return Results.BadRequest("Something went wrong");
}
IResult DeleteSession(ITrainerService service, int id)
{
    var ans = service.DeleteSession(id);
    if (ans != null) return Results.Ok(ans);
    return Results.BadRequest("Something went wrong");
}
IResult GetAllTrainer(IAdminService service)
{
    var ans = service.GetAllTrainer();
    if (ans == null) return Results.NotFound("not found");
    return Results.Ok(ans);

}
IResult GetAllSkill(IAdminService service)
{
    var ans = service.GetAllSkill();
    if (ans == null) return Results.NotFound("not found");
    return Results.Ok(ans);

}
IResult GetAllSession(IAdminService service)
{
    var ans = service.GetAllSession();
    if (ans == null) return Results.NotFound("not found");
    return Results.Ok(ans);

}
IResult GetTrainerById(IAdminService service,int id)
{
    var ans = service.GetTrainerById(id);
    if (ans == null) return Results.NotFound("not found");
    return Results.Ok(ans);
}
IResult GetTrainersBySkill(IAdminService service, int id)
{
    var ans = service.GetTrainersBySkill(id);
    if (ans == null) return Results.NotFound("not found");
    return Results.Ok(ans);
}
app.UseSwaggerUI();
app.Run();
