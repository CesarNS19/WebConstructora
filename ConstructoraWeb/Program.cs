using Microsoft.AspNetCore.StaticFiles;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".data"] = "application/octet-stream";
provider.Mappings[".wasm"] = "application/wasm";
provider.Mappings[".br"] = "application/octet-stream";
provider.Mappings[".js"] = "application/javascript";

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider,
    OnPrepareResponse = context =>
    {
        var path = context.Context.Request.Path.Value;
        var extension = Path.GetExtension(path);

        if (extension == ".gz" || extension == ".br")
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path) ?? "";
            if (provider.TryGetContentType(fileNameWithoutExtension, out string? contentType))
            {
                context.Context.Response.ContentType = contentType;
                context.Context.Response.Headers.Append("Content-Encoding", extension == ".gz" ? "gzip" : "br");
            }
        }
    },
});

app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Auth}/{id?}");

app.Run();