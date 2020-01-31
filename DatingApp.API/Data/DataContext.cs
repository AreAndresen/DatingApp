using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    // her trenger vi en entetiy framwork core classe med : DbContext - Må legge til autoassembly manuelt pga fjerning i ny SDK
    // må derfor bruke plugin NuGet og kommandolinjen: Microsoft.EntityFrameworkCore og velge riktig versjon 3.1.1
    // HUSK: pakken er lagt til gjennom terminal og Nuget og add og deretter Microsoft.Entity...så versjon 3.1.1 og enter, må deretter trykke ctrl + . og du kan velge-->

    // vi må også fortelle applikasjonen om DataContext klassen og tilgjengeliggjøre denne servicen; gå tilbaek til Startup.cs og legg til servicen: services.AddDbContext...
    public class DataContext : DbContext //lagt til DbContext for dette er klassen vi vil Drive fra og må hente denne entity fra nuget osv.
    {
        // Må gi construktøren noen options og spesifiserer det med DB..options og så type DataContext og så options, deretter klassen vi driver fra og options her også
        // det gjøres med : base(options)
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        
        //deretter forteller vi datacontext klassen om våre entities, med value som type i dbsettet og legger ved dataapp.models
        // Values er navnet som brukes til å representere tabellnavnet som opprettes når vi scafolder vår database
        public DbSet<Value> Values {get;  set;} 

        public DbSet<User> Users {get; set;} // må deretter legge til migration også i terminal for API
    }
}