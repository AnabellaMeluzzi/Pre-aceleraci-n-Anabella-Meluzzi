using DisneyAPI.Context;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.Repositories;
using DisneyAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SendGrid.Extensions.DependencyInjection;
using System.Text;

namespace DisneyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DisneyAPI", Version = "v1" });
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidAudience = "http://localhost:5000",
                        ValidIssuer = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]))
                    };
                });

            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<DisneyContext>((services , options) =>
            {
                options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("DisneyConnectionString"));
            });            
            services.AddDbContext<UserContext>((services , options) =>
            {
                options.UseInternalServiceProvider(services);
                options.UseSqlServer(Configuration.GetConnectionString("UserConnectionString"));
            });
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddSendGrid(options =>
            {
                options.ApiKey = Configuration["SendGridApiKey"];
            });

            services.AddScoped<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DisneyAPI v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
