using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Passwordless.Blazor.App.Models
{
    public class PasswordlessUser
    {
        public PasswordlessUser()
        {
            var userId = Guid.NewGuid().ToString();

            Aliases = new string[] { userId };
            UserId = userId;
        }

        [Required]
        [JsonProperty("displayname")]
        public string Displayname { get; set; }

        [Required]
        [JsonProperty("username")]

        public string Username { get; set; }

        [JsonProperty("userId")]

        public string UserId { get; private set; }

        [JsonProperty("aliases")]
        public string[] Aliases { get; private set; }
    }
}
