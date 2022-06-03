using System.ComponentModel.DataAnnotations;
namespace whatsappProject.Models
{
    public class Contact
    {
        public string id { get; set; }
        public string? UserName { get; set; }    
        public string name { get; set; }
        public string server { get; set; }
        public string last { get; set; }
        public string lastdate { get; set; }

        //public List<Message> Messages { get; set; }
    }
}
