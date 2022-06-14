using System.ComponentModel.DataAnnotations;
namespace whatsappProject.Models
{
    public class UserToken
    {
        [Key]
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}