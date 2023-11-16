using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Runtime.Intrinsics;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring = builder.Configuration.GetConnectionString(name :"Defaultconnection"); 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionstring));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IGenreServices, GenresService>();
builder.Services.AddTransient<IMovieServiecs, MovieServices>();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", info: new OpenApiInfo
    {
        Version = "v1",
        Title = "TestApi",
        Description = "My First API :)",
        TermsOfService = new Uri(uriString: "http://www.google.com"),
        Contact = new OpenApiContact
        {
            Name = "Asmaa Ali",
            Email = "asd@gf.com",
            Url= new Uri(uriString: "http://www.google.com"),

        },
        License=new OpenApiLicense
        {
            Name="My Lences",
            Url= new Uri(uriString: "http://www.google.com"),
        }

    });

    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme= "Berer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Description="Enter Your JWT Key"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                    
                },
                Name="Bearer",
                In=ParameterLocation.Header
            },
           new List<string>()
        }

    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
