using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password); // info bruker legger inn for å sjekke brukernavn og passord mot db
         Task<bool> UserExists(string username); // for å sjekke at brukernavn finnes i db
         
    }
}