namespace whatsappProject.Models
{
    public class ChatList
    {
        public int Id { get; set; }

        public User selfUser { get; set; }

        public List<Chat> Chats { get; set; }

    }
}
