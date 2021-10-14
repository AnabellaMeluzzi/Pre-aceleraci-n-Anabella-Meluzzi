using DisneyAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Character.Post
{
    public class PostReqCharacterVM
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        public string Story { get; set; }

        public int? MovieID { get; set; }
    }
}
