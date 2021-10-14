using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Movie.Get
{
    public class GetResMovieDetailVM
    {
        public string Image { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public int Score { get; set; }

        public ICollection<string> Characters { get; set; }

        public ICollection<string> Genres { get; set; }
    }
}
