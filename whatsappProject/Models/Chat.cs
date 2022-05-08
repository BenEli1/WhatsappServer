namespace whatsappProject.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public User User { get; set; }

        public List<Message> Messages { get; set; }

    }
}
