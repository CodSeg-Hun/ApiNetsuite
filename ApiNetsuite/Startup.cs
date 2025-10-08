using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ApiNetsuite
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
            var cadenaConexionSqlConfiguracion = new AccesoDatos(Configuration.GetConnectionString("SQL"));
            services.AddSingleton(cadenaConexionSqlConfiguracion);
            //services.AddControllers();
            services.AddSingleton<IActivoSQL, ActivoSQL>();
            services.AddSingleton<IBienSQL, BienSQL>();
            services.AddSingleton<IBancoSQL, BancoSQL>();
            services.AddSingleton<IClienteSQL, ClienteSQL>();
            services.AddSingleton<IEstadoSQL, EstadoSQL>();
            services.AddSingleton<IUsuariosSQLServer, UsuariosSQLServer>();
            services.AddSingleton<ITurnoSQL, TurnoSQL>();
            services.AddSingleton<IFacturaSQL, FacturaSQL>();
            services.AddSingleton<IRecepcionSQL, RecepcionSQL>();
            services.AddSingleton<IRetencionSQL, RetencionSQL>();
            services.AddSingleton<IDiarioSQL, DiarioSQL>();
            services.AddSingleton<IConvenioSQL, ConvenioSQL>();
            services.AddSingleton<IDigitalizacionSQL, DigitalizacionSQL>();
            services.AddSingleton<IOrdenServicioSQL, OrdenServicioSQL>();
            services.AddSingleton<ICoberturaSQL, CoberturaSQL>();
            services.AddSingleton<IDatosTecnicosSQL, DatosTecnicosSQL>();
            services.AddSingleton<IProcesoAMISQL, ProcesoAMISQL>();
            services.AddSingleton<IProcesoPXSQL, ProcesoPXSQL>();
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            // services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen(c =>{c.EnableAnnotations();});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiRest", Description = "API de Netsuite",  Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //para activar los comentarios
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autorizacion JWT esquema. \r\n\r\n Escribe 'Bearer' [espacio] y escribe tu token.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                        new List<string>()
                    }
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["JWT:ClaveSecreta"]))
                };
            });
            //Soporte para CORS
            //Se pueden habilitar: 1-Un dominio, 2-multiples dominios,
            //3-cualquier dominio (Tener en cuenta seguridad)
            //Usamos de ejemplo el dominio: http://localhost:3223, se debe cambiar por el correcto
            //Se usa (*) para todos los dominios
            services.AddCors(p => p.AddPolicy("PolicyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ////IsProduction
            //// if (env.IsDevelopment())
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            //app.UseSwagger(c =>
            //{
            //    c.RouteTemplate = "ApiRest/swagger/{documentName}/swagger.json";
            //});
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = string.Empty;
                //c.DocumentTitle="ddddd";
                c.DefaultModelsExpandDepth(-1); //quitar schemas de la pantalla
                c.SwaggerEndpoint("./v1/swagger.json", "ApiRest v1");

            });
            //// Configure the HTTP request pipeline.
            //if (env.IsProduction())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            //app.Use(async (ctx, next) =>
            //{
            //    await next();
            //    if (ctx.Response.StatusCode == 204)
            //    {
            //        ctx.Response.ContentLength = 0;
            //    }
            //});
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Content-Security-Policy",
            //        "default-src https: data: 'unsafe-inline' 'unsafe-eval'");
            //    context.Response.Headers.Add("Strict-Transport-Security",
            //        "max-age=31536000; includeSubDomains");
            //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            //    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            //    context.Response.Headers.Remove("X-Powered-By"); // Remove Powered-By header
            //    await next();
            //});
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           //app.UseCors();
            app.UseCors("PolicyCors");
        }

    }
}