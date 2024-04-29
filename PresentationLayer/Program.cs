using BusinessAccessLayer.Services;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PresentationLayer.Infrastucture.AutoMapper;
using PresentationLayer.Infrastucture.Services;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BootcampDbContext>(
options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=TalentosDB;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddIdentityCore<User>(options =>
{
    //options.Password.RequiredLength = 6;
    //options.Password.RequireDigit = false;
    //options.Password.RequireLowercase = false;
    //options.Password.RequireUppercase = false;
    //options.SignIn.RequireConfirmedAccount = false;
    //options.User.RequireUniqueEmail = true;
    //options.Password.RequiredUniqueChars = 0;

    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole<int>>()
    .AddRoleManager<RoleManager<IdentityRole<int>>>()
    .AddSignInManager<SignInManager<User>>()
    .AddRoleValidator<RoleValidator<IdentityRole<int>>>()
    .AddEntityFrameworkStores<BootcampDbContext>();

builder.Services.AddScoped<iTokenService, TokenService>();

//Transient: new instance every time it is called
//builder.Services.AddTransient<iPetService, PetService>();
//Scoped: new instance for each request
builder.Services.AddScoped<iPetService, PetService>();
//Singleton: same instance for all requests
//builder.Services.AddSingleton<iPetService, PetService>();

builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
//AutoMapper
builder.Services.AddAutoMapper(assemblies: new Assembly[] { typeof(AutoMapperProfile).Assembly });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    setup =>
    {
        setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet API", Version = "v1", });
        setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Input your Bearer token in this format - Bearer {your token key} to access this API"
        });
        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                 new OpenApiSecurityScheme
                 {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                 },
                 new string[] {}
            }
        });
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BootcampDbContext>();
//     try
//     {
//         var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

//         if (!roleManager.RoleExistsAsync("Owner").Result)
//         {
//             var role = new IdentityRole<int>();
//             role.Name = "Owner";
//             roleManager.CreateAsync(role).Wait();
//         }

//         if (!roleManager.RoleExistsAsync("Admin").Result)
//         {
//             var role = new IdentityRole<int>();
//             role.Name = "Admin";
//             roleManager.CreateAsync(role).Wait();
//         }
//         if(context.Users.Count() < 40)
//         {

//             var userManager = services.GetRequiredService<UserManager<User>>();

//             string[] names = new string[]   {   "Mateo", "Santiago", "Matías", "Sebastián", "Liam",
//                                                 "Thiago", "Lucas", "Benjamín", "Nicolás", "Emiliano",
//                                                 "Samuel", "Gael", "Joaquín", "Leonardo", "Felipe",
//                                                 "Martín", "Alejandro", "Tomás", "Daniel", "Bruno",
//                                                 "Diego", "Gabriel", "Emmanuel", "Ethan", "Julián",
//                                                 "Valentina", "Lila", "Vivian", "Nora", "Ángela",
//                                                 "Elena", "Clara", "Eliana", "Alana", "Miranda",
//                                                 "Amanda", "Diana", "Ana", "Penélope", "Aurora",
//                                                 "Alexandría", "Lola", "Alicia", "Amaya", "Alexia",
//                                                 "Jazmín", "Mariana", "Alina", "Lucía", "Fátima"
//                                             };
//             string[] lastNames = new string[] { "Pérez", "García", "Rodríguez", "López", "González",
//                                                 "Martínez", "Sánchez", "Ramírez", "Gómez", "Torres",
//                                                 "Vargas", "Jiménez", "Castro", "Morales", "Silva",
//                                                 "Ortega", "Méndez", "Aguilar", "Delgado", "Paredes",
//                                                 "Ríos", "Mendoza", "Navarro", "Velasco", "Rivas",
//                                                 "Peña", "Cordero", "Contreras", "Ponce", "Morales",
//                                                 "Santos", "Juárez", "Núñez", "León", "Cervantes",
//                                                 "Rangel", "Soto", "Hernández", "Cárdenas", "Ibarra",
//                                                 "Delgado", "Sánchez", "Miranda", "Guzmán", "Valencia",
//                                                 "Franco", "Vargas", "Villarreal", "Cordero", "Cárdenas"
//                                             };

         
            
//               for (int i = 1; i < 50; i++)
//               {
//                   Random random = new Random();
//                   var name = names[random.Next(names.Length)];
//                   var lastName = lastNames[random.Next(lastNames.Length)];
//                   var email = name.ToLower() + lastName.ToLower() + "@gmail.com";
//                   var user = new User() 
//                   {
//                       Name = name,
//                       LastName = lastName ,
//                       Email = email,
//                       UserName = email,
//                       Birthday = DateTime.Now
//                   };
//                   if(!userManager.Users.Any(x => x.UserName.ToLower() == email.ToLower()))
//                     {
//                         var result = await userManager.CreateAsync(user, "Asdf1234.");
//                         if (result.Succeeded)
//                         {
//                             await userManager.AddToRoleAsync(user, "Owner")
//                             ;
//                         }
                                                                                   
//                     } 
//             } 
//         }
// }
//     catch (Exception)
//     {
//         throw;
//     }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//CORS 
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.WithOrigins("http://localhost:4200")
    //.AllowAnyOrigin()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
   );


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
 
 





