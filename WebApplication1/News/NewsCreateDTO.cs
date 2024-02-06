using System.ComponentModel.DataAnnotations;

namespace WebApplication1.News
{
    public class NewsCreateDTO
    {
        [Required]
        //[MinLength(10)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        public string Content { get; set; }
    }
}
