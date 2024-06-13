using backend2.Contexto;
using backend2.Extensions;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Mapppings;
using backend2.Repositorios;
using backend2.Servicios;
using backend2.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

ConfigurationManager configuration = builder.Configuration;

//Contexto
builder.Services.AddDbContext<RegistroGeneralContext>(item => item.UseSqlServer(configuration.GetConnectionString("cadena")));
//JWTSettings
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
//Cors
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        policy => {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination");
            /*En nuestras peticiones de GetAll vamos a devolver metadatos los cuales van en las cabeceras de la respuesta, por lo que para permitir que esto se envíe, debemos
              indicar los headers que vamos a enviar */
        });
});

//Validación del token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     options.TokenValidationParameters = new TokenValidationParameters {
         ValidateIssuer = true,
         ValidateAudience = false,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuers = [configuration["JwtSettings:Issuer"]],
         ValidAudiences = [configuration["JwtSettings:Audience"]],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:KeySecret"])),
         ClockSkew = TimeSpan.Zero
     });

/* Todas las rutas requieren authenticación (a excepción de las marcadas con [AllowAnonymous]), si el token del usuario que haga la petición no cuenta con la claim requerida,
La petición será rechazada con un 401 - "Unauthorized"*/
builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("ESUSUARIO", e => e.RequireClaim("ESUSUARIO"));
    options.AddPolicy("ESADMINISTRADOR", e => e.RequireClaim("ESADMINISTRADOR"));
});

//Token en el swagger
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TFGPractica", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Injección de los servicios y nuestro repositorio
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IModeloService, ModeloService>();
builder.Services.AddScoped<ISesionService, SesionService>();
builder.Services.AddScoped<IEmpresaActivaService, EmpresaActivaService>();
builder.Services.AddScoped<IEmpresaService, ServicioEmpresa>();
builder.Services.AddScoped<IEmpresasUsuariosService, EmpresasUsuarioService>();
builder.Services.AddScoped<ICombustibleService, CombustibleService>();
builder.Services.AddScoped<ICambioEmpresaActivaService, CambioEmpresaActivaService>();

var app = builder.Build();


var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TFGPractica");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
}

app.UseCors();

//Uso del middleware

app.UseMiddleware<JwtMiddleware>();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
