using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message) 
        {
            response.Headers.Add("Application-Error", message); // ved exceptions vil klienten sende ut melding med en ny error msg
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error"); // er med for at den øverste skal kunne vises
            response.Headers.Add("Access-Control-Allow-Origin", "*"); // * sier allow any origin
        }
    }
}