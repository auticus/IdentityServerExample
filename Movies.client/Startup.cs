using System;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Movies.client.Clients;
using Movies.client.HttpHandler;

namespace Movies.client
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
            services.AddControllersWithViews();
            services.AddScoped<IMovieAPIService, MovieAPIService>();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:5005"; //hard coded badness to the identity server
                    options.ClientId = "movies_mvc_client";
                    options.ClientSecret = "smeagolCries";
                    options.ResponseType = "code id_token"; //changed from code to code id_token in hybrid 
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("moviesAPI");

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            //create httpClient used for accessing the Movies.API
            services.AddTransient<AuthenticationDelegatingHandler>(); //intercepts calls, this is a new message handler class

            services.AddHttpClient("MoviesAPIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddHttpMessageHandler<AuthenticationDelegatingHandler>(); //lets this client know that the handler class given will be intercepting its messages

            //create the httpClient to access the Identity server
            services.AddHttpClient("IdentityServerClient", client =>
            {
                client.BaseAddress = new Uri("https://loclhost:5005");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddHttpContextAccessor(); //added when adding hybrid grant type - access the token returned with this accessor
            /* because this is now using hybrid - do not need to access identity server directly any longer
            services.AddSingleton(new ClientCredentialsTokenRequest
            {
                Address="https://localhost:5005/connect/token",
                ClientId = "moviesClient",
                ClientSecret = "smeagolCries",
                Scope = "moviesAPI"
            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
