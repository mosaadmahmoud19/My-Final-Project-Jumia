using FinalProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using My_Final_Project.Controllers;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Reposatory.ClassesReposatory;
using My_Final_Project.Reposatory.Interfaces;
using My_Final_Project.Reposatory.InterfacesReposatory;
using My_Final_Project.Services;
using System.Security.Claims;
using System.Text;

namespace My_Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string txt = "hi";
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyFinal-Project", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {

                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]
                        {

                        }
                    }
                });
            });


            builder.Services.AddDbContext<ITIStore>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("Con1")));

            builder.Services.AddScoped<IProducsRepo, ProductRepo>();
            builder.Services.AddScoped<ImagesController, ImagesController>();
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IwishListRepo, WishListRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepository>();
            builder.Services.AddScoped<IFilterRepo, FilterRepo>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
               options =>
               {
                   options.User.RequireUniqueEmail = true;
                   options.SignIn.RequireConfirmedEmail = false;
                   options.Lockout.MaxFailedAccessAttempts = 3;
                   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                   options.Password.RequireNonAlphanumeric = false;

               }
               ).AddEntityFrameworkStores<ITIStore>().AddDefaultTokenProviders();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            //Add Config for Required Email
            builder.Services.Configure<IdentityOptions>(
                opts => opts.SignIn.RequireConfirmedEmail = true
                );

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( options => {
                //   var algorithmAndKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
            var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization(options =>
            {
                // options.AddPolicy("roles", policy => policy.RequireClaim("role","User","Seller"));
                //options.AddPolicy("EgyptOnly", policy =>
                // policy.RequireClaim("Nationality", "Egyptian", "Saudi")
                //    .RequireClaim(ClaimTypes.Role, "Admin"));

                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")); //just write role in string to fix it
                options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
                options.AddPolicy("SellerOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Seller"));

                options.AddPolicy("UserOrSeller", policy => policy.RequireClaim(ClaimTypes.Role, "User", "Seller"));
                options.AddPolicy("AdminOrSeller", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "Seller"));
                options.AddPolicy("AdminOrUser", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
                options.AddPolicy("AdminOrUserOrSeller", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User", "Seller"));

                options.AddPolicy("ElkoFelkol", policy => policy.RequireClaim(ClaimTypes.NameIdentifier, "1"));
                //options.AddPolicy("ElkoFelkol", policy => policy.RequireClaim(ClaimTypes.NameIdentifier, "1"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyFinal-Project v1"));
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API"); });

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/wwwroot"
            });

            app.UseCors(txt);
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
