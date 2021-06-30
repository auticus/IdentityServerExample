using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //in order for Identity Server to run correctly, the following items must be added beyond the basic AddIdentityServer() call
            //this is for "In Memory" configuration
            //https://identityserver4.readthedocs.io/en/latest/topics/startup.html
            //this is often used for testing or development where it is not necessary to dynamically consult database at runtime for config data
            //can also be appropriate in production if config rarely changes or is not convenient to restart the app if the value must be changed

            //https://docs.identityserver.io/en/latest/topics/deployment.html
            //The way you store the data depends on your environment.  In situations where configuration data rarely changes, they recommend using
            //in-memory stores and code or config files
            //In highly dynamic environments (SaaS) use a database or config service to load config dynamically
            //build a config store by implementing IResourceStore and IClientStore

            //by default, IdentityServer injects as an in-memory version

            //you can access this in postman or thunder client or whatever by visiting https://localhost:5005/.well-known/openid-configuration
            //5005 is how we have this service listed as 
            services.AddControllersWithViews();
            services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                //.AddTestUsers(Config.TestUsers)
                .AddTestUsers(TestUsers.Users)
                .AddDeveloperSigningCredential(); //temp signing credentials to use in dev when you dont have a certificate to use
                                                    //uses the tempkey.jwk
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); //uses static files from the wwwroot
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //adds default route Home/Index/id? which exists in the Quickstart folder
            });
        }
    }
}
