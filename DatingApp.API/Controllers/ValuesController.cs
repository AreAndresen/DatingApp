using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{

    //metodene i controllers kan brukes til å hente data fra databasen

    // controller er bare et placeholder navn som representerer controlleren og route path som kommer inn her (f.eks Values under), dette har med mapping og routing til en request
    // f.eks http:localhost:5000/api/values - her tar vi første del av controllernavnet og dette er routen vi skal treffe
    [Authorize] //gjør at det må være en authorisert request http
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase // benytter controllerbase fordi vi ikke benytter MVC, bare MC fordi Angular lager V (viewet vårt)
    {
        private readonly DataContext _context; //får denne til å bruke i klassen
        public ValuesController(DataContext context) //hurtig ctor - må enable context til å være available gjennom hele klassen ved å markere context og ctrl . og initialize
        {
            _context = context; //kan nå bruke _context for å ha tilgang til denne i hele klassen

        }


        // GET api/values - f.eks hente ulike verdier fra vår API 
        // Merk har med et array her og slik er metoden designet for å ta inn et values array og returnere string-array med verdiene i dette
        // ActionResult<IEnumerable<string>> Get() - Denne gjorde det bare mulig med string så byttet ut med IActionResult
        [AllowAnonymous] // kan med denne hentes uten authorization
        [HttpGet]
        public async Task<IActionResult> GetValues() ////Legger til async Task<...> for å kunne kjøre flere tråder med spørringer samtidig 
        {
            // throw new Exception("Test exception"); - til test av developer mode og production mode med feilvisninger i localhost
            //return new string[] { "value1", "value3" };

            //bruker values til å lagre listen fra spørringen - await for å vente og ToListAsync for å kjøre async metode
            var values = await _context.Values.ToListAsync(); // må konvertere til ToList for å kunne utføre sql query for å hente verdiene fra values

            // API values vil gå i denne metoden og returnere stringen fra spørringen mot db - Returneres videre mot klient 
            return Ok(values);
        }


        // GET api/values/5 - Merk har med en ID her og slik er metoden designet for å ta inn en int ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id) //ActionResult<string> Get(int id) // gammel kode
        {
            //return "value"; // gammel kode

            // x representerer verdien vi skal returnere, sørker for at x sin Id == iden fra parameteren til metoden - BRUKER POSTMAN FOR Å TESTE DETTE
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id); //FirstOrDefualt returnerer null istden for exepction og det er bedre enn bare first - EKS POSTMAN

            return Ok(value);
        }

        // POST api/values - f.eks create a record fra vår API - har ikke Logic enn så lenge så de gjør ikke noe ennå
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5 - f.eks edit a record fra vår API
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5 - f.eks delete a record fra vår API
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
