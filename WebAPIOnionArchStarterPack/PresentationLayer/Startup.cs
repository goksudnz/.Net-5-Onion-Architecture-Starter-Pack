using System;
using System.IO;
using System.Reflection;
using System.Text;
using BusinessLogicLayer;
using DataAccessLayer;
using Domain.Entities;
using Domain.Models.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PresentationLayer.Filters;

namespace PresentationLayer
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
            services.Configure<JwtConfig>(Configuration.GetSection(PLConstants.ConfigurationConstants.JwtConfig));
            
            #region Adding Tools
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddAutoMapper(config => config.AddProfile(typeof(MappingProfile)));
            
            // Adding filters
            services.AddMvc().AddMvcOptions(option =>
            {
                option.Filters.Add(new GlobalExceptionFilter());
                option.Filters.Add(new ActionAuthorizationFilter());
            });
            
            // JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.UTF8.GetBytes(Configuration[PLConstants.ConfigurationConstants.JwtSecret]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            #endregion

            #region Swagger Configurations
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PresentationLayer", Version = "v1" });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
            #endregion
            
            #region Identity Configurations
            services.AddDefaultIdentity<AppUser>(options =>
                {
                    // sign in options
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedPhoneNumber = true;
                    options.SignIn.RequireConfirmedEmail = true;

                    // password requirement options
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = true;

                    // lockout options
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

                    // user options
                    options.User.RequireUniqueEmail = true;
                }).ConfigureIdentityBuilder()
                .AddDefaultTokenProviders();
            #endregion

            services.AddDataAccessLayerDependencies();
            services.AddBusinessLayerDependencies();
            services.AddAuthorizationHandlers();
            services.AddPresentationLayerDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PresentationLayer v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                
                // if using fallback router you can use the given code below.
                // endpoints.MapFallbackToController("spa/{controller=Fallback}/{action=Index}", "Index", "Fallback");
            });
        }
    }
}