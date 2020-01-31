using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository // forteller at vi vil bruke IAuthRepository med : IAuthRep... ctrl . for å implemente alle interface metodene fra IAuthRepo..
    {
        private readonly DataContext _context;

        // Her vi skal injecte DataContext
        public AuthRepository(DataContext context) // ctor som hurtig - ctrl . på context og Intitialize field from parameter for å få readonly variabelen over
        {
            _context = context;

        }

        public async Task<User> Login(string username, string password) //metode for login, her tar vi inn den hashede versjonen av passordet, som igjen må sammenlignes
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username); //brukes for å fiske ut versjonen fra databasen - firstOrDef gir enten bruker eller null

            if (user == null) {
                return null;
            }

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var ComputeHash = passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //denne genererer den samme hashen som vi generer i CreateP..Hash
                for (int i = 0; i < ComputeHash.Length; i++) {
                    if(ComputeHash[i] != passwordHash[i]) return false; //matcher ikke 
                }
            }
            return true; //hashene matcher
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt); // med out paster vi en reference av variablene hash og salt og ikke en verdi

            user.PasswordHash = passwordHash; //overfører verdiene vi definerte under
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user); // legger dette til db med await async
            await _context.SaveChangesAsync(); // lagres endringene mot db 

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) // denne ble generert ved ctrl . på CreatePasswordHash over
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //alt inne i denne metoden blir disposed når vi er ferdig, derfor fin for salt
            {
                // begge disse variablene lagres i byte [] passwordHash, passwordSalt i metoden over
                passwordSalt = hmac.Key; //setter saltet 
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //setter passord hash
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username == username)) //sjekker brukernavn mot alle brukerne i DB for å se om det finnes
                return true;

            return false;
        }
    }
}