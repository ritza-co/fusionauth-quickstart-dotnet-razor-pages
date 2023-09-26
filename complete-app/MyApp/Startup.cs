using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MyApp.Pages
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
            services.AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddRazorPages();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "FusionAuth";
                })
                .AddCookie("cookie", options => { options.Cookie.Name = Configuration["FusionAuth:CookieName"]; })
                .AddOpenIdConnect("FusionAuth", options =>
                {
                    options.Authority = $"{Configuration["FusionAuth:Issuer"]}";

                    options.ClientId = Configuration["FusionAuth:ClientId"];
                    options.ClientSecret = Configuration["FusionAuth:ClientSecret"];

                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.Scope.Clear();
                    options.Scope.Add("openid");

                    options.CallbackPath = new PathString("/fusion-callback");
                    options.ClaimsIssuer = "FusionAuth";
                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;


                    options.Events = new OpenIdConnectEvents
                    {
                        // Handle logout events. This redirects to FusionAuth to logout of the session. 
                        OnRedirectToIdentityProviderForSignOut = (context) =>
                        {
                            var logoutUri =
                                $"{Configuration["FusionAuth:Issuer"]}/oauth2/logout?client_id={Configuration["FusionAuth:ClientId"]}";

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}