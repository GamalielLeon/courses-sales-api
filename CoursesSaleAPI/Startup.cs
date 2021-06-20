using CoursesSaleAPI.Extensions;
using CoursesSaleAPI.Helpers.ErrorHandler;
using CoursesSaleAPI.Helpers.ValidationsHandler;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            //This works only with the "AutoMapper.Extensions.Microsoft.DependencyInjection" package:
            //services.AddAutoMapper(typeof(MappingProfile));
            services.AddAutoMapper();
            /*Configure a filter for all controllers. This will act as a middleware to validate any request DTO.
              Then, disable automatic model validations handled by ASP.NET Core.*/
            services.AddControllers(op => op.Filters.Add(typeof(ValidateRequestDTOs))).ConfigureApiBehaviorOptions(static op => op.SuppressModelStateInvalidFilter = true);

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<OnlineCoursesContext>().AddSignInManager<SignInManager<User>>();
            //services.TryAddSingleton<ISystemClock, SystemClock>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoursesSaleAPI", Version = "v1" });
            });

            services.AddServices();
            services.AddRepositories();
            services.AddUnitOfWork();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoursesSaleAPI v1"));
            }

            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
