namespace whatsappProject.Models
{
    public class Contact
    {
        public string id { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string last { get; set; }
        public string lastdate { get; set; } 
        public User User { get; set; }
        public List<Message> messages { get; set; }


    }
}
