using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
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
            // HUSK: pakken er lagt til gjennom terminal og Nuget og add og deretter Microsoft.Entity...så versjon 3.1.1 og enter, må deretter trykke ctrl + .og du kan velge-- >
            // må alltid legge til nye services som blir lagt til i classer f.eks DataContext i Datacontext.cs

            //HUSk <DataContext> må spesifisere options siden vi snakker mot en database, dermed lambda og UsesSqlite(...) hvor Configuration.GetCon.... se appsettings.json
            services.AddDbContext<DataContext>(x => x.UseSqlite
            (Configuration.GetConnectionString("DefaultConnection"))); //Sqlite må vi adde som entity. Her må vi inn i ctrl, shift, p så Nuget add osv...(Se DatingApp.API)
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>(); //er da tilgjengelig for injection i klasser i resten av app i controllere osv - for auth repo
            // spesifiserer autenticationscheme som applikasjonen skal benytte - MÅ deretter fortelle applikasjonen om dette under
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //trenger denne for å legge til servicen for Token generering og validering 
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false, //localhost
                        ValidateAudience = false //localhost
                    };
                });

            services.AddControllers();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); //la til etter jeg så han hadde denne
        }   

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) //hvis i development mode
            {
                app.UseDeveloperExceptionPage(); //blir en developer vennlig exception side
            }
            
            // app.UseHttpsRedirection(); // kommentert fordi vi ikke ønsker å bli redirected til noe vi ikke lytter på lenger, iom https://localhost:5001 er fjernet fra tidligere

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseAuthentication(); // forteller applikasjonen om token autenticationscheme metoden vi lagde overr
            app.UseAuthorization();

            // metode som benyttes når vi starter opp og mapper controller endpoints, slik at API vet hvordan å route requests
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback", "area");
            });
        }
    }
}
