using BusinessAccessLayer.Services;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddDbContext<BootcampDbContext>(options => 
    options.UseSqlServer("Server=.\\SQLEXPRESS;Database=TalentosDB;Trusted_Connection=True;TrustServerCertificate=True;"));


builder.Services.AddIdentityCore<User>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole<int>>()
    .AddRoleManager<RoleManager<IdentityRole<int>>>()
    .AddSignInManager<SignInManager<User>>()
    .AddRoleValidator<RoleValidator<IdentityRole<int>>>()
    .AddEntityFrameworkStores<BootcampDbContext>();
    

builder.Services.AddScoped<iTokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });





builder.Services.AddControllers();



// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //var context = services.GetRequiredService<BootcampDbContext>();
    //context.Database.Migrate();

    // Add services to the container.
    //if (context.Pets.Count() == 0)
    //{
    //    context.Pets.Add(new Pet { Name = "Coco", Description = "A white dog", Type = "Dog", Birth = DateTime.Now, UserId = 1 });
    //    context.Pets.Add(new Pet { Name = "Zoe", Description = "A black cat", Type = "Cat", Birth = DateTime.Now, UserId = 1 });
    //    context.SaveChanges();
    //}

    //Add roles
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        if (!roleManager.RoleExistsAsync("Owner").Result)
        {
            var role = new IdentityRole<int>();
            role.Name = "Owner";
            roleManager.CreateAsync(role).Wait();
        }


        if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            var role = new IdentityRole<int>();
            role.Name = "Admin";
            roleManager.CreateAsync(role).Wait();
        }

    }
    catch (Exception)
    {

        throw ;
    }
}

  





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
