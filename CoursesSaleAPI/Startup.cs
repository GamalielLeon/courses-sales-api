using CoursesSaleAPI.Extensions;
using CoursesSaleAPI.Helpers.ErrorHandler;
using CoursesSaleAPI.Helpers.ValidationsHandler;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security;
using Security.Constants;
using Security.Contracts;

namespace CoursesSaleAPI
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
            services.AddDbContext<OnlineCoursesContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("OnlineCoursesDB")));

            /*This works only with the "AutoMapper.Extensions.Microsoft.DependencyInjection" package:
              services.AddAutoMapper(typeof(MappingProfile))*/
            services.AddAutoMapper();

            /*Configure a filter for all controllers. This will act as a middleware to validate any request DTO.
              Then, disable automatic model validations handled by ASP.NET Core.*/
            services.AddControllers(static opt =>
            {
                var policy = new AuthorizationPolicyBuilder(System.Array.Empty<string>()).RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
                opt.Filters.Add(typeof(ValidateRequestDTOs));
            }).ConfigureApiBehaviorOptions(static opt => opt.SuppressModelStateInvalidFilter = true);

            //Add and configure AspNetCore.Identity.
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<OnlineCoursesContext>().AddSignInManager<SignInManager<User>>();

            //Add a class in charge of generating JWT.
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            //Configure default JWT settings.
            services.AddAuthentication(static opt =>
            {
                /*This code yields a code 401 response for unauthenticated users.
                  Without this code, the response is sent as 404 for protected endpoints, no matter the token is valid or not.*/
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(static opt =>
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true, //Any request type has to be validated.
                    IssuerSigningKey = ConstantsSecurity.Key, //My secret keyword.
                    ValidateAudience = false, //Any kind of audience (IPs) can generate tokens.
                    ValidateIssuer = false, //The API is able to send a token to any IP.
                }
            );

            services.AddSwaggerGen(static c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoursesSaleAPI", Version = "v1" });
            });

            services.AddServices();
            services.AddRepositories();
            services.AddUnitOfWork();

            services.AddCors(static o => o.AddPolicy("CorsPolicy", static builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(static c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoursesSaleAPI v1"));
            }

            app.UseMiddleware<CustomExceptionMiddleware>();

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( static endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
