
using App_Bets.ErrosMiddleware;
using App_Bets.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddContextAppBet(builder.Configuration); 
builder.Services.AddInjecaoDependencias(builder.Configuration);
builder.Services.AddSettingsController();
builder.Services.AddJwtAuthetication(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "Sistema de controle de apostas esportivas",
        Description = "Api que gerencia gestão de apostas, para que o usuario tenha controel sobre seus ganhos",
        Contact = new OpenApiContact
        {
            Name = "Fabio dos Santos",
            Email = "f.santosdev1992@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/f%C3%A1bio-dos-santos-518612275/")
        }

    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                      new OpenApiSecurityScheme
                      {
                          Reference = new OpenApiReference
                           {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                           }
                       },
                           new string[] {}
                   }
                });

});

var app = builder.Build();

app.UseCors("AllowNextJS");


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseMiddleware(typeof(ErroMiddleware));

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseRouting();      

app.UseAuthentication(); 

app.UseAuthorization();    

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");

