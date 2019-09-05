using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CurrencyRate.Api.Services;
using CurrencyRate.Api.Clients;
using CurrencyRate.Api.Serializers;
using CurrencyRate.Api.AppSettingsModels;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {         
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
            );
            services.AddTransient<ICurrencyRateService, KztCurrencyRateService>();
            services.AddTransient<ICurrencyRateService, UahCurrencyRateService>();
            services.AddHttpClient<IBankGovUaClient, BankGovUaClient>()
                .ConfigureHttpClient((c) => 
                {
                    c.BaseAddress = new Uri("https://bank.gov.ua/NBUStatService/v1/");
                })
                .ConfigurePrimaryHttpMessageHandler((c) => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });
            services.AddHttpClient<INationalBankKzClient, NationalBankKzClient>()
                .ConfigureHttpClient((c) =>
                {
                    c.BaseAddress = new Uri("http://www.nationalbank.kz/rss/");
                });
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddSingleton<IXmlSerializer, XmlSerializer>();

            services.AddTransient(provider => provider.GetServices<ICurrencyRateService>().ToDictionary(service => service.GetType().Name, c => c));

            services.AddOptions();

            services.Configure<List<Currency>>(Configuration.GetSection("Currencies"));

            var appSettingsSection = Configuration.GetSection("Authentication");
            services.Configure<Authentication>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Authentication>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
