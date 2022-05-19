namespace whatsappProject.Models
{
    public class Contact
    {
        public string id { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string last { get; set; }
        public string lastdate { get; set; } 
        public string SecondSide { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Message> messages { get; set; }


    }
}
