using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebAPI.Data;
using WebAPI.Data.Services.Structures;
using WebAPI.Data.Services;
using AutoMapper;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Amazon.S3;
using WebAPI.Data.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();     
builder.Services.AddCors(opt=>{
    opt.AddPolicy("EnableCORS", policy => {
        policy.SetIsOriginAllowed(origin => true)
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IPhoto,PhotoService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
IdentityModelEventSource.ShowPII = true;

var keySection = builder.Configuration.GetSection("AppSettings:Key");
builder.Services.Configure<KeyConfiguration>(keySection);
var secretKey = keySection.Value;

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    //opt.Audience = "http://localhost:5265/";
    //opt.Authority = "http://localhost:5265/";
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddMvc();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//keySection = builder.Configuration.GetSection("AWS:BucketName");
//builder.Services.Configure<BucketConfiguration>(keySection); 

builder.Services.Configure<AWSOptions>(builder.Configuration.GetSection(AWSOptions.AWS));
//builder.Services.Configure<AWSOptions>(builder.Configuration.GetSection("AWS:AccessKey"));
//builder.Services.Configure<AWSOptions>(builder.Configuration.GetSection("AWS:AccessSecret"));
//keySection = builder.Configuration.GetSection("AWS:AccessSecret");
//builder.Services.Configure<AccessSecretConfirguration>(keySection);
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<S3Service>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.ConfigureExceptionHandler(app);     //extension added

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("EnableCORS");

app.UseAuthentication();    //added
app.UseAuthorization();

app.MapControllers();

app.Run();
