using System.ComponentModel.DataAnnotations;
namespace whatsappProject.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public string NickName { get; set; }
        
        public int Password { get; set; }

        public string Image { get; set; }

    }

}
