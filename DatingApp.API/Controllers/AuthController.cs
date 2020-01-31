using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase //trenger : ControllerBase for å bruke BadRequest i metoden under
    {
        // må injecte vårt Auth repo inn her - ctor hurtig
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config) // ctrl . (initialize from parameter)
        {
            _config = config;
            _repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto) // parameterne returnere egentlig et json serialized object - Må lage DataTransferObject (DTO)
        {
            // validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //må legge ved denne om [ApiController] er kommentert ut som kontrollerer request

            // vil lagre brukernavn med små bokstaver - slik at bruker kan logge inn uten case sensity
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            // lager brukeren vår med informasjonen vi innhenter fra parameterne 
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username // bare brukernavn her fordi vi må lage passordet gjennom Repo
            };

            // bruker den lagde brukeren til å sende passordet - 
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            // success msg til created registrert bruker til klient - må sende root tilbake som posisjon til resursen senere
            return StatusCode(201); // midlertidig 
        }

        // Lager ny metode for å la brukern logge på web API og returnere en Token som skal brukes for å authentisere seg mot API
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password); //sjekker at vi har en bruker og navn og passord matcher den 

            if (userFromRepo == null) //sjekker at det er noe fra repo 
                return Unauthorized(); //gir bare en default feilmelding uten å gi noen ekstra hint til f.eks en hacker

            //Genererer en token som skal returneres til brukeren - token vil inneholder brukerid og brukernavn og info kan legges til, token valideress av server uten db kall
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            //for at Token skal verfisieres må den signeres - trenger også en nøkkel for å signere Token - denne generer vi her - Dette må vi legge til i appsettings.json filen
            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_config.GetSection("AppSettings:Token").Value)); // må injecte denne configurasion inn i controlleren på topp

            // Etter nøkkel må vi har signing credentials - denne tar inn sikkerhetsnøkkelen vi lagde i appsettings.json og algoritmen
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Deretter trenger vi en security token descripter som inneholder signing credentials
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // exspire etter en dag 
                SigningCredentials = creds
            };

            // deretter Token handler
            var tokenHandler = new JwtSecurityTokenHandler(); //lager token basert på description

            // deretter kan vi lage en tokenhandler descriptor
            var token = tokenHandler.CreateToken(tokenDescripter);

            //returnerer Token som object til klienten - som deretter sendes til klienten
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

            
        }
    }
}