using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Entities
{
    public class Genre
    {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "The name is required")]
            [StringLength(15, MinimumLength = 2, ErrorMessage = "The content cannot be longer than 15 characters and less than 2 characters")]
            public string Name { get; set; }

            public string Image { get; set; }

            //A genre can be in multiple movies
            public ICollection<Movie> Movies { get; set; }
        }
}
