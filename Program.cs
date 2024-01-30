using myappdotnet.Controllers;
using Microsoft.EntityFrameworkCore;
using myappdotnet.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using myappdotnet.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization(); 
builder.Services.AddControllers();
builder.Services.AddScoped<UsersController>(); 
builder.Services.AddScoped<LocationsController>(); 

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });


builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder("BasicAuthentication")
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

