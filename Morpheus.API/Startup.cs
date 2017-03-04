using Morpheus.API.Filters;
using Morpheus.API.Middlewares;
using Morpheus.Repository.Interfaces;
using Morpheus.Repository.MySQL;
using Morpheus.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace Morpheus.API
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(options =>
			{
				options.Filters.Add(typeof(LogFilter));
				// TODO: Uncomment to use the basic AuthorizationFilter
				// options.Filters.Add(typeof(BasicAuthorizationFilter));
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

			app.UseJwtBearerAuthentication(new JwtBearerOptions
			{
				AutomaticAuthenticate = true,
				IncludeErrorDetails = true,
				Authority = "https://securetoken.google.com/{PROJECT_ID}",
				TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = "https://securetoken.google.com/{PROJECT_ID}",
					ValidateAudience = true,
					ValidAudience = "{PROJECT_ID}",
					ValidateLifetime = true,
				},
			});

			app.UseDefaultFiles();

			app.UseStaticFiles();

			app.UseMiddleware<ExceptionHandlerMiddleware>();

			app.UseMvc();

			
		}
	}
}
