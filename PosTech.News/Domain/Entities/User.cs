using System.ComponentModel.DataAnnotations;

namespace News.Domain.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}