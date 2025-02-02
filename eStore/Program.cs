using BusinessObject.models;
using eStore.Services;
using eStore.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Net.payOS;


//intial payos instance
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
					configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
					configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add auto mapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
//add accessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//add http client
builder.Services.AddHttpClient<IProductService, ProductService>(); // add client 
builder.Services.AddHttpClient<IOrderService, OrderService>(); 
builder.Services.AddHttpClient<IOrderDetailService, OrderDetailService>(); 
builder.Services.AddHttpClient<IMemberService, MemberService>(); 
builder.Services.AddHttpClient<ICategoryService, CategoryService>(); 
builder.Services.AddHttpClient<IAuthService, AuthService>();
//add scoped base service
builder.Services.AddScoped<IBaseService, BaseService>();
//add scoped services
builder.Services.AddScoped<IProductService, ProductService>(); // scoped is for case if requrest 10 times, it will the same object
builder.Services.AddScoped<IOrderService, OrderService>(); 
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>(); 
builder.Services.AddScoped<IMemberService, MemberService>(); 
builder.Services.AddScoped<ICategoryService, CategoryService>(); 
builder.Services.AddScoped<IAuthService, AuthService>();
//add singleton payos
builder.Services.AddSingleton(payOS);
// add token provider
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
//add cookie authen and google authen
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              }).AddGoogle(options =>
              {
                  options.ClientId = configuration["Google:ClientId"];
                  options.ClientSecret = configuration["Google:ClientSecret"];
              }); ;


//add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//cors
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
