using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Morpheus.API.Filters;
using Morpheus.API.Middlewares;
using Morpheus.Repository.Interfaces;
using Morpheus.Repository.MySQL;
using Morpheus.Service;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Morpheus.API
{
    public class Startup
	{
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
		{
            Configuration = configuration;
		}
        
		public void ConfigureServices(IServiceCollection services)
		{
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddMvc(options =>
			{
				options.Filters.Add(typeof(LogFilter));
				options.Filters.Add(typeof(ModelValidatorFilter));
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Morpheus-API", Version = "v1" });
			});


			services.AddDbContext<RepositoryContext>(options => {
				options.UseMySQL(Configuration.GetConnectionString("MySQL"));
			});

			services.AddOptions();

			services.AddSingleton<IConfiguration>(Configuration);

			services.AddScoped(typeof(IDbRepository<>), typeof(Repository<>));
			services.AddTransient<UserService, UserService>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime life)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));

			if (env.IsDevelopment())
			{
				loggerFactory.AddDebug(LogLevel.Debug);
				loggerFactory.AddFile("Logs/dev-{Date}.log", LogLevel.Debug, fileSizeLimitBytes: 1024 * 1024);
				app.UseDeveloperExceptionPage();
			}
			else
			{
				loggerFactory.AddFile("Logs/production-{Date}.log", LogLevel.Information);
				app.UseExceptionHandler("/error");
			}

			app.UseSwagger();

			app.UseSwaggerUi(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Morpheus-API");
			});

            app.UseAuthentication();

			app.UseDefaultFiles();

			app.UseStaticFiles();

			app.UseMiddleware<ExceptionHandlerMiddleware>();

			app.UseMvc();

			
		}
	}
}
