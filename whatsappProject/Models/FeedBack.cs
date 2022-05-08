using System.ComponentModel.DataAnnotations;

namespace whatsappProject.Models
{
    public class FeedBack
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Must be between 1 to 5")]
        public int Score { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string FeedbackContent { get; set; }

        public string? Date { get; set; }


    }
}
