using System.ComponentModel.DataAnnotations;
namespace whatsappProject.Models
{
    public class User
    {

        [Key]
        public string UserName { get; set; }

        public string NickName { get; set; }
        
        public string Password { get; set; }

        public string Image { get; set; }

        public string Server { get; set; }

        //public List<Contact> Contacts { get; set; }

    }

}
