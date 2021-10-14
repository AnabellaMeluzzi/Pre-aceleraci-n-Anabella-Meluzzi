using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "The title is required")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The title cannot be longer than 15 characters and less than 2 characters")]
        public string Title { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public int Score { get; set; }

        //A movie can have multiple characters
        public ICollection<Character> Characters { get; set; }

        //A movie can have multiple genres
        public ICollection<Genre> Genres { get; set; }
    }
}
