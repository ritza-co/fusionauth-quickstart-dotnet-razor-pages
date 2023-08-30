using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
SetupAuthentication(builder);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


static void SetupAuthentication(WebApplicationBuilder builder)
{

    var services = builder.Services;

    services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect("FusionAuth", options => {

        // TODO: Uncomment only if you are running FusionAuth on localhost for development: 
        // options.RequireHttpsMetadata = false;
        options.Authority = $"{builder.Configuration["FusionAuth:Issuer"]}";

        options.ClientId = builder.Configuration["FusionAuth:ClientId"];
        options.ClientSecret = builder.Configuration["FusionAuth:ClientSecret"];

        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.CallbackPath = new PathString("/callback");
        options.ClaimsIssuer = "FusionAuth";
        options.SaveTokens = true;

        // Add handling of logout
        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProviderForSignOut = (context) =>
            {
                var logoutUri = $"{builder.Configuration["FusionAuth:Issuer"]}/oauth2/logout?client_id={builder.Configuration["FusionAuth:ClientId"]}";

                context.Response.Redirect(logoutUri);
                context.HandleResponse();
                return Task.CompletedTask;
            }
        };
    });
}
