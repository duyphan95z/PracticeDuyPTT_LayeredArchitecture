using DuyPTT_Application;
using DuyPTT_Repositories;
using DuyPTT_Integrations;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PracticeDuyPTT_Api.MiddleWare;
using System.Text;

using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Mục 3a trong bài tập: OpenTelemery
builder.Services.AddOpenTelemetry()
	.WithTracing(tracerProviderBuilder =>
	{
		tracerProviderBuilder
			.SetResourceBuilder(ResourceBuilder.CreateDefault()
				.AddService("PracticeDuyPTT_AuthenAuthor")) 
			.AddAspNetCoreInstrumentation()  
			.AddHttpClientInstrumentation()  
			.AddConsoleExporter();          
	});
// Mục 8 trong bài tập: API version
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true; 
	options.AssumeDefaultVersionWhenUnspecified = true; 
	options.DefaultApiVersion = new ApiVersion(1, 0); 
	options.ApiVersionReader = new UrlSegmentApiVersionReader(); 
});

// Mục 14 trong bài tập: RateLimit
//builder.Services.AddRateLimiter(options =>
//{
//	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
//		RateLimitPartition.GetFixedWindowLimiter(
//			partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
//			factory: partition => new FixedWindowRateLimiterOptions
//			{
//				AutoReplenishment = true,
//				PermitLimit = 3,
//				QueueLimit = 0,
//				Window = TimeSpan.FromSeconds(30)
//			}));
//	options.OnRejected = async (context, token) =>
//	{
//		context.HttpContext.Response.StatusCode = 429;
//		await context.HttpContext.Response.WriteAsync("Gọi api từ từ thôi, gọi chi nhiều vậy.! ", cancellationToken: token);
//	};
//});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? Secretkey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY_DuyPTT", EnvironmentVariableTarget.Machine);
var key = Encoding.ASCII.GetBytes(Secretkey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

var services = builder.Services;
var configuration = builder.Configuration;
services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
services.AddRepositories(configuration);
services.AddApplication(configuration);
services.AddIntegration(configuration);

// Mục 4 trong bài tập: Healthcheck cho service và các dịch vụ phụ trợ (database, kafka, etc)
//builder.Services.AddHealthChecks()
//	.AddCheck("service_health", () =>
//	{
//		bool healthCheckResultHealthy = true;
//		return healthCheckResultHealthy
//			? HealthCheckResult.Healthy("Service is healthy")
//			: HealthCheckResult.Unhealthy("Service is unhealthy");
//	});
//var connectionString = builder.Configuration.GetConnectionString("SqlServer");
//builder.Services.AddHealthChecks()
//	.AddSqlServer(
//		connectionString: connectionString,
//		name: "Sql-Server",
//		failureStatus: HealthStatus.Unhealthy,
//		tags: new[] { "db", "sql" });
//builder.Services.AddHealthChecksUI()
//	.AddInMemoryStorage();


var app = builder.Build();
//app.UseRateLimiter();

app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();




// Mục 4 trong bài tập: Healthcheck cho service và các dịch vụ phụ trợ (database, kafka, etc)
//app.MapHealthChecks("/health", new HealthCheckOptions
//{
//	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});
//app.MapHealthChecksUI(setup => setup.UIPath = "/health-ui");

// Mục 9 trong bài tập: Xử lý exception global
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHsts();

app.Run();
