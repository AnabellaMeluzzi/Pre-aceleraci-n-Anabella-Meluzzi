using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Movie.Get
{
    public class GetReqMovieVM
    {
        public string Image { get; set; }

        [Required(ErrorMessage = "The title is required")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The title cannot be longer than 15 characters and less than 2 characters")]
        public string Title { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}
