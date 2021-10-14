using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The content cannot be longer than 15 characters and less than 2 characters")]
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        [Required(ErrorMessage = "The story of the character is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "The content cannot be longer than 500 characters and less than 10 characters")]
        public string Story { get; set; }

        public Movie Movie { get; set; }
    }
}
